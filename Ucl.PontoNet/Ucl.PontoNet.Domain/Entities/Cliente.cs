using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Domain.Base;

namespace Ucl.PontoNet.Domain.Entities
{
    public class Cliente : Entity
    {

        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string CPF { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }

    }
}
