using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Application.Base;
using Ucl.PontoNet.Application.Dto;
using Ucl.PontoNet.Application.Services;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Entities;
using Ucl.PontoNet.Domain.Repositories.Interfaces;
using Ucl.PontoNet.Domain.Services;

namespace Ucl.PontoNet.Application.Core.Services
{
    public class ClienteAppService : AppService, IClienteAppService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteAppService(IUnitOfWork uoW, IMapper Mapper, IClienteService clienteService, IClienteRepository clienteRepository) : base(uoW, Mapper)
        {
            this._clienteRepository = clienteRepository;
            this._clienteService = clienteService;
        }

        public IClienteService _clienteService { get; set; }

        public bool Delete(string Cpf)
        {
            var result = _clienteRepository.Delete(Cpf);

            return result;
        }

        public IEnumerable<ClienteDto> GetAll()
        {
            var result = _clienteRepository.GetAll();

            return Mapper.Map<IEnumerable<ClienteDto>>(result);
        }

        public ClienteDto GetByCpf(string Cpf)
        {
            var result = _clienteRepository.GetByCpf(Cpf);

            return Mapper.Map<ClienteDto>(result);
        }

        public bool Insert(ClienteDto cliente)
        {
            var Obj = Mapper.Map<Cliente>(cliente);

            var result = _clienteService.Insert(Obj);

            return result;
        }

        public bool Update(ClienteDto cliente)
        {
            var Obj = Mapper.Map<Cliente>(cliente);

            var result = _clienteService.Update(Obj);

            return result;
        }
    }
}
