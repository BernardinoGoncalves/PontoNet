using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ucl.PontoNet.Application.Services;

namespace Ucl.PontoNet.Api.Controllers
{
    /// <summary>
    /// Controller to Funcionario
    /// </summary>
    [Route("api/Funcionario")]
    public class FuncionarioController : Controller
    {
        private IFuncionarioAppService FuncionarioAppService { get; }
        /// <summary>
        /// Constructor to Funcionario Controller 
        /// </summary>
        /// <param name="FuncionarioAppService">app service</param>
        public FuncionarioController(IFuncionarioAppService FuncionarioAppService)
        {
            this.FuncionarioAppService = FuncionarioAppService;
        }


    }
}
