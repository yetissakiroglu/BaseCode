using Economy.Application.Commands.AppMenus;
using Economy.Application.Queries.AppMenus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Economy.AuthServer.API.Controllers
{
    public class AppMenuController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Create a new AppMenu.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAppMenu([FromBody] CreateAppMenuCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid request data.");
            }

            var result = await _mediator.Send(command);

            if (result == null)
            {
                return BadRequest("App menu could not be created.");
            }

            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        /// <summary>
        /// Get a specific AppMenu by ID.
        /// </summary>
        /// <param name="id">AppMenu ID</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAppMenuQuery(id);
            var menu = await _mediator.Send(query);
            return Ok(menu);
        }

        /// <summary>
        /// Get all AppMenus.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllAppMenuQuery();
            var menus = await _mediator.Send(query);
            return Ok(menus);
        }

        /// <summary>
        /// Update an existing AppMenu.
        /// </summary>
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAppMenu([FromBody] UpdateAppMenuCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateResult(result);
        }

        /// <summary>
        /// Delete an AppMenu by ID.
        /// </summary>
        /// <param name="id">AppMenu ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppMenu(int id)
        {
            var command = new DeleteAppMenuCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound("App menu not found.");
            }

            return NoContent(); // No content status indicates successful deletion
        }
    }
}

