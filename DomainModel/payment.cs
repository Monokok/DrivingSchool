namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("payment")]
    public partial class payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? type_id { get; set; }

        public int? cost { get; set; }

        public DateTime? date { get; set; }

        public int? lesson_id { get; set; }

        public int? course_id { get; set; }

        public int? student_id { get; set; }
    }
}
