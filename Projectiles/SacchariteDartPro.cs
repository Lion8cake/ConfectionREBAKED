using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class SacchariteDartPro : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 30;
            Projectile.aiStyle = 27;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            AIType = ProjectileID.IchorDart;
        }

        public override void AI()
        {
			Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : 1;
			Projectile.rotation = Projectile.velocity.ToRotation();
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
                float Speed = 4f;
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
    }
}