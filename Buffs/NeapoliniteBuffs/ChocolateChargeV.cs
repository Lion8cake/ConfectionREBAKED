using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class ChocolateChargeV : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chocolate Charge (V)");
            Description.SetDefault("20% increased ranged damage and 10% increased Critical strike chance");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Ranged) += 0.2f;
            player.GetCritChance(DamageClass.Generic) += 10f;
        }
    }
}
