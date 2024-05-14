using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamVines : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileCut[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileNoFail[Type] = true;

			TileID.Sets.TileCutIgnore.Regrowth[Type] = true;
			TileID.Sets.IsVine[Type] = true;
			TileID.Sets.ReplaceTileBreakDown[Type] = true;
			TileID.Sets.VineThreads[Type] = true;
			//TileID.Sets.DrawFlipMode[Type] = 1;

			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			DustType = ModContent.DustType<CreamGrassDust>();
			HitSound = SoundID.Grass;
			AddMapEntry(new Color(200, 170, 108));
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
			bool intoRenderTargets = true;
			bool flag = intoRenderTargets || Main.LightingEveryFrame;

			if (Main.tile[i, j].TileFrameX % 18 == 0 && Main.tile[i, j].TileFrameY % 54 == 0 && flag) {
				Main.instance.TilesRenderer.CrawlToTopOfVineAndAddSpecialPoint(j, i);
			}

			return false;
		}
	}
}
