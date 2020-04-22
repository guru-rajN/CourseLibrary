using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.api.models
{
    public class authorCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string MainCategory { get; set; }

        public ICollection<courseCreationDto> courses { get; set; }
        = new List<courseCreationDto>();
    }
}
