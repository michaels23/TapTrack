using TapTrack.Shared.Models;

namespace TapTrack.Shared.Services;

public interface IButtonProvider
{
    Task<ButtonDefinition[]> GetButtonsAsync(CancellationToken cancellationToken = default);
}
