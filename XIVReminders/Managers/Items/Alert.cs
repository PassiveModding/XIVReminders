namespace XIVReminders.Managers.Items
{
    public class Alert
    {
        public bool Enabled { get; set; }
        public string DisplayName { get; set; } = null!;
        public int Threshold { get; set; }
        public int DefaultThreshold { get; set; }
        public int MaxValue { get; set; }
        public int LastKnownValue { get; set; }
        public int ItemId { get; set; }
    }
}
