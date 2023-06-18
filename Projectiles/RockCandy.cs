using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
	public class RockCandy : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.aiStyle = 14;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 360;
			Projectile.DamageType = DamageClass.Magic;
		}
	
		public override void AI()
		{
			Projectile.velocity.X *= 0.9f;
			Projectile.velocity.Y *= 0.99f;
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
		}
		
		public override void Kill(int timeLeft)
		{
			float spread = 1.566f;
			double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - (double)(spread / 2f);
			double deltaAngle = spread / 8f;
			if (Projectile.owner == Main.myPlayer)
			{
				for (int i = 0; i < 4; i++)
				{
					double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2.0 + (double)(32f * i);
					Projectile.NewProjectile(new EntitySource_Misc("Rock candy shard from rock candy"), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5.0), (float)(Math.Cos(offsetAngle) * 5.0), ModContent.ProjectileType<RockCandyShard>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner, Projectile.ai[0]);
					Projectile.NewProjectile(new EntitySource_Misc("Rock candy shard from rock candy"), Projectile.Center.X, Projectile.Center.Y, (float)((0.0 - Math.Sin(offsetAngle)) * 5.0), (float)((0.0 - Math.Cos(offsetAngle)) * 5.0), ModContent.ProjectileType<RockCandyShard>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner, Projectile.ai[0]);
				}
			}
		}
	}
}