using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.GameContent;

namespace TheConfectionRebirth.Items.Placeable
{
	public class CookieBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;

			ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 1, ItemID.DirtBlock, 1);
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.CookieBlock>();
			Item.width = 12;
			Item.height = 12;
		}
	}
}
