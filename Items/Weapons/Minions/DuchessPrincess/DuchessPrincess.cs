using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace TheConfectionRebirth.Items.Weapons.Minions.DuchessPrincess
{
    internal class DuchessPrincessItem : MinionWeaponBaseClass<DuchessPrincessSummonBuff, DuchessPrincess>
    {
        public override int Damage => 27;
        public override float Knockback => 6;

        public override int UseStyleID => 4;
    }
    internal class DuchessPrincessSummonBuff : MinionBuffBaseClass<DuchessPrincess>
    {
    }
    internal class DuchessPrincess : MinionBaseClass<DuchessPrincessSummonBuff>
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 32;
            Projectile.height = 32;
        }
        public override void MinionAI(Player owner)
        {
            TryAttack();
            Animate();
            IdlePosition();
            Lighting.AddLight(Projectile.Center, 2f, 0.5f, 0.25f);
        }

        public void Attack(Vector2 displacement, Vector2 direction)
        {
            displacement.X = -displacement.X;
            direction.X = -direction.X;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + displacement, direction, ProjectileID.CrystalStorm, Projectile.damage, Projectile.knockBack, Projectile.owner);
        }

        public void IdlePosition()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 pos = player.Center;
            float ai0 = Projectile.ai[0];
            if (ai0 == 0)
            {
                pos.X -= 36 * player.direction;
                pos.Y -= 24;
            }
            else
            {
                pos.X -= 8 * player.direction;
                pos.Y -= 96;
                pos.X += MathF.Cos((40 - ai0) * MathF.PI / 20) * 48 * player.direction;
                pos.Y += MathF.Abs(MathF.Sin((40 - ai0) * MathF.PI / 20)) * 32;
            }
            Vector2 diff = pos - Projectile.Center;
            float len = diff.Length();
            if (len < 4)
            {
                Projectile.velocity = diff + player.velocity;
            }
            else
            {
                Projectile.velocity = diff * 4 / len + player.velocity;
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
                    Attack(new(-8, 0), new(-8, 0));
                    Projectile.ai[0] = 39;
                }
            }
            else {
                if (Projectile.ai[0] == 20)
                {
                    int target = -1; Projectile.Minion_FindTargetInRange(1400, ref target, false);
                    if (target != -1) Attack(new(8, 0), new(8, 0));
                }
                Projectile.ai[0] -= 1;
            }
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
