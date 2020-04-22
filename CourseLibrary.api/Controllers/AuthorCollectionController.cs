using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CourseLibrary.API.Services;
using CourseLibrary.api.models;
using CourseLibrary.API.Entities;
using AutoMapper;
using CourseLibrary.API.Helpers;
using CourseLibrary.api.ResourseParemeter;

namespace CourseLibrary.api.Controllers
{
    [ApiController]
    [Route("api/authorcollections")]
    public class AuthorCollectionController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorCollectionController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet("{ids}", Name = "GetAuthorCollections")]
        public IActionResult GetAuthorCollections(
            [FromRoute] 
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if(ids == null)
            {
                return BadRequest();
            }
            var authorEntities = _courseLibraryRepository.GetAuthors(ids);

            if(ids.Count() != authorEntities.Count())
            {
                return NotFound();
            }

            var authorToReturn = _mapper.Map<IEnumerable<authorDto>>(authorEntities);

            return Ok(authorToReturn);
        }

        [HttpPost]
        public ActionResult<IEnumerable<authorDto>> CreateAuthorCollection(IEnumerable<authorCreationDto> authors)
        {

            var authorCreated = _mapper.Map<IEnumerable<Author>>(authors);

            foreach (var author in authorCreated)
            {
                _courseLibraryRepository.AddAuthor(author);

            }

            _courseLibraryRepository.Save();
            var authorCollectionToReturn = _mapper.Map<IEnumerable<authorDto>>(authorCreated);

            var idsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetAuthorCollections", new { ids = idsString},authorCollectionToReturn);
        }

        [HttpOptions]
        public IActionResult getAllowDetails()
        {
            Response.Headers.Add("Allow", "Get,Post,Options");
            return Ok();
        }
    }
}