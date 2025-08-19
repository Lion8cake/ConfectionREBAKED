using TheConfectionRebirth.Tiles.Pylon;
using Terraria.Enums;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
	public class ConfectionPylon : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<ConfectionPylonTile>());

			Item.SetShopValues(ItemRarityColor.Blue1, Terraria.Item.buyPrice(gold: 10));
		}
	}
}
