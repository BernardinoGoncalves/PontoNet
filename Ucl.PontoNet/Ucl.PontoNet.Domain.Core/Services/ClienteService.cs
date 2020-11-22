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
        public ClienteService(IUnitOfWork context, IClienteRepository clienteRepository) : base(context, clienteRepository)
        {
            Validator = new ClienteValidator();
        }

    }
}
