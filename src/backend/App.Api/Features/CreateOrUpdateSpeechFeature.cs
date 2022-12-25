﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using App.Data;
using App.Api.Models;

namespace App.Api.Features;

public static class CreateOrUpdateSpeechFeature
{
    public record Response(SpeechModel Model);

    public record Request() : IRequest<Response>;

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILiteDbContext _liteDbContext;

        public Handler(ILiteDbContext liteDbContext)
        {
            _liteDbContext = liteDbContext;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            // TODO
            var model = new SpeechModel();
            return new Response(model);
        }
    }
}