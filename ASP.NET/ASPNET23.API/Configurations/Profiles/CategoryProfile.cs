using ASPNET23.Dto.Categories;
using ASPNET23.Model.Entities;

using AutoMapper;

namespace ASPNET23.API.Configurations.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() 
        {
            CreateMap<Category, CategoryOutputDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.NameWithDescription, d=> d.MapFrom(x => x.Name + x.Description));
        }
    }
}
