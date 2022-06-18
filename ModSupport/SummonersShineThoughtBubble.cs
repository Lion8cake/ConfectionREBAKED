using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.ModLoader;
using TheConfectionRebirth.Items.Weapons.Minions.RollerCookie;
using static Terraria.ModLoader.ModContent;

namespace TheConfectionRebirth.ModSupport
{
    public static class SummonersShineThoughtBubble
    {
        public static Texture2D ThoughtBubble;

        public class SummonersShineThoughtBubble_Loader : ILoadable
        {
            void ILoadable.Load(Mod mod)
            {
                ThoughtBubble = Request<Texture2D>("TheConfectionRebirth/ModSupport/BubbleData", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            }

            void ILoadable.Unload()
            {
                ThoughtBubble = null;
            }
        }

        public static void PostSetupContent()
        {
            if (SummonersShineCompat.SummonersShine != null)
            {
                //This is required to display the pretty bubbles
                SummonersShineCompat.ModSupport_AddSpecialPowerDisplayData(GetSpecialPowerDisplayData);
            }
        }
        public static Tuple<Texture2D, Rectangle> GetSpecialPowerDisplayData(int ItemType, int Frame)
        {
            //return empty if bubble is opening/closing
            if (Frame == 0 || Frame == 3)
                return null;
            if (Frame > 3)
                Frame--;
            Frame--;
            int disp = -1;
            if (ItemType == ItemType<SweetStaff>())
            {
                //compress frames 1/2/4/5 to 0/1/2/3

                //returns image. base images off bubble.png scaled 2x.
                disp = 0;
            }
            if(disp != -1)
                return new(ThoughtBubble, new(Frame * 40, 40 * disp, 40, 40));
            return null;
        }
    }
}
