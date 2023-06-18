using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs
{
    public class FoaminFloat : ModNPC
    {

        private Player player;

		public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(0f, 6f),
                PortraitPositionYOverride = 0f
            });
        }

		public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 45;
            NPC.defense = 8;
            NPC.lifeMax = 140;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 22;
            AIType = NPCID.FloatyGross;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<FoaminFloatBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.FoaminFloat")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionUndergroundBiome>()) && !spawnInfo.AnyInvasionActive())
            {
                return 0.1f;
            }
            return 0f;
        }

        public override void AI()
        {
            Target();
            if (player != null && Collision.CanHit(NPC, player) && --NPC.ai[1] <= 0f)
            {
                Shoot();
            }
        }

        private void Target()
        {
            NPC.TargetClosest();
            player = NPC.target == -1 ? null : Main.player[NPC.target];
        }

        private void Shoot()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            int type = Mod.Find<ModProjectile>("CreamySprayEvil").Type;
            Vector2 velocity = player.Center - NPC.Center;
            float magnitude = Magnitude(velocity);
            if (magnitude > 0f)
            {
                velocity *= 5f / magnitude;
            }
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, type, NPC.damage, 2f);
            NPC.ai[1] = 200f;
        }

        private float Magnitude(Vector2 mag)
        {
            return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CcretTicket>(), 100));
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
