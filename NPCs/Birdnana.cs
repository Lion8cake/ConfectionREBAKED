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
	public class Birdnana : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Bird];
			Main.npcCatchable[Type] = true;

			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
			NPCID.Sets.TownCritter[Type] = true;

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
			{
				Position = new(0, -16f),
				Velocity = 0.05f,
				PortraitPositionYOverride = -35f,
			});
		}

		public override void SetDefaults() 
		{
			NPC.width = 14;
			NPC.height = 14;
			NPC.aiStyle = 24;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.knockBackResist = 0.8f;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.catchItem = ModContent.ItemType<Items.Birdnana>();
			NPC.npcSlots = 0.4f;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<BirdnanaBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Birdnana")
			});
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) 
		{
			return ConfectionGlobalNPC.SpawnNPC_ConfectionNPC(spawnInfo, Type);
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.rotation = NPC.velocity.X * 0.1f;
			if (NPC.velocity.X == 0f && NPC.velocity.Y == 0f)
			{
				NPC.frame.Y = frameHeight * 4;
				NPC.frameCounter = 0.0;
				return;
			}
			int count = Main.npcFrameCount[Type] - 1;
			NPC.frameCounter += 1.0;
			if (NPC.frameCounter >= 4.0)
			{
				NPC.frame.Y += frameHeight;
				NPC.frameCounter = 0.0;
			}
			if (NPC.frame.Y >= frameHeight * count)
			{
				NPC.frame.Y = 0;
			}
		}

		public override void HitEffect(NPC.HitInfo hit) 
		{
			if (Main.netMode == NetmodeID.Server) 
			{
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
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BirdnanaGore").Type);
			}
		}
	}
}
