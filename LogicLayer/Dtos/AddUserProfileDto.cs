using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Dtos
{
    public class AddUserProfileDto
    {
        [Required]
        [StringLength(20)]
        [MinLength(2)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(20)]
        [MinLength(2)]
        public string Lastname { get; set; }
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string PersonalNumber { get; set; }
        [Required]

        public int UserId { get; set; }
    }
}
