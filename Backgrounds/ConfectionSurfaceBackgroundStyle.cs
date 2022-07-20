using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace TheConfectionRebirth.Backgrounds
{
    public class ConfectionSurfaceBackgroundStyle : ModSurfaceBackgroundStyle, IBackground
    {
        internal const int backgroundVariatonsTotal = 3;

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
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurface3Far");
        }

        public override int ChooseMiddleTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurface3Mid");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurface3Close");
        }

        public Asset<Texture2D> GetFarTexture(int i)
        {
            if (i == 0)
                return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface1Far");
            else if (i == 1)
                return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface2Far");
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface3Far");
        }

        public Asset<Texture2D> GetCloseTexture(int i)
        {
            if (i == 0)
                return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface1Close");
            else if (i == 1)
                return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface2Close");
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface3Close");
        }

        public Asset<Texture2D> GetMidTexture(int i)
        {
            if (i == 0)
                return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface1Mid");
            else if (i == 1)
                return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface2Mid");
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface3Mid");
        }

        public Asset<Texture2D> GetUltraFarTexture(int i)
        {
            if (i == 0)
                return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface1Far");
            else if (i == 1)
                return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface2Far");
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Backgrounds/ConfectionSurface3Far");
        }
    }
}