namespace XIVReminders.Modules
{
    internal partial class CurrencyAlerts
    {
        public class CurrencyAlert
        {
            public string DisplayName { get; set; } = null!;
            public int Threshold { get; set; }
            public int DefaultThreshold { get; set; }
            public int MaxValue { get; set; }
            public int LastKnownValue { get; set; }
            public int ItemId { get; set; }
            public bool Enabled { get; set; }
            public string Category { get; set; } = null!;
        }
    }
}
