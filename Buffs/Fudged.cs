using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs
{
    public class Fudged : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.pickSpeed -= 1.2f;
            player.tileSpeed -= 1.2f;
            player.wallSpeed -= 1.2f;
            player.moveSpeed -= 1.5f;
            player.jumpSpeedBoost -= 5f;
            player.runAcceleration -= 0.8f;
        }
    }
}
