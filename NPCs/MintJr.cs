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
	public class MintJr : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Mint Jr");  
		}

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 18;
			NPC.damage = 60;
			NPC.defense = 20;
			NPC.lifeMax = 120;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 60f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 5;
			AIType = NPCID.MeteorHead;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<MintJrBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			
				new FlavorTextBestiaryInfoElement("A giant mint jr that floats after anyone that the cream sandwitch commands. They are created through the cream sandwitch's powers.")
			});
		}
     }	
}
