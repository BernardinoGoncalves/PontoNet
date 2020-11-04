using AutoMapper;
using Ucl.PontoNet.Domain.Base.Interfaces;


namespace Ucl.PontoNet.Application.Base
{
    public abstract class AppService
    {
        public AppService(IUnitOfWork uoW, IMapper Mapper)
        {
            UoW = uoW;
            this.Mapper = Mapper;
        }

        protected IUnitOfWork UoW { get; set; }
        protected IMapper Mapper { get; set; }
    }
}