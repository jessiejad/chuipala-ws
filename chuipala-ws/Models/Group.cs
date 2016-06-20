namespace chuipala_ws.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Group")]
    public partial class Group
    {
        [Display(Name = "Référence")]
        public int GroupID { get; set; }
        public virtual ICollection<GroupStudents> GroupStudents { get; set; }
        public virtual ICollection<GroupClasses> GroupClasses { get; set; }

        [Display(Name = "Nom")]
        public string Name { get; set; }

        /*public int NbStudents
        {
            get
            {
                return (this.GroupStudents.Count);
            }
        }*/
    }
}