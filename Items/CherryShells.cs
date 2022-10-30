using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class CherryShells : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.value = 7500;
            Item.rare = 1;
            Item.maxStack = 9999;
        }
    }
}