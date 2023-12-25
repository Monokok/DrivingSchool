namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class lesson_typeDTO
    {
        public lesson_typeDTO(lesson_type type)
        {
            id = type.id;
            name = type.name;
        }
        public int id { get; set; }
        public string name { get; set; }
    }
}
