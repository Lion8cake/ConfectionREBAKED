using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.GameContent;
using Newtonsoft.Json.Linq;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.Items.Placeable
{
	public class CreamBeans : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;

			ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 1, ItemID.DirtBlock, 1);
			ItemID.Sets.GrassSeeds[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.useStyle = 1;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<CreamGrass>();
			Item.width = 14;
			Item.height = 14;
			Item.value = 500;
		}

		public override bool? UseItem(Player player) {
			Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
			if (tile.HasTile && (tile.TileType == ModContent.TileType<Tiles.CookieBlock>() || tile.TileType == TileID.Dirt) && player.IsInTileInteractionRange(Player.tileTargetX, Player.tileTargetY, Terraria.DataStructures.TileReachCheckSettings.Simple)) {
				Main.tile[Player.tileTargetX, Player.tileTargetY].TileType = (ushort)Item.createTile;
				WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
				return true;
			}
			return false;
		}
	}
}
