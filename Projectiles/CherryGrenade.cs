using Microsoft.Xna.Framework;
using Terraria.Audio;
using TheConfectionRebirth;
using TheConfectionRebirth.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TheConfectionRebirth.Projectiles
{
	public class CherryGrenade : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(30);
			AIType = 30;
			Projectile.timeLeft = 200;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.DamageType = DamageClass.Ranged;
		}
	
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}
	
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 15; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
				Dust obj = Main.dust[dustIndex];
				obj.velocity *= 1.4f;
			}
			for (int j = 0; j < 10; j++)
			{
				int dustIndex2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
				Main.dust[dustIndex2].noGravity = true;
				Dust obj2 = Main.dust[dustIndex2];
				obj2.velocity *= 5f;
				dustIndex2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
				Dust obj3 = Main.dust[dustIndex2];
				obj3.velocity *= 3f;
			}
			if (Main.myPlayer != Projectile.owner)
			{
				return;
			}

			for (int i = 0; i < 5; i++)
			{
				Projectile.NewProjectile(new EntitySource_Misc("Cherry shard from cherry bomb"), Projectile.Center.X, Projectile.Center.Y, -8 + Main.rand.Next(0, 17), -8 + Main.rand.Next(0, 17), ModContent.ProjectileType<CherryShard>(), 24, 1f, Main.myPlayer, 0f, 0f);
			}
		}
	}
}