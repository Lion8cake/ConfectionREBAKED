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
	public class ChocolateFrog : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Frog];
			Main.npcCatchable[Type] = true;

			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
			NPCID.Sets.TownCritter[Type] = true;

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
		}

		public override void SetDefaults() {
			NPC.width = 12;
			NPC.height = 10;
			NPC.aiStyle = 7;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.catchItem = ModContent.ItemType<Items.ChocolateFrog>();
			AIType = NPCID.Frog;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<ChocolateFrogBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.ChocolateFrog")
			});
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			//if (spawnInfo.Player.ZoneOverworldHeight && Main.dayTime && spawnInfo.Player.ZoneJungle && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive()) {
			//	return 0.75f;
			//}
			return 0f;
		}

		public override void PostAI()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && !NPC.noTileCollide && NPC.lifeMax > 1 && Collision.SwitchTiles(NPC.position, NPC.width, NPC.height, NPC.oldPosition, 2))
			{
				NPC.ai[0] = 1f;
				NPC.ai[1] = 400f;
				NPC.ai[2] = 0f;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;
			if (NPC.wet)
			{
				NPC.frameCounter = 0.0;
				if (NPC.velocity.X > 0.25f || NPC.velocity.X < -0.25f)
				{
					NPC.frame.Y = frameHeight * 10;
				}
				else if (NPC.velocity.X > 0.15f || NPC.velocity.X < -0.15f)
				{
					NPC.frame.Y = frameHeight * 11;
				}
				else
				{
					NPC.frame.Y = frameHeight * 12;
				}
			}
			else if (NPC.velocity.Y == 0f)
			{
				if (NPC.velocity.X == 0f)
				{
					NPC.frameCounter += 1.0;
					if (NPC.frameCounter > 6.0)
					{
						NPC.frameCounter = 0.0;
						NPC.frame.Y += frameHeight;
					}
					if (NPC.frame.Y > frameHeight * 5)
					{
						NPC.frame.Y = 0;
					}
					return;
				}
				NPC.frameCounter += 1.0;
				int num = 6;
				if (NPC.frameCounter < (double)num)
				{
					NPC.frame.Y = 0;
					return;
				}
				if (NPC.frameCounter < (double)(num * 2))
				{
					NPC.frame.Y = frameHeight * 6;
					return;
				}
				if (NPC.frameCounter < (double)(num * 3))
				{
					NPC.frame.Y = frameHeight * 8;
					return;
				}
				NPC.frame.Y = frameHeight * 9;
				if (NPC.frameCounter >= (double)(num * 4 - 1))
				{
					NPC.frameCounter = 0.0;
				}
			}
			else if (NPC.velocity.Y > 0f)
			{
				NPC.frame.Y = frameHeight * 9;
			}
			else
			{
				NPC.frame.Y = frameHeight * 8;
			}
		}

		public override void HitEffect(NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server) {
				return;
			}

			if (NPC.life > 0)
			{
				for (int num416 = 0; (double)num416 < hit.Damage / (double)NPC.lifeMax * 20.0; num416++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<ChocolateBlood>(), hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int num417 = 0; num417 < 10; num417++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<ChocolateBlood>(), 2 * hit.HitDirection, -2f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ChocolateFrogGore1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ChocolateFrogGore2").Type);
			}
		}
	}
}
