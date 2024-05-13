using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
	public class CreamstoneDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = false;
			dust.noLight = true;
			dust.scale *= 1f;
		}
	}
}
