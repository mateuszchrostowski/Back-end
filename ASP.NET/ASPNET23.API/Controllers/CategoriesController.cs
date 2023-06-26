using ASPNET23.Dto.Categories;
using ASPNET23.Model.Entities;
using ASPNET23.Repository.Categories;

using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace ASPNET23.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if(category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync();

            if (!categories.Any())
                return NotFound();

            var result = _mapper.Map<List<CategoryOutputDto>>(categories);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryInputDto category)
        {
            if(category == null)
                return BadRequest();
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var newCategory = new Category()
            {
                Name = category.Name
            };

            var result = await _categoryRepository.SaveCategoryAsync(newCategory);
            if(!result)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CategoryInputDto category)
        {
            if (category == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
                return NotFound();

            existingCategory.Name = category.Name;

            var result = await _categoryRepository.SaveCategoryAsync(existingCategory);
            if (!result)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            var result = await _categoryRepository.DeleteCategoryAsync(id);
            if(!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return Ok();
        }
    }
}
