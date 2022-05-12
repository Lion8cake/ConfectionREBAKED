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
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Biomes;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Capture;

namespace TheConfectionRebirth.NPCs
{
	
	public class ParfaitSlime_2 : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Parfait Slime");
			Main.npcFrameCount[NPC.type] = 2; 
		}
		
		public override void SetDefaults() {
			NPC.width = 32;
			NPC.height = 32;
			NPC.damage = 70;
			NPC.defense = 16;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 1;
			AIType = NPCID.Crimslime;
			AnimationType = NPCID.Crimslime;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<ParfaitSlimeBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<IceConfectionUndergroundBiome>().Type };
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("A fruity cake for the festive season covers this slime from anyone who sees them, oddly they are only seen in one place")
			});
		}
	}
}