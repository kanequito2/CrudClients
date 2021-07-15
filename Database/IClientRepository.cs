using CrudClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudClients.Database
{
    public interface IClientRepository
    {
        /// <summary>
        /// Gets the list of all the clients.
        /// </summary>
        /// <returns></returns>
        Task<List<Client>> GetClients();

        /// <summary>
        /// Gets a client by its id or null if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Client> GetClientById(string id);

        /// <summary>
        /// Inserts a client if posible. Returns null if successful or the existing client otherwise.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<Client> InsertClient(Client client);

        // <summary>
        /// Deletes a client by its id. Returns the deleted client, null if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Client> DeleteClientById(string id);

        /// <summary>
        /// Tries to replace a client if Ids match. Returns true if succesful, false otherwise.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<bool> TryReplaceClient(Client client);

        /// <summary>
        /// Updates a client. Returns the updated client if it exists, null otherwise.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<Client> UpdateClient(Client client, UpdateFieldsModel fields);

        /// <summary>
        /// Gets all the clients sorted by their age.
        /// </summary>
        /// <returns></returns>
        Task<List<Client>> GetClientsSortedByAge();
    }
}
