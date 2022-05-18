using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs.Critters
{
    internal class ChocolateFrog : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chocolate Frog");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Frog];
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Frog);
            NPC.catchItem = (short)ModContent.ItemType<ChocolateFrogItem>();
            NPC.aiStyle = 7;
            NPC.friendly = true;
            AIType = NPCID.Frog;
            AnimationType = NPCID.Frog;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<ChocolateFrogBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionSurfaceBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("A popular treat amongst kids although these frogs are not for eating.")
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

    internal class ChocolateFrogItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chocolate Frog");
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

            Item.makeNPC = (short)ModContent.NPCType<ChocolateFrog>();
        }
    }
}
