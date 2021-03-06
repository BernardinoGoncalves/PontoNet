﻿using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Entities;

namespace Ucl.PontoNet.Domain.Services
{
    public interface IClienteService : IService<Cliente>
    {
        bool Insert(Cliente cliente);
        bool Delete(string Cpf);
        bool Update(Cliente cliente);
    }
}
