using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs
{
    public class GoneBananas : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
    }
}
