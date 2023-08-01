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
            CreateRecipe()
                .AddIngredient(this)
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemID.Shiverthorn)
                .AddIngredient(ItemID.Moonglow)
                .AddIngredient(ItemID.Waterleaf)
                .AddTile(TileID.AlchemyTable)
                .ReplaceResult(ItemID.LifeforcePotion);
            CreateRecipe()
                .AddIngredient(this, 2)
                .AddTile(TileID.CookingPots)
                .ReplaceResult(ItemID.CookedFish);
        }
    }
}