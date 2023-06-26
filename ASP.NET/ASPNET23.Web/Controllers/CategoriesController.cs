using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNET23.Model;
using ASPNET23.Model.Entities;
using ASPNET23.Repository.Categories;
using Microsoft.AspNetCore.Authorization;

namespace ASPNET23.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              var categories = await _categoryRepository.GetAllAsync();
            
            ViewBag.Categories = categories
              .Select(x => new SelectListItem
              {
                  Text = x.Name,
                  Value = x.Id.ToString()
              })
              .ToList();

              return View(categories);
        }

        public async Task<IActionResult> Search(string text = null)
        {
            List<Category> categories;

            if(string.IsNullOrEmpty(text))
            {
                categories = await _categoryRepository.GetAllAsync();
            }
            else
            {
                categories = await _categoryRepository.GetBySearchText(text);
            }

            return PartialView("_CategoriesListPartial", categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Category category)
        {
            if (category != null && !string.IsNullOrEmpty(category.Name))
            {
                await _categoryRepository.SaveCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(nameof(category.Name), "The Name field is required");
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(category.Name))
            {
                try
                {
                    await _categoryRepository.SaveCategoryAsync(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(nameof(category.Name), "The Name field is required");
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        { 
            await _categoryRepository.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
