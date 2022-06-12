using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace TheConfectionRebirth.Backgrounds
{
    public class ConfectionSurfaceBackgroundStyle : ModSurfaceBackgroundStyle
    {
        internal const int backgroundVariatonsTotal = 2;

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
            if (ConfectionWorld.ConfectionSurfaceBG == 0)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
            }
            if (ConfectionWorld.ConfectionSurfaceBG == 1)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionMenuProgrammerArtFar");
            }
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
        }

        public override int ChooseMiddleTexture()
        {
            if (ConfectionWorld.ConfectionSurfaceBG == 0)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
            }
            if (ConfectionWorld.ConfectionSurfaceBG == 1)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
            }
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceMid0");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            if (ConfectionWorld.ConfectionSurfaceBG == 0)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
            }
            if (ConfectionWorld.ConfectionSurfaceBG == 1)
            {
                return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceFar");
            }
            return BackgroundTextureLoader.GetBackgroundSlot("TheConfectionRebirth/Backgrounds/ConfectionSurfaceClose");
        }
    }
}