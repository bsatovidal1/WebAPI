using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project1_BrunoVidal.Models
{
    public class Artwork : Auditable, IValidatableObject
    {
        public int ID { get; set; }

        [Display(Name = "Artwork")]
        public string Summary
        {
            get
            {
                return Name + " - " + Completed.ToShortDateString();
            }
        }

        [Display(Name = "Name or Title")]
        [Required(ErrorMessage = "You cannot leave the artwork's name blank.")]
        [StringLength(255, ErrorMessage = "First name cannot be more than 255 characters long.")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Completed")]
        public DateTime Completed { get; set; }

        [Required(ErrorMessage = "You cannot leave the description of the artwork blank.")]
        [StringLength(511, MinimumLength = 20, ErrorMessage = "Description must be a least 20 characters and no more than 511.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Estimated Value")]
        [Required(ErrorMessage = "You cannot leave the estimated value of the artwork blank.")]
        [Range(1.00, 999000.00, ErrorMessage = "Estimated value must be between one and 999 thousand dollars.")]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        public double Value { get; set; }

        [Display(Name = "Type of Art")]
        [Required(ErrorMessage = "Please identify the Type of art.")]
        public int ArtTypeID { get; set; }
        public ArtType ArtType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Completed > DateTime.Today)
            {
                yield return new ValidationResult("The completed artwork date cannot be in the future.", new[] { nameof(Completed) });
            }
        }
    }
}
