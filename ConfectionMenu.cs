using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;
using TheConfectionRebirth.Backgrounds.MenuBackgrounds;
using Terraria;

<<<<<<< HEAD
<<<<<<< Updated upstream
namespace TheConfectionRebirth
{
    public class ConfectionMenu : ModMenu
    {
        private const string menuAssetPath = "TheConfectionRebirth/Assets";

        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{menuAssetPath}/Logo");
=======
namespace TheConfectionRebirth {
	public class ConfectionMenu : ModMenu {

		private bool Background = false;
		private bool Background1 = false;
		private bool Background2 = false;
		private bool Background3 = false;

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("TheConfectionRebirth/Assets/Logo");
>>>>>>> Stashed changes
=======
namespace TheConfectionRebirth {
	public class ConfectionMenu : ModMenu {
		private const string menuAssetPath = "TheConfectionRebirth/Assets";

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{menuAssetPath}/Logo");
>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");

<<<<<<< HEAD
<<<<<<< Updated upstream
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle
        {
            get
            {
                if (Main.dayTime)
                {
                    return ModContent.GetInstance<ConfectionMenuBackground>();
                }
=======
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
>>>>>>> Stashed changes
=======
		public override ModSurfaceBackgroundStyle MenuBackgroundStyle {
			get {
				if (Main.dayTime) {
					return ModContent.GetInstance<ConfectionMenuBackground>();
				}
				return ModContent.GetInstance<ConfectionMenuBackgroundNight>();
			}
		}
>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb

		public override string DisplayName => "Confection Menu";
	}
}
