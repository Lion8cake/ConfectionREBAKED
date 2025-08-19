using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth.Projectiles
{
	public class CookiestCookieBlock : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 0, 1)
				.WithOffset(10, 0f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.CompanionCubePet);
			ProjectileID.Sets.FallingBlockDoesNotFallThroughPlatforms[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 67;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.timeLeft *= 5;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			AIType = ProjectileID.DirtiestBlock;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];

			player.petFlagDirtiestBlock = false;

			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.CookiestBlockBuff>()))
			{
				player.GetModPlayer<ConfectionPlayer>().cookiestPet = true;
			}
			if (player.GetModPlayer<ConfectionPlayer>().cookiestPet)
			{
				Projectile.timeLeft = 2;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
	}
}
