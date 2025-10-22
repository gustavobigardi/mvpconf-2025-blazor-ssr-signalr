using System.Text.Json;

using BlazorServerSentEvents.Model;
using BlazorServerSentEvents.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorServerSentEvents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Message message)
        {
            _messageService.NotifyNewMessageAvailable(message);

            return Ok();
        }

        [HttpGet]
        public async Task Get()
        {
            HttpContext.Response.Headers.Append("Content-Type", "text/event-stream");

            _messageService.RegisterListener(this);

            try
            {
                while (!HttpContext.RequestAborted.IsCancellationRequested)
                {
                    Message message = await _messageService.WaitForNewMessageAsync(this, HttpContext.RequestAborted);

                    await HttpContext.Response.WriteAsync("data: ");
                    await JsonSerializer.SerializeAsync(HttpContext.Response.Body, message);
                    await HttpContext.Response.WriteAsync("\n\n");
                    await HttpContext.Response.Body.FlushAsync();
                }
            }
            finally
            {
                _messageService.UnregisterListener(this);
            }
        }
    }
}
