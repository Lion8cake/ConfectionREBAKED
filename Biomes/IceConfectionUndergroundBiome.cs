using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Biomes
{
    public class IceConfectionUndergroundBiome : ModBiome
    {
        public override string BestiaryIcon => "TheConfectionRebirth/Biomes/BestiaryIcon4";

        public override string BackgroundPath => "TheConfectionRebirth/Biomes/ConfectionUndergroundIceMapBackground";

        public override Color? BackgroundColor => base.BackgroundColor;

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ConfectionUnderground");

        public override string MapBackground => BackgroundPath;

        public override bool IsBiomeActive(Player player) => ModContent.GetInstance<ConfectionUndergroundBiome>().IsBiomeActive(player) && player.ZoneSnow;
    }
}
