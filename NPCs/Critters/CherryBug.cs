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
            DisplayName.SetDefault("Cherry Bug");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Firefly];
            Main.npcCatchable[NPC.type] = true;
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
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionSurfaceBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("A specific bug used in making cherry jam or to add cherry flavoring to any food.")
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            int num = NPC.life > 0 ? 1 : 5;
            for (int k = 0; k < num; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CritterBlood>());
            }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        //Might add gore later

        public override void OnCatchNPC(Player player, Item item)
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
        }

    }

    internal class CherryBugItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cherry Bug");
        }

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
