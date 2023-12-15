using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTO
{
    public class categoryDTO
    {
        public int id {  get; set; }
        public string name { get; set; }
        public categoryDTO(category cat)
        {
            this.id = cat.id;
            this.name = cat.name;
        }
        public categoryDTO()
        {
        }
    }
}
