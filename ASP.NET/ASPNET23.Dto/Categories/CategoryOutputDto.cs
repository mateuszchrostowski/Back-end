using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET23.Dto.Categories
{
    public class CategoryOutputDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductDto> Products { get; set; }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameWithDescription { get; set; }
    }
}
