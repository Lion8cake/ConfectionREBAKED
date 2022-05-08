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
	public class SweetGummy : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Sweet Gummy");
			Main.npcFrameCount[NPC.type] = 16; 
		}

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 40;
			NPC.damage = 60;
			NPC.defense = 26;
			NPC.lifeMax = 180;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			//Sound 3 and 4 
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 3;
			AIType = NPCID.Mummy;
			AnimationType = NPCID.Mummy;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<SweetGummyBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionSandSurfaceBiome>().Type };
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			
				new FlavorTextBestiaryInfoElement("A mummy infected by the confection turning into a gummy bear. How sweet?")
			});
		}
	}
}
