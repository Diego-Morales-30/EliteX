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
    public class ContratoesController : Controller
    {
        private readonly RegistroDfContext _context;

        public ContratoesController(RegistroDfContext context)
        {
            _context = context;
        }

        // GET: Contratoes
        public async Task<IActionResult> Index()
        {
            var registroDfContext = _context.Contratos.Include(c => c.IdEmpleadoNavigation).Include(c => c.IdTipoHorarioNavigation);
            return View(await registroDfContext.ToListAsync());
        }

        // GET: Contratoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contrato = await _context.Contratos
                .Include(c => c.IdEmpleadoNavigation)
                .Include(c => c.IdTipoHorarioNavigation)
                .FirstOrDefaultAsync(m => m.IdContrato == id);
            if (contrato == null)
            {
                return NotFound();
            }

            return View(contrato);
        }

        // GET: Contratoes/Create
        public IActionResult Create()
        {
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado");
            ViewData["IdTipoHorario"] = new SelectList(_context.TipoHorarios, "IdTipoHorario", "IdTipoHorario");
            return View();
        }

        // POST: Contratoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdContrato,IdEmpleado,IdTipoHorario,Sueldo")] Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contrato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", contrato.IdEmpleado);
            ViewData["IdTipoHorario"] = new SelectList(_context.TipoHorarios, "IdTipoHorario", "IdTipoHorario", contrato.IdTipoHorario);
            return View(contrato);
        }

        // GET: Contratoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null)
            {
                return NotFound();
            }
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", contrato.IdEmpleado);
            ViewData["IdTipoHorario"] = new SelectList(_context.TipoHorarios, "IdTipoHorario", "IdTipoHorario", contrato.IdTipoHorario);
            return View(contrato);
        }

        // POST: Contratoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdContrato,IdEmpleado,IdTipoHorario,Sueldo")] Contrato contrato)
        {
            if (id != contrato.IdContrato)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contrato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContratoExists(contrato.IdContrato))
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
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", contrato.IdEmpleado);
            ViewData["IdTipoHorario"] = new SelectList(_context.TipoHorarios, "IdTipoHorario", "IdTipoHorario", contrato.IdTipoHorario);
            return View(contrato);
        }

        // GET: Contratoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contrato = await _context.Contratos
                .Include(c => c.IdEmpleadoNavigation)
                .Include(c => c.IdTipoHorarioNavigation)
                .FirstOrDefaultAsync(m => m.IdContrato == id);
            if (contrato == null)
            {
                return NotFound();
            }

            return View(contrato);
        }

        // POST: Contratoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato != null)
            {
                _context.Contratos.Remove(contrato);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContratoExists(int id)
        {
            return _context.Contratos.Any(e => e.IdContrato == id);
        }
    }
}
