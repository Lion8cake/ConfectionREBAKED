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
	public class GummyWorm : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Worm];
			Main.npcCatchable[Type] = true;

			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
			NPCID.Sets.TownCritter[Type] = true;

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers
			{
				Velocity = 1f,
				Position = new(1, 2)
			});
		}

		public override void SetDefaults() {
			NPC.width = 10;
			NPC.height = 4;
			NPC.aiStyle = 66;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.npcSlots = 0.1f;
			NPC.catchItem = ModContent.ItemType<Items.GummyWorm>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,

				new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.GummyWorm")
			});
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.localAI[0] = -2f;
			if (NPC.velocity.Y == 0f)
			{
				NPC.rotation = 0f;
				if (NPC.velocity.X == 0f)
				{
					NPC.frame.Y = frameHeight;
					NPC.frameCounter = 0.0;
				}
				else
				{
					NPC.frameCounter += 1.0;
					if (NPC.frameCounter > 12.0)
					{
						NPC.frameCounter = 0.0;
						NPC.frame.Y += frameHeight;
						if (NPC.frame.Y > frameHeight)
						{
							NPC.frame.Y = 0;
						}
					}
				}
			}
			else
			{
				NPC.rotation += (float)NPC.direction * 0.1f;
				NPC.frame.Y = frameHeight;
			}
			int x = (int)NPC.Center.X / 16;
			int y = (int)NPC.position.Y / 16;
			Tile tileSafely2 = Framing.GetTileSafely(x, y);
			if (tileSafely2 != null)
			{
				if (tileSafely2.Slope == 0)
				{
					y++;
					tileSafely2 = Framing.GetTileSafely(x, y);
				}
				if (tileSafely2.Slope == (SlopeType)1)
				{
					NPC.rotation = 0.785f;
					NPC.localAI[0] = 0f;
				}
				else if (tileSafely2.Slope == (SlopeType)2)
				{
					NPC.rotation = -0.785f;
					NPC.localAI[0] = 0f;
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) 
		{
			return ConfectionGlobalNPC.SpawnNPC_ConfectionNPC(spawnInfo, Type);
		}

		public override void HitEffect(NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server) {
				return;
			}

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 6; i++)
				{
					int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Worm, 2 * hit.HitDirection, -2f, newColor: Main.DiscoColor);
					if (Main.rand.NextBool(2))
					{
						Main.dust[dustID].noGravity = true;
						Main.dust[dustID].scale = 1.2f * NPC.scale;
					}
					else
					{
						Main.dust[dustID].scale = 0.7f * NPC.scale;
					}
				}
			}
		}
	}
}
