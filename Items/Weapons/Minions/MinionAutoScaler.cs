using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Utils
{
    public class MiniMinionStat
    {
        public float speed;
        public float damage;
        public int crit;
        public float prefixMinionPower;
        public float kb;
        public Projectile projectile;
    }

    public class ConfectionPlayerMinionScaler : ModPlayer
    {
        List<MinionAutoScaler> minions;
        public override void PreUpdate()
        {
            minions.ForEach(m => m.Scale());
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
        List<MiniMinionStat> From = new();
        public List<Projectile> Directed = new();

        public void Scale()
        {
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
                speed += x.speed;
                crit += x.crit;
                prefixMinionPower += x.prefixMinionPower;
            });
            speed /= From.Count;
            crit /= From.Count;
            prefixMinionPower /= From.Count;
            Directed.ForEach(x => {
                SummonersShineCompat.ModSupport_SetVariable_ProjFuncs(x, SummonersShineCompat.ProjectileFuncsVariableType.ProjectileCrit, crit);
                SummonersShineCompat.ModSupport_SetVariable_ProjFuncs(x, SummonersShineCompat.ProjectileFuncsVariableType.MinionASMod, speed);
                SummonersShineCompat.ModSupport_SetVariable_ProjData(x, SummonersShineCompat.ProjectileDataVariableType.prefixMinionPower, prefixMinionPower);
            });
        }

        public void Add_From(Projectile proj)
        {
            MiniMinionStat stat = new MiniMinionStat();
            stat.damage = proj.originalDamage;
            stat.kb = proj.knockBack;
            if (SummonersShineCompat.SummonersShine != null)
            {
                stat.speed = (float)SummonersShineCompat.ModSupport_GetVariable_ProjFuncs(proj, SummonersShineCompat.ProjectileFuncsVariableType.MinionASMod);
                stat.crit = (int)SummonersShineCompat.ModSupport_GetVariable_ProjFuncs(proj, SummonersShineCompat.ProjectileFuncsVariableType.ProjectileCrit);
                stat.prefixMinionPower = (float)SummonersShineCompat.ModSupport_GetVariable_ProjData(proj, SummonersShineCompat.ProjectileDataVariableType.prefixMinionPower);
            }
            From.Add(stat);
        }

        public void Remove_From(Projectile proj)
        {
            From.RemoveAll(i=>i.projectile == proj);
        }
    }
}
