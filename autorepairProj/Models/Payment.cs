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
        public int CarId { get; set; }

        [Display(Name = "Дата платежа")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Цена")]
        public int Cost { get; set; }

        [Display(Name = "Код механика")]
        [ForeignKey("Mechanic")]
        public int MechanicId { get; set; }

        [Display(Name = "Выполненная работа")]
        public string ProgressReport { get; set; }

        public Car Car { get; set; }
        public Mechanic Mechanic { get; set; }

        public override string ToString()
        {
            return PaymentId + " " + CarId + " " + Date + " " + Cost + " " + MechanicId + " " + ProgressReport;
        }
    }
}
