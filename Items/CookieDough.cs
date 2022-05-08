using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	public class CookieDough : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Cookie Dough");
			Tooltip.SetDefault("Don't consume it, it may contain raw eggs.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}

		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 12;
			Item.value = 15000;
			Item.rare = 1;
			Item.maxStack = 99;
		}
		
		public override void AddRecipes() 
		{
            CreateRecipe(15).AddIngredient(500, 15).AddIngredient(ItemID.FallenStar, 1).AddIngredient(ModContent.ItemType<Items.Placeable.Saccharite>(), 3).AddIngredient(ModContent.ItemType<Items.CookieDough>(), 1).AddTile(13).ReplaceResult(2209);
        }
	}
}