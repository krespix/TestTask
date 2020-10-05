
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTask.Models;

namespace TestTask.Controllers
{
    [Route("api/find-person")]
    [ApiController]
    public class FindFaceController : Controller
    {
        private readonly PersonContext _context;
        public FindFaceController(PersonContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Person>> GetFace ([FromForm]IFormFile file)
        {
            if (file != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)file.Length);
                }

                foreach (var item in _context.Faces)
                {
                    if (item.imageBytes.SequenceEqual(imageData))
                        return await _context.Persons.FindAsync(item.person_id);
                }
            }

            return NotFound();
        }
    }
}
