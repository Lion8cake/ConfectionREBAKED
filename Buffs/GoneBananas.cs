using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs
{
    class GoneBananas : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
    }
}
