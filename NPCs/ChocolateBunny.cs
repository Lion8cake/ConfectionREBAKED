using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
	public class ChocolateBunny : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Bunny];
			Main.npcCatchable[Type] = true;

			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
			NPCID.Sets.TownCritter[Type] = true;

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
			{
				Velocity = 1f
			});
		}

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 20;
			NPC.aiStyle = 7;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.catchItem = ModContent.ItemType<Items.ChocolateBunny>();
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<ChocolateBunnyBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.ChocolateBunny")
			});
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			//if (spawnInfo.Player.ZoneOverworldHeight && Main.dayTime && !spawnInfo.Player.ZoneDesert && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive()) {
			//	return 1f;
			//}
			return 0f;
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (Main.getGoodWorld)
			{
				NPC.SetDefaults(NPCID.ExplosiveBunny);
			}
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
			if (NPC.velocity.Y == 0f)
			{
				if (NPC.direction == 1)
				{
					NPC.spriteDirection = 1;
				}
				if (NPC.direction == -1)
				{
					NPC.spriteDirection = -1;
				}
				if (NPC.velocity.X == 0f)
				{
					NPC.frame.Y = 0;
					NPC.frameCounter = 0.0;
					return;
				}
				NPC.frameCounter += Math.Abs(NPC.velocity.X) * 1f;
				NPC.frameCounter += 1.0;
				if (NPC.frameCounter > 6.0)
				{
					NPC.frame.Y += frameHeight;
					NPC.frameCounter = 0.0;
				}
				if (NPC.frame.Y / frameHeight >= Main.npcFrameCount[Type])
				{
					NPC.frame.Y = 0;
				}
			}
			else if (NPC.velocity.Y < 0f)
			{
				NPC.frameCounter = 0.0;
				NPC.frame.Y = frameHeight * 4;
			}
			else if (NPC.velocity.Y > 0f)
			{
				NPC.frameCounter = 0.0;
				NPC.frame.Y = frameHeight * 6;
			}
		}

		public override void HitEffect(NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server) {
				return;
			}

			if (NPC.life > 0)
			{
				for (int i = 0; (double)i < hit.Damage / (double)NPC.lifeMax * 20.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<ChocolateBlood>(), hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int i = 0; i < 10; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<ChocolateBlood>(), 2 * hit.HitDirection, -2f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ChocolateBunnyGore1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ChocolateBunnyGore2").Type);
			}
		}
	}
}
