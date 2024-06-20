using System.Timers;

using Krevetki.ToDoBot.Application.Common.Interfaces;
using Krevetki.ToDoBot.Domain.Entities;

using Microsoft.EntityFrameworkCore;

using Timer = System.Timers.Timer;

namespace Krevetki.ToDoBot.Bot.Services
{
    public class NotificationService : INotificationService, IHostedService
    {
        private readonly Timer _timer;

        private readonly IRepository _repository;

        private readonly ILogger<NotificationService> _logger;

        private readonly ITelegramService _telegramService;

        public NotificationService(IRepository repository, ILogger<NotificationService> logger, ITelegramService telegramService)
        {
            _repository = repository;
            _logger = logger;
            _telegramService = telegramService;

            _timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            _timer.Elapsed += async (sender, e) => await TimerOnElapsed(sender, e);
        }

        private async Task TimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            try
            {
                using var transaction = await _repository.BeginTransactionAsync<Notification>(cancellationToken: new CancellationToken());
                var now = DateTime.Now;
                var notifications = await transaction.Set
                                                     .AsNoTracking()
                                                     .Include(x => x.ToDoItem)
                                                     .Include(x => x.User)
                                                     .Where(
                                                         x => x.NotificationTime.Date == now.Date
                                                              && x.NotificationTime.Hour == now.Hour
                                                              && x.NotificationTime.Minute == now.Minute)
                                                     .ToListAsync();

                foreach (var notification in notifications)
                {
                    // await _telegramService.SendMessageAsync(
                    //     notification.User.ChatId,
                    //     new Message() { Text = Messages.NotificationMessage(notification.ToDoItem) },
                    //     new CancellationToken());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing notifications.");
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
