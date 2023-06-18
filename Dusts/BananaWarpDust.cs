using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
    public class BananaWarpDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.4f;
            dust.noLight = true;
            dust.scale *= 1f;
        }
    }
}