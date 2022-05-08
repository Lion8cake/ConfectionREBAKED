using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

/// <summary>
/// Confection Drawcode Shortener
/// </summary>
namespace TheConfectionRebirth.NPCs
{
    public struct DS
    {
        public static Vector2 DrawPos(Vector2 center) => center - Main.screenPosition;
        public static Vector2 DrawOrigin(Texture2D tex) => new Vector2(tex.Width / 2, tex.Height / 2);
        public static Rectangle DrawFrame(Texture2D tex, int x = 0, int y = 0) => new Rectangle(x, y, tex.Width, tex.Height);
        public static SpriteEffects FlipTex(int direction, bool left = false) => direction == (left ? -1 : 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    }
    public struct Utilities
    {
        public static int Round(float f) => (int)Math.Round(f);
        public static int LerpRound(float f1, float f2, float scale) => Round(MathHelper.Lerp(f1, f2, scale));
        /// <summary>
        /// Damage Literal
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int DL(int i)
        {
            float divident = Main.expertMode ? 0.4f : 0.5f;
            return (int)Math.Round(i * divident);
        }
        public static Color LerpColor(Color c1, Color c2, float lerp) => new Color(LerpRound(c1.R, c2.R, lerp), LerpRound(c1.G, c2.G, lerp), LerpRound(c1.B, c2.B, lerp), LerpRound(c1.A, c2.A, lerp));
    }

}