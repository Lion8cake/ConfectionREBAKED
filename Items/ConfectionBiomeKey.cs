using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheConfectionRebirth.Items
{
    class ConfectionBiomeKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Confection Key");
            Tooltip.SetDefault("Unlocks a Confection Chest in the dungeon");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Yellow;
        }
    }
}