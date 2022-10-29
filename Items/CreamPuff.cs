using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class CreamPuff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cream Puff");
            Tooltip.SetDefault("'Sometimes carried by creatures in the dessert desert'");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.value = 4500;
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 9999;
        }
    }
}