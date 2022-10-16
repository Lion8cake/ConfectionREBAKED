using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Util;
using static TheConfectionRebirth.SummonersShineCompat.MinionPowerCollection;
using static TheConfectionRebirth.Util.FloodFindFuncs;

using static TheConfectionRebirth.SummonersShineCompat;
using TheConfectionRebirth.Items.Placeable;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace TheConfectionRebirth.Items.Weapons.Minions.RollerCookie
{
    public class SweetStaff : MinionWeaponBaseClass<RollerCookieSummonBuff, RollerCookieSummonProj>
    {
        public override int Damage => 45;
        public override float Knockback => 3;
        public override int Value => Item.buyPrice(0, 2, 0, 0);
        public override void AddRecipes()
        {
            CreateRecipe()
                //.AddIngredient(ModContent.ItemType<ChocolateChunk>(), 1)
                .AddIngredient(ModContent.ItemType<CookieDough>(), 10)
                .AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
                .AddIngredient(ItemID.SoulofSight, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        internal override bool SummonersShine_GetMinionPower(SummonersShineCompat.MinionPowerCollection minionPower)
        {
            minionPower.AddMinionPower(10);
            return true;
        }

        public override int SummonersShine_MaxEnergy => 600;

        internal override List<Projectile> SummonersShine_SpecialAbilityFindMinions(Player player, Item item, List<Projectile> valid)
        {
            return valid;
        }

        internal override Entity SummonersShine_SpecialAbilityFindTarget(Player player, Vector2 mousePos)
        {
            return Main.player[Main.myPlayer];
        }
    }
    public class RollerCookieSummonBuff : MinionBuffBaseClass<RollerCookieSummonProj>
    {
    }
    public class RollerCookieSummonProj : MinionBaseClass<RollerCookieSummonBuff>
    {
        const float blocksPerRotation = 9;
        const float rotationPerBlock = 1 / blocksPerRotation;

        public override int hitCooldown => 120;
        public override MinionType MinionType => MinionType.Local;

        public static int castType = 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 24; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.frame = castType;
            castType++;
            if (castType == 6)
                castType = 0;
            Projectile.width = 14;
            Projectile.height = 14;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -2;
            Projectile.extraUpdates = 3;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            int speedCount = (int)(Projectile.velocity.Length() * 4);
            speedCount = Math.Min(Projectile.oldPos.Length, speedCount);
            Rectangle frame = texture.Frame(1, 6, 0, Projectile.frame);
            float trailAlpha = Projectile.localAI[0] * 0.5f;
            for (int k = 0; k < speedCount; k += 2)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((speedCount - k) / (float)speedCount) * trailAlpha;
                Main.EntitySpriteDraw(texture, drawPos, frame, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }

        //ai0 guide
        //0 - normal
        //1 - retreating

        int State {
            get {
                return ((int)Projectile.ai[0]) % 2;
            }
            set
            {
                Projectile.ai[0] = Projectile.ai[0] - State + value;
            }
        }

        int WallClimbState
        {
            get
            {
                return ((int)Projectile.ai[0]) / 2;
            }
            set {

                Projectile.ai[0] = State + value * 2;
            }
        }

        const int wallLatchTime = 30;
        const int wallLatchTimePlusOne = wallLatchTime + 1;
        int AttackCooldown
        {
            get
            {
                return ((int)Projectile.ai[1]) / wallLatchTimePlusOne;
            }
            set
            {

                Projectile.ai[1] = WallLatchDuration + value * wallLatchTimePlusOne;
            }
        }
        int WallLatchDuration
        {
            get
            {
                return ((int)Projectile.ai[1]) % wallLatchTimePlusOne;
            }
            set
            {
                Projectile.ai[1] = Projectile.ai[1] - WallLatchDuration + value;
            }
        }

        public override void MinionAI(Player owner)
        {
            const int NORMAL = 0;
            const int RETREATING = 1;

            const int minionTrackingImperfection = 5;
            const int maxDist = (16 * 40) * (16 * 40);
            const int maxDistUnlatch = (16 * 100) * (16 * 100);
            const int maxDistTeleport = (16 * 150) * (16 * 150);

            const float normalGravity = 0.05f;

            float trailTargetAlpha = 1;


            if (SummonersShine != null && Projectile.Projectile_IsCastingSpecialAbility(ModContent.ItemType<SweetStaff>()))
            {
                Projectile.IncrementSpecialAbilityTimer(1200);
                Vector2 cdVec2 = (Vector2)ModSupport_GetVariable_ProjData(Projectile, ProjectileDataVariableType.specialCastPosition);
                if (cdVec2.Y > 0)
                    cdVec2.Y--;
                ModSupport_SetVariable_ProjData(Projectile, ProjectileDataVariableType.specialCastPosition, cdVec2);
            }

            int targetID = -1;
            if (Projectile.Center.DistanceSQ(owner.Center) < maxDistUnlatch)
                Projectile.Minion_FindTargetInRange(1400, ref targetID, false);
            Entity target;
            if (targetID == -1)
                target = owner;
            else
                target = Main.npc[targetID];

            Unadhere();


            //brain
            bool rollOnGround;
            bool unRetreated = false;
            if (target.Center.DistanceSQ(Projectile.Center) > maxDistTeleport)
                Projectile.Center = RollerCookie_GetIdlePos_Floating(owner);
            if (target == owner)
            {
                if (State == NORMAL && target.Center.DistanceSQ(Projectile.Center) > maxDist)
                    State = RETREATING;
                Projectile.tileCollide = State == RETREATING;
                Vector2 origin = RollerCookie_GetIdlePos_Standing(owner);
                rollOnGround = State != RETREATING && !Collision.CanHitLine(origin, 0, 0, origin + new Vector2(0, 480), 0, 0);
            }
            else
            {
                bool retreating = State == RETREATING;
                if (State == NORMAL && (!Projectile.CanHitWithOwnBody(target) || !Collision.CanHitLine(target.Center, 0, 0, Projectile.Center, 0, 0)))
                    State = RETREATING;
                else if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                    State = NORMAL;
                rollOnGround = State != RETREATING && !Collision.CanHitLine(target.Center, 0, 0, target.Center + new Vector2(0, 640), 0, 0);
                if(retreating && State != RETREATING)
                    unRetreated = true;

            }
            if (State == RETREATING)
                Projectile.tileCollide = false;
            else
                Projectile.tileCollide = true;

            if (!rollOnGround)
                WallClimbState = GROUND;

            //remove player vel to get working vel

            Vector2 targetVel;
            float targetVelLen = target.velocity.Length();
            if (targetVelLen > minionTrackingImperfection)
                targetVel = target.velocity * (targetVelLen - minionTrackingImperfection) / targetVelLen;
            else
                targetVel = Vector2.Zero;
            targetVel /= Projectile.extraUpdates + 1;

            if (target == owner)
                Projectile.velocity -= targetVel;

            //attack

            if (targetID != -1)
            {
                const float shotSpeed = 4;
                Vector2 dist = target.Center - Projectile.Center;
                Projectile.friendly = true;
                if (rollOnGround)
                {
                    if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                    {
                        if (unRetreated || Projectile.velocity.Y == 0)
                        {
                            float mag = dist.Length();
                            if (mag > 0)
                            {
                                WallClimbState = GROUND;
                                float timeTaken = mag / shotSpeed;
                                Projectile.velocity = dist * shotSpeed / mag;
                                Projectile.velocity.Y -= timeTaken * 0.7f * normalGravity;
                            }
                        }
                    }
                    AttackCooldown = 0;
                }
                else if (AttackCooldown > 0)
                    AttackCooldown--;
            }
            else
            {
                AttackCooldown = 0;
                Projectile.friendly = false;
            }
            //movement

            Vector2 targetPos;

            if (rollOnGround)
            {
                if (target == owner)
                {
                    targetPos = RollerCookie_GetIdlePos_Standing(owner);
                    targetPos.Y = Projectile.Center.Y;

                    Vector2 dist = targetPos - Projectile.Center;
                    if (dist.LengthSquared() > 300)
                    {
                        dist.X = Math.Sign(dist.X);
                        if (WallClimbState == 0)
                            Projectile.velocity.X += dist.X * 0.25f;
                        else

                            Projectile.velocity.X += dist.X * 0.35f;
                    }
                    Projectile.velocity.X = Projectile.velocity.X * 0.9f;
                }
                else
                {
                    if (Collision.CanHitLine(Projectile.Center, 0, 0, Projectile.Center + new Vector2(0, 160), 0, 0))
                    {
                        targetPos = target.Center;
                        targetPos.Y = Projectile.Center.Y;
                        Vector2 dist = targetPos - Projectile.Center;
                        if (dist.LengthSquared() > 300)
                        {
                            dist.X = Math.Sign(dist.X);
                            Projectile.velocity.X += dist.X * 0.3f;
                        }
                        Projectile.velocity.X = Projectile.velocity.X * 0.999f;
                        if (Projectile.velocity.Y != 0 && Projectile.Hitbox.Intersects(target.Hitbox))
                        {
                            Vector2 pos = Projectile.position;
                            Projectile.Center = target.Center;
                            Projectile.Damage();
                            Projectile.position = pos;
                            Vector2 diff = target.position - Projectile.position;
                            diff.Normalize();
                            float dot = Vector2.Dot(Projectile.velocity, diff);
                            if (dot > 0)
                            {
                                diff *= Vector2.Dot(Projectile.velocity, diff);
                                Projectile.velocity -= 2 * diff;
                            }
                        }
                    }
                }
            }
            else
            {
                targetPos = RollerCookie_GetIdlePos_Floating(owner);
                Vector2 dist = targetPos - Projectile.Center;
                if (target == owner)
                {
                    float lenSqr = dist.LengthSquared();
                    if (lenSqr > 300)
                    {
                        dist.Normalize();
                        Projectile.velocity += dist / 2;
                    }
                    if (lenSqr < 3200)
                    {
                        trailTargetAlpha = lenSqr / 3200;
                    }
                    else if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                        State = NORMAL;
                    Projectile.velocity = Projectile.velocity * 0.9f;
                }
                else
                {
                    Vector2 targetDist;
                    if (AttackCooldown == 0)
                    {
                        targetDist = target.Center - Projectile.Center;
                        if (Projectile.Hitbox.Intersects(target.Hitbox))
                        {
                            AttackCooldown = Main.rand.Next(130, 181);
                        }
                    }
                    else
                        targetDist = dist;
                    if (targetDist != Vector2.Zero)
                    {
                        targetDist.Normalize();
                        float dot = Vector2.Dot(Projectile.velocity, targetDist);
                        Projectile.velocity += targetDist * 0.1f;
                        if (dot < 0)
                        {
                            Projectile.velocity *= 0.9f;
                        }
                        else
                        {
                            Vector2 dotted = targetDist * dot;
                            Projectile.velocity -= dotted;
                            Projectile.velocity *= 0.998f;
                            Projectile.velocity += dotted;
                        }
                    }
                }
            }

            //animation
            int flipDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.rotation += Projectile.velocity.Length() * flipDirection * rotationPerBlock;
            if (target == owner)
                Projectile.velocity += targetVel;

            //wall climb
            if (rollOnGround)
            {
                if (WallClimbState == 0)
                    Projectile.velocity.Y += normalGravity;
                else
                    Projectile.velocity.Y = Math.Max(2, Projectile.velocity.Y) + 0.3f;
                Adhere();
                SwitchAdhering();
            }
            else
            {
                WallLatchDuration = 0;
                Adhere();
            }

            //effects

            Lighting.AddLight(Projectile.Center, new Vector3(0.67f, 0.44f, 0.0f) * 0.7f);
            CreateDust();


            //keep inside bounds
            Point worldPoint = (Projectile.position + Projectile.velocity).ToTileCoordinates();
            if (!WorldGen.InWorld(worldPoint.X, worldPoint.Y))
            {
                Projectile.Center = owner.Center;
                Projectile.velocity = Vector2.Zero;
            }

            //trail alpha
            Projectile.localAI[0] = MathHelper.Lerp(Projectile.localAI[0], trailTargetAlpha, 0.05f);
        }
        const int GROUND = 0;
        const int LEFTWALL = 1;
        const int RIGHTWALL = 2;
        const int CEIL = 3;
        void SwitchAdhering() {
            
            WallClimbState = SwitchAdhering(Projectile, WallClimbState, WallLatchDuration > 0);

            if (WallClimbState != 0)
                WallLatchDuration = 30;
            else if(WallLatchDuration > 0)
                WallLatchDuration--;
        }
        public static int SwitchAdhering(Projectile Projectile, int WallClimbState, bool floorSticky)
        {
            Vector2 tileCollisionChange = Collision.TileCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            float xDiff = tileCollisionChange.X - Projectile.velocity.X;
            float yDiff = tileCollisionChange.Y - Projectile.velocity.Y;
            if (xDiff == 0 || yDiff == 0)
            {
                Vector4 slopeCollisionChange = Collision.SlopeCollision(Projectile.position + tileCollisionChange, tileCollisionChange, Projectile.width, Projectile.height);
                tileCollisionChange.X = slopeCollisionChange.Z;
                tileCollisionChange.Y = slopeCollisionChange.W;
                xDiff = tileCollisionChange.X - Projectile.velocity.X;
                yDiff = tileCollisionChange.Y - Projectile.velocity.Y;
            }
            int newState = WallClimbState;
            switch (WallClimbState)
            {
                case GROUND:
                    {
                        if (xDiff > 0)
                            newState = LEFTWALL;
                        else if (xDiff < 0)
                            newState = RIGHTWALL;

                        else if (floorSticky && yDiff == 0 && !Collision.SolidCollision(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height))
                        {
                            Projectile.position += tileCollisionChange;
                            if (Projectile.velocity.X > 0)
                                newState = LEFTWALL;
                            else if (Projectile.velocity.X < 0)
                                newState = RIGHTWALL;
                        }
                    }
                    break;
                case LEFTWALL:
                case RIGHTWALL:
                    {
                        if (xDiff == 0 && !Collision.SolidCollision(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height))
                        {
                            Projectile.position += tileCollisionChange;
                            if (Projectile.velocity.Y > 0)
                                newState = CEIL;
                            else if (Projectile.velocity.Y < 0)
                                newState = GROUND;
                        }
                        else
                        {
                            if (yDiff > 0)
                                newState = CEIL;
                            else if (yDiff < 0)
                                newState = GROUND;
                        }
                    }
                    break;
                case CEIL:
                    {
                        if (yDiff == 0 && !Collision.SolidCollision(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height))
                        {
                            Projectile.position += tileCollisionChange;
                            if (Projectile.velocity.X > 0)
                                newState = LEFTWALL;
                            else
                                newState = RIGHTWALL;
                        }
                        else
                        {
                            if (xDiff > 0)
                                newState = LEFTWALL;
                            else if (xDiff < 0)
                                newState = RIGHTWALL;
                        }
                    }
                    break;
            }
            if (newState != WallClimbState)
            {
                Unadhere(Projectile, WallClimbState);
                WallClimbState = newState;
                Adhere(Projectile, WallClimbState);
            }
            return WallClimbState;
        }
        public static bool CheckSlope(SlopeType type, Vector2 checkPos)
        {
            Vector2[] points =
            {
                new Vector2(5, -5),
                new Vector2(-5, -5),
                new Vector2(5, 5),
                new Vector2(-5, 5),
            };
            for(int x = 0; x < 4; x++)
            {

                Tile tile = Main.tile[(points[x] + checkPos).ToTileCoordinates()];
                if (tile.Slope == type)
                {
                    return true;
                }
            }
            return false;
        }

        void CreateDust() { CreateDust(Projectile); }
        public static void CreateDust(Projectile Projectile)
        {
            if (!Main.rand.NextBool(30))
                return;
            Color dustColor;
            switch (Projectile.frame)
            {
                case 0:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            dustColor = Color.Red;
                            break;
                        case 1:
                            dustColor = Color.Green;
                            break;
                        default:
                            dustColor = Color.Beige;
                            break;
                    }
                    break;
                case 1:
                    dustColor = Color.Chocolate;
                    break;
                case 2:
                    dustColor = Color.SaddleBrown;
                    break;
                case 3:
                    dustColor = Color.AntiqueWhite;
                    break;
                case 4:
                    dustColor = Color.RosyBrown;
                    break;
                case 5:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            dustColor = Color.RosyBrown;
                            break;
                        default:
                            dustColor = Color.Chocolate;
                            break;
                    }
                    break;
                default:
                    dustColor = Color.Chocolate;
                    break;
            }
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SpectreStaff, 0, 0, 0, dustColor);
            dust.velocity *= 0.1f;
        }

        void Unadhere() { Unadhere(Projectile, WallClimbState); }
        void Adhere() { Adhere(Projectile, WallClimbState); }
        public static void Unadhere(Projectile Projectile, int WallClimbState)
        {
            switch (WallClimbState)
            {
                case LEFTWALL:
                    Projectile.velocity = new(Projectile.velocity.Y, -Projectile.velocity.X);
                    break;
                case RIGHTWALL:
                    Projectile.velocity = new(-Projectile.velocity.Y, Projectile.velocity.X);
                    break;
                case CEIL:
                    Projectile.velocity = new(-Projectile.velocity.X, -Projectile.velocity.Y);
                    break;
            }
        }
        public static void Adhere(Projectile Projectile, int WallClimbState)
        {
            switch (WallClimbState)
            {
                case LEFTWALL:
                    Projectile.velocity = new(-Projectile.velocity.Y, Projectile.velocity.X);
                    break;
                case RIGHTWALL:
                    Projectile.velocity = new(Projectile.velocity.Y, -Projectile.velocity.X);
                    break;
                case CEIL:
                    Projectile.velocity = new(-Projectile.velocity.X, -Projectile.velocity.Y);
                    break;
            }
        }

        Vector2 RollerCookie_GetIdlePos_Floating(Player player)
        {
            float ownedProjectiles = player.ownedProjectileCounts[Projectile.type];
            if (ownedProjectiles == 0)
                ownedProjectiles = 1;
            Vector2 rv = player.Center - new Vector2(player.direction * 48, 0).RotatedBy(MathF.PI * 2 * Projectile.minionPos / ownedProjectiles);

            if (Collision.SolidCollision(rv - new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), Projectile.width, Projectile.height))
                return player.Center;
            return rv;
        }
        Vector2 RollerCookie_GetIdlePos_Standing(Player player)
        {
            float squishRatio = player.ownedProjectileCounts[Projectile.type];
            if (squishRatio > 15)
                squishRatio = 15 / squishRatio;
            else
                squishRatio = 1;
            return player.Center - new Vector2(player.direction * squishRatio * 32 * (Projectile.minionPos + 1), 0);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (SummonersShine != null && Projectile.Projectile_IsCastingSpecialAbility(ModContent.ItemType<SweetStaff>()))
            {
                Vector2 cdVec2 = (Vector2)ModSupport_GetVariable_ProjData(Projectile, ProjectileDataVariableType.specialCastPosition);
                if (cdVec2.Y <= 0)
                {
                    cdVec2.Y = 30;
                    float mp = Projectile.SummonersShine_GetMinionPower(0) * Projectile.knockBack;
                    Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, Main.rand.NextVector2CircularEdge(3, 8), ModContent.ProjectileType<MiniRollerCookieSummonersShine>(), Projectile.damage, mp, Projectile.owner);
                    projectile.localNPCImmunity[target.whoAmI] = 300;
                    ModSupport_SetVariable_ProjData(Projectile, ProjectileDataVariableType.specialCastPosition, cdVec2);
                }
            }
        }
        public override void SummonersShine_OnSpecialAbilityUsed(Projectile projectile, Entity target, int SpecialType, bool FromServer)
        {
            ModSupport_SetVariable_ProjData(projectile, ProjectileDataVariableType.castingSpecialAbilityTime, 0);
            ModSupport_SetVariable_ProjData(projectile, ProjectileDataVariableType.energy, 0f);
            ModSupport_SetVariable_ProjData(projectile, ProjectileDataVariableType.energyRegenRateMult, 0f);
            ModSupport_SetVariable_ProjData(Projectile, ProjectileDataVariableType.specialCastPosition, Vector2.Zero);
        }

        public override void SummonersShine_TerminateSpecialAbility(Projectile projectile, Player owner)
        {
        }
    }
}
