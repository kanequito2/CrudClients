using CrudClients.Database;
using CrudClients.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrudClients.UseCase
{
    public class ClientUseCase
        : IClientUseCase
    {
        private readonly IClientRepository repository;
        private readonly ILogger<ClientUseCase> logger;

        public ClientUseCase(ILogger<ClientUseCase> logger, IClientRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<List<Client>> GetClients()
        {
            var clients = await repository.GetClients();

            return clients;
        }

        public async Task<Client> GetClientById(string id)
        {
            return await repository.GetClientById(id);
        }

        public async Task<Client> DeleteClientById(string id)
        {
            try
            {
                var data = await repository.DeleteClientById(id);
                return data;
            }
            catch (CustomException cex)
            {
                throw cex;
            }
        }

        public async Task<Client> InsertClient(Client client)
        {
            var data = await repository.InsertClient(client);
            return data;
        }

        public async Task<bool> PushClient(Client client)
        {
            try
            {
                var tryReplace = await repository.TryReplaceClient(client);

                if (!tryReplace)
                {
                    await repository.InsertClient(client);
                }

                return tryReplace;
            }
            catch (CustomException cex)
            {
                throw cex;
            }

        }

        public async Task<List<Client>> GetClientsSortedByAge()
        {
            var clients = await repository.GetClientsSortedByAge();
            return clients;
        }

        public async Task<Client> UpdateClient(Client client, UpdateFieldsModel fields)
        {
            try
            {
                var data = await repository.UpdateClient(client, fields);
                return data;
            }
            catch (CustomException cex)
            {

                throw cex;
            }

        }
    }
}
