using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class ChocolateChargeII : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chocolate Charge (II)");
            Description.SetDefault("8% increased ranged damage and 4% increased Critical strike chance");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Ranged) += 0.08f;
            player.GetCritChance(DamageClass.Generic) += 4f;
        }
    }
}
