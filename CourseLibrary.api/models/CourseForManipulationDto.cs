using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using CourseLibrary.API.ValidationAttributes;

namespace CourseLibrary.api.models
{
    [CourseTitleMustBeDifferentFromDescription]
    public abstract class CourseForManipulationDto
    {

        [Required(ErrorMessage ="You should fill the title")]
        [MaxLength(100, ErrorMessage ="The title shouldnt have more then 100 char ")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "The Description shouldnt have more then 1500 char")]
        public virtual string Description { get; set; }
    }
}
