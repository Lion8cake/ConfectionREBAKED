
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.NPCs;

namespace TheConfectionRebirth.Buffs
{
	public class SmashingLash : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ConfectionGlobalNPC>().SacchariteLashed = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<ConfectionPlayer>().SacchariteLashed = true;
			player.statDefense -= 4;
		}
	}
}
