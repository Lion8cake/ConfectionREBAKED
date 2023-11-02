using Terraria.GameContent.UI;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheConfectionRebirth.EmoteBubbles
{
	public class ItemSlammer : ModEmoteBubble
	{
		public override void SetStaticDefaults() {
			AddToCategory(EmoteID.Category.Items);
		}

		public override LocalizedText Command => Language.GetOrRegister("Mods.TheConfectionRebirth.Emotes.GrandSlammerEmote");
	}
}
