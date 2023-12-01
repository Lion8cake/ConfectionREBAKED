using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.ModSupport.Thorium.Items.Weapons;
using ThoriumMod.Projectiles.Scythe;

namespace TheConfectionRebirth.ModSupport.Thorium.Projectiles;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class NeapoliniteScythePro : ScythePro {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override LocalizedText DisplayName => ModContent.GetInstance<NeapoliniteScythe>().DisplayName;

	public override void SafeSetDefaults() {
		Projectile.width = 120;
		Projectile.height = 120;
		Projectile.idStaticNPCHitCooldown = 15;

		dustCount = 2;
		dustType = ModContent.DustType<NeapoliniteDust>();
		dustOffset = new Vector2(-8f, 8f);
	}

	public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex) {
		dust.scale = 1.2f;
	}
}
