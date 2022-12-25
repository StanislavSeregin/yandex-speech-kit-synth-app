using App.Api.Models;
using App.Data;
using App.YandexClient;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Api.Features;

public static class CreateOrUpdateSpeechFeature
{
    public record Response(SpeechModel Model);

    public record Request(string Text, string Transcription) : IRequest<Response>;

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IYandexClient _yandexClient;
        private readonly ILiteDbContext _liteDbContext;

        public Handler(
            IYandexClient yandexClient,
            ILiteDbContext liteDbContext)
        {
            _yandexClient = yandexClient;
            _liteDbContext = liteDbContext;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request?.Text))
            {
                throw new ArgumentException($"{nameof(request.Text)} should not be empty");
            }

            if (string.IsNullOrWhiteSpace(request?.Transcription))
            {
                throw new ArgumentException($"{nameof(request.Transcription)} should not be empty");
            }

            var speechCollection = _liteDbContext.GetCollection<Speech>();
            var speech = speechCollection
                .Query()
                .Where(speech => speech.Text == request.Text)
                .FirstOrDefault();

            if (speech?.Transcription == request.Transcription)
            {
                return new Response(SpeechModel.Map(speech));
            }

            var storage = _liteDbContext.GetStorage();
            var fileId = Guid.NewGuid();
            await using var speechStream = await _yandexClient.TextToSpeechInRussian(
                request.Transcription,
                cancellationToken);

            storage.Upload(
                fileId,
                fileId.ToString(),
                speechStream);

            var speechToUpsert = speech is not null
                ? speech with
                {
                    Transcription = request.Transcription,
                    FileId = fileId
                }
                : new Speech()
                {
                    Id = Guid.NewGuid(),
                    FileId= fileId,
                    Text = request.Text,
                    Transcription= request.Transcription
                };

            speechCollection.Upsert(speechToUpsert);
            return new Response(SpeechModel.Map(speechToUpsert));
        }
    }
}
