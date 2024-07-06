using System.Timers;

using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Application.Notifications.Queries.EveningNotification;
using Krevetki.ToDoBot.Infrastructure.Options;

using MediatR;

using Microsoft.Extensions.Options;

using Timer = System.Timers.Timer;

namespace Krevetki.ToDoBot.Bot.Services
{
    public class EveningNotificationService : IEveningNotificationService, IHostedService
    {
        private readonly Timer _timer;

        private readonly ILogger<NotificationService> _logger;

        private readonly EveningNotificationOptions _options;

        private readonly IMediator _mediator;

        public EveningNotificationService(
            ILogger<NotificationService> logger,
            IOptions<EveningNotificationOptions> options,
            IMediator mediator)
        {
            _logger = logger;
            _options = options.Value;
            _mediator = mediator;

            _timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            _timer.Elapsed += TimerOnElapsed;
        }

        private async void TimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            if (e.SignalTime.ToUniversalTime().Hour == _options.EveningNotificationTime.Hour
                && e.SignalTime.ToUniversalTime().Minute == _options.EveningNotificationTime.Minute)
            {
                try
                {
                    await _mediator.Send(new EveningNotificationQuery());
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing notifications.");
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.Start();
            _logger.LogInformation("Notification service started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            _logger.LogInformation("Notification service stopped");
            return Task.CompletedTask;
        }
    }
}
