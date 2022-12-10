using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace autorepairProj.Models
{
    public class Car
    {
        [Key]
        [Display(Name = "Код автомобиля")]
        public int CarId { get; set; }

        [Display(Name = "Марка автомобиля")]
        [Required(ErrorMessage = "Не указана марка автомобиля")]
        public string Brand { get; set; }

        [Display(Name = "Мощность")]
        [Required(ErrorMessage = "Не указана мощность автомобиля")]
        [Range(1, 2000, ErrorMessage = "Некорректное значение мощности автомобиля")]
        public int? Power { get; set; }

        [Display(Name = "Цвет")]
        [Required(ErrorMessage = "Не указан цвет автомобиля")]
        public string Color { get; set; }

        [Display(Name = "Гос номер")]
        [Required(ErrorMessage = "Не указан гос.номер автомобиля")]
        public string StateNumber { get; set; }

        [Display(Name = "Код владельца авто")]
        [ForeignKey("Owner")]
        [Required(ErrorMessage = "Не указан владелец автомобиля")]
        public int OwnerId { get; set; }

        [Display(Name = "Год выпуска авто")]
        [Required(ErrorMessage = "Не указан год автомобиля")]
        [Range(1700, 2025, ErrorMessage = "Некорректное указан год автомобиля")]
        public int? Year { get; set; }

        [Display(Name = "ВИН номер")]
        [Required(ErrorMessage = "Не указан ВИН автомобиля")]
        public string VIN { get; set; }

        [Display(Name = "Номер двигателя")]
        [Required(ErrorMessage = "Не указан номер двигателя")]
        public string EngineNumber { get; set; }

        [Display(Name = "Дата поступления")]
        [Required(ErrorMessage = "Не указана дата поступления в автомастерскую")]
        [DataType(DataType.Date)]
        public DateTime? AdmissionDate { get; set; }

        public Owner Owner { get; set; }
        public ICollection<Payment> Payments { get; set; }

        public Car()
        {
            Payments = new List<Payment>();
        }

        public override string ToString()
        {
            return "CarId: " + CarId + " | Brand: " + Brand + " | Power: " + Power + " | Color: " + Color +
               " | StateNumber: " + StateNumber + " | OwnerId: " + OwnerId + " | Year: " + Year + " | VIN: " + VIN +
               " | EngineNumber: " + EngineNumber + " | AdmissionDate: " + AdmissionDate;
        }
    }
}
