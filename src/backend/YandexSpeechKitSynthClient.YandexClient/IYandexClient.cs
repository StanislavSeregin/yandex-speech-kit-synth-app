using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace YandexSpeechKitSynthClient.YandexClient;

public interface IYandexClient
{
    Task<Stream> TextToSpeechInRussian(string text, CancellationToken cancellationToken);
}
