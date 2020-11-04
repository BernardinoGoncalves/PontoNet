using AutoMapper;

namespace Ucl.PontoNet.Application.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                // cfg.CreateMissingTypeMaps = true;
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new DataTransferToDomainMappingProfile());
            });
        }
    }
}