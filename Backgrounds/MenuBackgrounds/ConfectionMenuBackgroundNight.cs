using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Backgrounds.MenuBackgrounds
{
    public class ConfectionMenuBackgroundNight : ModSurfaceBackgroundStyle, IBackground
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
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurface2Far");
        }

        public override int ChooseMiddleTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurface2Mid");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/MenuBackgrounds/ConfectionMenu2Close");
        }


        public Asset<Texture2D> GetFarTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface2Far");
        }

        public Asset<Texture2D> GetCloseTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/MenuBackgrounds/ConfectionMenu2Close");
        }

        public Asset<Texture2D> GetMidTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface2Mid");
        }

        public Asset<Texture2D> GetUltraFarTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface2UltraFar");
        }
    }
}