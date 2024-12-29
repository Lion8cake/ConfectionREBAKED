using Terraria;
using Terraria.GameContent.UI;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheConfectionRebirth.EmoteBubbles
{
	public class BossDuchess : ModEmoteBubble
	{
		public override void SetStaticDefaults()
		{
			AddToCategory(EmoteID.Category.Dangers);
		}

		public override bool IsUnlocked()
		{
			return NPC.downedEmpressOfLight;
		}

		public override LocalizedText Command => Language.GetOrRegister("Mods.TheConfectionRebirth.Emotes.DuchessEmote");
	}
}
