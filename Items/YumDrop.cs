using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class YumDrop : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.White;
            Item.maxStack = 9999;
        }
    }
}