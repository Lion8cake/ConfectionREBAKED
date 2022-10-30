using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.Items.Archived
{
	public class MeetyMummyBanner : ModItem, IArchived
	{
		public int ArchivatesTo() => ItemID.BloodMummyBanner;

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<ConfectionBanners>();
			Item.placeStyle = 8;
		}
	}
}