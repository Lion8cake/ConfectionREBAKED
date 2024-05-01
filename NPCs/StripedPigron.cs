using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class StripedPigron : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(10f, 5f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = -12f
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 40;
            NPC.damage = 70;
            NPC.defense = 16;
            NPC.lifeMax = 210;
            NPC.HitSound = SoundID.NPCHit27;
            NPC.DeathSound = SoundID.NPCDeath30;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 2;
            AIType = 170;
            AnimationType = 170;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<StripedPigronBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<IceConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.StripedPigron")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Food(ItemID.Bacon, 15, 1, 1));
            npcLoot.Add(new ItemDropWithConditionRule(ItemID.HamBat, 10, 1, 1, new Conditions.DontStarveIsUp(), 1));
            npcLoot.Add(new ItemDropWithConditionRule(ItemID.HamBat, 25, 1, 1, new Conditions.DontStarveIsNotUp(), 1));
            npcLoot.Add(new ItemDropWithConditionRule(ItemID.KitePigron, 25, 1, 1, new Conditions.WindyEnoughForKiteDrops(), 1));
            npcLoot.Add(ItemDropRule.Common(ItemID.PigronMinecart, 100, 1, 1));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight) && spawnInfo.Player.ZoneSnow) {
                return 0.05f;
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
                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 3; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 13);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 12);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), 11);
                }
            }
        }
    }
}
