using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CreditScore.Models.ViewModel
{
    public class ScoreAPIRequest
    {
            [Required]
            public string Username { get; set; }

            [Required]
            public string IdNumber { get; set; }

    }
}
