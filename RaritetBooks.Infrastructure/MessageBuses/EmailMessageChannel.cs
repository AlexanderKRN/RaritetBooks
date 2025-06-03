using RaritetBooks.Application.Features.Notifications;
using System.Threading.Channels;

namespace RaritetBooks.Infrastructure.MessageBuses;

public class EmailMessageChannel
{
    private readonly Channel<EmailNotification> _channel = Channel.CreateUnbounded<EmailNotification>();

    public ChannelWriter<EmailNotification> Writer => _channel.Writer;
    public ChannelReader<EmailNotification> Reader => _channel.Reader;
}