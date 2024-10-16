using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using projFront.Models;

namespace projFront.Controllers
{
    [Authorize(Roles = "Admin, Operador")]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["DireitoUsuario"] = IdentificaDireitoUsuario();

            ViewData["PaginaSelecionada"] = "Home";
            return View();
        }

        private string IdentificaDireitoUsuario()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string direito = "Operador";

            if (userId != null)
            {
                var userRoles = _userManager.GetRolesAsync(_userManager.FindByIdAsync(userId).GetAwaiter().GetResult());

                if (userRoles.Result.Count != 0)
                    direito = userRoles.Result[0]; 
            }

            return direito;
        }

    }
}