using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheConfectionRebirth.Walls;
using TheConfectionRebirth.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace TheConfectionRebirth.Projectiles
{
	public class SolutionConversion : GlobalProjectile {
		public override void PostAI(Projectile projectile) {
			if ((projectile.type == 145 || projectile.type == 147 || projectile.type == 149 || projectile.type == 146 || projectile.type == ProjectileID.HolyWater || projectile.type == ProjectileID.UnholyWater || projectile.type == ProjectileID.BloodWater) && projectile.owner == Main.myPlayer) {
				if (projectile.owner == Main.myPlayer) {
					ConvertNormal((int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);
				}
			}
			if ((/*projectile.type == 10 || */projectile.type == 145) && projectile.owner == Main.myPlayer) {
				if (projectile.owner == Main.myPlayer) {
					ConvertPure((int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);
				}
			}
			if ((projectile.type == 147 || projectile.type == ProjectileID.UnholyWater) && projectile.owner == Main.myPlayer) {
				if (projectile.owner == Main.myPlayer) {
					ConvertCorrupt((int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);
				}
			}
			if ((projectile.type == 149 || projectile.type == ProjectileID.BloodWater) && projectile.owner == Main.myPlayer) {
				if (projectile.owner == Main.myPlayer) {
					ConvertCrimson((int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);
				}
			}
			if ((projectile.type == 146 || projectile.type == ProjectileID.HolyWater) && projectile.owner == Main.myPlayer) {
				if (projectile.owner == Main.myPlayer) {
					ConvertHallow((int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);
				}
			}
		}

		public void ConvertNormal(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size) && Main.tile[k, l] != null) {
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;

						#region Walls
						if (wall == ModContent.WallType<CreamWall>()) {
							Main.tile[k, l].WallType = 40;
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<CookieWall>() || wall == ModContent.WallType<Walls.GraveyardWalls.CookieWallArtificial>()) {
							Main.tile[k, l].WallType = 2;
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<CookieStonedWall>() || wall == ModContent.WallType<Walls.GraveyardWalls.CookieStonedWallArtificial>()) {
							Main.tile[k, l].WallType = 59;
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<BlueIceWall>()) {
							Main.tile[k, l].WallType = 71;
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if (wall == ModContent.WallType<PinkFairyFlossWall>()) {
							Main.tile[k, l].WallType = 73;
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						#endregion

						#region TileIDConversions
						if (type == ModContent.TileType<CookieBlock>()) {
							Main.tile[k, l].TileType = TileID.Dirt;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<CreamBlock>()) {
							Main.tile[k, l].TileType = TileID.SnowBlock;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						#endregion

						#region ManualTileConverting
						else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneRuby>()) {
							Main.tile[k, l].TileType = TileID.Ruby;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneSaphire>()) {
							Main.tile[k, l].TileType = TileID.Sapphire;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneDiamond>()) {
							Main.tile[k, l].TileType = TileID.Diamond;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneEmerald>()) {
							Main.tile[k, l].TileType = TileID.Emerald;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneAmethyst>()) {
							Main.tile[k, l].TileType = TileID.Amethyst;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == ModContent.TileType<CreamstoneTopaz>()) {
							Main.tile[k, l].TileType = TileID.Topaz;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == ModContent.TileType<PinkFairyFloss>()) {
							Main.tile[k, l].TileType = TileID.Cloud;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == ModContent.TileType<PurpleFairyFloss>()) {
							Main.tile[k, l].TileType = TileID.RainCloud;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == ModContent.TileType<BlueFairyFloss>()) {
							Main.tile[k, l].TileType = TileID.SnowCloud;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<ArgonCreamMoss>()) {
							Main.tile[k, l].TileType = TileID.ArgonMoss;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<BlueCreamMoss>()) {
							Main.tile[k, l].TileType = TileID.BlueMoss;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<BrownCreamMoss>()) {
							Main.tile[k, l].TileType = TileID.BrownMoss;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<GreenCreamMoss>()) {
							Main.tile[k, l].TileType = TileID.GreenMoss;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<KryptonCreamMoss>()) {
							Main.tile[k, l].TileType = TileID.KryptonMoss;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<LavaCreamMoss>()) {
							Main.tile[k, l].TileType = TileID.LavaMoss;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<PurpleCreamMoss>()) {
							Main.tile[k, l].TileType = TileID.PurpleMoss;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<RedCreamMoss>()) {
							Main.tile[k, l].TileType = TileID.RedMoss;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == (ushort)ModContent.TileType<XenomCreamMoss>()) {
							Main.tile[k, l].TileType = TileID.XenonMoss;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						#endregion
					}
				}
			}
		}

		public void ConvertPure(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size) && Main.tile[k, l] != null) {
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;

						#region Walls
						if (wall == ModContent.WallType<CreamstoneWall>()) {
							Main.tile[k, l].WallType = 1;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<CreamsandstoneWall>()) {
							Main.tile[k, l].WallType = 304;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<HardenedCreamsandWall>()) {
							Main.tile[k, l].WallType = 187;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<CreamGrassWall>()) {
							Main.tile[k, l].WallType = 63;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone5Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.MeltingConfectionWall>()) {
							Main.tile[k, l].WallType = WallID.RocksUnsafe4;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone4Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.LinedConfectionGemWall>()) {
							Main.tile[k, l].WallType = WallID.RocksUnsafe3;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone3Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.ConfectionaryCrystalWall>()) {
							Main.tile[k, l].WallType = WallID.RocksUnsafe2;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone2Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.CrackingConfectionWall>()) {
							Main.tile[k, l].WallType = WallID.RocksUnsafe1;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						#endregion

						#region TileIDConversions
						if (type == ModContent.TileType<Creamstone>()) {
							Main.tile[k, l].TileType = TileID.Stone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<Creamsand>()) {
							Main.tile[k, l].TileType = TileID.Sand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<CreamGrass>()) {
							Main.tile[k, l].TileType = TileID.Grass;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<BlueIce>()) {
							Main.tile[k, l].TileType = TileID.IceBlock;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<Creamsandstone>()) {
							Main.tile[k, l].TileType = TileID.Sandstone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<HardenedCreamsand>()) {
							Main.tile[k, l].TileType = TileID.HardenedSand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<CreamGrassMowed>()) {
							Main.tile[k, l].TileType = TileID.GolfGrass;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						#endregion
					}
				}
			}
		}

		public void ConvertCorrupt(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size) && Main.tile[k, l] != null) {
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;

						#region Walls
						if (wall == ModContent.WallType<CreamstoneWall>()) {
							Main.tile[k, l].WallType = 3;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<CreamsandstoneWall>()) {
							Main.tile[k, l].WallType = 305;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<HardenedCreamsandWall>()) {
							Main.tile[k, l].WallType = 220;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<CreamGrassWall>()) {
							Main.tile[k, l].WallType = 69;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone5Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.MeltingConfectionWall>()) {
							Main.tile[k, l].WallType = WallID.CorruptionUnsafe4;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone4Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.LinedConfectionGemWall>()) {
							Main.tile[k, l].WallType = WallID.CorruptionUnsafe3;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone3Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.ConfectionaryCrystalWall>()) {
							Main.tile[k, l].WallType = WallID.CorruptionUnsafe2;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone2Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.CrackingConfectionWall>()) {
							Main.tile[k, l].WallType = WallID.CorruptionUnsafe1;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						#endregion

						#region TileIDConversions
						if (type == ModContent.TileType<Creamstone>()) {
							Main.tile[k, l].TileType = TileID.Ebonstone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<Creamsand>()) {
							Main.tile[k, l].TileType = TileID.Ebonsand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<CreamGrass>()) {
							Main.tile[k, l].TileType = TileID.CorruptGrass;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<BlueIce>()) {
							Main.tile[k, l].TileType = TileID.CorruptIce;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<Creamsandstone>()) {
							Main.tile[k, l].TileType = TileID.CorruptSandstone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<HardenedCreamsand>()) {
							Main.tile[k, l].TileType = TileID.CorruptHardenedSand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<CreamGrassMowed>()) {
							Main.tile[k, l].TileType = TileID.CorruptGrass;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						#endregion
					}
				}
			}
		}

		public void ConvertCrimson(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size) && Main.tile[k, l] != null) {
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;

						#region Walls
						if (wall == ModContent.WallType<CreamstoneWall>()) {
							Main.tile[k, l].WallType = 83;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<CreamsandstoneWall>()) {
							Main.tile[k, l].WallType = 306;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<HardenedCreamsandWall>()) {
							Main.tile[k, l].WallType = 221;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<CreamGrassWall>()) {
							Main.tile[k, l].WallType = 69;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						#endregion

						#region TileIDConversions
						if (type == ModContent.TileType<Creamstone>()) {
							Main.tile[k, l].TileType = TileID.Crimstone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<Creamsand>()) {
							Main.tile[k, l].TileType = TileID.Crimsand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<CreamGrass>()) {
							Main.tile[k, l].TileType = TileID.CrimsonGrass;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<BlueIce>()) {
							Main.tile[k, l].TileType = TileID.FleshIce;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<Creamsandstone>()) {
							Main.tile[k, l].TileType = TileID.CrimsonSandstone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<HardenedCreamsand>()) {
							Main.tile[k, l].TileType = TileID.CrimsonHardenedSand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<CreamGrassMowed>()) {
							Main.tile[k, l].TileType = TileID.CrimsonGrass;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone5Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.MeltingConfectionWall>()) {
							Main.tile[k, l].WallType = WallID.CrimsonUnsafe4;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone4Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.LinedConfectionGemWall>()) {
							Main.tile[k, l].WallType = WallID.CrimsonUnsafe3;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone3Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.ConfectionaryCrystalWall>()) {
							Main.tile[k, l].WallType = WallID.CrimsonUnsafe2;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone2Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.CrackingConfectionWall>()) {
							Main.tile[k, l].WallType = WallID.CrimsonUnsafe1;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						#endregion
					}
				}
			}
		}

		public void ConvertHallow(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size) && Main.tile[k, l] != null) {
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;

						#region Walls
						if (wall == ModContent.WallType<CreamstoneWall>()) {
							Main.tile[k, l].WallType = 28;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<CreamsandstoneWall>()) {
							Main.tile[k, l].WallType = 307;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<HardenedCreamsandWall>()) {
							Main.tile[k, l].WallType = 222;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<CreamGrassWall>()) {
							Main.tile[k, l].WallType = 70;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone5Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.MeltingConfectionWall>()) {
							Main.tile[k, l].WallType = WallID.HallowUnsafe4;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone4Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.LinedConfectionGemWall>()) {
							Main.tile[k, l].WallType = WallID.HallowUnsafe3;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone3Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.ConfectionaryCrystalWall>()) {
							Main.tile[k, l].WallType = WallID.HallowUnsafe2;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == ModContent.WallType<Creamstone2Wall>() || wall == ModContent.WallType<Walls.GraveyardWalls.CrackingConfectionWall>()) {
							Main.tile[k, l].WallType = WallID.HallowUnsafe1;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						#endregion

						#region TileIDConversions
						if (type == ModContent.TileType<Creamstone>()) {
							Main.tile[k, l].TileType = TileID.Pearlstone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<Creamsand>()) {
							Main.tile[k, l].TileType = TileID.Pearlsand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<CreamGrass>()) {
							Main.tile[k, l].TileType = TileID.HallowedGrass;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<BlueIce>()) {
							Main.tile[k, l].TileType = TileID.HallowedIce;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<Creamsandstone>()) {
							Main.tile[k, l].TileType = TileID.HallowSandstone;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<HardenedCreamsand>()) {
							Main.tile[k, l].TileType = TileID.HallowHardenedSand;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<CreamGrassMowed>()) {
							Main.tile[k, l].TileType = TileID.GolfGrassHallowed;
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