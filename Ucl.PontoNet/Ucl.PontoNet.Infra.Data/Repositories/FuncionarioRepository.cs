using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Entities;
using Ucl.PontoNet.Domain.Repositories.Interfaces;
using Ucl.PontoNet.Infra.Data.Base;

namespace Ucl.PontoNet.Infra.Data.Repositories
{
    public class FuncionarioRepository : Repository<Funcionario>, IFuncionarioRepository
    {
        public FuncionarioRepository(IUnitOfWork uow) : base(uow)
        {
        }



    }
}
