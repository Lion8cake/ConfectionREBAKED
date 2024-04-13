using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.ModSupport.Thorium.Items.Weapons;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;

namespace TheConfectionRebirth.ModSupport.Thorium.Projectiles;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public abstract class BaseNeapoliniteMegaphonePro : BardProjectile {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public abstract int DustType { get; }

    public override LocalizedText DisplayName => ModContent.GetInstance<NeapoliniteMegaphone>().DisplayName;

	public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;

	public override string Texture => "TheConfectionRebirth/Assets/Empty";

	public override void SetBardDefaults()
    {
        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.aiStyle = 0;
        Projectile.scale = 1f;
        Projectile.alpha = 255;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 70;
        Projectile.friendly = true;
    }

    public override void AI()
    {
        Projectile.ai[1] += 1f;
        if (Projectile.ai[1] >= 0f)
        {
            Projectile.scale += 0.275f;
            Projectile.ai[1] = -3f;

            const int numDusts = 30;
            for (int i = 0; i < numDusts; i++)
            {
                var offset = (-Vector2.UnitY.RotatedBy(i * MathF.Tau / numDusts)
                        * new Vector2(4f, 10f)
                        * Projectile.scale).RotatedBy(
                    Projectile.velocity.ToRotation());

                var dust = Dust.NewDustPerfect(Projectile.Center + offset, DustType, offset.SafeNormalize(Vector2.UnitY));
                dust.scale = 0.75f;
                dust.noGravity = true;
            }
        }

        Projectile.position = Projectile.Center;
        Projectile.Size = new Vector2(20f * Projectile.scale);
        Projectile.Center = Projectile.position;

		NPC closestNPC = null;
		float closestDistance = -0.0f;
		foreach (var npc in Main.ActiveNPCs) {
			float distance = npc.DistanceSQ(Projectile.Center);

			if (npc.CanBeChasedBy() && distance > closestDistance) {
				closestNPC = npc;
				closestDistance = distance;
			}
		}
		closestDistance = MathF.Sqrt(closestDistance);

		Projectile.velocity = (Projectile.Center - closestNPC.Center).SafeNormalize(default) * MathF.Pow(closestDistance, 0.69f);
    }
}

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class NeapoliniteMegaphonePro1 : BaseNeapoliniteMegaphonePro {
	public override int DustType => DustID.GemAmethyst;
}
[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class NeapoliniteMegaphonePro2 : BaseNeapoliniteMegaphonePro {
	public override int DustType => DustID.GemDiamond;
}
[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class NeapoliniteMegaphonePro3 : BaseNeapoliniteMegaphonePro {
	public override int DustType => DustID.GemAmber;
}
