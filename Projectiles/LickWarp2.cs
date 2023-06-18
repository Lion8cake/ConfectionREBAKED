using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    class LickWarp2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 10;
            Projectile.ai[0] = 0;
        }
		
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 8;
		}

        public override void AI()
        {			
			if (++Projectile.frameCounter >= 5) {
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 8) {
					Projectile.frame = 0;
				}
			}
        }
		
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.boss == false && target.lifeMax < 1000 && target.type != NPCID.TargetDummy)
            {
                Projectile warppoint = Main.player[Projectile.owner].GetModPlayer<ConfectionPlayer>().DimensionalWarp;
                target.position.X = warppoint.position.X;
                target.position.Y = warppoint.position.Y;
                target.HealEffect(1);
                Projectile.ai[0] = 1;
                Projectile.Kill();
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.player[Main.myPlayer] == target)
            {
                Projectile warppoint = Main.player[Projectile.owner].GetModPlayer<ConfectionPlayer>().DimensionalWarp;
                target.Teleport(warppoint.position, 1);
                target.HealEffect(1);
                Projectile.ai[0] = 1;
                Projectile.Kill();
            }
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
                }
                owner.GetModPlayer<ConfectionPlayer>().DimensionalWarp.Kill();
            }
            return true;
        }
    }
}
