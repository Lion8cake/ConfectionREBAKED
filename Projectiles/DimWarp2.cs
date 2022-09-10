using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    class DimWarp2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Name = "Dimensional Warp";
            Projectile.width = 32;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 10;
            Projectile.ai[0] = 0;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            
            
            Player test = Main.player[Projectile.owner];
            float x1 = Projectile.position.X;
            float y1 = Projectile.position.Y;
            float x2 = Projectile.position.X + Projectile.width;
            float y2 = Projectile.position.Y + Projectile.height;
            if (test != null && test.active && test.position.X + test.width > x1 && test.position.X < x2 && test.position.Y + test.height > y1 && test.position.Y < y2)
            {
                OnHitPlayer(test)
            }
        }

        const float warpRad = 144;
        public override bool CanHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Vector2 diff = target.Center - Projectile.Center;
            float detectRad = warpRad + MathF.Max(target.width, target.height);
            if(target.boss == false && diff.X > -detectRad && diff.X < detectRad && diff.Y > -detectRad && diff.Y < detectRad && diff.LengthSquared() < detectRad * detectRad)
            {
                Projectile warppoint = Main.player[Projectile.owner].GetModPlayer<ConfectionPlayer>().DimensionalWarp;
                target.position.X = warppoint.position.X;
                target.position.Y = warppoint.position.Y;
                Projectile.ai[0] = 1;
                Projectile.Kill();
            }
            return false;
        }
        void OnHitPlayer(Player target)
        {
            //if (Main.player[Main.myPlayer] == target)
            //{
                Projectile warppoint = Main.player[Projectile.owner].GetModPlayer<ConfectionPlayer>().DimensionalWarp;
                target.Teleport(warppoint.position, 1);
                if (target.HasBuff(ModContent.BuffType<Buffs.GoneBananas>()))
                {
                    target.Hurt(PlayerDeathReason.ByCustomReason("DimensionSplit"), (int)((target.statLifeMax2 + target.statDefense) * (target.endurance + 1) / 7), 0);
                }
                target.AddBuff(ModContent.BuffType<Buffs.GoneBananas>(), 360);
                Projectile.ai[0] = 1;
                Projectile.Kill();
            //}
        }
        public override bool PreKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Player owner = Main.player[Main.myPlayer];
                if (Projectile.ai[0] == 0)
                {
                    owner.Teleport(owner.GetModPlayer<ConfectionPlayer>().DimensionalWarp.position, 1);
                    owner.GetModPlayer<ConfectionPlayer>().DimensionalWarp.Kill();
                    if (owner.HasBuff(ModContent.BuffType<Buffs.GoneBananas>()))
                    {
                        owner.Hurt(PlayerDeathReason.ByCustomReason("DimensionSplit"), (int)((owner.statLifeMax2 + owner.statDefense) * (owner.endurance + 1) / 7), 0);
                    }
                    owner.AddBuff(ModContent.BuffType<Buffs.GoneBananas>(), 360);
                }
                owner.GetModPlayer<ConfectionPlayer>().DimensionalWarp.Kill();
            }
            return true;
        }
    }
}
