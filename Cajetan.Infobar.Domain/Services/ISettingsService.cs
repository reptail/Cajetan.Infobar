namespace Cajetan.Infobar.Domain.Services;

public interface ISettingsService
{
    T Get<T>(string key);
    bool TryGet<T>(string key, out T value);

    void Set<T>(string key, T value);

    void SaveChanges();

    event EventHandler<string[]> SettingsUpdated;
    void RaiseSettingsUpdated(string[] updatedKeys);
}
