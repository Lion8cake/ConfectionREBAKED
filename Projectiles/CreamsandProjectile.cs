using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.Projectiles {
	public class CreamsandProjectile : ModProjectile {
		public override void SetStaticDefaults() {
			ProjectileID.Sets.FallingBlockDoesNotFallThroughPlatforms[Type] = true;
			ProjectileID.Sets.ForcePlateDetection[Type] = true;
			ProjectileID.Sets.FallingBlockTileItem[Type] = new(ModContent.TileType<Creamsand>(), ModContent.ItemType<Items.Placeable.Creamsand>());
		}

		public override void SetDefaults() {
			Projectile.knockBack = 6f;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
		}

		public override void AI() {
			if (Main.rand.NextBool(2)) {
				int i = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.CreamsandDust>(), 0f, Projectile.velocity.Y * 0.5f);
				Main.dust[i].velocity.X *= 0.2f;
			}
			Projectile.velocity.Y += 0.41f;
			Projectile.rotation += 0.1f;
			if (Projectile.velocity.Y > 10f) {
				Projectile.velocity.Y = 10f;
			}
		}

		public override void OnKill(int timeLeft) {
			Point p = Projectile.Center.ToTileCoordinates();
			if (p.X >= 0 && p.X < Main.maxTilesX && p.Y >= 0 && p.Y < Main.maxTilesY) {
				Tile t = Main.tile[p.X, p.Y];
				if (t.IsHalfBlock && Projectile.velocity.Y > 0f && Math.Abs(Projectile.velocity.Y) > Math.Abs(Projectile.velocity.X)) {
					t = Main.tile[p.X, --p.Y];
				}
				if (Main.tileCut[t.TileType]) {
					WorldGen.KillTile(p.X, p.Y);
				}
				if (!Main.tileSolid[t.TileType] || TileID.Sets.IsATreeTrunk[t.TileType]) {
					Item.NewItem(WorldGen.GetItemSource_FromTileBreak(p.X, p.Y), p.X * 16, p.Y * 16, 16, 16, ModContent.ItemType<Items.Placeable.Creamsand>());
					SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
				}
				if (!t.HasTile && t.TileType != TileID.MinecartTrack) {
					Tile tBelow = Main.tile[p.X, p.Y + 1];
					if (tBelow.Slope != SlopeType.Solid) {
						tBelow.Slope = SlopeType.Solid;
					}
					if (tBelow.IsHalfBlock) tBelow.IsHalfBlock = false;
					WorldGen.PlaceTile(p.X, p.Y, ModContent.TileType<Tiles.Creamsand>(), forced: true);
					WorldGen.SquareTileFrame(p.X, p.Y);
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			fallThrough = !ProjectileID.Sets.FallingBlockDoesNotFallThroughPlatforms[Projectile.type];
			return true;
		}
		public override bool? CanDamage() => Projectile.localAI[1] != -1f;
	}
}
