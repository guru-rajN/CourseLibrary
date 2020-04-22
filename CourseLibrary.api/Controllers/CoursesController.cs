using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseLibrary.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CourseLibrary.api.models;
using CourseLibrary.API.Entities;

namespace CourseLibrary.api.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        public ActionResult<IEnumerable<CoursesDto>> GetCourseForAuthors(Guid authorId) {
            if (_courseLibraryRepository.AuthorExists(authorId))
            {
                var getCouseFromRepo = _courseLibraryRepository.GetCourses(authorId);
                return Ok(_mapper.Map<IEnumerable<CoursesDto>>(getCouseFromRepo));
            }
            return NotFound();

        }
        //[Route("{courseId}")]
        [HttpGet("{courseId}" , Name = "GetCourseForAuthor")]
        public ActionResult<CoursesDto> GetCourseForAuthor(Guid authorId,Guid courseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {

                return NotFound();
                //return Ok(_mapper.Map<IEnumerable<CoursesDto>>(getCouseFromRepo));
            }
             
            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);

            if(courseForAuthorFromRepo == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CoursesDto>(courseForAuthorFromRepo));
        }
        [HttpPost]
        public ActionResult<CoursesDto> CreateCourse(Guid authorId,courseCreationDto course)
        {
            if(!_courseLibraryRepository.AuthorExists(authorId))
            {
              return  NotFound();
            }

            var courseEntity = _mapper.Map<Course>(course);
            _courseLibraryRepository.AddCourse(authorId, courseEntity);
            _courseLibraryRepository.Save();

            var courseToreturn = _mapper.Map<CoursesDto>(courseEntity);

            return CreatedAtRoute("GetCourseForAuthor", new { authorId = authorId, courseId = courseEntity.Id },
                courseToreturn);
        }

    }
}