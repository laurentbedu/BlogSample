#nullable disable
using BlogSampleApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSampleApi.Controllers
{
    public class RootController<M> : ControllerBase where M : Model
    {
        protected readonly AppDbContext _context;

        public RootController(AppDbContext context)
        {
            _context = context;
        }

        private DbSet<M> GetModels()
        {
            return (DbSet<M>)(typeof(AppDbContext).GetProperty(typeof(M).Name + "s").GetValue(_context, null));
        }

        public virtual async Task<ActionResult<IEnumerable<M>>> GetAll()
        {
            return await GetModels().ToListAsync();
        }

        public virtual async Task<ActionResult<M>> GetOne(int id)
        {
            var entity = await GetModels().FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        public virtual async Task<IActionResult> PutOne(int id, M entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(id))
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

        public virtual async Task<ActionResult<M>> PostOne(M entity)
        {
            GetModels().Add(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        public virtual async Task<IActionResult> DeleteOne(int id)
        {
            var entity = await GetModels().FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            GetModels().Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntityExists(int id)
        {
            return GetModels().Any(e => e.Id == id);
        }
    }
}
