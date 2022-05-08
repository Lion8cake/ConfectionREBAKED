using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	public class SugarFish : ModItem
	{
		public override void SetStaticDefaults() 
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
		}

		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 12;
			Item.value = 150000;
			Item.rare = ItemRarityID.LightRed;
			Item.maxStack = 999;
		}
		
		public override void AddRecipes()
		{
		    CreateRecipe(1).AddIngredient(this, 1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ItemID.Blinkroot, 1).AddIngredient(ItemID.Fireblossom, 1).AddTile(TileID.AlchemyTable).ReplaceResult(ItemID.TeleportationPotion);
		    CreateRecipe(1).AddIngredient(this, 2).AddTile(96).ReplaceResult(ItemID.CookedFish);
		}
	}
}