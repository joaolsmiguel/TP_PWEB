using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_PWEB2.Data;
using TP_PWEB2.Models;

namespace TP_PWEB2.Controllers
{
    public class AlojamentosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AlojamentosController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Alojamentos
        public async Task<IActionResult> Index(string id, string meu)
        {
            if (User.IsInRole("Gestor")){
                if (id != null){
                    var alojamento =  await _context.Alojamento
                        .Where(x => x.AlojamentoId == id)
                        .ToListAsync();
                    
                    alojamento[0].pode_ser_alugado = !alojamento[0].pode_ser_alugado;
                    _context.SaveChanges();
                }
            }

            List<Alojamento> result = null;
            
            if(User.IsInRole("Gestor") && meu != null){
                var este_user_id = User.FindFirstValue(ClaimTypes.NameIdentifier);

                result = await _context.Alojamento
                .Include(x => x.imagens)
                .Include(x => x.Dono)
                .Where(x => x.DonoId == este_user_id)
                .ToListAsync();

            }else{
                result = await _context.Alojamento
                .Include(x => x.imagens)
                .Where(x => x.pode_ser_alugado == true)
                .ToListAsync();
            }

            return View(result);
        }

        // GET: Alojamentos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alojamento = await _context.Alojamento
                .Include(x => x.imagens)
                .Include(x => x.Avaliacoes)
                .FirstOrDefaultAsync(m => m.AlojamentoId == id);

            
            if (alojamento == null)
            {
                return NotFound();
            }

           // var dia_de_hoje = ;
            alojamento.check_in = DateTime.Today;
            alojamento.check_out = DateTime.Today.AddDays(1);

            return View(alojamento);
        }


        bool verifica_se_esta_disponvel(Alojamento aloj)
        {
            bool livre = true;
            var todas_reservas = aloj.Reservas.ToList();        //reservas deste alojamento

            for (int i = 0; i < aloj.Reservas.Count; i++)
            {
                if (todas_reservas[i].check_in >= aloj.check_in && todas_reservas[i].check_in <= aloj.check_out) { livre = false; }

                if(todas_reservas[i].check_in >= aloj.check_out && todas_reservas[i].check_out <= aloj.check_in) { livre = false; }
            }
            return livre;
        }


        //PSOT Details
        [HttpPost]
        [Authorize(Roles = "Cliente, Gestor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([Bind("AlojamentoId,check_in,check_out")] Alojamento alojamento)
        {
            var este_alojamento = await _context.Alojamento
                .AsNoTracking()
                .Include(x => x.Reservas)
                .Include(y => y.imagens)
                .FirstOrDefaultAsync(m => m.AlojamentoId == alojamento.AlojamentoId);

            este_alojamento.check_in = alojamento.check_in;
            este_alojamento.check_out = alojamento.check_out;

            
            if (verifica_se_esta_disponvel(este_alojamento))
            {
                Reserva res = new Reserva();
                AppUser user = await _userManager.GetUserAsync(User);

                res.ReservaId = Convert.ToString(Guid.NewGuid());
                res.AlojamentoId = este_alojamento.AlojamentoId;
                //res.Alojamento = alojamento;
                res.check_in = este_alojamento.check_in;
                res.check_out = este_alojamento.check_out;
                
                res.Reserva_Confirmada = false;
                res.Entregue = false;
                res.Recebida = false;

                //res.User = user;
                res.UserId = user.Id;


                este_alojamento.Reservas.Add(res);
                user.Reservas.Add(res);

                _context.Add<Reserva>(res);

                await _context.SaveChangesAsync();
            }
            return View(este_alojamento);
            //return RedirectToPage("/Index");
        }


        // GET: Alojamentos/Create
        public IActionResult Create()
        {
            var categorias = _context.Categorias
                .Where(x => x.Ativo == true)
                .ToList();

            Alojamento aloj_temp = new Alojamento();
            aloj_temp.lista_categorias = categorias;
            return View(aloj_temp);
        }

        public string img_root_path()
        {
            return "\\wwwroot\\imagens_alojamento\\";
        }

        // POST: Alojamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Gestor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlojamentoId,path_img,descricao,preco,categoria_Id")] Alojamento alojamento)
        {            
            if (ModelState.IsValid)
            {
                //vai buscar o que precisa para saber quem e o dono
                AppUser user = await _userManager.GetUserAsync(User);
                alojamento.DonoId = user.Id;
                alojamento.Dono = user;
                alojamento.pode_ser_alugado = true;

                //adicionar a categoria
                var categoria = await _context.Categorias.Where(x => x.CategoriaId == alojamento.categoria_Id).FirstAsync();
                categoria.alojamentos.Add(alojamento);
                alojamento.Categoria = categoria;

                //cria um id unico para o alojamento
                alojamento.AlojamentoId = Convert.ToString(Guid.NewGuid());

                _context.Add<Alojamento>(alojamento);
                //await _context.SaveChangesAsync();

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0){
                    for (int i = 0; i < files.Count; i++)
                    {
                        Imagens_Alojamento img = new Imagens_Alojamento();

                        img.AlojamentoId = alojamento.AlojamentoId;
                        
                        //GUID (unico "id") + extencao do ficheiro
                        string novo_nome_imagem = Convert.ToString(Guid.NewGuid()) + Path.GetExtension(files[i].FileName);

                        var path_final = Path.Combine( ( Directory.GetCurrentDirectory() + img_root_path() ), novo_nome_imagem);

                        img.path = novo_nome_imagem;



                        //alojamento.path_img = novo_nome_imagem;
                        //path_images.Add<string>(novo_nome);



                        using (FileStream fs = System.IO.File.Create(path_final)){
                            files[i].CopyTo(fs);
                            fs.Flush();
                        }

                        alojamento.imagens.Add(img);
                        //_context.Add<Imagens_Alojamento>(img);
                        //alojamento.imagens.Append<Imagens_Alojamento>(img);
                        await _context.SaveChangesAsync();                        
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(alojamento);
        }

    }
}
