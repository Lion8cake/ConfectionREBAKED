using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{

    public class Hunger : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 20;
            NPC.damage = 80;
            NPC.defense = 22;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 25;
            AIType = NPCID.PresentMimic;
            AnimationType = NPCID.PresentMimic;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<HungerBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Hunger")
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            if (NPC.life <= 0)
            {
                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 3; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 13);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 12);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 11);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("HungerGore").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("HungerGore").Type);
                }
            }
        }
    }
}