using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Backgrounds
{
    public class ConfectionUndergroundBackgroundStyle : ModUndergroundBackgroundStyle
    {
        public override void FillTextureArray(int[] textureSlots)
        {
            textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionUndergroundStyleFallback1");
            textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionUndergroundStyleFallback2");
            textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionUndergroundStyleFallback3");
            textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionUnderground3_" + ConfectionWorldGeneration.confectionUGBG);
			textureSlots[4] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionUndergroundStyleFallback4");
		}
	}
}