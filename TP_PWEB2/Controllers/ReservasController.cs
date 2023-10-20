using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_PWEB2.Data;
using TP_PWEB2.Models;

namespace TP_PWEB2.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        [Authorize(Roles = "Cliente, Funcionario, Gestor")]
        public async Task<IActionResult> Index()
        {
            List<Reserva> lista = null;

            if (User.IsInRole("Cliente")){
                lista = await _context.Reserva
                    .Include(r => r.Alojamento)
                    .Include(r => r.User)
                    .Include(r => r.Alojamento.imagens)
                    .Include(r => r.avaliacao)
                    .Where(r => r.User.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
                    .ToListAsync();
            }else if(User.IsInRole("Gestor")){
                lista = await _context.Reserva
                    .Include(x => x.avaliacao)
                    .Include(x => x.Alojamento)
                    .Include(x => x.Alojamento.imagens)
                    .Include(x => x.User)
                    .Where(x => x.Alojamento.DonoId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                    .ToListAsync();
            }else if(User.IsInRole("Funcionario")){
                var este_user = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
                lista = await _context.Reserva
                    .Include(x => x.Alojamento)
                    .Include(x => x.Alojamento.Dono)
                    .Include(x => x.Alojamento.imagens)
                    .Include(x => x.User)
                    .Where(x => x.Alojamento.Dono.Empresa == este_user.Empresa)
                    .ToListAsync();
            }

            return View(lista);
        }

        // GET: Reservas/Details/5
        [Authorize(Roles = "Cliente, Funcionario")]
        public async Task<IActionResult> Details(string id, string ativa)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva
                .Include(r => r.Alojamento)
                .Include(r => r.img_dados)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReservaId == id);

            reserva.Avaliacoes_aos_clientes = await _context.Avaliacao
                .Where(x => x.id_cliente == reserva.UserId && x.avalicacao_do_gestor != null)
                .ToListAsync();

            if (ativa != null && ativa == "1"){
                reserva.Reserva_Confirmada = true;
                await _context.SaveChangesAsync();
            }

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        public class InputModel
        {
            public ICollection<Categoria> Lista_categorias { get; set; }
            public ICollection<CategoriaCheck_List> Cat_Check_List { get; set; }
            public IFormFile imagem { get; set; }

        }

        [HttpGet]
        [Authorize(Roles = "Funcionario")]
        public async Task<IActionResult> Entrega_Alojamento(string id)
        {
            InputModel input = new InputModel();

            if (id != null)
            {
                var l_res = _context.Reserva
                    .Include(x => x.Alojamento)
                    .Where(x => x.ReservaId == id && x.Reserva_Confirmada == true)
                    .ToList();

                var res = l_res[0];

                res.Entregue = true;

                var esta_categoria = _context.Categorias
                    .Where(x => x.CategoriaId == res.Alojamento.categoria_Id)
                    .First();

                var lista = _context.CategoryCheckList
                    .Include(x => x.Categoria)
                    .Include(x => x.Check)
                    .Where(x => x.CategoriaId == esta_categoria.CategoriaId)
                    .ToList();

                input.Cat_Check_List = lista;

                _context.SaveChanges();
            }

            return View(input);
        }


        [HttpGet]
        [Authorize(Roles = "Funcionario")]
        public async Task<IActionResult> Recebe_Alojamento(string id)
        {
            InputModel input = new InputModel();

            if (id != null){
                var l_res = _context.Reserva
                    .Include(x => x.Alojamento)
                    .Where(x => x.ReservaId == id)
                    .ToList();

                var res = l_res[0];

                var esta_categoria = _context.Categorias
                    .Where(x => x.CategoriaId == res.Alojamento.categoria_Id)
                    .First();

                var lista = _context.CategoryCheckList
                    .Include(x => x.Categoria)
                    .Include(x => x.Check)
                    .Where(x => x.CategoriaId == esta_categoria.CategoriaId)
                    .ToList();

                input.Cat_Check_List = lista;
            }

            return View(input);
        }

        public string img_root_path() {
            return "\\wwwroot\\imagens_alojamento\\";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Funcionario")]
        public async Task<IActionResult> Recebe_Alojamento([Bind("imagem, Cat_Check_List, Lista_categorias")] InputModel Input,string id)
        {
            Reserva res = null; ;
            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    var l_res = _context.Reserva
                    .Include(x => x.Alojamento)
                    .Where(x => x.ReservaId == id)
                    .ToList();

                    res = l_res[0];
                    res.Recebida = true;
                }

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        Imagens_Alojamento img = new Imagens_Alojamento();

                        img.ReservaId = res.ReservaId;

                        //GUID (unico "id") + extencao do ficheiro
                        string novo_nome_imagem = Convert.ToString(Guid.NewGuid()) + Path.GetExtension(files[i].FileName);

                        var path_final = Path.Combine((Directory.GetCurrentDirectory() + img_root_path()), novo_nome_imagem);

                        img.path = novo_nome_imagem;



                        //alojamento.path_img = novo_nome_imagem;
                        //path_images.Add<string>(novo_nome);



                        using (FileStream fs = System.IO.File.Create(path_final))
                        {
                            files[i].CopyTo(fs);
                            fs.Flush();
                        }

                        res.img_dados.Add(img);
                        //_context.Add<Imagens_Alojamento>(img);
                        //alojamento.imagens.Append<Imagens_Alojamento>(img);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }



        // GET: Reservas/Create
        /*
        public IActionResult Create()
        {
            ViewData["AlojamentoId"] = new SelectList(_context.Alojamento, "AlojamentoId", "AlojamentoId");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservaId,UserId,AlojamentoId,check_in,check_out")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlojamentoId"] = new SelectList(_context.Alojamento, "AlojamentoId", "AlojamentoId", reserva.AlojamentoId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reserva.UserId);
            return View(reserva);
        }

        
        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["AlojamentoId"] = new SelectList(_context.Alojamento, "AlojamentoId", "AlojamentoId", reserva.AlojamentoId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reserva.UserId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        */
    }
}
