﻿using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Entities;

namespace Ucl.PontoNet.Domain.Repositories.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        IEnumerable<Cliente> GetAll();
        Cliente GetByCpf(string Cpf);
        bool Insert(Cliente cliente);
        bool Delete(string Cpf);
        bool Update(Cliente cliente);
    }
}
