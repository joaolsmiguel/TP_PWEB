using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP_PWEB2.Data;

namespace TP_PWEB2.Areas.Identity.Pages.Account.Manage
{
    public class Gerir_FuncionariosModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public Gerir_FuncionariosModel(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _signInManager = signInManager;
        }

        //public InputModel Input { get; set; }
        /*
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        */

        public IEnumerable<AppUser> Utilizadores { get; private set; }
        //public List<AppUser> Utilizadores { get; private set; }
        //public IEnumerable<AppUser> GetEnumerator() { return (IEnumerable<AppUser>)Utilizadores.GetEnumerator(); }




        private string Get_empresa(string user_id)
        {
            var utilizador = _context.Users.Where(
                s => s.Id == user_id
                ).ToList();
            return utilizador[0].Empresa;
        }

        public IActionResult OnGetAsync(string id)
        {
            if (id != null){
                var user = _userManager.Users.Where(x => x.Id == id).ToList()[0];
                user.EmailConfirmed = !user.EmailConfirmed;
                _context.SaveChanges();
            }

            var user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var lista = _context.Users
                .Where(s => s.Empresa == Get_empresa(user_id))
                .Where(s => s.Id != user_id)
                .ToList();

            Utilizadores = lista;

            return Page();
        }

    }
}
