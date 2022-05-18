using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class TheUnfirm : ModNPC //If anyone wants to animate it, GO FOR IT!!!
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Unfirm");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.width = 100;
            NPC.height = 100;
            NPC.damage = 90;
            NPC.defense = 25;
            NPC.lifeMax = 5000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 3;
            AIType = 494;
            AnimationType = 494;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<TheUnfirmBanner>();
        }
    }
}
