using System.Collections.Generic;
using Dapper;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Entities;
using Ucl.PontoNet.Domain.Repositories.Interfaces;
using Ucl.PontoNet.Infra.Data.Base;

namespace Ucl.PontoNet.Infra.Data.Repositories
{
    public class PersonSampleRepository : Repository<PersonSample>, IPersonSampleRepository
    {
        public PersonSampleRepository(IUnitOfWork context) : base(context)
        {
        }


        public IEnumerable<PersonSample> Get(bool? active, string sort, int page, int per_page, out int total)
        {
            var sql = @"SELECT [Id],
                    P.[Active],
                    P.[FirstName],
                    P.[LastName],
                    P.[DateBirth],
                    P.[Type],
                    COUNT(1) OVER () as Total
                FROM PersonSamples P
                WHERE (@Active IS NULL OR  P.Active = @Active) ";

            sql += string.Format(@"
                ORDER BY P.{0}
                OFFSET ({1}-1)*{2} ROWS FETCH NEXT {2} ROWS ONLY", sort, page, per_page);

            var count = 0;

            var result = Connection.Query<PersonSample, int, PersonSample>(sql,
                (p, t) =>
                {
                    count = t;
                    return p;
                },
                splitOn: "Total",
                param: new { Active = active });
            total = count;
            return result;
        }
    }
}