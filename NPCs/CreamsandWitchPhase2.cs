using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Armor;
using TheConfectionRebirth.Items.Banners;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Pets.CreamsandWitchPet;

namespace TheConfectionRebirth.NPCs
{
    public class CreamsandWitchPhase2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
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

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.CreamsandWitch")
            });
        }

        public override void AI()
        {
            NPC.ai[0] += 1f;
            if (Main.rand.NextBool(500) && NPC.CountNPCS(ModContent.NPCType<Hunger>()) < 25)
            {
                NPC.ai[0] = 0f;
                int i = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Hunger>(), 0, NPC.whoAmI);
                Main.npc[i].velocity.X = Main.rand.NextFloat(-0.4f, 0.4f);
                Main.npc[i].velocity.Y = Main.rand.NextFloat(-0.5f, -0.05f);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(5, ModContent.ItemType<CreamHat>(), ModContent.ItemType<CookieCorset>(), ModContent.ItemType<CakeDress>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Creamsand>(), 1, 30, 50));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PixieStick>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CreamySandwhich>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Brownie>(), 150));
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

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("CreamsandWitchGore1").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("CreamsandWitchGore2").Type);
                }
            }
        }
    }
}
