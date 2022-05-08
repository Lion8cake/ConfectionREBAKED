using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Placeable
{
	public class CreamwoodDoor : ModItem
	{
		public override void SetStaticDefaults() 
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 0;
			Item.createTile = Mod.Find<ModTile>("CreamwoodDoorClosed").Type;
		}
		
		public override void AddRecipes() 
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.CreamWood>(), 6).AddTile(TileID.WorkBenches).Register();
		}
	}
}
