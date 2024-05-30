using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.Projectiles {
	public class SugarWaterBottle : ModProjectile {

		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
		}

		public override bool? CanCutTiles() {
			return false;
		}

		public override void AI() {
			if (Main.windPhysics) {
				Projectile.velocity.X += Main.windSpeedCurrent * Main.windPhysicsStrength;
			}
			Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * (float)Projectile.direction;
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 10f) {
				Projectile.velocity.Y += 0.25f;
				Projectile.velocity.X *= 0.99f;
			}
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}
		}

		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Shatter, new Vector2((int)Projectile.position.X, (int)Projectile.position.Y));
			for (int num990 = 0; num990 < 5; num990++) {
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Glass);
			}
			for (int num991 = 0; num991 < 30; num991++) {
				int num992 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CreamWaterSplash>(), 0f, -2f, 0, default(Color), 1.1f);
				Main.dust[num992].alpha = 100;
				Main.dust[num992].velocity.X *= 1.5f;
				Dust dust230 = Main.dust[num992];
				Dust dust334 = dust230;
				dust334.velocity *= 3f;
			}

			if (Projectile.owner == Main.myPlayer) {
				int i2 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
				int j2 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
				ConfectionWorldGeneration.ConfectionConvert(i2, j2);
			}
		}
	}
}
