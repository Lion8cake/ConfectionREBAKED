using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons.Minions.Gastropod
{
public class GastropodSummonBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoTimeDisplay[Type] = true;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex) {
			ConfectionPlayer modPlayer = player.GetModPlayer<ConfectionPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<GastropodSummonProj>()] > 0) {
				modPlayer.miniGastropod = true;
			}
			if (!modPlayer.miniGastropod) {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else {
				player.buffTime[buffIndex] = 18000;
			}
		}
}
}