using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AdService.Models;

namespace AdService.DTOs
{
    public class DeleteAdDto
    {
        [Required]
        public int Id { get; set; }
    }
}