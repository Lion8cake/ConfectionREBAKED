using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	public class Cakekite : ModItem
	{
		public override void SetStaticDefaults() 
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 12;
			Item.value = 50000;
			Item.rare = 3;
			Item.maxStack = 999;
		}
		
		public override void AddRecipes()
		{
		    CreateRecipe(1).AddIngredient(this, 1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ItemID.Shiverthorn, 1).AddIngredient(ItemID.Moonglow, 1).AddIngredient(ItemID.Waterleaf, 1).AddTile(TileID.AlchemyTable).ReplaceResult(ItemID.LifeforcePotion);
		    CreateRecipe(1).AddIngredient(this, 2).AddTile(96).ReplaceResult(ItemID.CookedFish);
		}
	}
}