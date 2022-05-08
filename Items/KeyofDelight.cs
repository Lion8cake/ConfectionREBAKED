using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	public class KeyofDelight : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Key of Delight");
			Tooltip.SetDefault("'Charged with the essence of many souls'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 12;
			Item.maxStack = 99;
		}
		
		public override void AddRecipes() 
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.SoulofDelight>(), 15).AddTile(TileID.WorkBenches).Register();
		}
	}
}