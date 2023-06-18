using Microsoft.Xna.Framework;
using Terraria.Audio;
using TheConfectionRebirth;
using TheConfectionRebirth.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace TheConfectionRebirth.ModSupport.Thorium.Projectiles
{
	[ExtendsFromMod("ThoriumMod")]
	public class NeapoliniteMegaphonePro2 : ThoriumMod.Projectiles.Bard.BardProjectile {

		public override string Texture => "TheConfectionRebirth/ModSupport/Thorium/Projectiles/NeapoliniteMegaphonePro";
		public override ThoriumMod.BardInstrumentType InstrumentType => ThoriumMod.BardInstrumentType.Electronic;

		public override void SetBardDefaults() {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 70;
			Projectile.friendly = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			width = 20;
			height = 20;
			return true;
		}

		public override void AI() {
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 0f) {
				Projectile projectile = Projectile;
				projectile.scale += 0.275f;
				int numDusts = 30;
				for (int i = 0; i < numDusts; i++) {
					Vector2 offset = Utils.RotatedBy(-Utils.RotatedBy(Vector2.UnitY, (double)((float)i * ((float)Math.PI * 2f) / (float)numDusts), default(Vector2)) * new Vector2(4f, 10f) * Projectile.scale, (double)Utils.ToRotation(Projectile.velocity), default(Vector2));
					Dust obj = Dust.NewDustPerfect(Projectile.Center + offset, 138, (Vector2?)Utils.SafeNormalize(offset, Vector2.UnitY), 0, default(Color), 1f);
					obj.scale = 0.75f;
					obj.noGravity = true;
				}
				Projectile.ai[1] = -3f;
			}
			Projectile.position = Projectile.Center;
			Projectile.Size = (new Vector2(20f * Projectile.scale));
			Projectile.Center = (Projectile.position);
		}
	}
}