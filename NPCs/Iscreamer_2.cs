using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
    public class Iscreamer_2 : ModNPC
    {

        private float degrees;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iscreamer");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
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
            SpawnModBiomes = new int[1] { ModContent.GetInstance<IceConfectionUndergroundBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Ghostly screams echo from confections underground, these creatures use their ghostly powers to teleport to any unsuspecting victom.")
            });
        }

        public override void HitEffect(int hitDirection, double damage)
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
            if (NPC.life > NPC.lifeMax * 0.15f && Main.rand.NextBool(60))
            {
                Player player = Main.player[NPC.target];
                Point playerPos = new((int)(player.Center.X / 16), (int)(player.Center.Y / 16));
                List<Tuple<int, int>> floodFindResults = Util.FloodFindFuncs.FloodFind(playerPos, 16, 25);
                if (floodFindResults.Count == 0)
                    return;

                Teleport(NPC, floodFindResults);
                List<NPC> npcsToTeleportCandidates = new();
                for (int x = 0; x < Main.npc.Length; x++)
                {
                    if (x == NPC.whoAmI)
                        continue;
                    NPC test = Main.npc[x];
                    if (test.active && !test.friendly && !test.boss && test.Center.DistanceSQ(NPC.Center) < 1400 * 1400)
                        npcsToTeleportCandidates.Add(test);

                }
                List<NPC> npcsToTeleport = new();
                for (int i = 0; i < 4; i++)
                {
                    if (npcsToTeleportCandidates.Count == 0) break;
                    int index = Main.rand.Next(npcsToTeleportCandidates.Count);
                    npcsToTeleport.Add(npcsToTeleportCandidates[index]);
                    npcsToTeleportCandidates.RemoveAt(index);
                }
                npcsToTeleport.ForEach(i => Teleport(i, floodFindResults));

                SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
            }
        }

        public void Teleport(NPC npc, List<Tuple<int, int>> floodFindResults)
        {
            float swirlSize = 1.664f;

            List<Tuple<int, int>> floodFindResultsClone = new();
            floodFindResultsClone.AddRange(floodFindResults);

            Vector2 originalPos = npc.Center;
            while (true)
            {
                int rand = Main.rand.Next(0, floodFindResultsClone.Count);
                Tuple<int, int> location = floodFindResultsClone[rand];
                Vector2 pos = new Vector2(location.Item1 * 16, location.Item2 * 16);
                npc.Center = pos;
                if (!Collision.SolidCollision(npc.position, npc.width, npc.height))
                    break;
                floodFindResultsClone.RemoveAt(rand);
                if (floodFindResultsClone.Count == 0)
                {
                    npc.Center = originalPos;
                    return;
                }
            }

            npc.netUpdate = true;
            float Closeness = 50f;
            degrees += 2.5f;
            for (float swirlDegrees = degrees; swirlDegrees < 160f + degrees; swirlDegrees += 7f)
            {
                Closeness -= swirlSize;
                double radians = (double)swirlDegrees * (Math.PI / 180.0);
                Vector2 position = npc.Center + new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
                Vector2 westPosFar = npc.Center - new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
                Vector2 northPosFar = npc.Center + new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
                Vector2 southPosFar = npc.Center - new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
                int d5 = Dust.NewDust(position, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                int d6 = Dust.NewDust(westPosFar, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                int d7 = Dust.NewDust(northPosFar, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                int d8 = Dust.NewDust(southPosFar, 2, 2, 159, 0f, 0f, 0, new Color(209, 255, 0));
                Vector2 position2 = npc.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
                Vector2 westPosClose = npc.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
                Vector2 northPosClose = npc.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
                Vector2 southPosClose = npc.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BearClaw>(), 100));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<DimensionSplit>(), 500, 400));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()))
            {
                return 0.2f;
            }
            return 0f;
        }
    }
}
