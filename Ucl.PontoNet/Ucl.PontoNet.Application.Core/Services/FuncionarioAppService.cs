using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Application.Base;
using Ucl.PontoNet.Application.Services;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Repositories.Interfaces;
using Ucl.PontoNet.Domain.Services;

namespace Ucl.PontoNet.Application.Core.Services
{
    public class FuncionarioAppService : AppService, IFuncionarioAppService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;

        public FuncionarioAppService(IUnitOfWork uoW, IMapper Mapper, IFuncionarioRepository funcionarioRepository, IFuncionarioService funcionarioService) : base(uoW, Mapper)
        {
            this._funcionarioRepository = funcionarioRepository;
            this._funcionarioService = funcionarioService;
        }

        private IFuncionarioService _funcionarioService { get; set; }
    }
}
