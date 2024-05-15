using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
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
			AnimationType = NPCID.Frog;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<ChocolateFrogBanner>();
			//SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
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

		public override void HitEffect(NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server) {
				return;
			}

			if (NPC.life <= 0) {
				var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 1; i++) {
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("ChocolateFrogGore1").Type);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("ChocolateFrogGore2").Type);
				}
			}
		}
	}
}
