using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.ModSupport.Thorium.Dusts;
using TheConfectionRebirth.ModSupport.Thorium.Items.Weapons;
using ThoriumMod;
using ThoriumMod.Projectiles;

namespace TheConfectionRebirth.ModSupport.Thorium.Projectiles;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public abstract class BaseNeapoliniteScepter<T> : ThoriumProjectile where T : ModDust {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override LocalizedText DisplayName => ModContent.GetInstance<NeapoliniteScepter>().DisplayName;

	public override string Texture => "TheConfectionRebirth/Assets/Empty";

	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 2;
	}

	public override void SetDefaults() {
		Projectile.width = 14;
		Projectile.height = 14;

		Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
		Projectile.friendly = true;
		Projectile.penetrate = 1;

		Projectile.timeLeft = 600;
		Projectile.aiStyle = 1;
		AIType = 14;
	}

	public override Color? GetAlpha(Color lightColor) {
		return new Color(255, 255, 255, 100) * 0.75f;
	}

	public override void AI() {
		for (int i = 0; i < 3; i++) {
			var position = Projectile.Center - Projectile.velocity / 3f * i;

			var dust = Dust.NewDustDirect(position, Projectile.width, Projectile.height, ModContent.DustType<T>(), Alpha: 150, Scale: 1.75f);
			dust.velocity = Vector2.Zero;
			dust.noGravity = true;
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		for (int j = 0; j < 10; j++) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<T>(), Main.rand.Next(-2, 2), Main.rand.Next(-2, 2), 150, Scale: 1.5f);
			dust.noGravity = true;
		}

		return true;
	}
}

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class NeapoliniteScepterVanilla : BaseNeapoliniteScepter<NeapoliniteVanillaDust> { }
[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class NeapoliniteScepterChocolate : BaseNeapoliniteScepter<NeapoliniteChocolateDust> { }
[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class NeapoliniteScepterStrawberry : BaseNeapoliniteScepter<NeapoliniteStrawberryDust> { }
