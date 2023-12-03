using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class Cakekite : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.value = 50000;
            Item.rare = ItemRarityID.Orange;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
			Recipe.Create(ItemID.SeafoodDinner)
			   .AddIngredient(this, 2)
			   .AddTile(TileID.CookingPots)
			   .Register();
		}
    }
}