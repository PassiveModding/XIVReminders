namespace XIVReminders.Managers.Items
{
    internal class ItemConfig : IManagerConfig
    {
        public const string Key = "Currency";

        public bool Enabled { get; set; }
        public Category[] Categories { get; set; } = null!;
        public string Name { get; set; } = Key;

        public static ItemConfig Default => new ItemConfig
        {
            Enabled = true,
            Categories = new Category[]
            {
                Items.Categories.Battle,
                Items.Categories.CrafterGatherer,
                Items.Categories.GrandCompany,
                Items.Categories.Hunt,
                Items.Categories.PvP,
            }
        };
    }
}
