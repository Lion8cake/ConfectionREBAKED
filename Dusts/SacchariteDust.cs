using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
	public class SacchariteDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noLight = true;
			dust.scale *= 1f;
		}
	}
}
