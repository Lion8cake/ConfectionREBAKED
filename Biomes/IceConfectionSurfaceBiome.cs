using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth.Biomes
{
    public class IceConfectionSurfaceBiome : ModBiome
    {
        public override string BestiaryIcon => "TheConfectionRebirth/Biomes/BestiaryIcon3";

        public override string BackgroundPath => "TheConfectionRebirth/Biomes/ConfectionIceBiomeMapBackground";

        public override Color? BackgroundColor => base.BackgroundColor;

        public override string MapBackground => BackgroundPath;

		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<ConfectionSnowSurfaceBackgroundStyle>();

<<<<<<< HEAD
<<<<<<< Updated upstream
=======
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<ConfectionSnowUGBackgroundStyle>();

>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb
        public override bool IsBiomeActive(Player player) => ModContent.GetInstance<ConfectionBiomeSurface>().IsBiomeActive(player) && player.ZoneSnow;
=======
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<ConfectionSnowUGBackgroundStyle>();

        public override bool IsBiomeActive(Player player) => ModContent.GetInstance<ConfectionBiome>().IsBiomeActive(player) && player.ZoneSnow;
>>>>>>> Stashed changes
    }
}
