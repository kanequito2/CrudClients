using CrudClients.Models;
using CrudClients.UseCase;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

namespace CrudClients.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {

        private readonly ILogger<ClientsController> _logger;
        private readonly IClientUseCase manager;

        public ClientsController(ILogger<ClientsController> logger, IClientUseCase manager)
        {
            this.manager = manager;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(200, Type = typeof(ResponseModel<IEnumerable<Client>>))]
        public async Task<IActionResult> GetClients()
        {
            var clients = await manager.GetClients();

            var message = $"Found {clients.Count} clients.";

            return Ok(ResponseModel<IEnumerable<Client>>.New((int)HttpStatusCode.OK, message, clients));
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(ResponseModel<Client>))]
        [ProducesResponseType(400, Type = typeof(ResponseModel<Client>))]
        [ProducesResponseType(404, Type = typeof(ResponseModel<Client>))]
        public async Task<IActionResult> GetClientById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(ResponseModel<Client>.New((int)HttpStatusCode.BadRequest, "Id must be provided and cannot be empty", null));
            }

            var client = await manager.GetClientById(id);

            if (client == null)
            {
                return NotFound(ResponseModel<Client>.New((int)HttpStatusCode.NotFound, $"No client found with id: '{id}'.", client));
            }

            return Ok(ResponseModel<Client>.New((int)HttpStatusCode.OK, $"The following client was found.", client));
        }

        [HttpGet]
        [Route("SortedByAge")]
        [ProducesResponseType(200, Type = typeof(ResponseModel<Client>))]
        public async Task<IActionResult> GetClientsSortedByAge()
        {
            var clients = await manager.GetClientsSortedByAge();
            string message;
            if(clients.Count > 0)
            {
                message = $"Found {clients.Count} clients.";
            }
            else
            {
                message = "No clients found.";
            }
            return Ok(ResponseModel<IEnumerable<Client>>.New((int)HttpStatusCode.OK, message, clients));
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(ResponseModel<Client>))]
        [ProducesResponseType(400, Type = typeof(ResponseModel<Client>))]
        [ProducesResponseType(404, Type = typeof(ResponseModel<Client>))]
        public async Task<IActionResult> DeleteClientById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(ResponseModel<Client>.New((int)HttpStatusCode.BadRequest, "Id must be provided and cannot be empty", null));
            }

            var client = await manager.DeleteClientById(id);

            if (client == null)
            {
                return NotFound(ResponseModel<Client>.New((int)HttpStatusCode.NotFound, $"No client found with id: '{id}'.", client));
            }

            return Ok(ResponseModel<Client>.New((int)HttpStatusCode.OK, $"The following client was deleted.", client));
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(200, Type = typeof(ResponseModel<Client>))]
        [ProducesResponseType(400, Type = typeof(ResponseModel<Client>))]
        [ProducesResponseType(500, Type = typeof(ResponseModel<Client>))]
        public async Task<IActionResult> InsertClient([FromBody] Client client)
        {
            if (!ValidateClient(client))
            {
                return BadRequest(ResponseModel<Client>.New((int)HttpStatusCode.BadRequest, $"The client has invalid fields.", client));
            }

            var response = await manager.InsertClient(client);

            if (response == null)
            {
                return Ok(ResponseModel<Client>.New((int)HttpStatusCode.OK, $"Client has been inserted.", client));
            }
            else
            {
                return StatusCode(500, ResponseModel<Client>.New((int)HttpStatusCode.InternalServerError, $"Client already exists.", client));
            }

        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(ResponseModel<Client>))]
        [ProducesResponseType(400, Type = typeof(ResponseModel<Client>))]
        [ProducesResponseType(400, Type = typeof(ResponseModel<UpdateFieldsModel>))]
        public async Task<IActionResult> UpdateClient(string id, [FromBody] UpdateFieldsModel fields)
        {

            if (!ValidateUpdate(fields))
            {
                return BadRequest(ResponseModel<UpdateFieldsModel>.New((int)HttpStatusCode.BadRequest, $"Error reading fields to update", fields));
            }

            var client = await manager.GetClientById(id);

            if (client == null)
            {
                return NotFound(ResponseModel<Client>.New((int)HttpStatusCode.NotFound, $"No client found with id: '{id}'.", client));
            }

            var response = await manager.UpdateClient(client, fields);

            if (response == null)
            {
                return Ok(ResponseModel<Client>.New((int)HttpStatusCode.OK, $"Client found but was not modified.", client));
            }

            return Ok(ResponseModel<Client>.New((int)HttpStatusCode.OK, $"The client has been modified as follows", response));
        }

        [HttpPut]
        [Route("")]
        [ProducesResponseType(200, Type = typeof(ResponseModel<Client>))]
        [ProducesResponseType(400, Type = typeof(ResponseModel<Client>))]
        public async Task<IActionResult> PushClient([FromBody] Client client)
        {
            if (!ValidateClient(client))
            {
                return BadRequest(ResponseModel<Client>.New((int)HttpStatusCode.BadRequest, $"The client has invalid fields.", client));
            }

            var response = await manager.PushClient(client);

            string message = response ? "The client has been replaced." : "Client not found. It was inserted";

            return Ok(ResponseModel<Client>.New((int)HttpStatusCode.OK, message, client));
        }

        #region Private
        private bool ValidateClient(Client client)
        {
            if (client == null)
            {
                return false;
            }

            bool ValidName = !string.IsNullOrEmpty(client.Name);
            bool ValidAge = (client.Age > 18) && (client.Age < 100);
            bool ValidActive = client.Active != null;
            bool ValidId = client.Id != null;

            return ValidName && ValidAge && ValidActive && ValidId;
        }
        private bool ValidateUpdate(UpdateFieldsModel fields)
        {
            if (fields == null)
            {
                return false;
            }
            bool ValidName = fields.NameUpdate == null || !fields.NameUpdate.Equals(string.Empty);
            bool ValidAge = (fields.AgeUpdate > 18) && (fields.AgeUpdate < 100);
            return ValidName && ValidAge;
        }
        #endregion
    }
}
