using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class ChocolateChargeIV : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Ranged) += 0.16f;
            player.GetCritChance(DamageClass.Generic) += 8f;
        }
    }
}
