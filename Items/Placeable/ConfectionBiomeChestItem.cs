using Terraria.Enums;
using Terraria.ModLoader;
using Terraria;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.Items.Placeable
{
	public class ConfectionBiomeChestItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile((ushort)ModContent.TileType<ConfectionBiomeChestTile>());
			Item.SetShopValues(ItemRarityColor.White0, Item.buyPrice(0, 0, 25));
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 22;
		}
	}
}
