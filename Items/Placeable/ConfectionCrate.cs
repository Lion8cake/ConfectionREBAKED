using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class ConfectionCrate : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Confection Crate");
            Tooltip.SetDefault("Right Click to open");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.consumable = true;
            Item.width = 34;
            Item.height = 34;
            Item.rare = ItemRarityID.Green;
            Item.createTile = ModContent.TileType<Tiles.ConfectionCrate>();
            Item.placeStyle = 0;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.value = 50000;
            Item.useStyle = 1;
        }

        public override bool CanRightClick()
        {
            return true;
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