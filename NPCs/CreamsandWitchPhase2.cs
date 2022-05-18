using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class CreamsandWitchPhase2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creamsand Witch");
            Main.npcFrameCount[NPC.type] = 16;
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 40;
            NPC.damage = 85;
            NPC.defense = 15;
            NPC.lifeMax = 2000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 3;
            AIType = NPCID.ChaosElemental;
            AnimationType = NPCID.Mummy;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CreamsandWitchBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("A witch that uses her sugary powers do defeat anyone that get near her home land of the confection desert.")
            });
        }

        /*public override void AI()
	{
		NPC.ai[0] += 1f;
		if (Main.rand.NextBool(500) && NPC.CountNPCS(ModContent.NPCType<Hunger>()) < 25)
		{
			NPC.ai[0] = 0f;
			int i = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Hunger>(), 0, NPC.whoAmI);
			Main.npc[i].velocity.X = Main.rand.NextFloat(-0.4f, 0.4f);
			Main.npc[i].velocity.Y = Main.rand.NextFloat(-0.5f, -0.05f);
		}
	}*/
    }
}
