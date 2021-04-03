﻿using System;
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
    public class ArtworksController : ControllerBase
    {
        private readonly ArtContext _context;

        public ArtworksController(ArtContext context)
        {
            _context = context;
        }

        // GET: api/Artworks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtworkDTO>>> GetArtworks()
        {
            return await _context.Artworks
                .Include(p => p.ArtType)
                .Select(a => new ArtworkDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Completed = a.Completed,
                    Description = a.Description,
                    Value = a.Value,
                    ArtTypeID = a.ArtTypeID,
                    ArtType = new ArtTypeDTO
                    {
                        ID = a.ArtType.ID,
                        Type = a.ArtType.Type
                    },
                    RowVersion = a.RowVersion
                })
                .ToListAsync();
        }

        // GET: api/Artworks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtworkDTO>> GetArtwork(int id)
        {
            var artworkDTO = await _context.Artworks
                .Include(p => p.ArtType)
                .Select(a => new ArtworkDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Completed = a.Completed,
                    Description = a.Description,
                    Value = a.Value,
                    ArtTypeID = a.ArtTypeID,
                    ArtType = new ArtTypeDTO
                    {
                        ID = a.ArtType.ID,
                        Type = a.ArtType.Type
                    },
                    RowVersion = a.RowVersion
                })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (artworkDTO == null)
            {
                return NotFound();
            }

            return artworkDTO;
        }

        // GET: api/ArtworkByArtType
        [HttpGet("ByArtType/{id}")]
        public async Task<ActionResult<IEnumerable<ArtworkDTO>>> GetArtworkByArtType(int id)
        {
            return await _context.Artworks
                .Include(e => e.ArtType)
                .Where(e => e.ArtTypeID == id)
                .Select(a => new ArtworkDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Completed = a.Completed,
                    Description = a.Description,
                    Value = a.Value,
                    ArtTypeID = a.ArtTypeID,
                    ArtType = new ArtTypeDTO
                    {
                        ID = a.ArtType.ID,
                        Type = a.ArtType.Type
                    },
                    RowVersion = a.RowVersion
                })
                .ToListAsync();
        }

        // PUT: api/Artworks/5 - Update
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtwork(int id, ArtworkDTO artworkDTO)
        {
            if (id != artworkDTO.ID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artworkToUpdate = await _context.Artworks.FindAsync(id);

            //Check that you got it
            if (artworkToUpdate == null)
            {
                return BadRequest(new { message = "Error: Artwork record not found." });
            }

            artworkToUpdate.ID = artworkDTO.ID;
            artworkToUpdate.Name = artworkDTO.Name;
            artworkToUpdate.Completed = artworkDTO.Completed;
            artworkToUpdate.Description = artworkDTO.Description;
            artworkToUpdate.Value = artworkDTO.Value;
            artworkToUpdate.ArtTypeID = artworkDTO.ArtTypeID;


            //Put the original RowVersion value in the OriginalValues collection for the entity
            _context.Entry(artworkToUpdate).Property("RowVersion").OriginalValue = artworkDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtworkExists(id))
                {
                    return BadRequest(new { message = "Concurrency Error: Artwork has been Removed." });
                }
                else
                {
                    return BadRequest(new { message = "Concurrency Error: Artwork has been updated by another user.  Back out and try editing the record again." });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Name, Completed and ArtTypeID combination." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }

        }

        // POST: api/Artworks - Insert
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ArtworkDTO>> PostArtwork(ArtworkDTO artworkDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Artwork artwork = new Artwork
            {
                ID = artworkDTO.ID,
                Name = artworkDTO.Name,
                Completed = artworkDTO.Completed,
                Description = artworkDTO.Description,
                Value = artworkDTO.Value,
                ArtTypeID = artworkDTO.ArtTypeID
            };

            try
            {
                _context.Artworks.Add(artwork);
                await _context.SaveChangesAsync();

                //Assign Database Generated values back into the DTO
                artworkDTO.ID = artwork.ID;
                artworkDTO.RowVersion = artwork.RowVersion;

                return CreatedAtAction(nameof(GetArtwork), new { id = artwork.ID }, artworkDTO);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Name, Completed and ArtTypeID combination." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // DELETE: api/Artworks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Artwork>> DeleteArtwork(int id)
        {
            var artwork = await _context.Artworks.FindAsync(id);
            if (artwork == null)
            {
                return BadRequest(new { message = "Delete Error: Artwork has already been removed." });
            }
            try
            {
                _context.Artworks.Remove(artwork);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Artwotk." });
            }
        }

        private bool ArtworkExists(int id)
        {
            return _context.Artworks.Any(e => e.ID == id);
        }
    }
}
