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

namespace TheConfectionRebirth.NPCs
{
	public class SherbetSlime : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Sherbet Slime");
			Main.npcFrameCount[NPC.type] = 2; 
		}

		public override void SetDefaults() {
			NPC.width = 44;
			NPC.height = 34;
			NPC.damage = 77;
			NPC.defense = 22;
			NPC.lifeMax = 420;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 1;
			AIType = NPCID.Crimslime;
			AnimationType = NPCID.Crimslime;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<SherbetSlimeBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionSurfaceBiome>().Type };
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			
			    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,

				new FlavorTextBestiaryInfoElement("A lump of icecream that comes to life when it rains.")
			});
		}

	}
}