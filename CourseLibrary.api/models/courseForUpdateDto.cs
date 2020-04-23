using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.api.models
{
    public class courseForUpdateDto:CourseForManipulationDto
    {

        [Required(ErrorMessage = "You should fill the Dsscription")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
