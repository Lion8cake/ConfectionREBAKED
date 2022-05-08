using TheConfectionRebirth.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
public class CreamBolt : ModProjectile
{
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Cream Bolt");
	}

	public override void SetDefaults()
	{
		Projectile.width = 4;
		Projectile.height = 4;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = 10;
		Projectile.extraUpdates = 100;
		Projectile.timeLeft = 300;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 4;
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Projectile.damage = (int)((double)Projectile.damage * 1.25);
		Projectile.penetrate--;
		if (Projectile.penetrate <= 0)
		{
			Projectile.Kill();
		}
		else
		{
			Projectile.ai[0] += 0.1f;
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = 0f - oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = 0f - oldVelocity.Y;
			}
		}
		return false;
	}

	public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
	{
		if (target.type != 488)
		{
			Projectile.damage = (int)((double)Projectile.damage * 1.1);
		}
	}

	public override void AI()
	{
		Projectile.localAI[0] += 1f;
		if (Projectile.localAI[0] > 9f)
		{
			for (int num447 = 0; num447 < 4; num447++)
			{
				Vector2 vector33 = Projectile.position;
				vector33 -= Projectile.velocity * ((float)num447 * 0.25f);
				Projectile.alpha = 255;
				int num448 = Dust.NewDust(vector33, 1, 1, 133);
				Main.dust[num448].position = vector33;
				Main.dust[num448].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Dust obj = Main.dust[num448];
				obj.velocity *= 0.2f;
			}
		}
	}
}
}