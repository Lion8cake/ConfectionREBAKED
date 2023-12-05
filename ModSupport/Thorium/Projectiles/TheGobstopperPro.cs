using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.ModSupport.Thorium.Items.Tools;
using ThoriumMod.Projectiles;

namespace TheConfectionRebirth.ModSupport.Thorium.Projectiles;

public sealed class TheGobstopperPro : ModProjectile {
	public override LocalizedText DisplayName => ModContent.GetInstance<TheGobstopper>().DisplayName;

	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.BobberReinforced);
	}

	public override bool PreDrawExtras() {
		Vector2? playerOffset = null;
		if (Main.player[Projectile.owner].HeldItem.type == ModContent.ItemType<TheGobstopper>()) {
			playerOffset = new Vector2(52f, 28f);
		}

		ProjectileExtras.DrawFishingLine(Projectile.whoAmI, new Color(133, 148, 169, 100), playerOffset, new Vector2(0f, -6f));
		return false;
	}
}
