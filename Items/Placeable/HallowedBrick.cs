using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class HallowedBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.HallowedBrick>();
        }

		public override void AddRecipes() {
			CreateRecipe(1).AddIngredient(ModContent.ItemType<HallowedBrickWall>(), 4).Register();
			CreateRecipe(5)
                .AddIngredient(ItemID.StoneBlock, 5)
                .AddIngredient<HallowedOre>()
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}
