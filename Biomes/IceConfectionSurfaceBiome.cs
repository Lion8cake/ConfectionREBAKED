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

        public override string MapBackground => BackgroundPath;
    }
}
