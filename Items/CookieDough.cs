using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.Items
{
    public class CookieDough : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.value = 15000;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            Recipe.Create(2209, 15)
                .AddIngredient(ItemID.GreaterManaPotion, 15)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddIngredient<Saccharite>(3)
                .AddIngredient<CookieDough>()
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}