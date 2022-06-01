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
    public class BirthdayCookie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Birthday Cookie");
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
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiomeBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("This cookie loves to celebrate birthdays by crushing people to death.")
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
            npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ModContent.ItemType<TopCake>(), ModContent.ItemType<BirthdaySuit>(), ModContent.ItemType<RightTrousers>()));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiomeBiome>()))
            {
                return 0.01f;
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
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("BirthdayCookieGore").Type);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>("BirthdayCookieGore").Type);
                }
            }
        }
    }
}
