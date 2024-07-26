using AutoMapper;
using Entities;
using Saas.Api.Model;
using Saas.Entities;

namespace Saas.Api.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Secret, SecretModel>().ReverseMap();
            CreateMap<Secret, SecretToUpdateModel>().ReverseMap();
            CreateMap<Collection, CollectionModel>().ReverseMap();
            CreateMap<Collection, CollectionResponseModel>().ReverseMap();
            CreateMap<Organization, OrganizationRequestModel>().ReverseMap();
            CreateMap<Organization, OrganizationResponseModel>().ReverseMap();
            CreateMap<OneTimeShare, OneTimeShareRequestModel>().ReverseMap();
            CreateMap<OneTimeShare, OneTimeShareResponseModel>().ReverseMap();
        }
    }
}