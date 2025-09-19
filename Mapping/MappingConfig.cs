using Mapster;
using myapp.auth.Dtos;
using myapp.auth.Models;

namespace myapp.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register ( TypeAdapterConfig config )
        {
            config.NewConfig<CreateProfileDto, Profile>();
            config.NewConfig<TrainingHistoryItemDto, TrainingHistoryItem>();
            config.NewConfig<CertificateItemDto, CertificateItem>();
        }
    }
}
