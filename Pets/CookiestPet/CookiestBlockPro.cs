using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Pets.CreamyFoxPet;

namespace TheConfectionRebirth.Pets.CookiestPet
{
	public class CookiestBlockPro : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 1;
			Main.projPet[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DirtiestBlock);
			AIType = ProjectileID.DirtiestBlock;

			Projectile.width = 16;
			Projectile.height = 16;
		}

		public override bool PreAI()
		{
			Main.player[Projectile.owner].petFlagDirtiestBlock = false;
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (!player.dead && player.HasBuff(ModContent.BuffType<CookiestBlockBuff>()))
			{
				Projectile.timeLeft = 2;
			}
		}
	}
}
