using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseLibrary.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CourseLibrary.api.models;
using CourseLibrary.API.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        public ActionResult<IEnumerable<CoursesDto>> GetCourseForAuthors(Guid authorId)
        {
            if (_courseLibraryRepository.AuthorExists(authorId))
            {
                var getCouseFromRepo = _courseLibraryRepository.GetCourses(authorId);
                return Ok(_mapper.Map<IEnumerable<CoursesDto>>(getCouseFromRepo));
            }
            return NotFound();

        }
        //[Route("{courseId}")]
        [HttpGet("{courseId}", Name = "GetCourseForAuthor")]
        public ActionResult<CoursesDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {

                return NotFound();
                //return Ok(_mapper.Map<IEnumerable<CoursesDto>>(getCouseFromRepo));
            }

            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CoursesDto>(courseForAuthorFromRepo));
        }
        [HttpPost]
        public ActionResult<CoursesDto> CreateCourse(Guid authorId, courseCreationDto course)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseEntity = _mapper.Map<Course>(course);
            _courseLibraryRepository.AddCourse(authorId, courseEntity);
            _courseLibraryRepository.Save();

            var courseToreturn = _mapper.Map<CoursesDto>(courseEntity);

            return CreatedAtRoute("GetCourseForAuthor", new { authorId = authorId, courseId = courseEntity.Id },
                courseToreturn);
        }
        [HttpPut("{courseId}")]

        public IActionResult updateCourseForAuthor(Guid authorId,
            Guid courseId,
            courseForUpdateDto course)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {

                return NotFound();
                //return Ok(_mapper.Map<IEnumerable<CoursesDto>>(getCouseFromRepo));
            }

            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo == null)
            {
                var courseEntity = _mapper.Map<Course>(course);
                courseEntity.Id = courseId;

                _courseLibraryRepository.AddCourse(authorId, courseEntity);

                _courseLibraryRepository.Save();

                var courseToReturn = _mapper.Map<CoursesDto>(courseEntity);

                return CreatedAtRoute("GetCourseForAuthor", new
                {
                    authorId = authorId,
                    courseId = courseToReturn.Id
                }, courseToReturn);
            }

            _mapper.Map(course, courseForAuthorFromRepo);

            _courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);

            _courseLibraryRepository.Save();

            return NoContent();
        }

        //patch method
        [HttpPatch("{courseId}")]
        public IActionResult PartiallyUpdateCourseForAuthor(Guid authorId,
            Guid courseId,
            JsonPatchDocument<courseForUpdateDto> patchDocument)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {

                return NotFound();
            }

            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo == null)
            {
                var courseEntity = new courseForUpdateDto();
                patchDocument.ApplyTo(courseEntity, ModelState);

                if (!TryValidateModel(courseEntity))
                {
                    return ValidationProblem(ModelState);
                }

                var courseToAdd = _mapper.Map<Course>(courseEntity);

                courseToAdd.Id = courseId;

                _courseLibraryRepository.AddCourse(authorId, courseToAdd);

                _courseLibraryRepository.Save();

                return CreatedAtRoute("GetCourseForAuthor", new
                {
                    authorId = authorId,
                    courseId = courseToAdd.Id
                }, courseToAdd );
            }
            var courseToPatch = _mapper.Map<courseForUpdateDto>(courseForAuthorFromRepo);

            patchDocument.ApplyTo(courseToPatch, ModelState);

            if (!TryValidateModel(courseToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(courseToPatch, courseForAuthorFromRepo);

            _courseLibraryRepository.UpdateCourse(courseForAuthorFromRepo);

            _courseLibraryRepository.Save();
            return NoContent();

        }
        [HttpDelete("{courseId}")]

        public IActionResult DeleteCourseFromAuthor(Guid authorId, Guid courseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {

                return NotFound();
            }

            var courseForAuthorFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);

            if (courseForAuthorFromRepo == null)
            {
                return NotFound();
            }

            _courseLibraryRepository.DeleteCourse(courseForAuthorFromRepo);

            _courseLibraryRepository.Save();

            return NoContent();
        }

        //to change 400 to 422 for patch document.
        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var option = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)option.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    } 
}