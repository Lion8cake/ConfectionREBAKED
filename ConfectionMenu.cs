using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;
using Terraria;
using Terraria.Localization;

namespace TheConfectionRebirth {
	public class ConfectionMenu : ModMenu {
		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/Logo");

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");

		public override ModSurfaceBackgroundStyle MenuBackgroundStyle {
			get {
				if (Main.time < 100 && Main.dayTime && !Main.lockMenuBGChange) {
					ConfectionWorldGeneration.confectionBG = Main.rand.Next(4);
				}
				int bgType = ConfectionWorldGeneration.confectionBG;
				if (bgType == 0) {
					return ModContent.GetInstance<ConfectionMenuStyle0>();
				}
				else if (bgType == 1) {
					return ModContent.GetInstance<ConfectionMenuStyle1>();
				}
				else if (bgType == 2) {
					return ModContent.GetInstance<ConfectionMenuStyle2>();
				}
				else if (bgType == 3) {
					return ModContent.GetInstance<ConfectionMenuStyle3>();
				}
				return ModContent.GetInstance<ConfectionMenuStyle0>();
			}
		}

		public override string DisplayName => "Confection Menu";
	}
}
