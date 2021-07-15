using CrudClients.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudClients.Database.MongoDb
{
    public class MongoContext
        : IContext
    {
        private readonly MongoClient Client;
        private readonly IMongoDatabase Database;
        private readonly IMongoCollection<Client> Collection;
        public MongoContext(string connectionString, string dbName)
        {
            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(dbName);
            Collection = Database.GetCollection<Client>("clients");
        }

        public async Task<List<Client>> GetClients()
        {
            var query = await Collection.FindAsync<Client>(Builders<Client>.Filter.Empty);
            var clients = query.ToList();
            return clients;
        }
        public async Task<Client> GetClientById(string id)
        {
            var query = await Collection.FindAsync(Builders<Client>.Filter.Eq(c => c.Id, id));
            var client = query.FirstOrDefault();
            return client;
        }
        public async Task<Client> InsertClient(Client client)
        {
            try
            {
                await Collection.InsertOneAsync(client);
                return (Client)null;
            }
            catch
            {
                return client;
            }
        }
        public async Task<Client> DeleteClientById(string id)
        {
            var exists = await GetClientById(id);
            if (exists != null)
            {
                var result = await Collection.DeleteOneAsync(Builders<Client>.Filter.Eq(c => c.Id, id));
                if (result.IsAcknowledged)
                {
                    return result.DeletedCount > 0 ? exists : null;
                }
                else
                {
                    throw CustomException.New(500, "Transaction not acknowledged.");
                }
            }

            return exists;
        }

        public async Task<bool> TryReplaceClient(Client client)
        {
            var exists = await GetClientById(client.Id);

            if (exists == null)
            {
                return false;
            }

            var filter = Builders<Client>.Filter.Eq(c => c.Id, client.Id);
            var result = await Collection.ReplaceOneAsync(filter, client);

            return result.IsAcknowledged ? result.ModifiedCount > 0 :
                throw CustomException.New(500, "Transaction not acknowledged.");
        }

        public async Task<bool> UpdateClient(Client client, UpdateFieldsModel fields)
        {
            var filter = Builders<Client>.Filter.Eq(c => c.Id, client.Id);
            var update = CreateUpdate(client, fields);
            var result = await Collection.UpdateOneAsync(filter, update);

            if (result.IsAcknowledged)
            {
                return result.ModifiedCount > 0;
            }
            else
            {
                throw CustomException.New(500, "Transaction not acknowledged.");
            }
        }
        private UpdateDefinition<Client> CreateUpdate(Client client, UpdateFieldsModel fields)
        {
            if (fields.NameUpdate == null)
            {
                fields.NameUpdate = client.Name;
            }
            if (fields.AgeUpdate == 0)
            {
                fields.AgeUpdate = client.Age;
            }
            if (fields.ActiveUpdate == null)
            {
                fields.ActiveUpdate = client.Active;
            }
            var update = Builders<Client>.Update.Combine(
                Builders<Client>.Update.Set(c => c.Name, fields.NameUpdate),
                Builders<Client>.Update.Set(c => c.Active, fields.ActiveUpdate),
                Builders<Client>.Update.Set(c => c.Age, fields.AgeUpdate));
            return update;
        }
    }
}
