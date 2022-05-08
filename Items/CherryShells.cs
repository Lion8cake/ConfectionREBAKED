using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	public class CherryShells : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Cherry Shells"); 
			Tooltip.SetDefault("'Surprisingly explosive'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}

		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 12;
			Item.value = 7500;
			Item.rare = 1;
			Item.maxStack = 99;
		}
	}
}