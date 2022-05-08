using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	public class YumDrop : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("YumDrop");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults() 
		{
			Item.width = 10;
			Item.height = 12;
			Item.value = 15000;
			Item.rare = ItemRarityID.White;
			Item.maxStack = 99;
		}
	}
}