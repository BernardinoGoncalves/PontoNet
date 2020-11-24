using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Domain.Base;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Core.Validations;
using Ucl.PontoNet.Domain.Entities;
using Ucl.PontoNet.Domain.Repositories.Interfaces;
using Ucl.PontoNet.Domain.Services;

namespace Ucl.PontoNet.Domain.Core.Services
{
    public class ClienteService : Service<Cliente>, IClienteService
    {
        private IClienteRepository _clienteRepository;
        public ClienteService(IUnitOfWork context, IClienteRepository clienteRepository) : base(context, clienteRepository)
        {
            Validator = new ClienteValidator();
            this._clienteRepository = clienteRepository;
        }

        public bool Delete(string Cpf)
        {
            try
            {
                var result = _clienteRepository.Delete(Cpf);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Insert(Cliente cliente)
        {
            try
            {
                var result = _clienteRepository.Insert(cliente);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }
    }
}
