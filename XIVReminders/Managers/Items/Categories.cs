using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XIVReminders.Managers.Items
{

    public static class Categories
    {
        public static Category CrafterGatherer => new Category
        {
            Enabled = true,
            Name = "Crafter Gatherer",
            Items = new Dictionary<int, Alert>()
            {
                {
                    25199,
                    new Alert
                    {
                        DisplayName = "White Crafters' Scrip",
                        ItemId = 25199,
                        Threshold = 1500,
                        DefaultThreshold = 1500,
                        MaxValue = 2000,
                        Enabled = true
                    }
                },
                {
                    33913,
                    new Alert
                    {
                        DisplayName = "Purple Crafters' Scrip",
                        ItemId = 33913,
                        Threshold = 1500,
                        DefaultThreshold = 1500,
                        MaxValue = 2000,
                        Enabled = true
                    }
                },
                {
                    25200,
                    new Alert
                    {
                        DisplayName = "White Gatherers' Scrip",
                        ItemId = 25200,
                        Threshold = 1500,
                        DefaultThreshold = 1500,
                        MaxValue = 2000,
                        Enabled = true
                    }
                },
                {
                    33914,
                    new Alert
                    {
                        DisplayName = "Purple Gatherers' Scrip",
                        ItemId = 33914,
                        Threshold = 1500,
                        DefaultThreshold = 1500,
                        MaxValue = 2000,
                        Enabled = true
                    }
                },
                {
                    28063,
                    new Alert
                    {
                        DisplayName = "Skybuilders' Scrip",
                        ItemId = 28063,
                        Threshold = 7500,
                        DefaultThreshold = 7500,
                        MaxValue = 10000,
                        Enabled = true
                    }
                }
            }
        };

        public static Category Hunt => new Category
        {
            Enabled = true,
            Name = "Hunt",
            Items = new Dictionary<int, Alert>()
            {
                {
                    27,new Alert
                    {
                        DisplayName = "Allied Seals",
                        ItemId = 27,
                        Threshold = 3500,
                        DefaultThreshold = 3500,
                        MaxValue = 4000,
                        Enabled = true
                    }
                },
                {
                    10307,new Alert
                    {
                        DisplayName = "Centurio Seals",
                        ItemId = 10307,
                        Threshold = 3500,
                        DefaultThreshold = 3500,
                        MaxValue = 4000,
                        Enabled = true
                    }
                },
                {
                    26533,new Alert
                    {
                        DisplayName = "Sack of Nuts",
                        ItemId = 26533,
                        Threshold = 3500,
                        DefaultThreshold = 3500,
                        MaxValue = 4000,
                        Enabled = true
                    }
                },
                {
                    26807,new Alert
                    {
                        DisplayName = "Bicolor Gemstone",
                        ItemId = 26807,
                        Threshold = 800,
                        DefaultThreshold = 800,
                        MaxValue = 1000,
                        Enabled = true
                    }
                }
            }
        };

        public static Category PvP => new Category
        {
            Enabled = true,
            Name = "PvP",
            Items = new Dictionary<int, Alert>()
            {
                {
                    25,
                    new Alert
                    {
                        DisplayName = "Wolf Marks",
                        ItemId = 25,
                        Threshold = 18000,
                        DefaultThreshold = 18000,
                        MaxValue = 20000,
                        Enabled = true

                    }
                },
                {
                    36656,
                    new Alert
                    {
                        DisplayName = "Trophy Crystal",
                        ItemId = 36656,
                        Threshold = 18000,
                        DefaultThreshold = 18000,
                        MaxValue = 20000,
                        Enabled = true
                    }
                }
            }
        };

        public static Category GrandCompany => new Category
        {
            Enabled = true,
            Name = "Grand Company",
            Items = new Dictionary<int, Alert>()
            {
                {
                     20,
                    new Alert
                    {
                        DisplayName = "Storm Seals",
                        ItemId = 20,
                        Threshold = 75000,
                        DefaultThreshold = 75000,
                        MaxValue = 90000,
                        Enabled = true
                    }
                },
                {
                    21,
                    new Alert
                    {
                        DisplayName = "Serpent Seals",
                        ItemId = 21,
                        Threshold = 75000,
                        DefaultThreshold = 75000,
                        MaxValue = 90000,
                        Enabled = true
                    }
                },
                {
                    22,
                    new Alert
                    {
                        DisplayName = "Flame Seals",
                        ItemId = 22,
                        Threshold = 75000,
                        DefaultThreshold = 75000,
                        MaxValue = 90000,
                        Enabled = true
                    }
                }
            }
        };

        public static Category Battle => new Category
        {
            Enabled = true,
            Name = "Battle",
            Items = new Dictionary<int, Alert>()
            {
                {
                    28,
                    new Alert
                    {
                        DisplayName = "Tomestones of Poetics",
                        ItemId = 28,
                        Threshold = 1400,
                        DefaultThreshold = 1400,
                        MaxValue = 2000,
                        Enabled = true
                    }
                },
                {
                    42,
                    new Alert
                    {
                        DisplayName = "Tomestones of Aphorism",
                        ItemId = 42,
                        Threshold = 1400,
                        DefaultThreshold = 1400,
                        MaxValue = 2000,
                        Enabled = true
                    }
                },
                {
                    43,
                    new Alert
                    {
                        DisplayName = "Tomestones of Astronomy",
                        ItemId = 43,
                        Threshold = 1400,
                        DefaultThreshold = 1400,
                        MaxValue = 2000,
                        Enabled = true
                    }
                }
            }
        };
    }
}
