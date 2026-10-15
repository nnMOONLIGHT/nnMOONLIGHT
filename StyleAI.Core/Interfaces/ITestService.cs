using StyleAI.Core.DTOs.Style;
using StyleAI.Core.DTOs.Tests;

namespace StyleAI.Core.Interfaces;

public interface ITestService
{
    Task<int> CreateTestAsync(int managerId, CreateTestDto dto, CancellationToken ct);
    Task<List<TestResultDto>> GetUserResultsAsync(int userId, CancellationToken ct);
    Task<TestResultDto> SubmitAsync(int testId, int userId, SubmitAnswersDto dto, CancellationToken ct);
    Task<ConfirmWithPhotoDto> ConfirmWithPhotoAsync(int resultId, int userId, MlPrediction prediction, CancellationToken ct);
}