using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs
{
    class GoneBananas : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Going Bananas");
            Description.SetDefault("Teleporting through rifts will take life");
            Main.debuff[Type] = true;
        }
    }
}
