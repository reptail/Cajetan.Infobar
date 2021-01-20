using System;
using System.Collections.Generic;

namespace Cajetan.Infobar.Domain.Services
{
    public interface ISettingsService
    {
        bool Contains(string key);
        T Get<T>(string key);
        void Set<T>(string key, T value);

        void SaveChanges();

        event EventHandler<IEnumerable<string>> SettingsUpdated;
        void RaiseSettingsUpdated(IEnumerable<string> updatedKeys);
    }
}
