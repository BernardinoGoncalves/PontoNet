using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucl.PontoNet.Api.Filters;
using Ucl.PontoNet.Application.Dto;
using Ucl.PontoNet.Application.Services;

namespace Ucl.PontoNet.Api.Controllers
{
    /// <summary>
    /// Controller to Cliente
    /// </summary>
    [Route("api/Cliente")]
    public class ClienteController : Controller
    {
        private IClienteAppService ClienteAppService { get; }

        /// <summary>
        /// Constructor to Cliente Controller 
        /// </summary>
        /// <param name="ClienteAppService">app service</param>
        public ClienteController(IClienteAppService ClienteAppService)
        {
            this.ClienteAppService = ClienteAppService;
        }


        /// <summary>
        ///     Retorna todos os clientes.
        /// </summary>
        /// <remarks>
        ///     Retorna todos os clientes.
        /// </remarks>
        /// <returns>No content</returns>
        /// <response code="200">Sucess!</response>
        /// <response code="400">Cliente has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<ClienteDto>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetAll()
        {
            var result = ClienteAppService.GetAll();
            return Ok(result);
        }

        /// <summary>
        ///     Retorna o cliente pelo Cpf
        /// </summary>
        /// <remarks>
        ///     Retorna o cliente de acordo com o Cpf informado.
        /// </remarks>
        /// <param name="Cpf">Cpf do cliente a ser retornado.</param>
        /// <returns>No content</returns>
        /// <response code="200">Sucess!</response>
        /// <response code="400">Cliente has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpGet("Cpf/{Cpf}")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult GetByCpf(string Cpf)
        {
            var result = ClienteAppService.GetByCpf(Cpf);
            return Ok(result);
        }

        /// <summary>
        ///     Insere um novo cliente.
        /// </summary>
        /// <remarks>
        ///     Insere um novo cliente.
        /// </remarks>
        /// <returns>No content</returns>
        /// <response code="200">Sucess!</response>
        /// <response code="400">Cliente has not inserted</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPost("Insert")]
        [ProducesResponseType(typeof(bool), 201)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Insert([FromBody] ClienteDto cliente)
        {
            var result = ClienteAppService.Insert(cliente);
            return Ok(result);
        }

        /// <summary>
        ///     Deleta um Cliente de acordo com o CPF informado.
        /// </summary>
        /// <remarks>
        ///    Deleta um Cliente de acordo com o CPF informado.
        /// </remarks>
        /// <param name="Cpf">Cpf do Cliente</param>
        /// <returns>true or false</returns>
        /// <response code="204">Cliente deletado!</response>
        /// <response code="400">Cliente nao existe ou invalido </response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpDelete("Delete/{Cpf}")]
        [ProducesResponseType(typeof(bool), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Delete(string Cpf)
        {
            var result = ClienteAppService.Delete(Cpf);
            return Ok(result);
        }


        /// <summary>
        ///     Atualizar cliente
        /// </summary>
        /// <remarks>
        ///    Atualiza o cliente de acordo com o CPF informado.
        /// </remarks>
        /// <response code="200">Cliente atualizado!</response>
        /// <response code="400">Cliente has missing/invalid values</response>
        /// <response code="500">Oops! Can't list your area right now</response>
        [HttpPut("Update")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 500)]
        public IActionResult Put([FromBody] ClienteDto cliente)
        {
            var result = ClienteAppService.Update(cliente);
           return Ok(result);
        }


    }
}
