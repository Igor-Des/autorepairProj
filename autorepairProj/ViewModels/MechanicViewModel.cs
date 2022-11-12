using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace autorepairProj.ViewModels
{
    public class MechanicViewModel
    {
        public int MechanicId { get; set; }

        [Required(ErrorMessage = "Не указано имя механика")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Не указана фамилия механика")]
        [Display(Name = "Фамилия")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Не указано отчество механика")]
        [Display(Name = "Отчество")]
        public string LastName { get; set; }

        [Display(Name = "Должность")]
        public string QualificationName { get; set; }

        [Required]
        [Range(1, 80, ErrorMessage = "Введите корректный стаж работы")]
        [Display(Name = "Стаж работы")]
        public int Experience { get; set; }
    }
}
