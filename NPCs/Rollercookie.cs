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

namespace TheConfectionRebirth.NPCs
{
    public class Rollercookie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Roller Cookie");
        }

        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 44;
            NPC.damage = 58;
            NPC.defense = 17;
            NPC.lifeMax = 360;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            // npc.noGravity = false;
            // npc.noTileCollide = false;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 26;
            AIType = NPCID.Unicorn;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<RollerCookieBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiomeSurface>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("A cookie brought to life in the magical biome that was created from the spirits of light and night")
            });
        }

        public override void AI()
        {
            NPC.rotation += NPC.velocity.X * 0.05f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CookieDough>(), maximumDropped: 2));
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ChocolateChunk>(), 100));
            npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(20, ModContent.ItemType<CookieMask>(), ModContent.ItemType<CookieShirt>(), ModContent.ItemType<CookiePants>()));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiomeSurface>()))
            {
                return 0.1f;
            }
            return 0f;
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
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("RollercookieGore1").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("RollercookieGore2").Type);
                }
            }
        }
    }
}
