using TheConfectionRebirth.Items.Placeable;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Placeable
{
	public class HallowedOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.HallowedOre>();
			Item.width = 12;
			Item.height = 12;
			Item.value = 3000;
			Item.rare = ItemRarityID.LightRed;
		}
		
		public override void AddRecipes() 
		{
			CreateRecipe(1).AddIngredient(this, 5).AddTile(TileID.AdamantiteForge).ReplaceResult(ItemID.HallowedBar);
		}
	}
}
