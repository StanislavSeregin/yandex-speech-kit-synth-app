using System;

namespace App.Data;

public record Speech
{
    public Guid Id { get; set; }

    public Guid FileId { get; set; }

    public string Text { get; set; }

    public string Transcription { get; set; }
}
