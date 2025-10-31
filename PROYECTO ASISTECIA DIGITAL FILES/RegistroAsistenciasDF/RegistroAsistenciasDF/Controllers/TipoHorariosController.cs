using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegistroAsistenciasDF.Models;

namespace RegistroAsistenciasDF.Controllers
{
    public class TipoHorariosController : Controller
    {
        private readonly RegistroDfContext _context;

        public TipoHorariosController(RegistroDfContext context)
        {
            _context = context;
        }

        // GET: TipoHorarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoHorarios.ToListAsync());
        }

        // GET: TipoHorarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoHorario = await _context.TipoHorarios
                .FirstOrDefaultAsync(m => m.IdTipoHorario == id);
            if (tipoHorario == null)
            {
                return NotFound();
            }

            return View(tipoHorario);
        }

        // GET: TipoHorarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoHorarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoHorario,Descripcion")] TipoHorario tipoHorario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoHorario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoHorario);
        }

        // GET: TipoHorarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoHorario = await _context.TipoHorarios.FindAsync(id);
            if (tipoHorario == null)
            {
                return NotFound();
            }
            return View(tipoHorario);
        }

        // POST: TipoHorarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoHorario,Descripcion")] TipoHorario tipoHorario)
        {
            if (id != tipoHorario.IdTipoHorario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoHorario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoHorarioExists(tipoHorario.IdTipoHorario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tipoHorario);
        }

        // GET: TipoHorarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoHorario = await _context.TipoHorarios
                .FirstOrDefaultAsync(m => m.IdTipoHorario == id);
            if (tipoHorario == null)
            {
                return NotFound();
            }

            return View(tipoHorario);
        }

        // POST: TipoHorarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoHorario = await _context.TipoHorarios.FindAsync(id);
            if (tipoHorario != null)
            {
                _context.TipoHorarios.Remove(tipoHorario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoHorarioExists(int id)
        {
            return _context.TipoHorarios.Any(e => e.IdTipoHorario == id);
        }
    }
}
