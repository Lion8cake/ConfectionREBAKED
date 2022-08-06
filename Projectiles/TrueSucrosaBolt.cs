using TheConfectionRebirth.Dusts;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace TheConfectionRebirth.Projectiles
{
    public class TrueSucrosaBolt : ModProjectile
    {
        public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = 27;
			Projectile.penetrate = 1;
			Projectile.light = 0.2f;
			Projectile.alpha = 0;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			if (this.Projectile.localAI[1] >= 15f)
			{
				return new Color(255, 255, 255, this.Projectile.alpha);
			}
			if (this.Projectile.localAI[1] < 5f)
			{
				return Color.Transparent;
			}
			int num7 = (int)((this.Projectile.localAI[1] - 5f) / 10f * 255f);
			return new Color(num7, num7, num7, num7);
		}
		
		public override void AI()
        {
			if (Projectile.localAI[1] > 5f)
			{
				int num208 = Main.rand.Next(3);
				if (num208 == 0)
				{
					num208 = 15;
				}
				else if (num208 == 1)
				{
					num208 = 57;
				}
				else if (num208 == 2)
				{
					num208 = 58;
				}
				int num209 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<NeapoliniteCrumbs>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Dust dust = Main.dust[num209];
				dust.velocity *= 0.1f;
			}
		}
		
        public override void Kill(int timeLeft)
        {
			for (int num394 = 4; num394 < 24; num394++)
			{
				float num395 = Projectile.oldVelocity.X * (30f / (float)num394);
				float num396 = Projectile.oldVelocity.Y * (30f / (float)num394);
				int num397 = Main.rand.Next(3);
				if (num397 == 0)
				{
					num397 = 15;
				}
				else if (num397 == 1)
				{
					num397 = 57;
				}
				else
				{
					num397 = 58;
				}
				int num398 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<NeapoliniteCrumbs>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Main.dust[num398].velocity *= 1.5f;
				Main.dust[num398].noGravity = true;
			}
		}
    }
}