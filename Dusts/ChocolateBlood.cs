using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
	public class ChocolateBlood : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = false;
			dust.noLight = true;
			dust.scale *= 1f;
			ChildSafety.SafeDust[Type] = false;
		}
	}
}
