using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class ChocolateChargeIII : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chocolate Charge (III)");
            Description.SetDefault("12% increased ranged damage and 6% increased Critical strike chance");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Ranged) += 0.12f;
            player.GetCritChance(DamageClass.Generic) += 6f;
        }
    }
}
