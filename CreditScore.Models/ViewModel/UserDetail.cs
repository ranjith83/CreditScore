using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CreditScore.Models
{
   public class UserDetail
    {

       public long Id { get; set; }
       public long CompanyId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long UserId { get; set; }
        
    }
}
