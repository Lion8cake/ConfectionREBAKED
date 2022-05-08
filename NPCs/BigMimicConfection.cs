using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using TheConfectionRebirth.Biomes;

namespace TheConfectionRebirth.NPCs
{
	public class BigMimicConfection : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Confection Mimic");
			Main.npcFrameCount[NPC.type] = 14;
		}

		public override void SetDefaults() {
			NPC.width = 30;
			NPC.height = 40;
			NPC.damage = 180;
			NPC.defense = 34;
			NPC.lifeMax = 3500;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 87;
			AIType = NPCID.BigMimicHallow;
			AnimationType = NPCID.BigMimicHallow;
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("Mimics that are engolthed by the Confection turn into a Monsterest cake. A certain key can bring them to life.")
			});
		}
	}
}