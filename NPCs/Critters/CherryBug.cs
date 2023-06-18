using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs.Critters
{
    internal class CherryBug : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Firefly];
            Main.npcCatchable[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(0, 8f),
                Velocity = 0.5f
            });
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Firefly);
            NPC.catchItem = (short)ModContent.ItemType<CherryBugItem>();
            NPC.aiStyle = 64;
            NPC.friendly = true;
            AIType = NPCID.Firefly;
            AnimationType = NPCID.Firefly;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CherryBugBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.CherryBug")
            });
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
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
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CritterBlood>());
                }
            }
        }

        /*public virtual void OnCatchNPC(Player player, Item item)
        {
            item.stack = 1;

            try
            {
                var npcCenter = NPC.Center.ToTileCoordinates();
            }
            catch
            {
                return;
            }
        }*/

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()))
            {
                return 2f;
            }
            return 0f;
        }
    }

    internal class CherryBugItem : ModItem
    {
        public override void SetStaticDefaults() => Item.ResearchUnlockCount = 5;

        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 12;
            Item.height = 12;
            Item.makeNPC = 360;
            Item.noUseGraphic = true;
            Item.bait = 35;

            Item.makeNPC = (short)ModContent.NPCType<CherryBug>();
        }
    }
}
