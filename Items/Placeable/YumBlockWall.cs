using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheConfectionRebirth.Items.Placeable
{
	public class YumBlockWall : ModItem
	{
		public override void SetDefaults() {
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Walls.YumBlockWall>();
		}

		public override void AddRecipes() {
			Recipe recipe = CreateRecipe(4);
			recipe.AddIngredient(ModContent.ItemType<YumBlock>());
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}