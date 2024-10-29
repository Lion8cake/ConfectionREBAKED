using Terraria.ModLoader;

namespace TheConfectionRebirth.Backgrounds
{
    public class ConfectionUndergroundSnowBackgroundStyle : ModUndergroundBackgroundStyle
    {
        public override void FillTextureArray(int[] textureSlots)
        {
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionUndergroundStyleFallback5");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionUndergroundStyleFallback6");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionUndergroundStyleFallback7");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionUndergroundSnow3_" + ConfectionWorldGeneration.confectionUGBGSnow);
			textureSlots[4] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionUndergroundStyleFallback4");
		}
    }
}