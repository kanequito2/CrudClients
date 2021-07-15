using CrudClients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudClients.UseCase
{
    public interface IClientUseCase
    {
        Task<List<Client>> GetClients();

        Task<Client> GetClientById(string id);

        Task<Client> DeleteClientById(string id);

        Task<Client> InsertClient(Client client);

        Task<bool> PushClient(Client client);

        Task<Client> UpdateClient(Client client, UpdateFieldsModel fields);

        Task<List<Client>> GetClientsSortedByAge();
    }
}
