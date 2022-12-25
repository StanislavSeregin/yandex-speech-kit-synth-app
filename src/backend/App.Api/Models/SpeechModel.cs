using App.Data;
using System;

namespace App.Api.Models;

public class SpeechModel
{
    public string Text { get; set; }

    public string Transcription { get; set; }

    public Guid FileId { get; set; }

    public static SpeechModel Map(Speech speech) => new()
    {
        Text = speech.Text,
        Transcription = speech.Transcription,
        FileId = speech.FileId
    };
}
