using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.NPCs;

namespace TheConfectionRebirth.Buffs
{
	public class HumanCandle : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.pvpBuff[Type] = true;
			BuffID.Sets.TimeLeftDoesNotDecrease[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ConfectionGlobalNPC>().candleFire = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<ConfectionPlayer>().candleFire = true;
		}
	}
}
