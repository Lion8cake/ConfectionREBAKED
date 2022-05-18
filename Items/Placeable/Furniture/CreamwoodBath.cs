using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Placeable.Furniture
{
	public class CreamwoodBath : ModItem
	{
		public override void SetStaticDefaults() 
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 12;
			Item.height = 30;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile = ModContent.TileType<Tiles.Furniture.CreamwoodBath>();
		}

		public override void AddRecipes() 
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.CreamWood>(), 14).AddTile(TileID.Sawmill).Register();
		}
	}
}