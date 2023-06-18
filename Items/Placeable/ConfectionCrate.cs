using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class ConfectionCrate : ModItem
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsFishingCrate[Type] = true;
            ItemID.Sets.IsFishingCrateHardmode[Type] = true;

            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 34;
            Item.height = 34;
            Item.rare = ItemRarityID.Green;
            Item.createTile = ModContent.TileType<Tiles.ConfectionCrate>();
            Item.placeStyle = 0;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.value = Item.sellPrice(gold: 1);
            Item.useStyle = 1;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // adapted from ItemDropDatabase.TML.cs cuz lazy
            IItemDropRule bc_goldCoin = ItemDropRule.NotScalingWithLuck(ItemID.GoldCoin, 4, 5, 13);

            IItemDropRule bc_sol = ItemDropRule.NotScalingWithLuck(ModContent.ItemType<SoulofDelight>(), 2, 2, 5);
            IItemDropRule bc_shard = ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Saccharite>(), 2, 4, 10);

            var oresTier1 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.CopperOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.TinOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.IronOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.LeadOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.SilverOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.TungstenOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.GoldOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.PlatinumOre, 1, 30, 49)
            };
            var hardmodeOresTier1 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.CobaltOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.PalladiumOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.MythrilOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.OrichalcumOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.AdamantiteOre, 1, 30, 49),
                ItemDropRule.NotScalingWithLuck(ItemID.TitaniumOre, 1, 30, 49)
            };
            var barsTier1 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.IronBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.LeadBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.SilverBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.TungstenBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.GoldBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.PlatinumBar, 1, 10, 20)
            };
            var hardmodeBarsTier1 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.CobaltBar, 1, 8, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.PalladiumBar, 1, 8, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.MythrilBar, 1, 8, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.OrichalcumBar, 1, 8, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.AdamantiteBar, 1, 8, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.TitaniumBar, 1, 8, 20)
            };
            var potions = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.ObsidianSkinPotion, 1, 2, 4),
                ItemDropRule.NotScalingWithLuck(ItemID.SpelunkerPotion, 1, 2, 4),
                ItemDropRule.NotScalingWithLuck(ItemID.HunterPotion, 1, 2, 4),
                ItemDropRule.NotScalingWithLuck(ItemID.GravitationPotion, 1, 2, 4),
                ItemDropRule.NotScalingWithLuck(ItemID.MiningPotion, 1, 2, 4),
                ItemDropRule.NotScalingWithLuck(ItemID.HeartreachPotion, 1, 2, 4)
            };
            var extraPotions = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.HealingPotion, 1, 5, 17),
                ItemDropRule.NotScalingWithLuck(ItemID.ManaPotion, 1, 5, 17)
            };
            var extraBait = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.MasterBait, 3, 2, 6),
                ItemDropRule.NotScalingWithLuck(ItemID.JourneymanBait, 1, 2, 6)
            };

            var oresList = new List<IItemDropRule>();
            var barsList = new List<IItemDropRule>();
            oresList.AddRange(oresTier1);
            oresList.AddRange(hardmodeOresTier1);
            barsList.AddRange(barsTier1);
            barsList.AddRange(hardmodeBarsTier1);

            IItemDropRule[] hallowed = new IItemDropRule[] {
                bc_goldCoin,
                ItemDropRule.SequentialRulesNotScalingWithLuck(1, new OneFromRulesRule(5, oresList.ToArray()), new OneFromRulesRule(3, 2, barsList.ToArray())),
                new OneFromRulesRule(3, potions),

                bc_sol,
                bc_shard,
            };
            itemLoot.Add(ItemDropRule.AlwaysAtleastOneSuccess(hallowed));
            itemLoot.Add(new OneFromRulesRule(2, extraPotions));
            itemLoot.Add(ItemDropRule.SequentialRulesNotScalingWithLuck(2, extraBait));
        }

        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);

            if (Main.rand.NextBool(2))
            {
                player.QuickSpawnItem(entitySource, ModContent.ItemType<Items.SoulofDelight>(), Main.rand.Next(2, 5));
                player.QuickSpawnItem(entitySource, ModContent.ItemType<Items.Placeable.Saccharite>(), Main.rand.Next(4, 10));
            }

            if (Main.rand.NextBool(4))
            {
                player.QuickSpawnItem(entitySource, ItemID.GoldCoin, Main.rand.Next(5, 13));
            }

            if (Main.rand.NextBool(7))
            {
                int oreType = Main.rand.Next(new int[] {
                    ItemID.CopperOre,
                    ItemID.TinOre,
                    ItemID.IronOre,
                    ItemID.LeadOre,
                    ItemID.SilverOre,
                    ItemID.TungstenOre,
                    ItemID.GoldOre,
                    ItemID.PlatinumOre,
                    ItemID.CobaltOre,
                    ItemID.PalladiumOre,
                    ItemID.MythrilOre,
                    ItemID.OrichalcumOre,
                    ItemID.AdamantiteOre,
                    ItemID.TitaniumOre,
                });

                player.QuickSpawnItem(entitySource, oreType, Main.rand.Next(30, 50));
            }

            if (Main.rand.NextBool(4))
            {
                int barType = Main.rand.Next(new int[] {
                    ItemID.IronBar,
                    ItemID.LeadBar,
                    ItemID.SilverBar,
                    ItemID.TungstenBar,
                    ItemID.GoldBar,
                    ItemID.PlatinumBar,
                    ItemID.CobaltBar,
                    ItemID.PalladiumBar,
                    ItemID.MythrilBar,
                    ItemID.OrichalcumBar,
                    ItemID.AdamantiteBar,
                    ItemID.TitaniumBar,
                });

                player.QuickSpawnItem(entitySource, barType, Main.rand.Next(10, 21));
            }

            if (Main.rand.NextBool(4))
            {
                int potionType = Main.rand.Next(new int[] {
                    ItemID.ObsidianSkinPotion,
                    ItemID.SpelunkerPotion,
                    ItemID.HunterPotion,
                    ItemID.GravitationPotion,
                    ItemID.MiningPotion,
                    ItemID.HeartreachPotion,
                });

                player.QuickSpawnItem(entitySource, potionType, Main.rand.Next(2, 5));
            }

            if (Main.rand.NextBool(2))
            {
                int resourcePotionType = Main.rand.NextBool() ? ItemID.HealingPotion : ItemID.ManaPotion;
                player.QuickSpawnItem(entitySource, resourcePotionType, Main.rand.Next(5, 18));
            }

            if (Main.rand.NextBool(2))
            {
                int baitType = Main.rand.NextBool() ? ItemID.JourneymanBait : ItemID.MasterBait;
                player.QuickSpawnItem(entitySource, baitType, Main.rand.Next(2, 7));
            }
        }
    }
}