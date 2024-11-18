using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
    public class CreamBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 900;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
        }

        public override void AI()
        {
			for (int i = 0; i < 2; i++)
            {
                Vector2 pos = Projectile.position;
                Projectile.alpha = 255;
                int dustID = Dust.NewDust(pos, 1, 1, ModContent.DustType<ChocolateFlame>());
				Dust dust = Main.dust[dustID];
				dust.position = pos;
                dust.scale = Main.rand.Next(70, 110) * 0.025f;
				dust.noGravity = true;
                dust.velocity *= 0.2f;
            }
        }

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.timeLeft > 895)
				return false;
			return null;
		}

		public override bool CanHitPlayer(Player target)
		{
			if (Projectile.timeLeft > 895)
				return false;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.ai[0] > 0f && Projectile.ai[1] > 0f && Projectile.timeLeft > 840)
			{
				return false;
			}
            SplitBeam(-1);
            return true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            SplitBeam(target.whoAmI);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			SplitBeam(-1);
		}

		private void SplitBeam(int pastHitNPC)
        {
			if (Projectile.ai[0] == 2f)
			{
				return;
			}

			float homingOnNPC = 0f;

			Vector2? velcoity = null;
			float maxDistance = 1000f;
			bool foundAnNPC = false;
			Vector2 center = Projectile.position;

			Vector2? velcoity2 = velcoity;
			float maxDistance2 = maxDistance;
			bool foundNPCforSecond = false;
			Vector2 center2 = center;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
                NPC npc = Main.npc[i];
				if (npc.CanBeChasedBy(Projectile, false) && Collision.CanHitLine(Projectile.position, 1, 1, npc.position, 1, 1) && npc.whoAmI != pastHitNPC)
				{
					float npcDistance = Math.Abs(Projectile.position.X - npc.position.X) + Math.Abs(Projectile.position.Y - npc.position.Y);
					if (npcDistance < maxDistance)
					{
						maxDistance = npcDistance;
						center = npc.position;
						foundAnNPC = true;
					}
					else if (npcDistance < maxDistance2)
					{
						maxDistance2 = npcDistance;
						center2 = npc.position;
						foundNPCforSecond = true;
					}
				}
			}
			if (foundAnNPC)
			{
				Vector2 newPos = center - Projectile.position;
				float finalAngle = (float)Math.Sqrt(newPos.X * newPos.X + newPos.Y * newPos.Y);
				finalAngle = 3f / finalAngle;
				newPos *= finalAngle;
				velcoity = (newPos) / 2f;
			}
			if (foundNPCforSecond)
			{
				Vector2 newPos2 = center2 - Projectile.position;
				float finalAngle2 = (float)Math.Sqrt(newPos2.X * newPos2.X + newPos2.Y * newPos2.Y);
				finalAngle2 = 3f / finalAngle2;
				newPos2 *= finalAngle2;
				velcoity2 = (newPos2) / 2f;
			}

			Vector2 newVel = Vector2.Zero;
			Vector2 newVel2 = Vector2.Zero;
			Vector2 fakeVel = Projectile.velocity;
			float speed = 3f;
			bool flag = false;
			bool flag2 = false;
			if (Projectile.velocity.X < 0f)
			{
				fakeVel.X = -Projectile.velocity.X;
				flag = true;
			}
			if (Projectile.velocity.Y < 0f)
			{
				fakeVel.Y = -Projectile.velocity.Y;
				flag2 = true;
			}
			if (fakeVel.X > fakeVel.Y)
            {
				newVel = new Vector2(flag ? speed : -speed, speed);
                newVel2 = new Vector2(flag ? speed : -speed, -speed);
			}
			else
            {
				newVel = new Vector2(speed, flag2 ? speed : -speed);
                newVel2 = new Vector2(-speed, flag2 ? speed : -speed);
			}
			
			if (velcoity == null)
            {
                velcoity = newVel;
            }
			else
				homingOnNPC = 1f;
			if (velcoity2 == null)
			{
				velcoity2 = newVel2;
			}
			else
				homingOnNPC = 1f;

			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, (Vector2)velcoity, Projectile.type, (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner, Projectile.ai[0] + 1f, homingOnNPC);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, (Vector2)velcoity2, Projectile.type, (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner, Projectile.ai[0] + 1f, homingOnNPC);
        }
	}
}