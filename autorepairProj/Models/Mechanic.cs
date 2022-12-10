using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace autorepairProj.Models
{
    public class Mechanic
    {
        [Key]
        [Display(Name = "Код")]
        public int MechanicId { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Не указано имя механика")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Не указана фамилия механика")]
        public string MiddleName { get; set; }

        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "Не указано отчество механика")]
        public string LastName { get; set; }

        [Display(Name = "Код должности")]
        [ForeignKey("Qualification")]
        [Required(ErrorMessage = "Не указано должность механика")]
        public int QualificationType { get; set; }

        [Display(Name = "Стаж работы")]
        [Required(ErrorMessage = "Не указан стаж работы")]
        [Range(1, 80, ErrorMessage = "Некорректно указан стаж работы механика")]
        public int? Experience { get; set; }

        public Qualification Qualification { get; set; }
        public ICollection<Payment> Payments { get; set; }

        public Mechanic()
        {
            Payments = new List<Payment>();
        }
        public override string ToString()
        {
            return MechanicId + " " + FirstName + " " + MiddleName + " " + LastName + " " + QualificationType + " " + Experience;
        }
    }
}
