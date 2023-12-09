using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTO
{
    public class carDTO
    {
        public int id { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string number { get; set; }
        public int? teacher_id { get; set; }
        public carDTO(car car)
        {
            this.id = car.id;
            this.brand = car.brand;
            this.model = car.model;
            this.number = car.number;
            teacher_id = car.teacher_id;
        }

    }
}
