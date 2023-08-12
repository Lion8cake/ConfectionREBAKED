using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;
using TheConfectionRebirth.Walls;

namespace TheConfectionRebirth.Projectiles
{
	public class CreamSolution : ModProjectile {
		public ref float Progress => ref Projectile.ai[0];

		public override void SetDefaults() {
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 2;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override bool? CanCutTiles() {
			return false;
		}

		public override void AI() {
			int dustType = ModContent.DustType<Dusts.CreamSolution>();

			if (Projectile.owner == Main.myPlayer) {
				Convert((int)(Projectile.position.X + Projectile.width / 2) / 16, (int)(Projectile.position.Y + Projectile.height / 2) / 16, 2);
			}

			if (Projectile.timeLeft > 133) {
				Projectile.timeLeft = 133;
			}

			if (Progress > 7f) {
				float dustScale = 1f;

				if (Progress == 8f) {
					dustScale = 0.2f;
				}
				else if (Progress == 9f) {
					dustScale = 0.4f;
				}
				else if (Progress == 10f) {
					dustScale = 0.6f;
				}
				else if (Progress == 11f) {
					dustScale = 0.8f;
				}

				Progress += 1f;


				var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);

				dust.noGravity = true;
				dust.scale *= 1.75f;
				dust.velocity.X *= 2f;
				dust.velocity.Y *= 2f;
				dust.scale *= dustScale;
			}
			else {
				Progress += 1f;
			}

			Projectile.rotation += 0.3f * Projectile.direction;
		}

		public static void Convert(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size)) {
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;

						#region Walls
						if (WallID.Sets.Conversion.Stone[wall] && wall != ModContent.WallType<CreamstoneWall>() && wall != WallID.RocksUnsafe1 && wall != WallID.RocksUnsafe2 && wall != WallID.RocksUnsafe3 && wall != WallID.RocksUnsafe4 && wall != WallID.CorruptionUnsafe1 && wall != WallID.CorruptionUnsafe2 && wall != WallID.CorruptionUnsafe3 && wall != WallID.CorruptionUnsafe4 && wall != WallID.CrimsonUnsafe1 && wall != WallID.CrimsonUnsafe2 && wall != WallID.CrimsonUnsafe3 && wall != WallID.CrimsonUnsafe4 && wall != WallID.HallowUnsafe1 && wall != WallID.HallowUnsafe2 && wall != WallID.HallowUnsafe3 && wall != WallID.HallowUnsafe4) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamstoneWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Snow[wall] && wall != ModContent.WallType<CreamWall>()) 
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Dirt[wall] && wall != ModContent.WallType<CookieWall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CookieWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.HardenedSand[wall] && wall != ModContent.WallType<CreamsandstoneWall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamsandstoneWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Sandstone[wall] && wall != ModContent.WallType<HardenedCreamsandWall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<HardenedCreamsandWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Grass[wall] && wall != ModContent.WallType<CreamGrassWall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamGrassWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Ice[wall] && wall != ModContent.WallType<BlueIceWall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<BlueIceWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if (wall == WallID.Cloud && wall != ModContent.WallType<PinkFairyFlossWall>())
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<PinkFairyFlossWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if ((wall == WallID.RocksUnsafe1 || wall == WallID.CorruptionUnsafe1 || wall == WallID.CrimsonUnsafe1 || wall == WallID.HallowUnsafe1) && wall != ModContent.WallType<Creamstone2Wall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone2Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if ((wall == WallID.RocksUnsafe2 || wall == WallID.CorruptionUnsafe2 || wall == WallID.CrimsonUnsafe2 || wall == WallID.HallowUnsafe2) && wall != ModContent.WallType<Creamstone3Wall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone3Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if ((wall == WallID.RocksUnsafe3 || wall == WallID.CorruptionUnsafe3 || wall == WallID.CrimsonUnsafe3 || wall == WallID.HallowUnsafe3) && wall != ModContent.WallType<Creamstone4Wall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone4Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if ((wall == WallID.RocksUnsafe4 || wall == WallID.CorruptionUnsafe4 || wall == WallID.CrimsonUnsafe4 || wall == WallID.HallowUnsafe4) && wall != ModContent.WallType<Creamstone5Wall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone5Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						#endregion

						#region TileIDConversions
						if (TileID.Sets.Conversion.Stone[type] && type != ModContent.TileType<Creamstone>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Creamstone>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Sand[type] && type != ModContent.TileType<Creamsand>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Creamsand>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Grass[type] && type != ModContent.TileType<CreamGrass>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamGrass>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Ice[type] && type != ModContent.TileType<BlueIce>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<BlueIce>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Sandstone[type] && type != ModContent.TileType<Creamsandstone>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Creamsandstone>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.HardenedSand[type] && type != ModContent.TileType<HardenedCreamsand>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<HardenedCreamsand>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Dirt[type] && type != ModContent.TileType<CookieBlock>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CookieBlock>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Snow[type] && type != ModContent.TileType<CreamBlock>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamBlock>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.GolfGrass[type] && type != ModContent.TileType<CreamGrassMowed>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamGrassMowed>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						#endregion

						#region ManualTileConverting
						else if (Main.tile[k, l].TileType == TileID.Ruby) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneRuby>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Sapphire) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneSaphire>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Diamond) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneDiamond>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Emerald) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneEmerald>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Amethyst) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneAmethyst>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Topaz) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneTopaz>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Cloud) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<PinkFairyFloss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.RainCloud) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<PurpleFairyFloss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.SnowCloud) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<BlueFairyFloss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.ArgonMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<ArgonCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.BlueMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<BlueCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.BrownMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<BrownCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.GreenMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<GreenCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.KryptonMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<KryptonCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.LavaMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<LavaCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.PurpleMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<PurpleCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.RedMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<RedCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.XenonMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<XenomCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == 3 && Main.tile[k, l].TileFrameX > 16 * 9 && Main.tile[k, l].TileFrameX < 16 * 8) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamGrass_Foliage>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == 3 && Main.tile[k, l].TileFrameX < 16 * 9 && Main.tile[k, l].TileFrameX > 16 * 8) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<YumDrop>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						#endregion
					}
				}
			}
		}
    }
}