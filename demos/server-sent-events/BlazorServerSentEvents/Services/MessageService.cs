using System;
using System.Collections.Concurrent;
using System.Threading.Channels;

using BlazorServerSentEvents.Model;

namespace BlazorServerSentEvents.Services;

public sealed class MessageService
{
    private readonly ConcurrentDictionary<object, Channel<Message>> _channels = new();

    public void RegisterListener(object listener)
    {
        if (!_channels.TryAdd(listener, Channel.CreateUnbounded<Message>()))
        {
            throw new InvalidOperationException("Listener already registered.");
        }
    }

    public void UnregisterListener(object listener)
    {
        if (!_channels.TryRemove(listener, out _))
        {
            throw new InvalidOperationException("Listener not registered.");
        }
    }

    public void NotifyNewMessageAvailable(Message message)
    {
        foreach (var channel in _channels.Values)
        {
            _ = Task.Run(async () => await channel.Writer.WriteAsync(message));
        }
    }

    public async Task<Message> WaitForNewMessageAsync(object listener, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_channels.TryGetValue(listener, out var channel))
            {
                throw new InvalidOperationException($"Listener not registered.");
            }

            return await channel.Reader.ReadAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was canceled.");
            return default!;
        }

    }
}
