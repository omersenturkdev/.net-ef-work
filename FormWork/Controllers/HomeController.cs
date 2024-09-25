using FormWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.IO;

namespace FormWork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string q, string category)
        {
            var entity = Repository.Products;
            if (q is not null)
            {
                entity = entity.Where(p => p.ProductName.ToLower().Contains(q)).ToList();
                ViewBag.Search = q;
            }
            if (category is not null && category != "0")
            {
                entity = entity.Where(p => p.CategoryId == int.Parse(category)).ToList();
            }
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "CategoryName", category);
            return View(entity);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            var extension = Path.GetExtension(imageFile.FileName);
            var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

            if (ModelState.IsValid)
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                product.Image = randomFileName;
                Repository.Products.Add(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var prd = Repository.Products.FirstOrDefault(p => p.ProductId == id);
            if (prd is null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "CategoryName");
            return View(prd);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile? imageFile, int id)
        {

            if (id != product.ProductId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var extension = Path.GetExtension(imageFile.FileName);
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    product.Image = randomFileName;
                }
                Repository.EditProduct(product);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "CategoryName");
            return View(product);
        }

        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
            if (entity is null)
            {
                return NotFound();
            }
            Repository.Products.Remove(entity);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
