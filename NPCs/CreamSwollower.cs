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
	public class CreamSwollower : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Cream Swollower");  
			Main.npcFrameCount[NPC.type] = 4; 
		}

		public override void SetDefaults() {
			NPC.width = 40;
			NPC.height = 40;
			NPC.damage = 50;
			NPC.defense = 10;
			NPC.lifeMax = 360;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 60f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.behindTiles = true;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 103;
			AIType = NPCID.SandShark;
			AnimationType = NPCID.SandShark;
			//banner = npc.type;
			//bannerItem = ModContent.ItemType<FoaminFloatBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionSandSurfaceBiome>().Type };
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			
			    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm,

				new FlavorTextBestiaryInfoElement("In ancient times, a saltwater river once ran through the desert. These powerful creatures evolved to survive in the now dry sand.")
			});
		}
  }
}
