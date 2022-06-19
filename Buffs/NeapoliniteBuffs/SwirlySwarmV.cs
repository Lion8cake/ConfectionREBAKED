using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class SwirlySwarmV : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swirly Swarm (V)");
            Description.SetDefault("30% increased whip speed, 10% increased summoner critical strike chance and damage.");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.30f;
            player.GetCritChance(DamageClass.Summon) += 10f;
            player.GetDamage(DamageClass.Summon) += 0.10f;
        }
    }
}
