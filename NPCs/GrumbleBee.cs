using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
	public class GrumbleBee : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GoldButterfly];
			Main.npcCatchable[Type] = true;

			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
			NPCID.Sets.TownCritter[Type] = true;

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
		}

		public override void SetDefaults() 
		{
			NPC.width = 10;
			NPC.height = 10;
			NPC.aiStyle = 65;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.npcSlots = 0.25f;
			NPC.noGravity = true;
			NPC.catchItem = ModContent.ItemType<Items.GrumbleBee>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.GrumbleBee")
			});
		}

		public override void FindFrame(int frameHeight)
		{
			int num = 7;
			NPC.rotation = NPC.velocity.X * 0.3f;
			NPC.spriteDirection = NPC.direction;
			NPC.frameCounter = NPC.frameCounter + 1.0 + (double)((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) / 2f);
			if (NPC.frameCounter < (double)num)
			{
				NPC.frame.Y = 0;
			}
			else if (NPC.frameCounter < (double)(num * 2))
			{
				NPC.frame.Y = frameHeight;
			}
			else if (NPC.frameCounter < (double)(num * 3))
			{
				NPC.frame.Y = frameHeight * 2;
			}
			else
			{
				NPC.frame.Y = frameHeight;
				if (NPC.frameCounter >= (double)(num * 4 - 1))
				{
					NPC.frameCounter = 0.0;
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) 
		{
			return ConfectionGlobalNPC.SpawnNPC_ConfectionNPC(spawnInfo, Type);
		}

		public override void HitEffect(NPC.HitInfo hit) 
		{
			if (Main.netMode == NetmodeID.Server) 
			{
				return;
			}

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 6; i++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<ChocolateBlood>(), 2 * hit.HitDirection, -2f);
					if (Main.rand.NextBool(2))
					{
						Main.dust[dustID].noGravity = true;
						Main.dust[dustID].scale = 1.5f * NPC.scale;
					}
					else
					{
						Main.dust[dustID].scale = 0.8f * NPC.scale;
					}
				}
			}
		}
	}
}
