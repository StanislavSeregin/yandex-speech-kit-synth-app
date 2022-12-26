using App.Data;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace App.Api.Features;

public static class GettingFileFeature
{
    public record Response(Stream Stream);

    public record Request(Guid FileId) : IRequest<Response>;

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILiteDbContext _liteDbContext;

        public Handler(ILiteDbContext liteDbContext)
        {
            _liteDbContext = liteDbContext;
        }

        public Task<Response> Handle(Request request, CancellationToken _)
        {
            var storage = _liteDbContext.GetStorage();
            var stream = storage.OpenRead(request.FileId);
            return Task.FromResult(new Response(stream));
        }
    }
}
