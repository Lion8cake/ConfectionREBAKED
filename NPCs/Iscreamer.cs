using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;
using TheConfectionRebirth.Items.Weapons;

namespace TheConfectionRebirth.NPCs
{
    public class Iscreamer : ModNPC
    {

        private float degrees;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iscreamer");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 60;
            NPC.damage = 50;
            NPC.defense = 22;
            NPC.lifeMax = 400;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 22;
            AIType = NPCID.FloatyGross;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<IscreamerBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Ghostly screams echo from confections underground, these creatures use their ghostly powers to teleport to any unsuspecting victom.")
            });
        }

        int speed = 10;
        int maxFrames = 3;
        int frame;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= speed)
            {
                frame++;
                NPC.frameCounter = 0;
            }

            if (frame > maxFrames)
                frame = 0;

            NPC.frame.Y = frame * frameHeight;
            NPC.spriteDirection = NPC.direction;
        }

        public override void AI()
        {
            int num184 = (int)(NPC.Center.X / 16f);
            int num185 = (int)(NPC.Center.Y / 16f);
            if (NPC.life > NPC.lifeMax * 0.15f)
            {
                Teleport();
                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
            }
        }

        public void Teleport()
        {
            float swirlSize = 1.664f;
            Player player = Main.player[NPC.target];
            NPC.position.X = player.position.X + Main.rand.Next(-150, 150);
            NPC.position.Y = player.position.Y - 300f;
            NPC.netUpdate = true;
            float Closeness = 50f;
            degrees += 2.5f;
            for (float swirlDegrees = degrees; swirlDegrees < 160f + degrees; swirlDegrees += 7f)
            {
                Closeness -= swirlSize;
                double radians = (double)swirlDegrees * (Math.PI / 180.0);
                Vector2 position = NPC.Center + new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
                Vector2 westPosFar = NPC.Center - new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
                Vector2 northPosFar = NPC.Center + new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
                Vector2 southPosFar = NPC.Center - new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
                int d5 = Dust.NewDust(position, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                int d6 = Dust.NewDust(westPosFar, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                int d7 = Dust.NewDust(northPosFar, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                int d8 = Dust.NewDust(southPosFar, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                Vector2 position2 = NPC.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
                Vector2 westPosClose = NPC.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
                Vector2 northPosClose = NPC.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
                Vector2 southPosClose = NPC.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BearClaw>(), 100));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<DimensionSplit>(), 500, 400));
        }
    }
}
