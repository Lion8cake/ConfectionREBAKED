using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Gores;

namespace TheConfectionRebirth.Biomes
{
    public class CreamWaterStyle : ModWaterStyle
    {
        public override int ChooseWaterfallStyle()
        {
            return ModContent.Find<ModWaterfallStyle>("TheConfectionRebirth/CreamWaterfallStyle").Slot;
        }

        public override int GetSplashDust()
        {
            return ModContent.DustType<CreamWaterSplash>();
        }

        public override int GetDropletGore()
        {
            return ModContent.GoreType<CreamDroplet>();
        }

        public override Color BiomeHairColor()
        {
            return new(186, 154, 119);
        }

        public override byte GetRainVariant()
        {
            return (byte)Main.rand.Next(3);
        }

        public override Asset<Texture2D> GetRainTexture()
        {
            return ModContent.Request<Texture2D>("TheConfectionRebirth/Biomes/ConfectionRain");
        }
    }
}
