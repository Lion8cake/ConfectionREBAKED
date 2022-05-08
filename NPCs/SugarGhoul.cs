using TheConfectionRebirth.Items.Banners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;
using TheConfectionRebirth.Biomes;

namespace TheConfectionRebirth.NPCs
{
	public class SugarGhoul : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Sugar Ghoul");
			Main.npcFrameCount[NPC.type] = 8;
		}

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 40;
			NPC.damage = 50;
			NPC.defense = 26;
			NPC.lifeMax = 180;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 3;
			AIType = 524;
			AnimationType = 524;
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionSandUndergroundBiome>().Type };
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			
				new FlavorTextBestiaryInfoElement("A ghoul found in the depths of the desert tainted by the confection will hunt down anyone it sees.")
			});
		}
	}
}
