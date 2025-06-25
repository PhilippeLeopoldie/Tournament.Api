using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dtos;

public record TournamentForManipulationDto
{
    [Required(ErrorMessage = "Tournament Title is a required field.")]
    [MaxLength(60, ErrorMessage = "Maximum length for the Title is 60 characters ")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Tournament StartDate is a required field.")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    //public DateTime EndDate { get; set; }
}
