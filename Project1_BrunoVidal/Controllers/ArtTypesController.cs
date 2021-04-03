using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1_BrunoVidal.Data;
using Project1_BrunoVidal.Models;

namespace Project1_BrunoVidal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtTypesController : ControllerBase
    {
        private readonly ArtContext _context;

        public ArtTypesController(ArtContext context)
        {
            _context = context;
        }

        // GET: api/ArtTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtTypeDTO>>> GetArtTypes()
        {
            return await _context.ArtTypes
                .Select(a => new ArtTypeDTO
                {
                    ID = a.ID,
                    Type = a.Type,
                    RowVersion = a.RowVersion
                })
                .ToListAsync();
        }

        // GET: api/ArtTypes/inc - Include the Artworks collection
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<ArtTypeDTO>>> GetArtTypesInc()
        {
            return await _context.ArtTypes
                .Select(a => new ArtTypeDTO
                {
                    ID = a.ID,
                    Type = a.Type,
                    RowVersion = a.RowVersion,
                    Artworks = a.Artworks.Select(aArtwork => new ArtworkDTO
                    {
                        ID = aArtwork.ID,
                        Name = aArtwork.Name,
                        Completed = aArtwork.Completed,
                        Description = aArtwork.Description,
                        Value = aArtwork.Value
                    }).ToList()
                })
                .ToListAsync();
        }

        // GET: api/ArtTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtTypeDTO>> GetArtType(int id)
        {
            var artTypeDTO = await _context.ArtTypes
                .Select(a => new ArtTypeDTO
                {
                    ID = a.ID,
                    Type = a.Type,
                    RowVersion = a.RowVersion
                })
                .FirstOrDefaultAsync(a => a.ID == id);

            if (artTypeDTO == null)
            {
                return NotFound();
            }

            return artTypeDTO;
        }

        // GET: api/ArtTypes/inc/5
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<ArtTypeDTO>> GetArtTypeInc(int id)
        {
            var artTypeDTO = await _context.ArtTypes
                .Select(a => new ArtTypeDTO
                {
                    ID = a.ID,
                    Type = a.Type,
                    RowVersion = a.RowVersion,
                    Artworks = a.Artworks.Select(aArtwork => new ArtworkDTO
                    {
                        ID = aArtwork.ID,
                        Name = aArtwork.Name,
                        Completed = aArtwork.Completed,
                        Description = aArtwork.Description,
                        Value = aArtwork.Value
                    }).ToList()
                })
                .FirstOrDefaultAsync(a => a.ID == id);

            if (artTypeDTO == null)
            {
                return NotFound();
            }

            return artTypeDTO;
        }

        // PUT: api/ArtTypes/5 - Update
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtType(int id, ArtTypeDTO artTypeDTO)
        {
            if (id != artTypeDTO.ID)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var artTypeToUpdate = await _context.ArtTypes.FindAsync(id);

            //Check that you got it
            if (artTypeToUpdate == null)
            {
                return BadRequest(new { message = "Error: ArtType record not found." });
            }

            artTypeToUpdate.ID = artTypeDTO.ID;
            artTypeToUpdate.Type = artTypeDTO.Type;

            //Put the original RowVersion value in the OriginalValues collection for the entity
            _context.Entry(artTypeToUpdate).Property("RowVersion").OriginalValue = artTypeDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtTypeExists(id))
                {
                    return BadRequest(new { message = "Concurrency Error: Art Type has been Removed." });
                }
                else
                {
                    return BadRequest(new { message = "Concurrency Error: Art Type has been updated by another user.  Back out and try editing the record again." });
                }
            }
        }

        // POST: api/ArtTypes - Insert
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ArtTypeDTO>> PostArtType(ArtTypeDTO artTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ArtType artType = new ArtType
            {
                ID = artTypeDTO.ID,
                Type = artTypeDTO.Type
            };

            try
            {
                _context.ArtTypes.Add(artType);
                await _context.SaveChangesAsync();

                artTypeDTO.ID = artType.ID;
                return CreatedAtAction("GetArtType", new { id = artType.ID }, artType);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }
        }

        // DELETE: api/ArtTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ArtType>> DeleteArtType(int id)
        {
            var artType = await _context.ArtTypes.FindAsync(id);
            if (artType == null)
            {
                return BadRequest(new { message = "Delete Error: Art Type has already been removed." });
            }
            try
            {
                _context.ArtTypes.Remove(artType);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    return BadRequest(new { message = "Delete Error: Remember, you cannot delete a ArtType that has patients assigned." });
                }
                else
                {
                    return BadRequest(new { message = "Delete Error: Unable to delete Doctor. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        private bool ArtTypeExists(int id)
        {
            return _context.ArtTypes.Any(e => e.ID == id);
        }
    }
}
