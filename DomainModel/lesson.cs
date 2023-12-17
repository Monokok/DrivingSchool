namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("lesson")]
    public partial class lesson
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public DateTime date { get; set; }

        [StringLength(50)]
        public string topic { get; set; }

        public int type_id { get; set; }

        public int teacher_id { get; set; }

        public int student_id { get; set; }

        public int payment_type_id { get; set; }

        [Required]
        [StringLength(10)]
        public string cost { get; set; }

        public virtual lesson_type lesson_type { get; set; }

        public virtual payment_type payment_type { get; set; }

        public virtual student student { get; set; }

        public virtual teacher teacher { get; set; }

        [Required]
        [StringLength(10)]
        public string status { get; set; }
    }
}
