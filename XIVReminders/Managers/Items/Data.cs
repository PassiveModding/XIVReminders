using System;
using System.Collections.Generic;
using System.Linq;

namespace XIVReminders.Managers.Items
{
    public class Data
    {
        [AttributeUsage(AttributeTargets.All)]
        public class ItemAttribute : Attribute
        {
            public ItemAttribute(string name, int defaultThreshold, int maxCount, ItemCategory category)
            {
                Name = name;
                DefaultThreshold = defaultThreshold;
                MaxCount = maxCount;
                Category = category;
            }
            public string Name { get; }
            public int DefaultThreshold { get; }
            public int MaxCount { get; }
            public ItemCategory Category { get; }
        }

        public enum ItemCategory
        {
            PvP,
            Battle,
            CrafterGatherer,
            GrandCompany,
            Hunt
        }

        public static IEnumerable<(Item, ItemAttribute)> GetItemEnumerator()
        {
            foreach (Item itemInfo in (Item[])Enum.GetValues(typeof(Item)))
            {
                ItemAttribute? attr = Helpers.GetAttributeOfType<ItemAttribute>(itemInfo);
                if (attr == null) continue;
                yield return (itemInfo, attr);
            }
        }

        public enum Item
        {
            [Item("White Crafters' Scrip", 1500, 2000, ItemCategory.CrafterGatherer)]
            WhiteCraftersScrip = 25199,
            [Item("Purple Crafters' Scrip", 1500, 2000, ItemCategory.CrafterGatherer)]
            PurpleCraftersScrip = 33913,
            [Item("White Gatherers' Scrip", 1500, 2000, ItemCategory.CrafterGatherer)]
            WhiteGatherersScrip = 25200,
            [Item("Purple Gatherers' Scrip", 1500, 2000, ItemCategory.CrafterGatherer)]
            PurpleGatherersScrip = 33914,
            [Item("Skybuilders Scrip", 7500, 10000, ItemCategory.CrafterGatherer)]
            SkybuildersScrip = 28063,
            [Item("Allied Seals", 3500, 4000, ItemCategory.Hunt)]
            AlliedSeals = 27,
            [Item("Centurio Seals", 3500, 4000, ItemCategory.Hunt)]
            CenturioSeals = 10307,
            [Item("Sack of Nuts", 3500, 4000, ItemCategory.Hunt)]
            SackOfNuts = 26533,
            [Item("Bicolor Gemstone", 800, 1000, ItemCategory.Hunt)]
            BiColorGemstone = 26807,
            [Item("Wolf Marks", 18000, 20000, ItemCategory.PvP)]
            WolfMarks = 25,
            [Item("Trophy Crystal", 18000, 20000, ItemCategory.PvP)]
            TrophyCrystal = 36656,
            [Item("Tomestones of Poetics", 1400, 2000, ItemCategory.Battle)]
            Poetics = 28,
            [Item("Tomestones of Aphorism", 1400, 2000, ItemCategory.Battle)]
            Aphorism = 42,
            [Item("Tomestones of Astronomy", 1400, 2000, ItemCategory.Battle)]
            Astronomy = 43,
            [Item("Storm Seals", 75000, 90000, ItemCategory.GrandCompany)]
            StormSeals = 20,
            [Item("Serpent Seals", 75000, 90000, ItemCategory.GrandCompany)]
            SerpentSeals = 21,
            [Item("Flame Seals", 75000, 90000, ItemCategory.GrandCompany)]
            FlameSeals = 22

        }
    }
}