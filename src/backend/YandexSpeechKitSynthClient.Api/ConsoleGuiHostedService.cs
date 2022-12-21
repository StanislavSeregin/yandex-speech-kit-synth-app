using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Gui;

namespace YandexSpeechKitSynthClient.Api
{
    public class ConsoleGuiHostedService : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IServer _server;

        public ConsoleGuiHostedService(
            IHostApplicationLifetime hostApplicationLifetime,
            IServer server)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _server = server;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Application.Init();
            Application.QuitKey = Key.C | Key.CtrlMask;
            return Task.Run(
                () =>
                {
                    var view = new MainWindow(OpenCurrentUrl);
                    Application.Run(view);
                    Application.Shutdown();
                    _hostApplicationLifetime.StopApplication();
                },
                stoppingToken);
        }

        private void OpenCurrentUrl()
        {
            var serverAddressesFeature = _server.Features.Get<IServerAddressesFeature>();
            var currentUrl = serverAddressesFeature.Addresses.LastOrDefault();
            var processStartInfo = new ProcessStartInfo(currentUrl)
            {
                UseShellExecute = true
            };

            Process.Start(processStartInfo);
        }
    }

    internal class MainWindow : Window
    {
        private readonly Label _label = new()
        {
            Width = 4,
            Height = 1,
            X = Pos.Center(),
            Y = Pos.Center(),
            Text = "YandexSpeechKitSynthClient"
        };

        private readonly Button _button = new()
        {
            Width = 12,
            X = Pos.Center(),
            Y = Pos.Center() + 1,
            Text = "Web GUI",
            TextAlignment = TextAlignment.Centered,
            IsDefault = false
        };

        public MainWindow(Action openCurrentUrlFunc)
        {
            Width = Dim.Fill(0);
            Height = Dim.Fill(0);
            X = 0;
            Y = 0;
            Modal = false;
            Border.BorderStyle = BorderStyle.Single;
            Border.Effect3D = false;
            Border.DrawMarginFrame = true;
            TextAlignment = TextAlignment.Left;
            Title = "Press Ctrl+C to quit";
            Add(_label);
            Add(_button);

            _button.Clicked += openCurrentUrlFunc;
        }
    }
}
