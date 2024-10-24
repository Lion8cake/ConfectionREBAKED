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
			Projectile.localAI[0] = Main.rand.Next(1, 4);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			int iD = DustID.Torch;
			switch (Projectile.localAI[0])
			{
				case 1:
					iD = DustID.PinkTorch;
					break;
				case 2:
					iD = DustID.Smoke;
					break;
				case 3:
					iD = DustID.IceTorch;
					break;
			}
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
			switch (Projectile.localAI[0])
			{
				case 1:
					color = new Color(241, 97, 151);
					break;
				case 2:
					color = new Color(205, 214, 235);
					break;
				case 3:
					color = new Color(83, 210, 253);
					break;
			}
			return color;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return false;
		}
	}
}