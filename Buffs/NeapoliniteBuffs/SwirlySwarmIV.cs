using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class SwirlySwarmIV : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.20f;
			player.GetCritChance(DamageClass.Summon) += 10f;
			player.GetModPlayer<ConfectionPlayer>().neapolinitePowerLevel = 4;
		}
    }
}
