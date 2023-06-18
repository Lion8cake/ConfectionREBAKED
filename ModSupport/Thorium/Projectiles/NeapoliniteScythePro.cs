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
	public class NeapoliniteScythePro : ThoriumMod.Projectiles.Scythe.ScythePro {

		public override void SafeSetStaticDefaults() {
			// DisplayName.SetDefault("Neapolinite Scythe");
		}

		public override void SafeSetDefaults() {
			Projectile.Size = (new Vector2(120f));
			Projectile.idStaticNPCHitCooldown = 15;
			dustCount = 2;
			dustType = ModContent.DustType<NeapoliniteDust>();
			dustOffset = new Vector2(-8f, 8f);
		}

		public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex) {
			dust.scale = 1.2f;
		}
	}
}