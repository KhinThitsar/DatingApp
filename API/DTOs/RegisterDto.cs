using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {

        [Required]
        public string userName{get;set;}
        [Required]
        [StringLength(8,MinimumLength =4)]
        public string password{get;set;}
        [Required]
        public string gender{get;set;}
        [Required]
        public string knowAs{get;set;}
        [Required]
        public DateTime dateOfBirth{get;set;}
        [Required]
        public string city{get;set;}
        [Required]
        public string country{get;set;}

    }
}