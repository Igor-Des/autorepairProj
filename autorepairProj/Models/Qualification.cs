﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace autorepairProj.Models
{
    public class Qualification
    {
        [Key]
        [Display(Name = "Код")]
        public int QualificationId { get; set; }

        [Display(Name = "Должность")]
        [Required(ErrorMessage = "Не указана должность для механика")]
        public string Name { get; set; }

        [Display(Name = "Изначальная зарплата")]
        [Required(ErrorMessage = "Не указана зарплата")]
        [Range(1, 20000, ErrorMessage = "Некорректное значение зарплаты")]
        public int? Salary { get; set; }

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
