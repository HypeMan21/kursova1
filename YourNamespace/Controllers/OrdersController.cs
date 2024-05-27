using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YourNamespace.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Перевірка, чи користувач є адміністратором
            var isAdmin = User.IsInRole("Admin");

            // Якщо користувач є адміністратором, відобразити всі замовлення
            if (isAdmin)
            {
                var orders = await _context.Orders.Include(o => o.Car).Include(o => o.User).ToListAsync();
                return View(orders);
            }
            else
            {
                // Якщо користувач не є адміністратором, перенаправити його на домашню сторінку
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Model");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                // Додати замовлення до бази даних
                _context.Add(order);
                await _context.SaveChangesAsync();

                // Отримати адміністраторів
                var admins = await _userManager.GetUsersInRoleAsync("Admin");

                // Надіслати повідомлення адміністраторам про нове замовлення
                foreach (var admin in admins)
                {
                    // Відправити повідомлення адміністраторам про нове замовлення
                    // Наприклад, відправити електронного листа або повідомлення через службу сповіщень
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Model", order.CarId);
            return View(order);
        }
    }
}
