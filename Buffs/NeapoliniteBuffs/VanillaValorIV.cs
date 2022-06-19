using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class VanillaValorIV : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vanilla Valor (IV)");
            Description.SetDefault("8% increased Critical strike chance");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 8f;
        }
    }
}
