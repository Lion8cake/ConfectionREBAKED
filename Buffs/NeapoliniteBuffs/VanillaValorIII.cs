using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class VanillaValorIII : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vanilla Valor (III)");
            Description.SetDefault("6% increased Critical strike chance");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 6f;
        }
    }
}
