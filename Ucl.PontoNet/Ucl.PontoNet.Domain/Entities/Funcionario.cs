using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Domain.Base;

namespace Ucl.PontoNet.Domain.Entities
{
    public class Funcionario : Pessoa
    {

        public string Telefone { get; set; }
        public string Matricula { get; set; }
        public string Cargo { get; set; }


    }
}
