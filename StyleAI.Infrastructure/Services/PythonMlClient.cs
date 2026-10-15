using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using StyleAI.Core.Interfaces;

namespace StyleAI.Infrastructure.Services;

public class PythonMlClient : IPythonMlClient
{
    private readonly HttpClient _http;
    public PythonMlClient(HttpClient http) => _http = http;

    public async Task<MlPrediction> PredictAsync(Stream image, string fileName, CancellationToken ct)
    {
        using var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(image);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        content.Add(streamContent, "file", fileName);

        var response = await _http.PostAsync("/predict", content, ct);
        response.EnsureSuccessStatusCode();

        var raw = await response.Content.ReadFromJsonAsync<RawPrediction>(cancellationToken: ct)
                  ?? throw new InvalidOperationException("Empty ML response");

        return new MlPrediction(
            raw.StyleId,
            raw.StyleName,
            raw.Confidence,
            raw.Features,
            raw.AllProbs
        );
    }

    private record RawPrediction(
        [property: JsonPropertyName("style_id")] string StyleId,
        [property: JsonPropertyName("style_name")] string StyleName,
        [property: JsonPropertyName("confidence")] float Confidence,
        [property: JsonPropertyName("features")] float[] Features,
        [property: JsonPropertyName("all_probs")] Dictionary<string, float> AllProbs
    );
}