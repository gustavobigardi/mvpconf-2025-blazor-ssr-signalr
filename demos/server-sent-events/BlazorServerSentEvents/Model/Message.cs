using System;

namespace BlazorServerSentEvents.Model;

public class Message
{
    public string Sender { get; set; } = null!;

    public string Text { get; set; } = null!;
}
