using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Util
{
    public struct MiniMinionStat
    {
        public float speed;
        public float damage;
        public int crit;
        public float prefixMinionPower;
        public float kb;
        public Projectile projectile;

        public MiniMinionStat(Projectile proj)
        {
            projectile = proj;
            damage = proj.originalDamage;
            kb = proj.knockBack;
            if (SummonersShineCompat.SummonersShine != null)
            {
                speed = (float)SummonersShineCompat.ModSupport_GetVariable_ProjFuncs(proj, SummonersShineCompat.ProjectileFuncsVariableType.MinionASMod);
                crit = (int)SummonersShineCompat.ModSupport_GetVariable_ProjFuncs(proj, SummonersShineCompat.ProjectileFuncsVariableType.ProjectileCrit);
                prefixMinionPower = (float)SummonersShineCompat.ModSupport_GetVariable_ProjFuncs(proj, SummonersShineCompat.ProjectileFuncsVariableType.PrefixMinionPower);
            }
            else {
                speed = 0;
                crit = 0;
                prefixMinionPower = 0;
            }
        }
    }

    public class ConfectionPlayerMinionScaler : ModPlayer
    {
        List<MinionAutoScaler> minions;
        
        public override ModPlayer NewInstance(Player entity)
        {
            ConfectionPlayerMinionScaler rv = (ConfectionPlayerMinionScaler)base.NewInstance(entity);
            rv.minions = new List<MinionAutoScaler>();
            return rv;
        }
        public override void Initialize()
        {
            minions.Clear();
        }

        public override void PreUpdate()
        {
            minions.ForEach(x => x.Scale());
        }
        public T GetAutoScaler<T>() where T : MinionAutoScaler, new()
        {
            T rv = minions.Find(i => i as T != null) as T;
            if (rv == null)
            {
                rv = new T();
                minions.Add(rv);
            }
            return rv;
        }
    }
    public abstract class MinionAutoScaler
    {
        public List<MiniMinionStat> From = new();
        public List<Projectile> Directed = new();

        public virtual float InitialDamageMod => 0;
        public virtual float DamagePerMinion => 1;

        public void Scale()
        {
            if (From.Count == 0)
                return;
            if (SummonersShineCompat.SummonersShine != null)
                Scale_SummonersShine();
            float damage = 0;
            float kb = 0;
            From.ForEach(x =>
            {
                damage += x.damage;
                kb += x.kb;
            });
            damage /= From.Count;
            damage *= InitialDamageMod + DamagePerMinion * From.Count;
            kb /= From.Count;
            Directed.ForEach(x => {
                x.originalDamage = (int)damage;
                x.knockBack = kb;
            });
        }
        void Scale_SummonersShine()
        {
            float speed = 0;
            int crit = 0;
            float prefixMinionPower = 0;
            From.ForEach(x =>
            {
                speed += 1 / x.speed;
                crit += x.crit;
                prefixMinionPower += x.prefixMinionPower;
            });
            speed /= From.Count;
            crit /= From.Count;
            prefixMinionPower /= From.Count;
            Directed.ForEach(x => {
                SummonersShineCompat.ModSupport_SetVariable_ProjFuncs(x, SummonersShineCompat.ProjectileFuncsVariableType.ProjectileCrit, crit);
                SummonersShineCompat.ModSupport_SetVariable_ProjFuncs(x, SummonersShineCompat.ProjectileFuncsVariableType.MinionASMod, 1 / speed);
                SummonersShineCompat.ModSupport_SetVariable_ProjFuncs(x, SummonersShineCompat.ProjectileFuncsVariableType.PrefixMinionPower, prefixMinionPower);
            });
        }

        public void Add_From(Projectile proj)
        {
            MiniMinionStat stat = new MiniMinionStat(proj);
            From.Add(stat);
        }

        public void Remove_From(Projectile proj)
        {
            From.RemoveAll(i=>i.projectile == proj);
        }
    }
}
