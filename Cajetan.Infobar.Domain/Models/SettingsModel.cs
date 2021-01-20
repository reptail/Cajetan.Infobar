namespace Cajetan.Infobar.Models
{
    public interface ISettingsModel
    {
        string Key { get; }
        object Value { get; set; }
    }

    public interface ISettingsModel<T> : ISettingsModel
    {
        new T Value { get; set; }
    }

    public class SettingsModel<T> : ISettingsModel<T>
    {
        public string Key { get; private set; }
        public T Value { get; set; }

        object ISettingsModel.Value
        {
            get => Value;
            set => Value = (T)value;
        }

        public SettingsModel(string key, T value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Key, Value);
        }
                
    }
}
