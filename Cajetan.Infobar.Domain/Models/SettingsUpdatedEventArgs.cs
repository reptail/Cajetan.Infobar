using System;
using System.Collections.Generic;

namespace Cajetan.Infobar.Models
{
    public class SettingsUpdatedEventArgs : EventArgs
    {
        public IEnumerable<string> UpdatedKeys { get; private set; }

        public SettingsUpdatedEventArgs(IEnumerable<string> updatedKeys)
        {
            UpdatedKeys = updatedKeys;
        }
    }
}
