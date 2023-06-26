using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET23.Dto.Categories
{
    public class CategoryInputDto
    {
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
    }
}
