using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Backgrounds
{
    public class ConfectionSandSurfaceBackgroundStyle : ModSurfaceBackgroundStyle, IBackground
    {
        // Use this to keep far Backgrounds like the mountains.
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
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceFar");
        }

        public override int ChooseMiddleTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceMid0");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceClose");
        }

        public Asset<Texture2D> GetFarTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceFar");
        }

        public Asset<Texture2D> GetCloseTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceClse");
        }

        public Asset<Texture2D> GetMidTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceMid0");
        }

        public Asset<Texture2D> GetUltraFarTexture(int i)
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSandSurfaceFar");
        }
    }
}