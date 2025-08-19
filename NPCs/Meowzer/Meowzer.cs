using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.NPCs.Meowzer.Gores;
using TheConfectionRebirth.Items;

namespace TheConfectionRebirth.NPCs.Meowzer
{
    public class Meowzer : ModNPC
    {
        NPC hitbox;
        bool[] cannon = new bool[3];
        float[] lockOn = new float[5];
        Vector2 oldVel;
        int lockDir;
        bool[] sight = new bool[2];
        Vector2 lastSeen;
        float[] jitter = new float[3];
        float[] recoil = new float[4];
        int[] frame = new int[2];
        int[] index = new int[2];
        Vector2[] tailSeg = new Vector2[4];
        public override void SetStaticDefaults()
        {
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "TheConfectionRebirth/NPCs/Meowzer/Meowzer_Bestiary",
                Position = new Vector2(0f, -12f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }
        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 50;
            AIType = -1;
            NPC.lifeMax = 280;
            NPC.defense = 24;
            NPC.damage = 70;
            NPC.knockBackResist = 0.2f;
            NPC.value = Item.buyPrice(0, 0, 7, 50);
            NPC.npcSlots = 1f;
            NPC.noGravity = true;
            NPC.netAlways = true;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.Item2;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<MeowzerBanner>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionBiome>().Type };
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

                new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.Meowzer")
            });
        }
        public override void AI()
        {
            var entitySource = NPC.GetSource_FromAI();

            if (!cannon[1])
            {
                NPC.lifeMax = NPC.life = 560;
                int index = NPC.NewNPC(entitySource, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MeowzerCannonHitbox>());
                MeowzerCannonHitbox h = Main.npc[index].ModNPC as MeowzerCannonHitbox;
                h.owner = NPC;
                hitbox = h.NPC;
                cannon[1] = true;
            }
            if ((hitbox == null || !hitbox.active) && !cannon[2])
            {
                cannon[2] = true;
                Enrage();
            }
            UCR();
            if (oldVel != Vector2.Zero)
                NPC.velocity = new Vector2(MathHelper.Lerp(oldVel.X, 0, lockOn[1]), MathHelper.Lerp(oldVel.Y, 0, lockOn[1] + 0.02f));
            NPC.spriteDirection = NPC.velocity.X < 0 ? -1 : 1;
            NPC.noTileCollide = cannon[0];
            NPC.aiStyle = cannon[0] ? 2 : 22;
            NPC.rotation = NPC.velocity.ToRotation();
            if (!cannon[0])
                NPC.rotation *= NPC.direction == -1 ? -0.01f : 0.05f;
            if (!cannon[0])
            {
                if (recoil[1] > 0)
                {
                    jitter[2] = 0;
                    recoil[1]++;
                    if (recoil[1] >= 31)
                        recoil[1] = 0;
                    Recoil();
                }
                lockOn[1] = (float)Math.Sin(lockOn[4]);
                if (lockOn[3] > 0)
                {
                    lockOn[4] -= (float)Math.PI / 30;
                    if (lockOn[4] < 0)
                        Reset();
                }
                lockOn[0]++;
                if (lockOn[0] >= 240)
                {
                    if (Target() != null)
                    {
                        Player targ = Target();
                        if (Collision.CanHitLine(targ.MountedCenter, targ.width, targ.height, NPC.Center, NPC.width, NPC.height))
                            sight[0] = true;
                        if (sight[0])
                        {
                            if (Collision.CanHitLine(targ.MountedCenter, targ.width, targ.height, NPC.Center, NPC.width, NPC.height) && !sight[1])
                                lastSeen = targ.MountedCenter;
                            else
                                sight[1] = true;
                            if (lockDir == 0)
                                lockDir = Target().MountedCenter.X < NPC.position.X ? -1 : 1;
                            NPC.direction = NPC.spriteDirection = lockDir;
                            if (oldVel == Vector2.Zero)
                                oldVel = NPC.velocity;
                            if (lockOn[1] < 1f)
                                lockOn[4] += (float)Math.PI / 60;
                            if (lockOn[4] >= Math.PI * 0.475f)
                            {
                                Charge();
                                if (lockOn[0] >= 340)
                                    Fire();
                            }
                        }
                        else
                            lockOn[0] = 0;
                    }
                }
            }
            else
            {
                if (index[1]++ > 10)
                {
                    index[1] = 0;
                    if (index[0]++ > 2)
                        index[0] = 0;
                    tailSeg[index[0]] = NPC.Center;
                }
            }
            //Update Cannon Rotation
            void Reset()
            {
                lockOn[1] = 0;
                lockOn[3] = 0;
                oldVel = Vector2.Zero;
                lockDir = 0;
                sight[0] = false;
                sight[1] = false;
                jitter[0] = 0;
                jitter[1] = 0;
            }
            void UCR()
            {
                if (Target() != null)
                {
                    float rot = (float)Math.Atan2((double)(lastSeen.Y - NPC.Center.Y), (double)(lastSeen.X - NPC.Center.X));
                    lockOn[2] = MathHelper.Lerp(NPC.rotation + (NPC.spriteDirection == 1 ? 1.5f : -1.5f), rot + 1.75f, lockOn[1]);
                }
            }
            void Charge()
            {
                float scale = lockOn[0] / 400;
                jitter[2] += 0.03f;
                if (jitter[2] > 1)
                    jitter[2] = 1;
                jitter[0] = Main.rand.NextFloat(-0.75f, 0.75f) * scale;
                jitter[1] = Main.rand.NextFloat(-0.75f, 0.75f) * scale;
                if (frame[0]++ > 15)
                {
                    frame[0] = 0;
                    if (frame[1]++ > 2)
                        frame[1] = 0;
                }
            }
            void Fire()
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    return;

                Vector2 val = lastSeen;
                Vector2 val2 = val - NPC.Center;
                float num2 = 10f;
                float num3 = (float)Math.Sqrt(val2.X * val2.X + val2.Y * val2.Y);
                if (num3 > num2)
                    num3 = num2 / num3;
                val2 *= num3;
                recoil[2] = val2.X;
                recoil[3] = val2.Y;
                Vector2 firePos = NPC.Center + new Vector2((NPC.spriteDirection == -1 ? 20f : 0f), -45f);
                Projectile.NewProjectile(NPC.GetSource_FromAI("Fire"), firePos, val2 *= 0.5f, ModContent.ProjectileType<MeowzerBeam>(), Utilities.DL(125), 2.5f);
                lockOn[3] = 1;
                lockOn[0] = 0;
                recoil[1] = 1;
                SoundEngine.PlaySound(SoundID.Item67, NPC.Center);
                if (Main.rand.NextBool(4))
                    SoundEngine.PlaySound(SoundID.Item57, NPC.Center);
            }
            void Recoil()
            {
                recoil[0] += (float)Math.PI / 30;
                float sine = (float)(0.5 * Math.Cos(recoil[0]) + 0.5);
                NPC.position.X += (recoil[2] * sine) * -0.25f;
                NPC.position.Y += (recoil[3] * sine) * -0.25f;
            }
            void Enrage()
            {
                cannon[0] = true;
                Reset();
                SoundEngine.PlaySound(SoundID.NPCDeath30, NPC.Center);
                //SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/Pigron"));
            }
        }
        Player Target()
        {
            NPC.TargetClosest(true);
            return NPC.target == -1 ? null : Main.player[NPC.target];
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (!cannon[0])
            {
                Texture2D idle = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Meowzer/MeowzerIdle").Value;
                Texture2D cannon = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Meowzer/MeowzerCannon").Value;
                Texture2D charge = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Meowzer/MeowzerCannonCharge").Value;
                Vector2 pos = DS.DrawPos(NPC.Center) + (NPC.spriteDirection == 1 ? new Vector2(-15, 0) : Vector2.Zero);
                Vector2 cannonOrg = DS.DrawOrigin(cannon);
                SpriteEffects effect = DS.FlipTex(NPC.spriteDirection);
                cannonOrg.X += jitter[0];
                cannonOrg.Y += jitter[1];
                spriteBatch.Draw(idle, pos + new Vector2(10, -20), DS.DrawFrame(idle), drawColor, NPC.rotation, DS.DrawOrigin(idle), 1, effect, 0);
                spriteBatch.Draw(cannon, pos + new Vector2((NPC.spriteDirection == -1 ? 20f : 0f), -45f), DS.DrawFrame(cannon), drawColor, lockOn[2], cannonOrg, 1, effect, 0);
                spriteBatch.Draw(charge, pos + new Vector2((NPC.spriteDirection == -1 ? 20f : 0f), -45f), DS.DrawFrame(cannon, 0, cannon.Height * frame[1]), Utilities.LerpColor(Color.Transparent, drawColor, jitter[2]), lockOn[2], cannonOrg, 1, effect, 0);
            }
            else
            {
                Texture2D tex = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Meowzer/MeowzerTailSeg").Value;
                Texture2D tex2 = TextureAssets.Npc[NPC.type].Value;
                for (int a = 0; a < tailSeg.Length; a++)
                {
                    if (tailSeg[a] != null || tailSeg[a] != Vector2.Zero)
                    {
                        Rectangle rect = new Rectangle(0, Y(), 14, 14);
                        spriteBatch.Draw(tex, tailSeg[a] - Main.screenPosition, rect, drawColor, 0, new Vector2(7), 1, DS.FlipTex(-1), 0);
                    }
                    int Y()
                    {
                        switch (a)
                        {
                            case 0:
                                return 28;
                            case 1:
                                return 14;
                            case 2:
                                return 0;
                            case 3:
                                return 14;
                        }
                        return 42;
                    }
                }
                spriteBatch.Draw(tex2, DS.DrawPos(NPC.Center), DS.DrawFrame(tex2), drawColor, NPC.rotation *= 0.25f, DS.DrawOrigin(tex2), NPC.scale, NPC.velocity.X < 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
            return false;
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
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), Mod.Find<ModGore>($"Meowzer{i}").Type);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 5, 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PastryBlock>(), 2, 15, 25));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ToastyToaster>(), 20));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PastryTart>(), 95, 1));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ConfectionBiome>()) && !spawnInfo.AnyInvasionActive() && Main.hardMode && !spawnInfo.Player.ZoneRockLayerHeight && !spawnInfo.Player.ZoneDirtLayerHeight && !Main.dayTime) {
                return 0.8f;
            }
            return 0f;
        }
    }
    public class MeowzerBeam : ModProjectile
    {
        int[] alpha = new int[2];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Bullet);
            AIType = ProjectileID.Bullet;
            Projectile.width = 30;
            Projectile.height = 42;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.coldDamage = true;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D beam = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Meowzer/MeowzerBeam").Value;
            Texture2D tex = ModContent.Request<Texture2D>("TheConfectionRebirth/NPCs/Meowzer/MeowzerBeamPrime").Value;
            float val = (float)alpha[1] / 255;

            Vector2 drawOrigin = new Vector2(tex.Width / 4, tex.Height / 2);
            drawOrigin.X -= 15f;
            drawOrigin.Y -= 25f;
            Rectangle refRect = new Rectangle(tex.Width / 2, 0, tex.Width / 2, tex.Height);
            Rectangle rectangle = new Rectangle(refRect.X, refRect.Y, (tex.Width / 2), (tex.Height / Main.projFrames[Projectile.type]));
            Main.EntitySpriteDraw(beam, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, beam.Width / 2, beam.Height), Utilities.LerpColor(Color.Transparent, lightColor, val), Projectile.rotation, drawOrigin + new Vector2(15, -5), Projectile.scale * 1.25f, DS.FlipTex(-1), 0);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(15f, Projectile.gfxOffY - 5);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(beam, drawPos, new Rectangle(beam.Width / 2, 0, beam.Width / 2, beam.Height), Utilities.LerpColor(Color.Transparent, color, val * 0.5f), Projectile.rotation, drawOrigin + new Vector2(15, 0), Projectile.scale, DS.FlipTex(-1), 0);
                Main.EntitySpriteDraw(tex, drawPos, rectangle, color, Projectile.rotation, drawOrigin + new Vector2(15, -5), Projectile.scale, DS.FlipTex(-1), 0);
            }
            return false;
        }
        public override void AI()
        {
            if (alpha[0]++ > 20)
            {
                float val = MathHelper.Lerp(0, 255, (float)alpha[1] / 255);
                alpha[1] += 5;
                if (alpha[1] > 255)
                    alpha[1] = 255;
                Lighting.AddLight(Projectile.Center, new Vector3(val *= 0.015f));
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.Center);
            for (int i = 0; i < 14; i++)
            {
                Vector2 position = Projectile.Center + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / 14 * i));
                Dust dust = Dust.NewDustPerfect(position, 66);
                dust.noGravity = true;
                dust.velocity = Vector2.Normalize(dust.position - Projectile.Center) * 8.75f;
                dust.noLight = false;
                dust.fadeIn = 1f;
            }
        }
    }
    public class MeowzerCannonHitbox : ModNPC
    {
        public NPC owner;
        bool scaleStat;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }
        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 20;
            NPC.aiStyle = -1;
            NPC.lifeMax = 75;
            NPC.defense = 16;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 1f;
            NPC.noGravity = true;
            NPC.netAlways = true;
            NPC.HitSound = SoundID.NPCHit38;
            NPC.DeathSound = SoundID.NPCDeath41;
        }
        public override void AI()
        {
            if (!scaleStat)
            {
                NPC.lifeMax = NPC.life = 150;
                scaleStat = true;
            }
            if (owner == null || !owner.active)
            {
                NPC.life = int.MinValue;
                NPC.checkDead();
            }
            else if (owner.ModNPC is Meowzer m)
                NPC.position = m.NPC.Center + new Vector2(owner.spriteDirection == -1 ? 10f : -20f, -55f);
        }
		public override void OnKill()
		{
            if (Main.netMode == NetmodeID.Server)
                return;

            Gore.NewGore(NPC.GetSource_Death(), NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), ModContent.GoreType<MeowzerTail>());
        }
		public override bool CheckActive() => false;
    }

    namespace Gores
    {
        public class MeowzerTail : ModGore { }
    }
}


