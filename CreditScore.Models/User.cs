using System;

using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditScore.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Company")]
        public long CompanyId { get; set; }

        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? Createdby { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public long? Modifiedby { get; set; }


    }
}
