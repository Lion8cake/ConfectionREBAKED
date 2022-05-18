using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.NPCs.Critters
{
    internal class GummyWorm : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gummy Worm");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Worm];
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Worm);
            NPC.catchItem = (short)ModContent.ItemType<GummyWormItem>();
            NPC.aiStyle = 66;
            NPC.friendly = true;
            AIType = NPCID.Worm;
            AnimationType = NPCID.Worm;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionSurfaceBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Digging into the ground, gummy worms look for food to satisfy their hunger. Not to be confused with Gummy Wyrm.")
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

    internal class GummyWormItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gummy Worm");
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
            Item.bait = 40;

            Item.makeNPC = (short)ModContent.NPCType<GummyWorm>();
        }
    }
}
