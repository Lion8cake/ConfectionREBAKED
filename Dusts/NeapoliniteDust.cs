using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
    public class NeapoliniteDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
			dust.noLight = true;
		}
    }
}