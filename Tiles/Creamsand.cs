using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Tiles
{
	public class Creamsand : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileBrick[Type] = true;
			Main.tileSand[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;

			TileID.Sets.Conversion.Sand[Type] = true; 
			TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true;
			TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.Falling[Type] = true;
			TileID.Sets.Suffocate[Type] = true;
			TileID.Sets.FallingBlockProjectile[Type] = new TileID.Sets.FallingBlockProjectileInfo(ModContent.ProjectileType<CreamsandProjectile>(), 10);
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			MineResist = 0.5f;
			DustType = ModContent.DustType<CreamsandDust>();
			AddMapEntry(new Color(99, 57, 46));
		}

		public override bool HasWalkDust() {
			return Main.rand.NextBool(3);
		}

		public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color) {
			dustType = DustType;
		}
	}
}
