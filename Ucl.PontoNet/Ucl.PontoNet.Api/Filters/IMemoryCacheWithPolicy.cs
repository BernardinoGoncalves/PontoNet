using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ucl.PontoNet.Api.Filters
{
    public interface IMemoryCacheWithPolicy<UserInfoDto>
    {
        Task<UserInfoDto> GetOrCreateAsync(object key, Func<Task<UserInfoDto>> createItem);
    }
}