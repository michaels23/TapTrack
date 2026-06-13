using TapTrack.Shared.Models;
using TapTrack.Shared.Services;

namespace TapTrack.Web.Services;

public class EmptyButtonProvider : IButtonProvider
{
    public Task<ButtonDefinition[]> GetButtonsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Array.Empty<ButtonDefinition>());
    }
}
