using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs
{
    public class PixieStickBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mounts.PixieStickMount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}
