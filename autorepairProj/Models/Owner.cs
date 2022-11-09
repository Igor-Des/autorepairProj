﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace autorepairProj.Models
{
    public class Owner
    {
        [Key]
        [Display(Name = "Код владельца авто")]
        public int OwnerId { get; set; }

        [Display(Name = "Имя владельца")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия владельца")]
        public string MiddleName { get; set; }

        [Display(Name = "Отчество владельца")]
        public string LastName { get; set; }

        [Display(Name = "Номер вод.уд.")]
        public int DriverLicenseNumber { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Телефон")]
        public int Phone { get; set; }
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
