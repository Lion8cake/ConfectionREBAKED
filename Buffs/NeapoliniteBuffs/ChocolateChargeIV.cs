using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class ChocolateChargeIV : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chocolate Charge (IV)");
            Description.SetDefault("16% increased ranged damage and 8% increased Critical strike chance");
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
