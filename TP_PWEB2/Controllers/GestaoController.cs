using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_PWEB2.Data;
using TP_PWEB2.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TP_PWEB2.Controllers
{
    public class GestaoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public GestaoController(ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public class InputModel
        {
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            public string uNome { get; set; }
            public string pNome { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public string nomeCategoria { get; set; }

            public ICollection<Categoria> Lista_categorias { get; set; }
            public ICollection<CategoriaCheck_List> Cat_Check_List { get; set; }
            public Check_List um_item_lista { get; set; }

            public Categoria uma_categoria { get; set; }

        }


        public ActionResult Index()
        {
            return Redirect("~/");
        }

        //GET: Empresas
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EmpresasAsync(){
            var empresas = await _context.Users
                .Include(x => x.Alojamentos)
                .Where(x => x.dono == true)
                .ToListAsync();

            return View(empresas);
        }

        //GET: Clientes
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Clientes(string id){
            if (id != null){
                var ativa_desativa = await _context.Users
                    .Where(x => x.Id == id)
                    .ToListAsync();
                
                ativa_desativa[0].EmailConfirmed = !ativa_desativa[0].EmailConfirmed;
                await _context.SaveChangesAsync();
            }

            var clientes = await _context.Users
                .Where(x => x.Empresa == null && x.Email != "admin@pweb.pt")
                .ToListAsync();

            return View(clientes);
        }
/*
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit_User(string id){
            var editar_user = await _context.Users
                .Where(x => x.Id == id)
                .ToListAsync();

            return View(editar_user[0]);
        }
*/

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit_User(string id){
            var usr = _context.Users.Where(x => x.Id == id).ToList();
            InputModel Input = new InputModel();
            Input.Email = usr[0].Email;
            Input.PhoneNumber = usr[0].PhoneNumber;
            Input.pNome = usr[0].pNome;
            Input.uNome = usr[0].uNome;
            return View(Input);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit_User([Bind("Password, Email, PhoneNumber, uNome, pNome")] InputModel Input, string id){
            var usr = _context.Users.Where(x => x.Id == id).ToList();
            if (Input.uNome != null){
                usr[0].uNome = Input.uNome;
            }

            if (Input.pNome != null){
                usr[0].pNome = Input.pNome;
            }

            if (Input.PhoneNumber != null){
                usr[0].PhoneNumber = Input.PhoneNumber;
            }

            if (Input.Email != null && Input.Email != usr[0].Email){
                usr[0].Email = Input.Email;
                usr[0].NormalizedEmail = Input.Email.ToUpper();
                usr[0].UserName = Input.Email;
                usr[0].NormalizedUserName = Input.Email.ToUpper();
            }

            if (Input.Password != null){
                usr[0].PasswordHash = null;
                
                if ( (await _userManager.HasPasswordAsync(usr[0])) == false ){
                    await _userManager.AddPasswordAsync(usr[0], Input.Password);
                }

            }

            await _context.SaveChangesAsync();

            return Redirect("/Gestao/Clientes");
        }




        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Categoria(int id)
        {
            if (id != null){
                var act_des = _context.Categorias.Where(x => x.CategoriaId == id).ToList();
                if (act_des.Count > 0){
                    act_des[0].Ativo = !act_des[0].Ativo;
                    _context.SaveChanges();
                }
            }
            var informacao = _context.Categorias
                .Include(x => x.alojamentos)
                .ToList();
            return View(informacao);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Cria_Categoria()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cria_Categoria([Bind("nomeCategoria")] InputModel Input)
        {
            var ja_existe = await _context.Categorias.Where(x => x.nome == Input.nomeCategoria).ToListAsync();
            if (ja_existe.Count == 0)
            {
                await _context.AddAsync(new Categoria { nome = Input.nomeCategoria });
                await _context.SaveChangesAsync();
            }
            return View();
        }



        [HttpGet]
        [Authorize(Roles = "Gestor")]
        public ActionResult Ver_Check_List(string id)
        {
            InputModel input = new InputModel();
            
            if (id != null){
                var esta_categoria = _context.Categorias
                    .Where(x => x.CategoriaId == Convert.ToInt32(id))
                    .First();

                var lista = _context.CategoryCheckList
                    .Include(x => x.Categoria)
                    .Include(x => x.Check)
                    .Where(x => x.CategoriaId == esta_categoria.CategoriaId)
                    .ToList();

                input.Cat_Check_List = lista;
            }

            input.Lista_categorias = _context.Categorias.Where(x => x.Ativo == true).ToList();

            return View(input);
        }



        [HttpGet]
        [Authorize(Roles = "Gestor")]
        public ActionResult Adicionar_Check_List()
        {
            InputModel input = new InputModel();
            input.Lista_categorias = _context.Categorias.Where(x => x.Ativo == true).ToList();

            return View(input);
        }


        [HttpPost]
        [Authorize(Roles = "Gestor")]
        [ValidateAntiForgeryToken]
        public ActionResult Adicionar_Check_List([Bind("um_item_lista,uma_categoria")] InputModel Input)
        {
            var procura_categoria = _context.Categorias.Where(x => x.nome == Input.uma_categoria.nome && x.Ativo == true).First();
            if (procura_categoria != null)
            {
                var conta = _context.Check_List.Count();
                Check_List novo = new Check_List();
                CategoriaCheck_List liga_tab = new CategoriaCheck_List();

                //novo.id = conta + 1;
                novo.texto = Input.um_item_lista.texto;
                novo.Confirmado = false;

                liga_tab.Categoria = procura_categoria;
                liga_tab.Check = novo;

                procura_categoria.CategoriaCheck_List.Add(liga_tab);

                _context.Add<Check_List>(novo);

                _context.SaveChanges();
            }
            return Redirect("/Gestao/Ver_Check_List");
        }











        // POST: Gestao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Gestao/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Gestao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Gestao/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Gestao/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
