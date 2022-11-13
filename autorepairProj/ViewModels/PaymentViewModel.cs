using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace autorepairProj.ViewModels
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Не указан гос. номер автомобиля")]
        [Display(Name = "Гос.номер машины")]
        public string StateNumberCar { get; set; }

        [Required(ErrorMessage = "Не указана дата платежа")]
        [Display(Name = "Дата платежа")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, 100000, ErrorMessage = "Не указана цена выполненной работы")]
        [Display(Name = "Цена")]
        public int Cost { get; set; }

        [Required(ErrorMessage = "Не указано ФИО механика, который выполнил заданную работу")]
        [Display(Name = "ФИО механика")]
        public string MechanicFIO { get; set; }

        [Required(ErrorMessage = "Не указана проделанная работа над автомобилем")]
        [Display(Name = "Выполненная работа")]
        public string ProgressReport { get; set; }
    }
}
