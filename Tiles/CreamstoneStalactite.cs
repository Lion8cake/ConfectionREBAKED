using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Dusts;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;

namespace TheConfectionRebirth.Tiles
{
	public class CreamstoneStalactite : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = false;
			DustType = ModContent.DustType<CreamDust>();
			TileID.Sets.DisableSmartCursor[Type] = true;
		}
	
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameY <= 18 || tile.TileFrameY == 72)
			{
				offsetY = -2;
			}
			else if ((tile.TileFrameY >= 36 && tile.TileFrameY <= 54) || tile.TileFrameY == 90)
			{
				offsetY = 2;
			}
		}
	
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			WorldGen.CheckTight(i, j);
			return false;
		}
	
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 4);
		}
	}
}