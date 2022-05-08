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
	
	public class Hunger : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Hunger");
			Main.npcFrameCount[NPC.type] = 6; 
		}
		
		public override void SetDefaults() {
			NPC.width = 20;
			NPC.height = 20;
			NPC.damage = 80;
			NPC.defense = 22;
			NPC.lifeMax = 500;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 25;
			AIType = NPCID.PresentMimic;
			AnimationType = NPCID.PresentMimic;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<HungerBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionSandSurfaceBiome>().Type };
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			
			    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm,

				new FlavorTextBestiaryInfoElement("A mimic type creature that possesed a smore and will try to kill anyone that the cream sandwitch commands. This confection was created through the cream sandwitch's spells.")
			});
		}
	}
}