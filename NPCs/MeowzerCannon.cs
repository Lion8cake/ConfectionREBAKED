using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using TheConfectionRebirth.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;

namespace TheConfectionRebirth.NPCs
{
	public class MeowzerCannon : ModNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults()
		{
			NPC.width = 20;
			NPC.height = 20;
			NPC.aiStyle = -1;
			NPC.lifeMax = 75;
			NPC.defense = 16;
			NPC.knockBackResist = 0f;
			NPC.npcSlots = 1f;
			NPC.noGravity = true;
			NPC.netAlways = true;
			NPC.HitSound = SoundID.NPCHit38;
			NPC.DeathSound = null; //played manually so it isnt heard when despawning
		}

		public override void OnSpawn(IEntitySource source)
		{
			NPC.lifeMax = NPC.life = 150;
		}

		public override void AI()
		{
			if (NPC.ai[0] < 0 || NPC.ai[0] > Main.maxNPCs || !Main.npc[(int)NPC.ai[0]].active)
			{
				NPC.life = int.MinValue;
				NPC.checkDead();
			}
			else
			{
				NPC.position = Main.npc[(int)NPC.ai[0]].Center + new Vector2(Main.npc[(int)NPC.ai[0]].spriteDirection == -1 ? 10f : -20f, -55f);
			}
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 50; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<PastryDust>(), 1.25f * (float)hit.HitDirection, -2.5f);
				}
				for (int j = 0; j < 2; j++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MeowzerCannonGore").Type);
				}
				SoundEngine.PlaySound(SoundID.NPCDeath41, NPC.Center);
			}
			else
			{
				for (int i = 0; i < hit.Damage / (double)NPC.lifeMax * 2.5; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<PastryDust>(), hit.HitDirection, -1f);
				}
			}
		}

		public override bool CheckActive() 
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return false;
		}
	}
}
