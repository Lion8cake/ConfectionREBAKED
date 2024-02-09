using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.ModSupport.Thorium.Items.Weapons;
using ThoriumMod.Projectiles.Scythe;

namespace TheConfectionRebirth.ModSupport.Thorium.Projectiles;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class TrueNeapoliniteScythePro : ScythePro {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override LocalizedText DisplayName => ModContent.GetInstance<TrueNeapoliniteScythe>().DisplayName;

	public override void SafeSetDefaults() {
		Projectile.width = 128;
		Projectile.height = 128;
		Projectile.idStaticNPCHitCooldown = 8;

		dustCount = 3;
		dustType = ModContent.DustType<NeapoliniteDust>();
		dustOffset = new Vector2(-12f, 12f);
	}

	public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex) {
		if (scytheIndex == 0) {
			dust.scale = 1.2f;
			dust.alpha = 100;
		}
		else {
			dust.active = false;

			dust = Dust.NewDustPerfect(position, DustID.GemSapphire, Scale: 1.2f);
			dust.velocity = Vector2.Zero;
			dust.noGravity = true;
		}
	}
}
