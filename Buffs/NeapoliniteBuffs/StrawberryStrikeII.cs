using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class StrawberryStrikeII : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<ConfectionPlayer>().strawberrySpawnStrawTimer++;
			player.GetModPlayer<ConfectionPlayer>().mageStrawberry = 1;
			player.GetModPlayer<ConfectionPlayer>().neapolinitePowerLevel = 2;
		}
	}
}
