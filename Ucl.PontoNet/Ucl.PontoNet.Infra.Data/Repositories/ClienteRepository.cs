using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Entities;
using Ucl.PontoNet.Domain.Repositories.Interfaces;
using Ucl.PontoNet.Infra.Data.Base;

namespace Ucl.PontoNet.Infra.Data.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public IEnumerable<Cliente> GetAll()
        {
            var Query = Connection.Query<Cliente>("SELECT * FROM Cliente");

            return Query;
        }

        public Cliente GetByCpf(string Cpf)
        {
            var Query = Connection.Query<Cliente>(@"SELECT * 
                                                    FROM Cliente
                                                    WHERE Cpf = '" + Cpf + "'").SingleOrDefault();

            return Query;
        }


        public bool Insert(Cliente cliente)
        {
            var Insert = @"INSERT INTO  [dbo].[CLIENTE]
                                        ([NOME]
                                        ,[SOBRENOME]
                                        ,[CPF]
                                        ,[ENDERECO]
                                        ,[TELEFONE])
                                    VALUES
                                        ( '"    + cliente.Nome
                                        + "','" + cliente.Sobrenome
                                        + "','" + cliente.CPF
                                        + "','" + cliente.Endereco
                                        + "','" + cliente.Telefone + "')";

            Connection.Execute(Insert);

            return true;
        }
        
        public bool Delete(string Cpf)
        {
            var Delete = @"DELETE Cliente
                           WHERE CPF = '"+ Cpf +"'";

            Connection.Execute(Delete);

            return true;
        }

    }
}
