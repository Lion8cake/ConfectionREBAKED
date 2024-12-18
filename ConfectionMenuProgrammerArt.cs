using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth {
	public class ConfectionMenuProgrammerArt : ModMenu {
		private const string menuAssetPath = "TheConfectionRebirth/Assets";

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{menuAssetPath}/LogoOld");

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ConfectionUnderground");

		public override ModSurfaceBackgroundStyle MenuBackgroundStyle {
			get {
				return ModContent.GetInstance<ConfectionMenuStyle1>();
			}
		}

		public override string DisplayName => "Confection 1.3.5.3";
	}
}
