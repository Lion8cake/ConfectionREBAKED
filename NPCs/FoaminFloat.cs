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
            DisplayName.SetDefault("Foamin' Float");
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

                new FlavorTextBestiaryInfoElement("A floating cloud of root beer is a odd sight to see in the underground confection although will shoot streems of root beer at anyone it finds.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneDirtLayerHeight && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionUndergroundBiome>()))
            {
                return 0.1f;
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
            int type = Mod.Find<ModProjectile>("CreamySprayEvil").Type;
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

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CcretTicket>(), 100));
        }
    }
}
