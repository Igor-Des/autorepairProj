using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace autorepairProj.Models
{
    public class Owner
    {
        [Key]
        [Display(Name = "Код владельца")]
        public int OwnerId { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Не указано имя владельца")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Не указана фамилия владельца")]
        public string MiddleName { get; set; }

        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "Не указано отчество владельца")]
        public string LastName { get; set; }

        [Display(Name = "Номер вод.уд.")]
        [Required(ErrorMessage = "Не указан номер вод.уд.")]
        [Range(1000, 9999, ErrorMessage = "Некорректно указан номер вод.уд.(Номер состоит из четырехзначного числа)")]
        public int? DriverLicenseNumber { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Не указан адрес")]
        public string Address { get; set; }

        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Не указан телефон")]
        [Range(1, 99999999999, ErrorMessage = "Некорректно указан телефон")]
        public int? Phone { get; set; }
        public ICollection<Car> Cars { get; set; }

        public Owner()
        {
            Cars = new List<Car>();
        }

        public override string ToString()
        {
            return OwnerId + " " + FirstName + " " + MiddleName + " " + LastName + " " + DriverLicenseNumber + " " + Address + " " + Phone;
        }
    }
}
