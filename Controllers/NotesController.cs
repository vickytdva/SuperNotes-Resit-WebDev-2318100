using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperNotesBackend.Data;
using SuperNotesBackend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SuperNotesBackend.Controllers
{
    [Route("api/notes")]
    [ApiController]
    [Authorize]  // Require authentication for all note operations
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            return User.FindFirst("UserId")?.Value ?? 
                   User.FindFirst(ClaimTypes.Name)?.Value ?? 
                   HttpContext.Session.GetString("User");
        }

        // Get all notes for the current user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            return await _context.Notes
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        // Get a specific note by ID (only if owned by the user)
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            if (note.UserId != userId)
            {
                return Unauthorized("You don't have permission to access this note");
            }

            return note;
        }

        // Add a new note (automatically associated with the current user)
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote([FromBody] Note note)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            note.UserId = userId;
            note.CreatedAt = DateTime.Now;
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
        }

        // Update an existing note (only if owned by the user)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] Note note)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            if (id != note.Id)
            {
                return BadRequest();
            }

            var existingNote = await _context.Notes.FindAsync(id);
            if (existingNote == null)
            {
                return NotFound();
            }

            if (existingNote.UserId != userId)
            {
                return Unauthorized("You don't have permission to update this note");
            }

            note.UserId = userId;  // Ensure the note stays with the same user
            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete a note (only if owned by the user)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            if (note.UserId != userId)
            {
                return Unauthorized("You don't have permission to delete this note");
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Search notes by title (only for the current user)
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Note>>> SearchNotes([FromQuery] string title)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var notes = await _context.Notes
                .Where(n => n.UserId == userId && n.Title.Contains(title))
                .ToListAsync();

            return notes;
        }

        // Filter notes by date (only for the current user)
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Note>>> FilterNotes([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }

            var notes = _context.Notes
                .Where(n => n.UserId == userId)
                .AsQueryable();

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
