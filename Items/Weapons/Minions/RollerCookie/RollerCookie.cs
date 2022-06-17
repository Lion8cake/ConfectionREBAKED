using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TheConfectionRebirth.Util;
using static TheConfectionRebirth.SummonersShineCompat.MinionPowerCollection;
using static TheConfectionRebirth.Util.FloodFindFuncs;

namespace TheConfectionRebirth.Items.Weapons.Minions.RollerCookie
{
    public class SweetStaff : MinionWeaponBaseClass<RollerCookieSummonBuff, RollerCookieSummonProj>
    {
        public override int Damage => 45;
        public override float Knockback => 3;
        internal override bool SummonersShine_GetMinionPower(SummonersShineCompat.MinionPowerCollection minionPower)
        {
            minionPower.AddMinionPower(5);
            return true;
        }

        public override int SummonersShine_MaxEnergy => 600;

        internal override List<Projectile> SummonersShine_SpecialAbilityFindMinions(Player player, Item item, List<Projectile> valid)
        {
            return valid;
        }

        internal override Entity SummonersShine_SpecialAbilityFindTarget(Player player, Vector2 mousePos)
        {
            return base.SummonersShine_SpecialAbilityFindTarget(player, mousePos);
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
        public override void MinionAI(Player owner)
        {
            const int NORMAL = 0;
            const int RETREATING = 1;

            const int minionTrackingImperfection = 5;
            const int maxDist = (16 * 40) * (16 * 40);
            const int maxDistTeleport = (16 * 150) * (16 * 150);

            const float normalGravity = 0.05f;

            int targetID = -1;
            Projectile.Minion_FindTargetInRange(1400, ref targetID, true);
            Entity target;
            if (targetID == -1)
                target = owner;
            else
                target = Main.npc[targetID];
            float targetVelLen = target.velocity.Length();
            Projectile.velocity = Unadhere();


            //brain
            bool rollOnGround;
            if (target.Center.DistanceSQ(Projectile.Center) > maxDistTeleport)
                Projectile.Center = RollerCookie_GetIdlePos_Floating(owner);
            if (target == owner)
            {
                if (State == NORMAL && target.Center.DistanceSQ(Projectile.Center) > maxDist)
                    State = RETREATING;
                Projectile.tileCollide = State == RETREATING;
                Vector2 origin = RollerCookie_GetIdlePos_Standing(owner);
                rollOnGround = State != RETREATING && !Collision.CanHitLine(origin, 0, 0, origin + new Vector2(0, 480), 0, 0) && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height);
            }
            else
            {
                rollOnGround = State != RETREATING && !Collision.CanHitLine(target.Center, 0, 0, target.Center + new Vector2(0, 480), 0, 0) && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height);
                if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                    State = NORMAL;
            }
            if (State == RETREATING)
                Projectile.tileCollide = false;
            else
                Projectile.tileCollide = true;

            if (!rollOnGround)
                WallClimbState = GROUND;

            //attack

            Vector2 targetVel;
            if (targetVelLen > minionTrackingImperfection)
                targetVel = target.velocity * (targetVelLen - minionTrackingImperfection) / targetVelLen;
            else
                targetVel = Vector2.Zero;
            targetVel /= Projectile.extraUpdates + 1;

            if (target == owner)
                Projectile.velocity -= targetVel;

            if (targetID != -1)
            {
                const float shotSpeed = 4;
                Vector2 dist = target.Center - Projectile.Center;
                Projectile.friendly = true;
                if (rollOnGround)
                {
                    if (Projectile.velocity.Y == 0)
                    {
                        float mag = dist.Length();
                        if (mag > 0)
                        {
                            WallClimbState = GROUND;
                            float timeTaken = mag / shotSpeed;
                            Projectile.velocity = dist * shotSpeed / mag;
                            Projectile.velocity.Y -= timeTaken / 2 * normalGravity;
                        }
                    }
                    Projectile.ai[1] = 0;
                }
                else
                {
                    if (Projectile.ai[1] > 0)
                        Projectile.ai[1]--;
                }
            }
            else
            {
                Projectile.ai[1] = 0;
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
                    if(Projectile.Hitbox.Intersects(target.Hitbox))
                    {
                        Vector2 pos = Projectile.position;
                        Projectile.Center = target.Center;
                        Projectile.Damage();
                        Projectile.position = pos;
                        Vector2 diff = target.position - Projectile.position;
                        diff.Normalize();
                        diff *= Vector2.Dot(Projectile.velocity, diff);
                        Projectile.velocity -= 2 * diff;
                    }
                }
            }
            else
            {
                targetPos = RollerCookie_GetIdlePos_Floating(owner);
                Vector2 dist = targetPos - Projectile.Center;
                if (target == owner)
                {
                    if (dist.LengthSquared() > 300)
                    {
                        dist.Normalize();
                        Projectile.velocity += dist / 2;
                    }
                    else if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                        State = NORMAL;
                    Projectile.velocity = Projectile.velocity * 0.9f;
                }
                else
                {
                    Vector2 targetDist;
                    if (Projectile.ai[1] == 0)
                    {
                        targetDist = target.Center - Projectile.Center;
                        if (Projectile.Hitbox.Intersects(target.Hitbox))
                        {
                            Projectile.ai[1] = 180;
                        }
                    }
                    else
                        targetDist = dist;
                    if (targetDist != Vector2.Zero)
                    {
                        targetDist.Normalize();
                        float dot = Vector2.Dot(Projectile.velocity, targetDist);
                        Projectile.velocity += targetDist * 0.2f;
                        if (dot < 0)
                        { Projectile.velocity *= 0.8f; }    
                        Vector2 dotted = targetDist * dot;
                    }
                }
            }
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
                    Projectile.velocity.Y = Math.Max(5, Projectile.velocity.Y) + 0.3f;
                Projectile.velocity = Adhere();
                SwitchAdhering();
            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.67f, 0.44f, 0.0f) * 0.7f);

            //keep inside bounds
            Point worldPoint = (Projectile.position + Projectile.velocity).ToTileCoordinates();
            if (!WorldGen.InWorld(worldPoint.X, worldPoint.Y))
            {
                Projectile.Center = owner.Center;
                Projectile.velocity = Vector2.Zero;
            }
        }
        const int GROUND = 0;
        const int LEFTWALL = 1;
        const int RIGHTWALL = 2;
        const int CEIL = 3;

        void SwitchAdhering()
        {
            Vector2 tileCollisionChange = Collision.TileCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            Vector4 slopeCollisionResult = Collision.SlopeCollision(Projectile.position, tileCollisionChange, Projectile.width, Projectile.height);
            tileCollisionChange.X = slopeCollisionResult.Z;
            tileCollisionChange.Y = slopeCollisionResult.W;
            int newState = WallClimbState;
            switch (WallClimbState)
            {
                case GROUND:
                    {

                        float xDiff = tileCollisionChange.X - Projectile.velocity.X;
                        if (xDiff > 0)
                            newState = LEFTWALL;
                        else if (xDiff < 0)
                            newState = RIGHTWALL;
                    }
                    break;
                case LEFTWALL:
                case RIGHTWALL:
                    {
                        float xDiff = tileCollisionChange.X - Projectile.velocity.X;
                        if (xDiff == 0)
                        {
                            Projectile.position += tileCollisionChange;
                            if (Projectile.velocity.Y > 0)
                                newState = CEIL;
                            else
                                newState = GROUND;
                        }
                        else
                        {
                            float yDiff = tileCollisionChange.Y - Projectile.velocity.Y;
                            if (yDiff > 0)
                                newState = CEIL;
                            else if (yDiff < 0)
                                newState = GROUND;
                        }
                    }
                    break;
                case CEIL:
                    {

                        float yDiff = tileCollisionChange.Y - Projectile.velocity.Y;
                        if (yDiff == 0)
                        {
                            Projectile.position += tileCollisionChange;
                            if (Projectile.velocity.X > 0)
                                newState = LEFTWALL;
                            else
                                newState = RIGHTWALL;
                        }
                        else
                        {
                            float xDiff = tileCollisionChange.X - Projectile.velocity.X;
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
                Projectile.velocity = Unadhere();
                WallClimbState = newState;
                Projectile.velocity = Adhere();
            }

        }
        Vector2 Unadhere()
        {
            switch (WallClimbState)
            {
                case LEFTWALL:
                    return new(Projectile.velocity.Y, -Projectile.velocity.X);
                case RIGHTWALL:
                    return new(-Projectile.velocity.Y, Projectile.velocity.X);
                case CEIL:
                    return new(-Projectile.velocity.X, -Projectile.velocity.Y);
            }
            return Projectile.velocity;
        }
        Vector2 Adhere()
        {
            switch (WallClimbState)
            {
                case LEFTWALL:
                    return new(-Projectile.velocity.Y, Projectile.velocity.X);
                case RIGHTWALL:
                    return new(Projectile.velocity.Y, -Projectile.velocity.X);
                case CEIL:
                    return new(-Projectile.velocity.X, -Projectile.velocity.Y);
            }
            return Projectile.velocity;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
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
        public override void SummonersShine_OnSpecialAbilityUsed(Projectile projectile, Entity target, int SpecialType, bool FromServer)
        {
        }
    }
}
