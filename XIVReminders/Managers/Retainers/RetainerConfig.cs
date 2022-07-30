using System.Collections.Generic;

namespace XIVReminders.Managers.Retainers
{
    internal class RetainerConfig : IManagerConfig
    {
        public bool Enabled { get; set; }
        public bool HideRetainersUntilComplete { get; set; } = false;
        public Dictionary<ulong, RetainerAlert> PlayerRetainers { get; set; } = null!;

        public static RetainerConfig Default =>
            new RetainerConfig
            {
                Enabled = true,
                HideRetainersUntilComplete = false,
                PlayerRetainers = new Dictionary<ulong, RetainerAlert>()
            };
    }
}
