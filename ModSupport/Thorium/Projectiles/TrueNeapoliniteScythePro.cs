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
	public class TrueNeapoliniteScythePro : ThoriumMod.Projectiles.Scythe.ScythePro {

		public override void SafeSetStaticDefaults() {
			// DisplayName.SetDefault("True Neapolinite Scythe");
		}

		public override void SafeSetDefaults() {
			Projectile.Size = (new Vector2(128f));
			Projectile.idStaticNPCHitCooldown = 8;
			dustCount = 3;
			dustType = ModContent.DustType<NeapoliniteCrumbs>();
			dustOffset = new Vector2(-12f, 12f);
		}

		public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex) {
			dust.scale = 1.2f;
			dust.alpha = 100;
			if (scytheIndex == 1) {
				dust.active = false;
				Dust obj = Dust.NewDustPerfect(position, 88, (Vector2?)null, 0, default(Color), 1.2f);
				obj.velocity *= 0f;
				obj.noGravity = true;
			}
		}
	}
}