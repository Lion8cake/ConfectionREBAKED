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
using static TheConfectionRebirth.SummonersShineCompat;

namespace TheConfectionRebirth.Items.Weapons.Minions.DuchessPrincess
{
    internal class DuchessPrincessItem : MinionWeaponBaseClass<DuchessPrincessSummonBuff, DuchessCrystal>
    {
        public override int Damage => 27;
        public override float Knockback => 6;

        public override int UseStyleID => 4;
    }
    internal class DuchessPrincessSummonBuff : MinionBuffBaseClass<DuchessCrystal>
    {
    }

    public class DuchessPrincessAutoScaler : MinionAutoScaler
    {
        public override float DamagePerMinion => 0.5f;
        public override float InitialDamageMod => 0.5f;
    }
    internal class DuchessCrystal : MinionBaseClass<DuchessPrincessSummonBuff>
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.tileCollide = false;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                DuchessPrincessAutoScaler scaler = Main.player[Projectile.owner].GetModPlayer<ConfectionPlayerMinionScaler>().GetAutoScaler<DuchessPrincessAutoScaler>();
                scaler.Remove_From(Projectile);
                scaler.Scale();
            }
        }
        public override void MinionAI(Player owner)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                DuchessPrincessAutoScaler scaler = Main.player[Projectile.owner].GetModPlayer<ConfectionPlayerMinionScaler>().GetAutoScaler<DuchessPrincessAutoScaler>();
                if (Projectile.localAI[0] == 0)
                {
                    Projectile.localAI[0] = 1;
                    scaler.Add_From(Projectile);
                }
                if (scaler.Directed.Count == 0)
                {
                    Projectile duchess = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, DuchessPrincess.id, 0, 0, Projectile.owner);
                    scaler.Directed.Add(duchess);
                }
                scaler.Scale();
            }

            if (Main.rand.NextBool(30))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.GemDiamond);
                dust.velocity *= 0.1f;
                dust.noGravity = true;
            }

            Projectile.frameCounter += 1;
            Projectile.frameCounter %= 10;
            if (Projectile.frameCounter != 0)
                return;
            Projectile.frame += 1;
            Projectile.frame %= 4;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White * 0.85f;
            lightColor.A = 190;
            return true;
        }

        public override void SummonersShine_OnSpecialAbilityUsed(Projectile projectile, Entity target, int SpecialType, bool FromServer)
        {
        }

        public override void SummonersShine_TerminateSpecialAbility(Projectile projectile, Player owner)
        {
        }
    }
    internal class DuchessPrincess : MinionBaseClass<DuchessPrincessSummonBuff>
    {
        public static int id;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 6;
            id = Projectile.type;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            if (SummonersShine != null)
            {
                ProjectileOnCreate_SetMinionPostAIPostRelativeVelocity(Projectile.type, ForceMinionPostAI);
            }
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.tileCollide = false;
            Projectile.minionSlots = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }

        public static void ForceMinionPostAI(Projectile self)
        {
            Lighting.AddLight(self.Center, 2f, 0.5f, 0.25f);
            MovePieces(self);
        }
        public override void MinionAI(Player owner)
        {
            TryAttack();
            Animate();
            IdlePosition();
            if (SummonersShine == null)
            {
                ForceMinionPostAI(Projectile);
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                DuchessPrincessAutoScaler scaler = Main.player[Projectile.owner].GetModPlayer<ConfectionPlayerMinionScaler>().GetAutoScaler<DuchessPrincessAutoScaler>();
                scaler.Directed.Remove(Projectile);
            }
        }

        public void Attack(Vector2 displacement, Vector2 direction, int target)
        {
            if (Projectile.spriteDirection == -1)
            {
                displacement.X = -displacement.X;
                direction.X = -direction.X;
            }
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + displacement, direction, ModContent.ProjectileType<DuchessPrincessRockCandy>(), Projectile.damage, Projectile.knockBack, Projectile.owner, target, Main.rand.Next(7));
        }

        public void IdlePosition()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 pos = player.Center;
            float ai0 = Projectile.ai[0];
            if (ai0 == 0)
            {
                pos.X -= 60 * player.direction;
                pos.Y -= 24;
            }
            else
            {
                pos.X -= 8 * player.direction;
                pos.Y -= 96;
                pos.X += MathF.Cos((40 - ai0) * MathF.PI / 20) * 24 * player.direction;
                pos.Y += MathF.Abs(MathF.Sin((40 - ai0) * MathF.PI / 20)) * 16;
            }
            Vector2 diff = pos - Projectile.Center;
            float len = diff.Length();
            if (len < 4 || len > 1600)
            {
                Projectile.velocity = diff + player.velocity;
            }
            else if(len < 400)
            {
                Projectile.velocity = diff * 4 / len + player.velocity;
            }
            else
            {
                Projectile.velocity = diff / 10 + player.velocity;
            }
            Projectile.direction = player.direction;
            Projectile.spriteDirection = player.direction;

        }
        public void TryAttack()
        {
            if (Projectile.ai[0] == 0)
            {
                int target = -1; Projectile.Minion_FindTargetInRange(1400, ref target, false);
                if (target != -1)
                {
                    Attack(new(-8, 0), new(-8, 0), target);
                    Attack(new(-8, 0), new(-6, 3), target);
                    Attack(new(-8, 0), new(-3, -3), target);
                    Projectile.ai[0] = 39;
                }
            }
            else {
                if (Projectile.ai[0] == 20)
                {
                    int target = -1; Projectile.Minion_FindTargetInRange(1400, ref target, false);
                    if (target != -1) Attack(new(8, 0), new(8, 0), target);
                }
                Projectile.ai[0] -= 1;
            }
        }

        public static void MovePieces(Projectile Projectile)
        {
            DuchessPrincessAutoScaler scaler = Main.player[Projectile.owner].GetModPlayer<ConfectionPlayerMinionScaler>().GetAutoScaler<DuchessPrincessAutoScaler>();
            int count = scaler.From.Count - 1;

            int iters = 0;
            Vector2 disp = new Vector2(0, 36 + 4 * count);
            if (count == 0)
            {
                MoveSingleCrownPiece(Projectile, scaler.From[0].projectile, -disp, 0);
                return;
            }
            scaler.From.ForEach(i =>
            {
                MoveSingleCrownPiece(Projectile, i.projectile, disp, MathF.PI * 0.5f + MathF.PI * iters / count);
                iters++;
            });
        }

        static void MoveSingleCrownPiece(Projectile Projectile, Projectile proj, Vector2 disp, float rotated)
        {
            Vector2 pos = Projectile.Center + disp.RotatedBy(rotated);
            proj.Center = pos;

        }

        public void Animate()
        {
            float ai0 = Projectile.ai[0];
            if (ai0 == 0)
            {
                Projectile.frameCounter += 1;
                Projectile.frameCounter %= 40;
                int frame = Projectile.frameCounter / 10;
                switch (frame)
                {
                    case 1:
                    case 3:
                        Projectile.frame = 1;
                        break;
                    case 0:
                    case 2:
                        Projectile.frame = frame;
                        break;
                }
            }
            else
            {
                int frame = (int)(ai0 / 10);
                switch (frame)
                {
                    case 1:
                        Projectile.frame = 5;
                        break;
                    case 0:
                    case 2:
                        Projectile.frame = 4;
                        break;
                    case 3:
                        Projectile.frame = 3;
                        break;
                }
            }
        }

        public override void SummonersShine_OnSpecialAbilityUsed(Projectile projectile, Entity target, int SpecialType, bool FromServer)
        {
        }

        public override void SummonersShine_TerminateSpecialAbility(Projectile projectile, Player owner)
        {
        }
    }
}
