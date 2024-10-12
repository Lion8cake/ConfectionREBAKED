using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class ConfectionGlobalItem : GlobalItem
	{
		public override bool? UseItem(Item item, Player player)
		{
			int type = item.type;
			int tileType = 0;
			if (type == ItemID.GreenMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossGreen>();
			}
			if (type == ItemID.BrownMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossBrown>();
			}
			if (type == ItemID.RedMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossRed>();
			}
			if (type == ItemID.BlueMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossBlue>();
			}
			if (type == ItemID.PurpleMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossPurple>();
			}
			if (type == ItemID.LavaMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossLava>();
			}
			if (type == ItemID.KryptonMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossKrypton>();
			}
			if (type == ItemID.XenonMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossXenon>();
			}
			if (type == ItemID.ArgonMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossArgon>();
			}
			if (type == ItemID.VioletMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossNeon>();
			}
			if (type == ItemID.RainbowMoss)
			{
				tileType = ModContent.TileType<Tiles.CreamstoneMossHelium>();
			}
			if (tileType != 0)
			{
				Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
				if (tile.HasTile && (tile.TileType == ModContent.TileType<Tiles.Creamstone>()) && player.IsInTileInteractionRange(Player.tileTargetX, Player.tileTargetY, Terraria.DataStructures.TileReachCheckSettings.Simple))
				{
					Main.tile[Player.tileTargetX, Player.tileTargetY].TileType = (ushort)tileType;
					WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
					SoundEngine.PlaySound(SoundID.Dig, player.position);
					return true;
				}
			}
			return null;
		}
	}
}
