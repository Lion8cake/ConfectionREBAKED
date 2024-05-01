using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
	public class NeapoliniteStrawberryDust : ModDust {
		public override void OnSpawn(Dust dust) {
			dust.noLight = true;
			dust.scale *= 0.75f;
		}
	}
}