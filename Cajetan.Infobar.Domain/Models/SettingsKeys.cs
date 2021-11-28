namespace Cajetan.Infobar.Domain.Models
{
    public static class SettingsKeys
    {
        public const string GENERAL_BACKGROUND_COLOR = "General_BackgroundColor";
        public const string GENERAL_FOREGROUND_COLOR = "General_ForegroundColor";
        public const string GENERAL_BORDER_COLOR = "General_BorderColor";
        public const string GENERAL_REFRESH_INTERVAL = "General_RefreshInterval_Milliseconds";

        public const string BATTERY_IS_ENABLED = "Module_Battery_IsEnabled";
        public const string BATTERY_SORT_ORDER = "Module_Battery_SortOrder";
        public const string BATTERY_SHOW_TEXT = "Module_Battery_ShowText";
        public const string BATTERY_SHOW_TIME = "Module_Battery_ShowTime";

        public const string MEMORY_IS_ENABLED = "Module_Memory_IsEnabled";
        public const string MEMORY_SORT_ORDER = "Module_Memory_SortOrder";
        public const string MEMORY_SHOW_TEXT  = "Module_Memory_ShowText";
        public const string MEMORY_SHOW_GRAPH  = "Module_Memory_ShowGraph";

        public const string NETWORK_IS_ENABLED = "Module_Network_IsEnabled";
        public const string NETWORK_SORT_ORDER = "Module_Network_SortOrder";
        public const string NETWORK_DISPLAY_FORMAT  = "Module_Network_DisplayFormat";

        public const string PROCESSOR_IS_ENABLED = "Module_Processor_IsEnabled";
        public const string PROCESSOR_SORT_ORDER = "Module_Processor_SortOrder";
        public const string PROCESSOR_SHOW_TEXT  = "Module_Processor_ShowText";
        public const string PROCESSOR_SHOW_GRAPH  = "Module_Processor_ShowGraph";

        public const string UPTIME_IS_ENABLED = "Module_Uptime_IsEnabled";
        public const string UPTIME_SORT_ORDER = "Module_Uptime_SortOrder";
        public const string UPTIME_SHOW_TEXT  = "Module_Uptime_ShowText";
        public const string UPTIME_SHOW_DAYS  = "Module_Uptime_ShowDays";

        public const string STARSHIP_INFO_IS_ENABLED = "Module_StarshipInfo_IsEnabled";
        public const string STARSHIP_INFO_LAUNCH_ID = "Module_StarshipInfo_LaunchId";
    }
}
