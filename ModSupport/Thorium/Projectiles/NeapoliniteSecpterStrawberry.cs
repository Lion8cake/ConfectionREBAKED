using Microsoft.Xna.Framework;
using Terraria.Audio;
using TheConfectionRebirth;
using TheConfectionRebirth.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.ModSupport.Thorium.Projectiles
{
	[ExtendsFromMod("ThoriumMod")]
	public class NeapoliniteScepterStrawberry : ThoriumMod.Projectiles.ThoriumProjectile {
		
		public override string Texture => "TheConfectionRebirth/ModSupport/Thorium/Projectiles/NeapoliniteScytheEffect";

		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults() {
			Projectile.DamageType = ((DamageClass)(object)ThoriumMod.ThoriumDamageBase<ThoriumMod.HealerDamage>.Instance);
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			AIType = (14);
		}

		public override Color? GetAlpha(Color lightColor) {

			return new Color(255, 255, 255, 100) * 0.75f;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			for (int num363 = 0; num363 < 3; num363++) {
				float num365 = Projectile.velocity.X / 3f * (float)num363;
				float num367 = Projectile.velocity.Y / 3f * (float)num363;
				int num369 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.NeapoliniteStrawberryDust>(), 0f, 0f, 150, default(Color), 1.75f);
				Main.dust[num369].position.X = Projectile.Center.X - num365;
				Main.dust[num369].position.Y = Projectile.Center.Y - num367;
				Dust obj2 = Main.dust[num369];
				obj2.velocity *= 0f;
				Main.dust[num369].noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			for (int j = 0; j < 10; j++) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.NeapoliniteStrawberryDust>(), (float)Main.rand.Next(-2, 2), (float)Main.rand.Next(-2, 2), 150, default(Color), 1.5f);
				Main.dust[dust].noGravity = true;
			}
			return true;
		}
	}
}