using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class VanillaValorV : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vanilla Valor (V)");
            Description.SetDefault("10% increased Critical strike chance and all ranged attacks ignore defence");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 10f;
        }
    }
}
