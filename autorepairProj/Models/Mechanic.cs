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
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string MiddleName { get; set; }

        [Display(Name = "Отчество")]
        public string LastName { get; set; }

        [Display(Name = "Код должности")]
        [ForeignKey("Qualification")]
        public int QualificationType { get; set; }

        [Display(Name = "Стаж работы")]
        public int Experience { get; set; }

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
