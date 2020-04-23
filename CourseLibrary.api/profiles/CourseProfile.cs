using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.api.models;

using CourseLibrary.API.Entities;

namespace CourseLibrary.api.profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CoursesDto>();
            CreateMap<courseCreationDto, Course>();
            CreateMap<courseForUpdateDto, Course>();
            CreateMap<Course, courseForUpdateDto>();
        }
    }
}
