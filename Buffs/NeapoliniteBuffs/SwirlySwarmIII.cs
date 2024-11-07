using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class SwirlySwarmIII : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.10f;
			player.GetCritChance(DamageClass.Summon) += 5f;
			player.GetModPlayer<ConfectionPlayer>().neapolinitePowerLevel = 3;
		}
    }
}
