using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.Items
{
    public class Sprinkles : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 12;
            Item.value = 500;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            Recipe.Create(ItemID.GreaterHealingPotion, 3)
                .AddIngredient(this, 3)
                .AddIngredient<Saccharite>()
                .AddIngredient(ItemID.BottledWater, 3)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
}