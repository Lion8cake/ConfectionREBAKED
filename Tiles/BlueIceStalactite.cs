using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class BlueIceStalactite : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileObsidianKill[Type] = true;
			Main.tileBlockLight[Type] = true;
			TileID.Sets.DrawFlipMode[Type] = 1;
			TileID.Sets.BreakableWhenPlacing[Type] = true;
			DustType = ModContent.DustType<CreamstoneDust>();
			AddMapEntry(new Color(100, 100, 100));
			HitSound = SoundID.Dig;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
			ConfectionWorldGeneration.CheckTight(i, j);
			return false;
		}
	}
}
