using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdService.Models;

namespace AdService.DTOs
{
    public class UpdateAdDto
    {
        public string Title { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
    }
}
