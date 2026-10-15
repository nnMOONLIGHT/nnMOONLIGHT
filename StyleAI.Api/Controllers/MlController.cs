using Microsoft.AspNetCore.Mvc;
using StyleAI.Core.Interfaces;

namespace StyleAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MlController : ControllerBase
{
    private readonly IPythonMlClient _client;

    public MlController(IPythonMlClient client) => _client = client;

    [HttpPost("predict")]
    public async Task<IActionResult> Predict([FromForm] IFormFile file, CancellationToken ct)
    {
        if (file == null) return BadRequest("File required");
        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);
        ms.Position = 0;
        var pred = await _client.PredictAsync(ms, file.FileName, ct);
        return Ok(pred);
    }
}
//одолжила ML для подключения
