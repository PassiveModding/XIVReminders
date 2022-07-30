using System.Collections.Generic;

namespace XIVReminders.Managers.Items
{
    public class Category
    {
        public bool Enabled { get; set; }
        public string Name { get; set; } = null!;
        public Dictionary<int, Alert> Items { get; set; } = null!;
    }
}
