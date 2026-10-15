namespace StyleAI.Core.Interfaces;

public interface IPythonMlClient
{
    Task<MlPrediction> PredictAsync(Stream image, string fileName, CancellationToken ct);
}

public record MlPrediction(
    string StyleId,
    string StyleName,
    float Confidence,
    float[] Features,
    Dictionary<string, float> AllProbs
);