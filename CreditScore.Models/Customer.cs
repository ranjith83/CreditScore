using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CreditScore.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public long Cell { get; set; }
        public string IDNumber { get; set; }
        public long CompanyId { get; set; }
        public string Address { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }

    }

   
}
