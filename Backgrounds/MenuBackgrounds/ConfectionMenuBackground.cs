using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth.Backgrounds.MenuBackgrounds
{
    public class ConfectionMenuBackground : ModSurfaceBackgroundStyle, IBackground
    {
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                    {
                        fades[i] = 1f;
                    }
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                    {
                        fades[i] = 0f;
                    }
                }
            }
        }

        public override int ChooseFarTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/MenuBackgrounds/ConfectionMenuFar");
        }

        public override int ChooseMiddleTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/MenuBackgrounds/ConfectionMenuMid");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/MenuBackgrounds/ConfectionMenuClose");
        }

        public Asset<Texture2D> GetFarTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
        }

        public Asset<Texture2D> GetCloseTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/MenuBackgrounds/ConfectionMenuClose");
        }

        public Asset<Texture2D> GetMidTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceMid");
        }

        public Asset<Texture2D> GetUltraFarTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurfaceUltraFar");
        }
    }
}