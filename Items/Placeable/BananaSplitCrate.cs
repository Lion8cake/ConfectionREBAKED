using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class BananaSplitCrate : ModItem
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsFishingCrate[Type] = true;

            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 34;
            Item.height = 34;
            Item.rare = ItemRarityID.Green;
            Item.createTile = ModContent.TileType<Tiles.BananaSplitCrate>();
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
            var barsTier1 = new IItemDropRule[]
            {
                ItemDropRule.NotScalingWithLuck(ItemID.IronBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.LeadBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.SilverBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.TungstenBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.GoldBar, 1, 10, 20),
                ItemDropRule.NotScalingWithLuck(ItemID.PlatinumBar, 1, 10, 20)
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

            IItemDropRule[] hallowed = new IItemDropRule[] {
                bc_goldCoin,
                ItemDropRule.SequentialRulesNotScalingWithLuck(1, new OneFromRulesRule(5, oresTier1), new OneFromRulesRule(3, 2, barsTier1)),
                new OneFromRulesRule(3, potions),
            };
            itemLoot.Add(ItemDropRule.AlwaysAtleastOneSuccess(hallowed));
            itemLoot.Add(new OneFromRulesRule(2, extraPotions));
            itemLoot.Add(ItemDropRule.SequentialRulesNotScalingWithLuck(2, extraBait));
        }
    }
}