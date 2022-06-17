using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static TheConfectionRebirth.SummonersShineCompat;
using static TheConfectionRebirth.SummonersShineCompat.MinionPowerCollection;

namespace TheConfectionRebirth.Items.Weapons.Minions
{
    public abstract class MinionWeaponBaseClass<T, U> : ModItem
        where T : ModBuff
        where U : MinionBaseClass<T>
    {
        public virtual int UseStyleID => ItemUseStyleID.Swing;
        public virtual int Damage => 30;
        public virtual float Knockback => 0.01f;
        public virtual int Crit => 0;
        public virtual int UseTime => 32;

        public virtual int SummonersShine_MaxEnergy => 0;

        //call minionPower.AddMinionPower here. For Summoner's Shine compat.
        internal virtual bool SummonersShine_GetMinionPower(MinionPowerCollection minionPower) { return false; }

        //note - this is treated as static. Do not use the "this" parameter.
        internal virtual Entity SummonersShine_SpecialAbilityFindTarget(Player player, Vector2 mousePos) { return null; }
        //note - this is treated as static. Do not use the "this" parameter.
        internal virtual List<Projectile> SummonersShine_SpecialAbilityFindMinions(Player player, Item item, List<Projectile> valid) { return valid; }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.StaffMinionSlotsRequired[Item.type] = 1;

            //Always check
            if (SummonersShine != null)
            {
                // Set minion power
                MinionPowerCollection minionPower = new MinionPowerCollection();
                bool add = SummonersShine_GetMinionPower(minionPower);
                if (add)
                    ModSupport_AddItemStatics(Item.type, SummonersShine_SpecialAbilityFindTarget, SummonersShine_SpecialAbilityFindMinions, minionPower, SummonersShine_MaxEnergy, true);

                ProjectileOnCreate_SetMaxEnergy(ProjectileType<U>(), SummonersShine_MaxEnergy);

            }
        }
        public override void SetDefaults()
        {
            // So the weapon doesn't damage like a sword while swinging 
            Item.noMelee = true;
            Item.useStyle = UseStyleID;
            // The damage type of this weapon
            Item.DamageType = DamageClass.Summon;
            Item.damage = Damage;
            Item.knockBack = Knockback;
            Item.crit = Crit;
            Item.useTime = UseTime;
            Item.useAnimation = UseTime;
            Item.buffType = BuffType<T>();
            Item.shoot = ProjectileType<U>();
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                player.AddBuff(Item.buffType, 2, true);
                position = Main.MouseWorld;

                player.SpawnMinionOnCursor(Item.GetSource_FromThis(), player.whoAmI, type, Item.damage, knockback);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }

    public enum MinionType
    {
        Ranged,
        IDStatic,
        Local,
    }
    public abstract class MinionBuffBaseClass<T> : ModBuff
        where T : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<T>()] > 0)
            {
                player.buffTime[buffIndex] = 2;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
    public abstract class MinionBaseClass<T> : ModProjectile
        where T : ModBuff
    {
        public virtual MinionType MinionType => MinionType.Ranged;
        public virtual int hitCooldown => 10;

        //note - this is treated as static. Do not use the "this" parameter.
        public abstract void SummonersShine_OnSpecialAbilityUsed(Projectile projectile, Entity target, int SpecialType, bool FromServer);
        public override void SetStaticDefaults()
        {
            // Denotes that this projectile is a pet or minion
            Main.projPet[Projectile.type] = true;
            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            if (SummonersShine != null)
            {
                ProjectileOnCreate_SetMinionOnSpecialAbilityUsed(Projectile.type, SummonersShine_OnSpecialAbilityUsed);
            }
        }
        public override void SetDefaults()
        {
            // Only determines the damage type
            Projectile.minion = true;
            // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            switch (MinionType)
            {
                case MinionType.Ranged:
                    break;
                case MinionType.Local:
                    Projectile.usesLocalNPCImmunity = true;
                    Projectile.localNPCHitCooldown = hitCooldown;
                    break;
                case MinionType.IDStatic:
                    Projectile.usesIDStaticNPCImmunity = true;
                    Projectile.idStaticNPCHitCooldown = hitCooldown;
                    break;

            }
            Projectile.netImportant = true;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public sealed override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<T>());
            }
            if (player.HasBuff(BuffType<T>()))
            {
                Projectile.timeLeft = 2;
            }
            MinionAI(Main.player[Projectile.owner]);
        }

        public void AutoSeparateMinions(float Weight)
        {
            float maxspace = (float)(Projectile.width * 3);
            for (int i = 0; i < 1000; i++)
            {
                if (i != Projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == Projectile.type && Math.Abs(Projectile.position.X - Main.projectile[i].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[i].position.Y) < maxspace)
                {
                    if (Projectile.position.X < Main.projectile[i].position.X)
                    {
                        Projectile.velocity.X = Projectile.velocity.X - Weight;
                    }
                    else
                    {
                        Projectile.velocity.X = Projectile.velocity.X + Weight;
                    }
                    if (Projectile.position.Y < Main.projectile[i].position.Y)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y - Weight;
                    }
                    else
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y + Weight;
                    }
                }
            }
        }

        public abstract void MinionAI(Player owner);
    }
}
