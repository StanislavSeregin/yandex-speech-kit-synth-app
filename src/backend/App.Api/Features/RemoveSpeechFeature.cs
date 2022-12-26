using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using App.Data;

namespace App.Api.Features;

public static class RemoveSpeechFeature
{
    public record Request(string Text) : IRequest<Unit>;

    public class Handler : IRequestHandler<Request, Unit>
    {
        private readonly ILiteDbContext _liteDbContext;

        public Handler(ILiteDbContext liteDbContext)
        {
            _liteDbContext = liteDbContext;
        }

        public Task<Unit> Handle(
            Request request,
            CancellationToken _)
        {
            if (string.IsNullOrWhiteSpace(request?.Text))
            {
                throw new ArgumentException($"{nameof(request.Text)} should not be empty");
            }

            var speechCollection = _liteDbContext.GetCollection<Speech>();
            var speech = speechCollection
                .Query()
                .Where(speech => speech.Text == request.Text)
                .FirstOrDefault();

            if (speech is not null)
            {
                speechCollection.Delete(speech.Id);
                _liteDbContext.GetStorage().Delete(speech.FileId);
            }

            return Task.FromResult(Unit.Value);
        }
    }
}
