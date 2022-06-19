using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class ChocolateChargeI : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chocolate Charge (I)");
            Description.SetDefault("4% increased ranged damage and 2% increased Critical strike chance");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Ranged) += 0.04f;
            player.GetCritChance(DamageClass.Generic) += 2f;
        }
    }
}
