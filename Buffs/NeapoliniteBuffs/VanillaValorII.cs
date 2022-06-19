using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class VanillaValorII : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vanilla Valor (II)");
            Description.SetDefault("4% increased Critical strike chance");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 0.04f;
        }
    }
}
