using App.Api.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Api.Controllers;

[ApiController]
[Route("api/speech")]
public class SpeechController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpeechController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("file")]
    public async Task<IActionResult> GetVoiceList(
        [FromQuery] Guid fileId,
        CancellationToken cancellationToken)
    {
        var request = new GettingFileFeature.Request(fileId);
        var response = await _mediator.Send(request, cancellationToken);
        return File(response.Stream, "audio/wav");
    }

    [HttpPost("get-list")]
    public async Task<IActionResult> GetVoiceList(
        [FromBody] GettingSpeechListFeature.Request request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("create-or-update")]
    public async Task<IActionResult> CreateOrUpdateVoice(
        [FromBody] CreateOrUpdateSpeechFeature.Request request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("remove")]
    public async Task<IActionResult> RemoveVoice(
        [FromBody] RemoveSpeechFeature.Request request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);
        return Ok();
    }
}
