using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class RocketPopExplosion : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.friendly = true;
			Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = -1;
			Projectile.scale = 0.7f;
        }

		public override void OnSpawn(IEntitySource source)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			int iD = DustID.Torch;
			for (int i = 0; i < 15; i++)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1f);
			}
			for (int j = 0; j < 10; j++)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, iD, 0f, 0f, 100, default(Color), 2f);
				Main.dust[dust].noGravity = true;
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, iD, 0f, 0f, 100, default(Color), 1.5f);
			}
		}

		public override void AI()
        {
			if (++Projectile.frameCounter >= 3)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 5)
				{
					Projectile.Kill();
					Projectile.frame = 0;
				}
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			Color color = Color.White;
			return color;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return false;
		}
	}
}