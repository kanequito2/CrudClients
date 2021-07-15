using CrudClients.Database.MongoDb;
using CrudClients.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CrudClients.Database
{
    public class ClientRepository
        : IClientRepository
    {
        private readonly IContext Context;
        public ClientRepository(IContext context)
        {
            Context = context;
        }
        /// <summary>
        /// Gets the list of all the clients.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Client>> GetClients()
        {
            return await Context.GetClients();
        }

        /// <summary>
        /// Gets a client by its id or null if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Client> GetClientById(string id)
        {
            return await Context.GetClientById(id);
        }

        /// <summary>
        /// Inserts a client if posible. Returns null if successful or the existing client otherwise.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<Client> InsertClient(Client client)
        {
            var query = await Context.InsertClient(client);

            return query ;
        }
        
        /// <summary>
        /// Deletes a client by its id. Returns the deleted client, null if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Client> DeleteClientById(string id)
        {
            var query = await Context.DeleteClientById(id);

            return query;
        }

        /// <summary>
        /// Tries to replace a client if Ids match. Returns true if succesful, false otherwise.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<bool> TryReplaceClient(Client client)
        {
            var replaced = await Context.TryReplaceClient(client);
            return replaced;
        }

        public async Task<List<Client>> GetClientsSortedByAge()
        {
            var clients = await Context.GetClients();

            return clients.OrderBy(c => c.Age).ToList();
        }

        /// <summary>
        /// Updates a client. Returns the updated client if it changed, null otherwise.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<Client> UpdateClient(Client client, UpdateFieldsModel fields)
        {
            var updated = await Context.UpdateClient(client, fields);

            return updated ? ModifiedClient(client.Id, fields)
                : null;
        }
        private Client ModifiedClient(string id, UpdateFieldsModel fields)
        {
            return new Client()
            {
                Id = id,
                Name = fields.NameUpdate,
                Age = fields.AgeUpdate,
                Active = fields.ActiveUpdate
            };
        }
    }
}
