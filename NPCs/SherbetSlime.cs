using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items.Banners;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.NPCs
{
    public class SherbetSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(2f, 0f),
                PortraitPositionXOverride = 0f
            });
        }

        public override void SetDefaults()
        {
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
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Rain,

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.SherbetSlime")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 1, 20));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SherbetBricks>(), 1, 30, 60));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.raining && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive())
            {
                return 0.1f;
            }
            return 0f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            if (NPC.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<SherbetDust>());
                }
            }
        }
    }
}