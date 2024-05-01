using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
    public class ShellBlockDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.4f;
            // dust.noGravity = false;
            dust.noLight = true;
            dust.scale *= 1f;
        }
    }
}