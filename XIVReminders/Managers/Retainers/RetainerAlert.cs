using System;

namespace XIVReminders.Managers.Retainers
{
    internal class RetainerAlert
    {
        public ulong RetainerId { get; set; }
        public string Name { get; set; } = null!;

        // Timestamp is saved so we can know what the retainer timer is after restart
        public DateTime TimeStamp { get; set; }
        public bool IsHidden { get; set; } = false;
    }
}
