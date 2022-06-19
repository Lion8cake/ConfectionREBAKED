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
    public class SwirlySwarmIV : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swirly Swarm (IV)");
            Description.SetDefault("30% increased whip speed and 10% increased summoner Critical strike chance.");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.30f;
            player.GetCritChance(DamageClass.Summon) += 10f;
        }
    }
}
