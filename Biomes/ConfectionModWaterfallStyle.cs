using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Gores;
using static Terraria.WaterfallManager;

namespace TheConfectionRebirth.Biomes {
	public abstract class ConfectionModWaterfallStyle : ModWaterfallStyle {
		public override void Load() {
			IL_WaterfallManager.DrawWaterfall_int_float += PreDrawWaterfallModifier;
		}

		public override void Unload() {
			IL_WaterfallManager.DrawWaterfall_int_float -= PreDrawWaterfallModifier;
		}

		/// <summary>
		/// Allows you to draw things behind the waterfall at the given coordinates. Return false to stop the game from drawing the waterfall normally. Returns true by default.
		/// </summary>
		/// <param name="currentWaterfallData">The current waterfall data, this is used inside of the waterfalls WaterfallData array.</param>
		/// <param name="i">The x position in tile coordinates.</param>
		/// <param name="j">The Y position in tile coordinates.</param>
		/// <param name="spriteBatch"></param>
		/// <returns></returns>
		public virtual bool PreDraw(int currentWaterfallData, int i, int j, SpriteBatch spriteBatch) {
			return true;
		}

		private void PreDrawWaterfallModifier(ILContext il) {
			ILCursor c = new(il);
			ILLabel IL_149d = null;
			c.GotoNext( //Gets the IL_149d instruction lable
				MoveType.After,
				i => i.MatchLdloc(12),               //if (Main.drewLava || waterfalls[i].stopAtStep == 0)
				i => i.MatchLdcI4(25),				 //{
				i => i.MatchBneUn(out _),            //	   continue;
				i => i.MatchLdsfld<Main>("drewLava"),//}
				i => i.MatchBrtrue(out IL_149d)); //used to get the ILLable from the continue
			c.GotoPrev( //Goes to after the intialisation of variables 3 through to 15. This is just before the drawing of lava, honey and shimmer waterfalls
				MoveType.After,
				i => i.MatchLdcI4(0),  //int num11 = 0;
				i => i.MatchStloc(18), //int num13 = 0;
				i => i.MatchLdcI4(0),  //int num14;
				i => i.MatchStloc(19), //int num15;
				i => i.MatchLdcI4(0),
				i => i.MatchStloc(20));
			c.EmitLdloc(10); //the current waterfall data type
			c.EmitLdloc(12); //Type of waterfall
			c.EmitLdloc(13); //X position of the waterfall
			c.EmitLdloc(14); //Y position of the waterfall
			c.EmitDelegate((int i, int num4, int num5, int num6) => {               //if (!PreModWaterfallDraw(i, num5, num6, num4, Main.spriteBatch))
				return !PreModWaterfallDraw(i, num5, num6, num4, Main.spriteBatch); //{
			});																		//	continue;
			c.EmitBrtrue(IL_149d);													//}
		}

		private static bool PreModWaterfallDraw(int currentWaterfallData, int i, int j, int type, SpriteBatch spriteBatch) {
			bool flag = true;
			if (LoaderManager.Get<WaterFallStylesLoader>().Get(type) is ConfectionModWaterfallStyle) {
				ConfectionModWaterfallStyle waterStyle = (ConfectionModWaterfallStyle)LoaderManager.Get<WaterFallStylesLoader>().Get(type);
				if (waterStyle != null) {
					flag = waterStyle?.PreDraw(currentWaterfallData, i, j, spriteBatch) ?? true;
				}
			}
			return flag;
		}
	}
}
