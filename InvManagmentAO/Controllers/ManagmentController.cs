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
        public async Task<IActionResult> Invetory(string productName, string productBrand,string productCategory, string productImage, int productQuantity ,double productPrice)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == productName);
            if (product.Name == productName)
            {
                ViewBag.Message = "Produsul este deja inregistrat";
            }
            if(productQuantity <= 0)
            {
                ViewBag.Message = "Cantitatea nu poate fi negativa";
            }
            if(productPrice <= 0)
            {
                ViewBag.Message = "Pretul nu poate fi negativ";
            }
            return View();
            var newProduct = new Models.Product
            {
                Name = productName,
                Brand = productBrand,
                Category = productCategory,
                ImagePath = productImage,
                Quantity = productQuantity,
                Price = productPrice
            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return View();

        }
    }
}
