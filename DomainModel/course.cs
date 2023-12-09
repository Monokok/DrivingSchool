namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("course")]
    public partial class course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public course()
        {
            invite_course = new HashSet<invite_course>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int category_id { get; set; }

        public DateTime start_date { get; set; }

        public DateTime end_time { get; set; }

        public int cost { get; set; }

        public int lecture_hours { get; set; }

        public int driving_hours { get; set; }

        public int teacher_id { get; set; }

        public int student_count { get; set; }

        public virtual category category { get; set; }

        public virtual teacher teacher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<invite_course> invite_course { get; set; }
    }
}
