using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace App.YandexClient;

public interface IYandexClient
{
    Task<Stream> TextToSpeechInRussian(string text, CancellationToken cancellationToken);
}
