using CrudClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudClients.Database.MongoDb
{
    public interface IContext
    {
        Task<List<Client>> GetClients();

        Task<Client> GetClientById(string id);

        Task<Client> InsertClient(Client client);

        Task<Client> DeleteClientById(string id);

        Task<bool> TryReplaceClient(Client client);

        Task<bool> UpdateClient(Client client, UpdateFieldsModel fields);

    }
}
