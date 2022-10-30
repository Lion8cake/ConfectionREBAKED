using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs
{
    public class CcretBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mounts.CcretMount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}
