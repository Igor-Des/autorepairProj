using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace autorepairProj.Models
{
    public class Qualification
    {
        [Key]
        [Display(Name = "Код должности")]
        public int QualificationId { get; set; }

        [Display(Name = "Наименование должности")]
        public string Name { get; set; }

        [Display(Name = "Изначальная зарплата")]
        public int Salary { get; set; }

        public ICollection<Mechanic> Mechanics { get; set; }

        public Qualification()
        {
            Mechanics = new List<Mechanic>();
        }

        public override string ToString()
        {
            return QualificationId + " " + Name + " " + Salary;
        }
    }
}
