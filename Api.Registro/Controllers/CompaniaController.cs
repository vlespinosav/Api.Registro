using Api.Registro.BaseDatos;
using Api.Registro.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Registro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniaController : ControllerBase
    {

        private readonly RegistroDBContext _context;

        public CompaniaController(RegistroDBContext context)
        {
            _context = context;
        }


      /// <summary>
      /// Metodo que Obtiene las comañias de la Base Datos
      /// </summary>
      /// <returns></returns>
        [HttpGet("ObtenerCompanias")]
        public async Task<ActionResult<IEnumerable<Compania>>> ObtenerCompania()
        {
            return await _context.DetalleCompanias.ToListAsync();
        }

        /// <summary>
        /// Metodo que obtiene una Compania por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "BuscarCompaniaId")]
        public async Task<ActionResult<Compania>> GetCompania(int id)
        {

            var compania = await _context.DetalleCompanias.FindAsync(id);

            if (compania == null)
            {
                return NotFound();
            }

            return compania;
        }

        /// <summary>
        /// Metodo que Gurda una Compañia en la Base de Datos
        /// </summary>
        /// <param name="companiaModel"></param>
        /// <returns></returns>
        [HttpPost("GuardarCompania")]
        public async Task<ActionResult<Compania>> GuardarCompania(Compania companiaGuardar)
        {
            try
            {
                //Validamos si ya existe la persona en comapañia
                var existeCompania = _context.DetalleCompanias.Where(p => p.NombreContacto == companiaGuardar.NombreContacto && p.Telefono == companiaGuardar.Telefono).FirstOrDefault();

                if (existeCompania == null)
                {
                    _context.Add(companiaGuardar);
                    await _context.SaveChangesAsync();
                    return Ok(companiaGuardar);
                }
                else
                {
                    return BadRequest();
                }
               
                
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message.ToString());
            }

        }

        /// <summary>
        /// Metodo que Actualiza una Compania en la Base Datos
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companiaEditar"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name ="EditarCompania")]
        public async Task<IActionResult> ActualizaPokemon(int id, Compania companiaEditar)
        {
            if (id != companiaEditar.Id)
            {
                return BadRequest();
            }

            _context.Entry(companiaEditar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteCompania(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Metodo que borra una Compania de la Base DatosS
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name ="BorraCompania")]
        public async Task<IActionResult> BorrarPokemon(int id)
        {
            var companiaBorrar = await _context.DetalleCompanias.FindAsync(id);
            if (companiaBorrar == null)
            {
                return NotFound();
            }

            _context.DetalleCompanias.Remove(companiaBorrar);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool ExisteCompania(int id)
        {
            return _context.DetalleCompanias.Any(e => e.Id == id);
        }


    }
}
