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
using TheConfectionRebirth.Biomes;
using Terraria.GameContent.Bestiary;

namespace TheConfectionRebirth.NPCs
{
	public class Sprinkling : ModNPC
    {
		
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Sprinkling");
			Main.npcFrameCount[NPC.type] = 10; 
		}
		
		public override void SetDefaults() {
			NPC.width = 36;
			NPC.height = 36;
			NPC.damage = 75;
			NPC.defense = 20;
			NPC.lifeMax = 140;
			NPC.HitSound = SoundID.NPCHit5;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.value = 60f;
			NPC.noGravity = true;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 22;
			AIType = NPCID.Pixie;
			AnimationType = NPCID.Pixie;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<SprinklingBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionSurfaceBiome>().Type };
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Sprinkles cursed with the soul power of flight will still try and kill anyone from taking over the confection.")
			});
		}
	}
}