using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Pets.BirdnanaLightPet
{
	public class BirdnanaLightPetProjectile : ModProjectile
	{
		public ref float AIFadeProgress => ref Projectile.ai[0];
		public ref float AIDashCharge => ref Projectile.ai[1];

		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 4;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.timeLeft *= 5;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 26;
			AIType = 492;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (!player.active)
			{
				Projectile.active = false;
				return;
			}

			if (!player.dead && player.HasBuff(ModContent.BuffType<BirdnanaLightPetBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			if (!Main.dedServ)
			{
				Lighting.AddLight(Projectile.Center, Projectile.Opacity * 2.48f, Projectile.Opacity * 1.99f, Projectile.Opacity * 0.05f);
			}
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 6)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame > 3)
			{
				Projectile.frame = 0;
			}
		}
	}
}