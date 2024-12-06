using System;
using System.Text.Json.Serialization;

namespace datasett.Models;

public class Movie
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("year")]
    public required int Year { get; set; }

    [JsonPropertyName("cast")]
    public required List<string> Cast { get; set; }

    [JsonPropertyName("genres")]
    public required List<string> Genres { get; set; }

    [JsonPropertyName("extract")]
    public string? Extract { get; set; }

    public override string ToString()
    {
        const int cutOff = 40;
        if (Extract is null)
        {
            return $"{Title} ({Year})";
        }
        if (Extract.Count() < cutOff)
        {
            return $"{Title} ({Year}) - {Extract}";
        }
        else
        {
            return $"{Title} ({Year}) - {Extract[0..cutOff]}...";
        }
    }
}
