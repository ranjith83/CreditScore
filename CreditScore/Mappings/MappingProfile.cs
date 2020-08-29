
using AutoMapper;
using CreditScore.Models;
using CreditScore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditScore.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreditInquiresViewModel, CreditInquires>();

            CreateMap<CreditInquires ,CreditInquiresViewModel>();

            CreateMap<Customer, CustomerViewModel>();

            CreateMap<CustomerViewModel, Customer>();
        }
            //CreateMap<UsersViewModel, Users>()
            //    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            //    .ForMember(dest => dest.Contactno, opt => opt.MapFrom(src => src.Contactno))
            //    .ForMember(dest => dest.EmailId, opt => opt.MapFrom(src => src.EmailId))
            //    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            //    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

    }
}
