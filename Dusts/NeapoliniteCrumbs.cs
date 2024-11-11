using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
    public class NeapoliniteCrumbs : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.4f;
            dust.noGravity = true;
            dust.noLight = true;
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.15f;
            dust.scale *= 0.98f;
            float light = 0.35f * dust.scale;
            Lighting.AddLight(dust.position, light, light, light);
            if (dust.scale < 0.1f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}