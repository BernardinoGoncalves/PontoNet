using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Application.Dto;

namespace Ucl.PontoNet.Application.Services
{
    public interface IClienteAppService
    {
        IEnumerable<ClienteDto> GetAll();
        ClienteDto GetByCpf(string Cpf);
        bool Insert(ClienteDto cliente);
        bool Delete(string Cpf);
    }
}
