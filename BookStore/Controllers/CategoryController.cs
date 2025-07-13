using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Category> response = categoryRepository.GetAll().ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Category category)
        {
            categoryRepository.Add(category);
            categoryRepository.Save();
            return Ok(category); 
        }

        [HttpPut]
        public async Task<IActionResult> Put(Category category)
        {
            categoryRepository.Update(category);
            categoryRepository.Save();
            return Ok(category);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Category category)
        {
            categoryRepository.Remove(category);
            return Ok();
        }
    }
}
