using TheConfectionRebirth.Items.Banners;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using TheConfectionRebirth.Biomes;

namespace TheConfectionRebirth.NPCs
{
	public class CrookedCookie : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Crooked Cookie");  
		}

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 18;
			NPC.damage = 60;
			NPC.defense = 24;
			NPC.lifeMax = 140;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 60f;
			// npc.noGravity = false;
			// npc.noTileCollide = false;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 26;
			AIType = NPCID.Unicorn;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<CrookedCookieBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionSandSurfaceBiome>().Type };
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			
				new FlavorTextBestiaryInfoElement("A smaller rarer version of the rolller cookie that hits like a truck. This cookie is created through the cream sandwitch's spells.")
			});
		}
		
		public override void AI()
		{
			NPC.rotation += NPC.velocity.X * 0.05f;
		}
     }	
}
