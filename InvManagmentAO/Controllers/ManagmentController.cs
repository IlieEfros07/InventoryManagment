using InvManagmentAO.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace InvManagmentAO.Controllers
{
    public class ManagmentController : Controller
    {
        private readonly AppDbContext _context;
        public ManagmentController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Inventory()
        {
            ViewData["Title"] = "Agricultural Inventory Management";
            var products = _context.Products.ToList();
            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(string productName, string productBrand, string productDescription,
            string productCategory, string productImage, int productQuantity, double productPrice)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == productName);
            if (existingProduct != null)
            {
                TempData["Message"] = "Produsul este deja înregistrat";
                return RedirectToAction("Inventory");
            }
            if (productQuantity <= 0)
            {
                TempData["Message"] = "Cantitatea nu poate fi negativă sau zero";
                return RedirectToAction("Inventory");
            }
            if (productPrice <= 0)
            {
                TempData["Message"] = "Prețul nu poate fi negativ sau zero";
                return RedirectToAction("Inventory");
            }
            var newProduct = new Models.Product
            {
                Name = productName,
                Brand = productBrand,
                Category = productCategory ?? "",
                ImagePath = productImage ?? "",
                Quantity = productQuantity,
                Price = productPrice,
                Description = productDescription ?? ""
            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Produs adăugat cu succes";
            return RedirectToAction("Inventory");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }

            TempData["Success"] = "Produs șters cu succes";
            return RedirectToAction("Inventory");
        }
    }
}