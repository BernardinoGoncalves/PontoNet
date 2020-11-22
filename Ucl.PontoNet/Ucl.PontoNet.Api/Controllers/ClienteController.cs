using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucl.PontoNet.Application.Services;

namespace Ucl.PontoNet.Api.Controllers
{
    /// <summary>
    /// Controller to PersonSample
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


    }
}
