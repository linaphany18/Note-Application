using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApi.Dto;
using NoteApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NoteApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/v1/note")]
    public class NoteController(INoteService noteService) : ControllerBase
    {
        [HttpGet("notes")]
        public async Task<ActionResult?> GetNoteList([FromQuery]NoteQueryParam query)
        {
            // Get user id from claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id") ?? User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (userIdClaim == null) return Forbid();

            var userId = userIdClaim.Value;

            var res = await noteService.GetNoteList(query);

            if (res.success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult?> GetNoteById([FromRoute] int id)
        {
            var res = await noteService.GetNoteById(id);
            if (res.success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult?> CreateNote([FromBody] Note note)
        {
            var res = await noteService.CreateNote(note);

            if (res.success)
            {
                return StatusCode(201, res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult?> UpdateNote([FromRoute] int id, [FromBody] Note note)
        {
            var res = await noteService.UpdateNote(id, note);
            if (res.success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult?> DeleteNote([FromRoute] int id)
        {
            var res = await noteService.DeleteNote(id);
            if (res.success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }
    }
}
