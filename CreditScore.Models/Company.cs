using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CreditScore.Models
{
    [Table("Company")]
    public class Company
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set;}

        public string Telephone { get; set; }

        public int Balance { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedBy { get; set; }

    }
}
