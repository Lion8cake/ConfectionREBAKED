using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{

    public class Sprinkler : ModNPC
    {
        private Player player;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sprinkler");
            Main.npcFrameCount[NPC.type] = 2;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 30;
            NPC.damage = 70;
            NPC.defense = 22;
            NPC.lifeMax = 120;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.value = 60f;
            // npc.noGravity = false;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 0;
            AIType = 0;
            AnimationType = NPCID.BlueSlime;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<SprinklingBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiomeSurface>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("A pile of evil sprinkles that will try prevent anyone from getting rid of their home land.")
            });
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.damage = (int)(NPC.damage * 0.2f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Vector2 spawnAt = NPC.Center + new Vector2(0f, NPC.height / 2f);
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<Sprinkling>());
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiomeSurface>()))
            {
                return 1f;
            }
            return 0f;
        }

        public override void AI()
        {
            Target();
            NPC.ai[1] -= 1f;
            if (NPC.ai[1] <= 0f)
            {
                Shoot();
            }
        }

        private void Target()
        {
            player = Main.player[NPC.target];
        }

        private void Shoot()
        {
            int type = Mod.Find<ModProjectile>("SprinklingBall").Type;
            Vector2 velocity = player.Center - NPC.Center;
            float magnitude = Magnitude(velocity);
            if (magnitude > 0f)
            {
                velocity *= 5f / magnitude;
            }
            NPC.ai[1] = 200f;
        }

        private float Magnitude(Vector2 mag)
        {
            return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
        }

    }
}