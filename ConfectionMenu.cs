using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;
using TheConfectionRebirth.Backgrounds.MenuBackgrounds;
using Terraria;

namespace TheConfectionRebirth {
	public class ConfectionMenu : ModMenu {

		private bool Background = false;
		private bool Background1 = false;
		private bool Background2 = false;
		private bool Background3 = false;

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/Logo");

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");

		public override ModSurfaceBackgroundStyle MenuBackgroundStyle {
			get {
				if (Main.time < 100 && Main.dayTime) {
					if (Main.rand.NextBool(4)) {
						Background = true;
						Background1 = false;
						Background2 = false;
						Background3 = false;
					}
					if (Main.rand.NextBool(4)) {
						Background1 = true;
						Background = false;
						Background2 = false;
						Background3 = false;
					}
					if (Main.rand.NextBool(4)) {
						Background2 = true;
						Background = false;
						Background1 = false;
						Background3 = false;
					}
					if (Main.rand.NextBool(4)) {
						Background3 = true;
						Background = false;
						Background1 = false;
						Background2 = false;
					}
				}
				if (Background) {
					return ModContent.GetInstance<ConfectionMenuBackground>();
				}
				if (Background1) {
					return ModContent.GetInstance<ConfectionSurface1BackgroundStyle>();
				}
				if (Background2) {
					return ModContent.GetInstance<ConfectionMenuBackgroundNight>();
				}
				if (Background3) {
					return ModContent.GetInstance<ConfectionMenuBackground3>();
				}
				return ModContent.GetInstance<ConfectionMenuBackground>();
			}
		}

		public override string DisplayName => "Confection Menu";
	}
}
