using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
	public class BirthdayDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = false;
			dust.noLight = true;
		}
	}
}
