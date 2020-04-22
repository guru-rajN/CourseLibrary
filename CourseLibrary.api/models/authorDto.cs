using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.api.models
{
    public class authorDto
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public string MainCategory { get; set; }
    }
}
