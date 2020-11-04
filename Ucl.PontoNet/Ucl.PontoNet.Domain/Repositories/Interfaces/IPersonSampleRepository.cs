using System.Collections.Generic;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Entities;

namespace Ucl.PontoNet.Domain.Repositories.Interfaces
{
    public interface IPersonSampleRepository : IRepository<PersonSample>
    {
        IEnumerable<PersonSample> Get(bool? active, string sort, int page, int per_page, out int total);
    }
}