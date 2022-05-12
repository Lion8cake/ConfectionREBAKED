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
	public class StripedPigron : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Striped Pigron");
			Main.npcFrameCount[NPC.type] = 14;
		}

		public override void SetDefaults() {
			NPC.width = 48;
			NPC.height = 40;
			NPC.damage = 70;
			NPC.defense = 16;
			NPC.lifeMax = 210;
			NPC.HitSound = SoundID.NPCHit27;
			NPC.DeathSound = SoundID.NPCDeath30;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 2;
			AIType = 170;
		    AnimationType = 170;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<StripedPigronBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<IceConfectionUndergroundBiome>().Type };
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			
				new FlavorTextBestiaryInfoElement("This eluesive dragon-pig hybrid has excellent stealth capabilities despite its rotund figure. It is uncertain how they came to exsist.")
			});
		}
	}
}
