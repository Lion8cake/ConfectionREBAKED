using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
public class BearClaw : ModProjectile
{
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Bear Claw");
	}

	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.aiStyle = 3;
		Projectile.timeLeft = 600;
		AIType = 52;
		Projectile.DamageType = DamageClass.Melee;
	}
}
}
