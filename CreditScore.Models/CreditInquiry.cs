using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CreditScore.Models
{
	[Table("CreditInquires")]
	public class CreditInquires
	{
		[Key]
		public long ID { get; set; }
		public long UserID { get; set; }
		public long CustomerID { get; set; }
		public long BatchID { get; set; }
		public long Score { get; set; }
		public bool Success { get; set; }
		public string ReasonCode1 { get; set; }
		public string Description1 { get; set; }
		public string ReasonCode2 { get; set; }
		public string Description2 { get; set; }
		public string ReasonCode3 { get; set; }
		public string Description3 { get; set; }
		public string ReasonCode4 { get; set; }
		public string Description4 { get; set; }
		public string ReasonCode5 { get; set; }
		public string Description5 { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreatedDate { get; set; }
		public long CreatedBy { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime ModifiedDate { get; set; }
		public long ModifiedBy { get; set; }

		
	}
}
