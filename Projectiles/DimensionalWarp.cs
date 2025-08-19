using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs;

namespace TheConfectionRebirth.Projectiles
{
	public class DimensionalWarp : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 24;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 38;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = int.MaxValue;
			Projectile.damage = 0;
		}

		public override void AI()
		{
			if (++Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame % 8 == 0)
					Projectile.frame = Projectile.ai[0] == 2 ? 8 : Projectile.ai[0] == 3 ? 16 : 0;
			}
			if (Projectile.velocity != Vector2.Zero)
			{
				Projectile.velocity = Vector2.Zero;
			}
			if (Projectile.ai[0] == 3 && Projectile.frame < 16)
			{
				Projectile.frame = 16;
			}
			if (Projectile.ai[0] == 2 && Projectile.frame < 8)
			{
				Projectile.frame = 8;
			}

			Player owner = Main.player[Projectile.owner];
			if (owner.ownedProjectileCounts[Type] < 2)
			{
				return;
			}
			bool flag = false;
			Player telePlayer = null;
			NPC teleNPC = null;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (!npc.active || npc.boss || npc.type == NPCID.TargetDummy)
				{
					continue;
				}
				if (Collision.CheckAABBvAABBCollision(Projectile.position, new Vector2(Projectile.width, Projectile.height), npc.position, new Vector2(npc.width, npc.height)))
				{
					flag = true; 
					teleNPC = npc;
				}
			}
			
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player player = Main.player[i];
                if (!player.active)
                {
					continue;
                }
				if (Collision.CheckAABBvAABBCollision(Projectile.position, new Vector2(Projectile.width, Projectile.height), player.position, new Vector2(player.width, player.height)))
				{
					flag = true;
					telePlayer = player;
				}
			}

			if (flag)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.active && projectile.type == Type && projectile != Projectile && projectile.owner == Projectile.owner)
					{
						if (teleNPC != null)
						{
							teleNPC.Teleport(projectile.position);
							if (Projectile.ai[0] < 2)
							{
								if (teleNPC.HasBuff(ModContent.BuffType<GoneBananas>()))
								{
									teleNPC.StrikeNPC(teleNPC.CalculateHitInfo(teleNPC.life / 2, 0, false, 0));
								}
								teleNPC.AddBuff(ModContent.BuffType<GoneBananas>(), 360);
							}
						}
						else
						{
							telePlayer.Teleport(projectile.position, 1);
							if (Projectile.ai[0] < 2)
							{
								if (telePlayer.HasBuff(ModContent.BuffType<GoneBananas>()))
								{
									telePlayer.Hurt(PlayerDeathReason.ByCustomReason("DimensionSplit"), telePlayer.statLife / 2, 0);
								}
								telePlayer.AddBuff(ModContent.BuffType<GoneBananas>(), 360);
							}
						}
						projectile.Kill();
						Projectile.Kill();
					}
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.ai[0] == 2 || Projectile.ai[0] == 3)
			{
				lightColor = Color.White;
				return true;
			}
			if (Projectile.ai[0] == 0)
			{
				lightColor = Main.hslToRgb(0.12f, 1f, 0.5f);
			}
			else if (Projectile.ai[0] == 1)
			{
				lightColor = Main.hslToRgb(0.52f, 1f, 0.6f);
			}
			return true;
		}
	}
}
