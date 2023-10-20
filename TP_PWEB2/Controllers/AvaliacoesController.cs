using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Routing;
using TP_PWEB2.Data;
using TP_PWEB2.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TP_PWEB2.Controllers
{
    public class AvaliacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvaliacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Avaliacoes
        [Authorize(Roles = "Cliente, Funcionario, Gestor")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Avaliacao.ToListAsync());
        }

        // GET: Avaliacoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacao = await _context.Avaliacao
                .FirstOrDefaultAsync(m => m.AvaliacaoId == id);
            if (avaliacao == null)
            {
                return NotFound();
            }

            return View(avaliacao);
        }

        // GET: Avaliacoes/Create
        [HttpGet]
        public async Task<IActionResult> CreateAsync([FromQuery(Name = "")]string reserva_id)
        {
            string get_data = Url.ActionContext.RouteData.Values.Values.Last().ToString();

            Avaliacao avalia = new Avaliacao();
            avalia.AvaliacaoId = Convert.ToString(Guid.NewGuid());

            var a_reserva = await _context.Reserva
                .Include(x => x.avaliacao)
                .FirstOrDefaultAsync(x => x.ReservaId == reserva_id);

            avalia.id_cliente = a_reserva.UserId;
            avalia.reservaId = a_reserva.ReservaId;


            _context.Add<Avaliacao>(avalia);
            await _context.SaveChangesAsync();

            return View( avalia );
        }

        // POST: Avaliacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AvaliacaoId,avaliacao_do_cliente,avalicacao_do_gestor,id_cliente,id_gestor,reservaId")] Avaliacao avaliacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(avaliacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(avaliacao);
        }

        // GET: Avaliacoes/Edit/5
        [Authorize(Roles = "Cliente, Gestor")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var l_avaliacao = await _context.Avaliacao.Where(x => x.AvaliacaoId == id).ToListAsync();
            var l_reserva = await _context.Reserva.Where(x => x.ReservaId == id).ToListAsync();

            Avaliacao avaliacao = null;
            Reserva reserva = null;

            if (l_avaliacao.Count > 0){
                avaliacao = l_avaliacao[0];
            }

            if (l_reserva.Count > 0){
                reserva = l_reserva[0];
            }

            if (avaliacao == null && reserva != null){
                if (id == reserva.ReservaId)
                {
                    Avaliacao ava = new Avaliacao();
                    ava.AvaliacaoId = Convert.ToString(Guid.NewGuid());
                    ava.reservaId = reserva.ReservaId;
                    if (User.IsInRole("Cliente")){
                        ava.id_cliente = reserva.UserId;
                    }else if (User.IsInRole("Gestor")){
                        ava.id_gestor = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    }
                    
                    ava.AlojamentoId = reserva.AlojamentoId;

                    reserva.avaliacaoId = ava.AvaliacaoId;

                    _context.Add(ava);
                    await _context.SaveChangesAsync();

                    return View(ava);
                }
            }else if(avaliacao != null && reserva== null)
            {
                if (id == avaliacao.AvaliacaoId)
                {
                    return View(avaliacao);
                }
            }


            return View(avaliacao);
        }

        // POST: Avaliacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente, Gestor")]
        public async Task<IActionResult> Edit(string id, [Bind("AvaliacaoId,avaliacao_do_cliente,avalicacao_do_gestor,id_cliente,id_gestor,reservaId")] Avaliacao avaliacao)
        {
            var query_avaliacao = await _context.Avaliacao
                .Where( s => s.AvaliacaoId == id || s.reservaId == id )
                .AsNoTracking()
                .ToListAsync();

            Avaliacao esta_avaliacao = null;

            if (ModelState.IsValid){
                if (query_avaliacao.Count == 0){
                    Avaliacao nova = new Avaliacao();

                    if (avaliacao.avalicacao_do_gestor != null){
                        nova.id_gestor = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        nova.avalicacao_do_gestor = avaliacao.avalicacao_do_gestor;
                    }
                    if (avaliacao.avaliacao_do_cliente != null)
                    {
                        nova.id_cliente = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        nova.avaliacao_do_cliente = avaliacao.avaliacao_do_cliente;
                    }
                    nova.AvaliacaoId = Convert.ToString(Guid.NewGuid());
                    nova.reservaId = id;

                    _context.AddAsync<Avaliacao>(nova);
                }
                else{
                    esta_avaliacao = query_avaliacao[0];

                    if (avaliacao.avaliacao_do_cliente != null)
                    {
                        esta_avaliacao.avaliacao_do_cliente = avaliacao.avaliacao_do_cliente;
                    }else if (avaliacao.avalicacao_do_gestor != null){
                        esta_avaliacao.avalicacao_do_gestor = avaliacao.avalicacao_do_gestor;
                        esta_avaliacao.id_gestor = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    }
                    _context.Update(esta_avaliacao);
                }
            await _context.SaveChangesAsync();
            }

            

            return View(avaliacao);
        }

        // GET: Avaliacoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacao = await _context.Avaliacao
                .FirstOrDefaultAsync(m => m.AvaliacaoId == id);
            if (avaliacao == null)
            {
                return NotFound();
            }

            return View(avaliacao);
        }

        // POST: Avaliacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var avaliacao = await _context.Avaliacao.FindAsync(id);
            _context.Avaliacao.Remove(avaliacao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvaliacaoExists(string id)
        {
            return _context.Avaliacao.Any(e => e.AvaliacaoId == id);
        }
    }
}
