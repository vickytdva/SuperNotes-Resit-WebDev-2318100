using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperNotesBackend.Data;
using SuperNotesBackend.Models;

namespace SuperNotesBackend.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        // Get all notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            return await _context.Notes.ToListAsync();
        }

        // Get a specific note by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        // Add a new note
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote([FromBody] Note note)
        {
            note.CreatedAt = DateTime.Now;
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
        }

        // Update an existing note
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] Note note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }

            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete a note
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Search notes by title
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Note>>> SearchNotes([FromQuery] string title)
        {
            var notes = await _context.Notes
                .Where(n => n.Title.Contains(title))
                .ToListAsync();

            return notes;
        }

        // Filter notes by date
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Note>>> FilterNotes([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var notes = _context.Notes.AsQueryable();

            if (startDate.HasValue)
            {
                notes = notes.Where(n => n.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                notes = notes.Where(n => n.CreatedAt <= endDate.Value);
            }

            return await notes.ToListAsync();
        }
    }
}
