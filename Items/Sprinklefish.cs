using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class Sprinklefish : ModItem
	{
		public override void SetStaticDefaults() {
			SacrificeTotal = 1;
		}

		public override void SetDefaults() {
			Item.questItem = true;
			Item.maxStack = 1;
			Item.width = 26;
			Item.height = 26;
			Item.uniqueStack = true;
			Item.rare = ItemRarityID.Quest;
		}

		public override bool IsQuestFish() {
			return true;
		}

		public override bool IsAnglerQuestAvailable() {
			return Main.hardMode;
		}

		public override void AnglerQuestChat(ref string description, ref string catchLocation) {
			description = Language.GetTextValue("Mods.TheConfectionRebirth.ItemAnglerChat.Sprinklefish");
			catchLocation = Language.GetTextValue("Mods.TheConfectionRebirth.Common.CaughtInConfectionSF");
		}
	}
}
