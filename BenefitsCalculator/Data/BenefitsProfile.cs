using AutoMapper;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Models;
using Microsoft.Build.Framework;

namespace BenefitsCalculator.Data
{
    public class BenefitsProfile : Profile
    {
        public BenefitsProfile()
        {
            CreateMap<Setup, SetupDTO>()
                .ReverseMap();

            CreateMap<Consumer, ConsumerDTO>()
                .ReverseMap();

            CreateMap<BenefitsHistDTO, BenefitsHistory>()
                .ForMember(dest => dest.BenefitsStatus,
                           opt => opt.MapFrom(src => (int)src.BenefitsStatus))
                .ReverseMap();

            CreateMap<HistGroupDTO, BenefitsHistGroup>()
                .ForMember(dest => dest.CreatedBy,
                           opt => opt.Ignore())
                .ForMember(dest => dest.BenefitsHistories,
                           opt => opt.MapFrom(src => src.BenefitsList));
        }
    }
}
