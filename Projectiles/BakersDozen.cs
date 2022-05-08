using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
public class BakersDozen : ModProjectile
{
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Baker's Dozen");
	}

	public override void SetDefaults()
	{
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.aiStyle = 3;
		Projectile.timeLeft = 600;
		AIType = 52;
		Projectile.DamageType = DamageClass.Melee;
	}
}
}
