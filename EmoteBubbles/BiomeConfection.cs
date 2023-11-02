using Terraria.GameContent.UI;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheConfectionRebirth.EmoteBubbles
{
	public class BiomeConfection : ModEmoteBubble
	{
		public override void SetStaticDefaults() {
			AddToCategory(EmoteID.Category.NatureAndWeather);
		}

		public override LocalizedText Command => Language.GetOrRegister("Mods.TheConfectionRebirth.Emotes.ConfectionBiomeEmote");
	}
}
