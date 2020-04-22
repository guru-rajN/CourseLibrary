using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CourseLibrary.API.Services;
using CourseLibrary.api.models;
using CourseLibrary.API.Entities;
using AutoMapper;
using CourseLibrary.api.ResourseParemeter;

namespace CourseLibrary.api.Controllers
{
    [ApiController]

    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository,IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<authorDto>> GetAuthors(
           [FromQuery] AuthorResourseParameter authorResourseParameter)
        {
            //throw new Exception("Test exception");
            var authorsFromRepo = _courseLibraryRepository.GetAuthors(authorResourseParameter);
            //var authors = new List<authorDto>();

            //foreach (var author in authorsFromRepo)
            //{
            //    authors.Add(new authorDto()
            //    {
            //        Id = author.Id,
            //        FirstName = $"{author.FirstName}{author.LastName}",
            //        MainCategory = author.MainCategory,
            //        Age = author.DateOfBirth.GetCurrentAge()
            //    });
            
            //AutoMapper replaces above code:


            return Ok(_mapper.Map<IEnumerable<authorDto>>(authorsFromRepo));
        }

        [HttpGet]
        [Route("{authorId}", Name ="GetAuthor")]
        public IActionResult GetAuthor(Guid authorId)
        {
            if (_courseLibraryRepository.AuthorExists(authorId))
            {
                var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

                return Ok(_mapper.Map<authorDto>(authorFromRepo));
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<authorDto> CreateAuthor(authorCreationDto author)
        {
            //this is handle in apiController
            //if(author is null)
            //{
            //    return BadRequest();
            //}
            var authorCreated = _mapper.Map<Author>(author);
            _courseLibraryRepository.AddAuthor(authorCreated);
            _courseLibraryRepository.Save();    

            var authorToReturn = _mapper.Map<authorDto>(authorCreated);
            return CreatedAtRoute("GetAuthor", new
            {
                authorID = authorToReturn.Id
            },authorToReturn);
        }
    }
}