using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.NPCs;

namespace TheConfectionRebirth.Buffs
{
	public class SacchariteAmmoInjection : ModBuff
	{
		public override void SetStaticDefaults()
		{
			BuffID.Sets.GrantImmunityWith[Type].Add(BuffID.BoneJavelin);
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ConfectionGlobalNPC>().sacchariteAmmoDebuff = true;
		}
	}
}
