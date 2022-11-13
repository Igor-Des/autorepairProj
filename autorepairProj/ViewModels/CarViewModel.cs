using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace autorepairProj.ViewModels
{
    public class CarViewModel
    {
        public int CarId { get; set; }

        [Required(ErrorMessage = "Не указана марка автомобиля")]
        [Display(Name = "Марка")]
        public string Brand { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Не указана мощность автомобиля")]
        [Display(Name = "Мощность (л.с.)")]
        public int Power { get; set; }

        [Required(ErrorMessage = "Не указан цвет автомобиля")]
        [Display(Name = "Цвет")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Не указан гос. номер автомобиля")]
        [Display(Name = "Гос.номер")]
        public string StateNumber { get; set; }

        [Required(ErrorMessage = "Не указано ФИО владельца автомобиля")]
        [Display(Name = "ФИО владельца")]
        public string OwnerFIO { get; set; }

        [Required]
        [Range(1700, 2024, ErrorMessage = "Не указан год автомобиля")]
        [Display(Name = "Год авто")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Не указан ВИН автомобиля")]
        [Display(Name = "ВИН номер")]
        public string VIN { get; set; }

        [Required(ErrorMessage = "Не указан номер двигателя автомобиля")]
        [Display(Name = "Номер двигателя")]
        public string EngineNumber { get; set; }

        [Required(ErrorMessage = "Не указана дата поступления автомобиля в сервис")]
        [Display(Name = "Дата поступления")]
        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; }
    }
}
