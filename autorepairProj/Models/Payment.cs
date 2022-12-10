using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace autorepairProj.Models
{
    public class Payment
    {
        [Key]
        [Display(Name = "Код платежа")]
        public int PaymentId { get; set; }

        [Display(Name = "Код машины")]
        [ForeignKey("Car")]
        [Required(ErrorMessage = "Не указан автомобиль")]
        public int CarId { get; set; }

        [Display(Name = "Дата платежа")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Не указана дата платежа")]
        public DateTime Date { get; set; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Не указана цена")]
        [Range(1, 10000, ErrorMessage = "Некорректное значение цены платежа")]
        public int Cost { get; set; }

        [Display(Name = "Код механика")]
        [ForeignKey("Mechanic")]
        [Required(ErrorMessage = "Не указан механик, которые выполнил работу")]
        public int MechanicId { get; set; }

        [Display(Name = "Выполненная работа")]
        [Required(ErrorMessage = "Не указана выполненная работа")]
        public string ProgressReport { get; set; }

        public Car Car { get; set; }
        public Mechanic Mechanic { get; set; }

        public override string ToString()
        {
            return PaymentId + " " + CarId + " " + Date + " " + Cost + " " + MechanicId + " " + ProgressReport;
        }
    }
}
