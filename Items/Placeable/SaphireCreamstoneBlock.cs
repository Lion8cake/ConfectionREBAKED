using Terraria.ID;
using TheConfectionRebirth.Tiles;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria;

namespace TheConfectionRebirth.Items.Placeable
{
	public class SaphireCreamstoneBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 9999;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<CreamstoneSaphire>();
			Item.SetShopValues(ItemRarityID.White, Item.sellPrice(0, 0, 1));
		}
	}
}
