namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("car")]
    public partial class car
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(50)]
        public string brand { get; set; }

        [StringLength(50)]
        public string model { get; set; }
        [StringLength(50)]
        public string car_type { get; set; }

        [StringLength(8)]
        public string number { get; set; }

        public int? teacher_id { get; set; }

        public virtual teacher teacher { get; set; }
    }
}
