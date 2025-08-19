using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
    public class NeapoliniteJoustingDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
			dust.noLight = false;
        }
		public override bool MidUpdate(Dust dust)
		{
			if (dust.noLight)
			{
				return false;
			}

			float strength = dust.scale;
			if (strength > 1f)
			{
				strength = 1f;
			}
			Lighting.AddLight(dust.position, 0.32f * strength * 0.5f, 1.74f * strength * 0.5f, 2.21f * strength * 0.5f);
			return false;
		}
	}
}