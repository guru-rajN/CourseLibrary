using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.api.Helpers;

namespace CourseLibrary.api.profiles
{
    public class authorProfiles : Profile
    {
        public authorProfiles()
        {
            CreateMap<Author, models.authorDto>()
                .ForMember(
                dest => dest.FirstName,
                opt => opt.MapFrom(src => $"{src.FirstName}{src.LastName}"))

                .ForMember(
                dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));

            CreateMap<models.authorCreationDto, Author>();
        }
    }
}
