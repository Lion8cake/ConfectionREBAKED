using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class SacchariteDart : ModProjectile
    {
		public override void SetStaticDefaults()
		{
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
		}

		public override void SetDefaults()
        {
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Ranged;
		}

		public override void OnSpawn(IEntitySource source)
		{
            Projectile.ai[2] = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
 		}

		public override void AI()
        {
			if (Projectile.localAI[1] < 5f)
			{
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
				Projectile.localAI[1] += 1f;
			}
			else
			{
				Projectile.rotation = (Projectile.rotation * 2f + (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f) / 3f;
			}
			float CenterX = Projectile.Center.X;
            float CenterY = Projectile.Center.Y;
            float Distanse = 400f;
            bool CheckDistanse = false;
            for (int MobCounts = 0; MobCounts < 200; MobCounts++)
            {
                if (Main.npc[MobCounts].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[MobCounts].Center, 1, 1))
                {
                    float Position1 = Main.npc[MobCounts].position.X + Main.npc[MobCounts].width / 2;
                    float Position2 = Main.npc[MobCounts].position.Y + Main.npc[MobCounts].height / 2;
                    float Position3 = Math.Abs(Projectile.position.X + Projectile.width / 2 - Position1) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - Position2);
                    if (Position3 < Distanse)
                    {
                        Distanse = Position3;
                        CenterX = Position1;
                        CenterY = Position2;
                        CheckDistanse = true;
                    }
                }
            }
            if (CheckDistanse)
            {
                float Speed = Projectile.ai[2];
                Vector2 FinalPos = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
                float NewPosX = CenterX - FinalPos.X;
                float NewPosY = CenterY - FinalPos.Y;
                float FinPos = (float)Math.Sqrt(NewPosX * NewPosX + NewPosY * NewPosY);
                FinPos = Speed / FinPos;
                NewPosX *= FinPos;
                NewPosY *= FinPos;
                Projectile.velocity.X = (Projectile.velocity.X * 10f + NewPosX) / 11f;
                Projectile.velocity.Y = (Projectile.velocity.Y * 10f + NewPosY) / 11f;
            }
        }

		public override Color? GetAlpha(Color lightColor)
		{
			if (Projectile.alpha == 0)
			{
				return new Color(255, 255, 255, 200);
			}
			return new Color(0, 0, 0, 0);
		}
	}
}