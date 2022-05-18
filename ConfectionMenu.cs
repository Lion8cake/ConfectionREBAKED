using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth
{
    public class ConfectionMenu : ModMenu
    {
        private const string menuAssetPath = "TheConfectionRebirth/Assets";

        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{menuAssetPath}/Logo");

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");

        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<ConfectionMenuBackground>();

        public override string DisplayName => "Confection Menu";
    }
}
