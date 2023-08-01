using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;


/*
 * 
 * A familiar face.. she will hop around quickly when you are near, jumping around 18 blocks in height, and will try to land on you.
 * Sometimes she summons stars that circle around her that will fly at you like normal caster shots.
 * She will always drop at least 3 Souls of Delight and about 15 gold.
 * She is a rare spawn, about as often as a Lost Girl, and can be thought as a miniboss.
 * 
*/


namespace TheConfectionRebirth.NPCs
{
    public class IcecreamGal : ModNPC
    {
        #region Hostility
        const float searchDistance = 26f; // Measured in tiles, how far to look for nearby players before attacking
        #endregion

        #region Movement
        const float maxMoveSpeed = 3.5f; // Maximum number of pixels per update, while chasing the player
        const float jumpProximity = 18f; // Measured in tiles.  Start jumping really high when within this horizontal distance from player target
        const float acceleration = 0.4f; // Measured in pixels per update per update
        const float jumpForce = 13f; // How hard to jump when jumping really high
        const float hopForce = 4f; // How hard to jump when only hopping
        #endregion

        #region Star summoning
        const int numberOfStars = 6; // How many stars can the NPC summon
        const int averageNumberOfJumpsPerSummon = 4; // One in "X" chance that the NPC summons stars after a high jump
        const float targetRadius = 90f; // The distance in pixels between the center of the NPC and the stars flying around it (i.e. the size of the star circle)
        const float rotationSpeed = 0.045f; // How fast the stars fly around the NPC
        const float summonSpeed = 4f; // How fast each star is summoned
        #endregion

        #region Attack behavior
        const int starDamage = 30; // Damage that each star can cause
        const int starKnockBack = 6; // Knock back from each star
        const int shootingInterval = 25; // How many frames between the shooting of each star
        const float projectileSpeed = 11f; // How fast the stars travel toward the target player
        #endregion


        public int[] starProjectiles = new int[numberOfStars];
        static readonly int[] starRange;

        static IcecreamGal()
        {
            starRange = new int[numberOfStars];
            for (int i = 0; i < numberOfStars; i++)
            {
                starRange[i] = i;
            }
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 16;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Velocity = 0.5f
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 40;
            NPC.damage = 64;
            NPC.defense = 8;
            NPC.lifeMax = 1000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 40000f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1;
            NPC.npcSlots = 2; // Advanced, boss-like AI is used, so the max number of spawns of this NPC should be a little smaller.
            AIType = -1;
            AnimationType = NPCID.Mummy;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<IcecreamGalBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<IceConfectionUndergroundBiome>().Type };

            NPC.ai[1] = -1f; // Pause the star summoning animation
            for (int i = 0; i < numberOfStars; i++)
            {
                starProjectiles[i] = -1;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.TheConfectionRebirth.IcecreamGal")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(Mod.Find<ModItem>("SoulofDelight").Type,
                amountDroppedMinimum: 3,
                amountDroppedMaximum: 5,
                chanceNumerator: 1,
                chanceDenominator: 1
            ));
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && (spawnInfo.Player.ZoneRockLayerHeight || spawnInfo.Player.ZoneUnderworldHeight)) {
                return 0.01f;
            }
            return 0f;
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

        #region Custom AI Behavior by sOvr9000 (a.k.a. MetroidManiax)
        public override void AI()
        {
            SearchForPlayerTarget();
            ChasePlayerTarget();
            SummonStarProjectiles();
            UpdateStarProjectiles();
            PerformAttacks();
        }

        private void SearchForPlayerTarget()
        {
            if (NPC.target >= Main.player.Length)
            {
                Player targetPlayer = null;
                for (int i = 0; i < Main.player.Length; i++)
                {
                    targetPlayer = Main.player[i];
                    if (targetPlayer != null && targetPlayer.active && targetPlayer.statLife > 0 && (targetPlayer.Center - NPC.Center).LengthSquared() <= 256f * searchDistance * searchDistance)
                    {
                        NPC.target = i;
                        return;
                    }
                }
            }
        }

        private void ChasePlayerTarget()
        {
            if (NPC.target < Main.player.Length)
            {
                Player targetPlayer = Main.player[NPC.target];
                if (targetPlayer == null || !targetPlayer.active || targetPlayer.statLife <= 0)
                {
                    NPC.target = 256;
                }
                else
                {
                    float dx = targetPlayer.Center.X - NPC.Center.X;
                    float hdist = MathF.Abs(dx); // horizontal distance
                    float speed = MathF.Min(maxMoveSpeed, hdist / 20f);
                    NPC.velocity.X += (dx > 0f ? 1 : -1) * acceleration;
                    if (MathF.Abs(NPC.velocity.X) > speed)
                    {
                        NPC.velocity.X *= speed / MathF.Abs(NPC.velocity.X); // Normalize so that it doesn't move too fast horizontally
                    }
                    if (NPC.collideY)
                    {
                        if (hdist <= jumpProximity * 16f)
                        {
                            NPC.velocity.Y = -jumpForce;
                            // Chance to summon stars
                            if (NPC.ai[1] == -1f && Main.rand.Next(averageNumberOfJumpsPerSummon) == 0)
                            {
                                NPC.ai[1] = 0f; // Triggers the animation
                            }
                        }
                        else
                        {
                            NPC.velocity.Y = -hopForce;
                        }
                    }
                }
            }
            else
            {
                NPC.velocity.X *= 0.6f;
            }
        }

        private void PerformAttacks()
        {
            if (NPC.target < Main.player.Length)
            {
                if (Main.GameUpdateCount % shootingInterval == 0)
                {
                    Shoot();
                }
            }
        }

        private void SummonStarProjectiles()
        {
            if (NPC.ai[1] >= 0f && NPC.ai[1] < numberOfStars)
            { // ai value increments by one only when a star has finished the summoning animation
                if (starProjectiles[(int)NPC.ai[1]] == -1)
                {
                    starProjectiles[(int)NPC.ai[1]] = Projectile.NewProjectile(
                        spawnSource: NPC.GetSource_None(),
                        position: NPC.Center - Vector2.One * 5f,
                        velocity: Vector2.Zero,
                        Type: Mod.Find<ModProjectile>("IcecreamStar").Type,
                        Damage: starDamage,
                        KnockBack: starKnockBack
                    );
                }
            }
        }

        private void UpdateStarProjectiles()
        {
            for (int i = 0; i < numberOfStars; i++)
            {
                if (starProjectiles[i] == -1) continue;
                Projectile star = Main.projectile[starProjectiles[i]];
                float t = (float)Main.time * rotationSpeed + i * MathF.Tau / numberOfStars;
                float nextRadius;
                if (star.ai[1] == 1f)
                {
                    nextRadius = targetRadius;
                }
                else
                {
                    NPC.ai[2] += summonSpeed;
                    nextRadius = NPC.ai[2];
                    if (nextRadius >= targetRadius)
                    {
                        star.ai[1] = 1f; // Mark as fully summoned
                        NPC.ai[1]++; // Allow to summon the next star
                        NPC.ai[2] = 0f;
                    }
                }
                star.Center = NPC.Center + new Vector2(nextRadius * MathF.Cos(t), nextRadius * MathF.Sin(t));
            }
        }

        private void Shoot()
        {
            if (NPC.ai[1] == numberOfStars)
            {
                Player targetPlayer = Main.player[NPC.target];
                int[] remaining = starRange.Where(i => starProjectiles[i] != -1).ToArray();
                int i = remaining[Main.rand.Next(remaining.Length)];
                Projectile star = Main.projectile[starProjectiles[i]];
                Vector2 delta = targetPlayer.Center - star.Center;
                star.velocity = delta * projectileSpeed / delta.Length();
                starProjectiles[i] = -1;
                Mod.Logger.Info("Shot star #" + i);
                // Basically what happens above is that stars are selected randomly from the circle to be shot toward the target player
                if (remaining.Length == 1)
                {
                    NPC.ai[1] = -1f; // Preparing to summon stars again sometime
                    NPC.ai[2] = 0f;
                }
            }
        }
        #endregion
    }
}
