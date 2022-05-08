using Terraria.ModLoader;

namespace TheConfectionRebirth.Backgrounds
{
	public class ConfectionSnowUGBackgroundStyle : ModUndergroundBackgroundStyle
	{
		public override void FillTextureArray(int[] textureSlots) {
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSnowUG0");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSnowUG1");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSnowUG2");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSnowUG3");
		}
	}
}