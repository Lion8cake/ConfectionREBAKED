using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace TheConfectionRebirth.Backgrounds
{
    public class ConfectionSurfaceBackgroundStyle : ModSurfaceBackgroundStyle
    {
        public static int confectionBackStyle;

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
            if (confectionBackStyle == 0)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
            }
            if (confectionBackStyle == 1)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionMenuProgrammerArtFar");
            }

            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
        }

        public override int ChooseMiddleTexture()
        {
            if (confectionBackStyle == 0)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
            }
            if (confectionBackStyle == 1)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
            }
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceMid0");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            if (confectionBackStyle == 0)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
            }
            if (confectionBackStyle == 1)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
            }
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose");
        }
    }
}