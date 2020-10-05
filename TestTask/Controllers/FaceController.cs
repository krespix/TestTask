using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestTask.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace TestTask.Controllers
{
    [Route("api/person/{person_id:int}/face")]
    [ApiController]
    public class FaceController : Controller
    {
        private readonly PersonContext _context;

        public FaceController(PersonContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Face>>> GetFaces(int person_id)
        {
            bool predicate(Face face)
            {
                return face.person_id == person_id;
            }
    
            var faces = await _context.Faces.ToListAsync();
            List<Face> result = faces.FindAll(predicate);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Face>> GetFace(int id)
        {
            var face = await _context.Faces.FindAsync(id);

            if (face == null)
            {
                return NotFound();
            }

            return face;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFace([FromRoute] int id, [FromRoute] int person_id,
            [FromForm] IFormFile file)
        {
            if (!(await PersonExists(person_id)))
            {
                return BadRequest();
            }

            if (!string.Equals(file.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest();
            }

            if (file != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int) file.Length);
                }

                Face face = new Face()
                    {id = id, person_id = person_id, imageName = file.FileName, imageBytes = imageData};
                _context.Entry(face).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await FaceExists(id)))
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


        [HttpPost]
        public async Task<ActionResult<Face>> PostFace([FromRoute] int person_id, [FromForm] IFormFile file)
        {
            if (!string.Equals(file.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest();
            }

            if (file != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int) file.Length);
                }

                _context.Faces.Add(
                    new Face() {person_id = person_id, imageName = file.FileName, imageBytes = imageData});
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Face>> DeleteFace(int id)
        {
            var face = await _context.Faces.FindAsync(id);
            if (face == null)
            {
                return NotFound();
            }

            _context.Faces.Remove(face);
            await _context.SaveChangesAsync();

            return face;
        }

        private Task<bool> FaceExists(int id)
        {
            return _context.Faces.AnyAsync(e => e.id == id);
        }

        private Task<bool> PersonExists(int id)
        {
            return _context.Persons.AnyAsync(e => e.id == id);
        }
    }
}