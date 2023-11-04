using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using ReLogic.Content;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.Patches;

#region Data Structs
file ref struct CloudfallParams {
	private Span<int> _x;
	private Span<int> _y;
	private Span<int> _fgX;
	private Span<int> _bgX;
	private Span<int> _rbX;
	private Span<int> _rfX;
	private Span<int> _sfX;

	public readonly int X => _x[0];
	public readonly int Y => _y[0];
	public ref int ForegroundFrameX => ref _fgX[0];
	public ref int BackgroundFrameX => ref _bgX[0];
	public readonly int RainBackgroundFrame => _rbX[0];
	public readonly int RainForegroundFrame => _rfX[0];
	public readonly int SnowForegroundFrame => _sfX[0];

	public static CloudfallParams Create(ref int x, ref int y, ref int foregroundFrameX, ref int backgroundFrameX, ref int rainFrameBackground, ref int rainFrameForeground, ref int snowFrameForeground) => new CloudfallParams {
		_x = MemoryMarshal.CreateSpan(ref x, 1),
		_y = MemoryMarshal.CreateSpan(ref y, 1),
		_fgX = MemoryMarshal.CreateSpan(ref foregroundFrameX, 1),
		_bgX = MemoryMarshal.CreateSpan(ref backgroundFrameX, 1),
		_rbX = MemoryMarshal.CreateSpan(ref rainFrameBackground, 1),
		_rfX = MemoryMarshal.CreateSpan(ref rainFrameForeground, 1),
		_sfX = MemoryMarshal.CreateSpan(ref snowFrameForeground, 1)
	};
}

file abstract class FlossWaterfallStyle : ModWaterfallStyle {
	protected Asset<Texture2D> Asset { get; private set; }

	public override void Load() {
		Asset = ModContent.Request<Texture2D>(Texture);
	}

	public override void Unload() {
		Asset = null;
	}

	public abstract void ModifyWaterfallLength(int waterfallDistance, ref int length);

	public abstract void ModifyWaterfallParams(in CloudfallParams cloudfallParams);

	public virtual void Draw(Vector2 position, Rectangle backgroundSource, Rectangle foregroundSource, Color backgroundColor, Color foregroundColor, Vector2 origin) {
		Main.spriteBatch.Draw(Asset.Value, position, foregroundSource, foregroundColor, 0f, origin, 1f, SpriteEffects.None, 0f);
	}
}
#endregion

#region Waterfalls
// Blue uses this
file sealed class Chocofall : FlossWaterfallStyle {
	public override string Texture => "TheConfectionRebirth/Assets/Chocofall";

	public override void ModifyWaterfallLength(int waterfallDistance, ref int length) {
		length = waterfallDistance / 5;
	}

	public override void ModifyWaterfallParams(in CloudfallParams cloudfallParams) {
		var x = cloudfallParams.X;
		if (x % 2 == 0) {
			cloudfallParams.ForegroundFrameX = cloudfallParams.SnowForegroundFrame + 3;
			if (cloudfallParams.ForegroundFrameX > 7) {
				cloudfallParams.ForegroundFrameX -= 8;
			}

			cloudfallParams.BackgroundFrameX = cloudfallParams.RainBackgroundFrame + 3;
			if (cloudfallParams.BackgroundFrameX > 7) {
				cloudfallParams.BackgroundFrameX -= 8;
			}
		}
		else {
			cloudfallParams.ForegroundFrameX = cloudfallParams.SnowForegroundFrame;
			cloudfallParams.BackgroundFrameX = cloudfallParams.RainBackgroundFrame;
		}
	}
}

// Purple uses this
file sealed class Chocofall2 : FlossWaterfallStyle {
	public override string Texture => "TheConfectionRebirth/Assets/Chocofall2";

	public override void ModifyWaterfallLength(int waterfallDistance, ref int length) {
		length = waterfallDistance / 3;
	}

	public override void ModifyWaterfallParams(in CloudfallParams cloudfallParams) {
		var x = cloudfallParams.X;
		if (x % 2 == 0) {
			cloudfallParams.ForegroundFrameX = cloudfallParams.RainForegroundFrame + 7;
			if (cloudfallParams.ForegroundFrameX > 7) {
				cloudfallParams.ForegroundFrameX -= 8;
			}

			cloudfallParams.BackgroundFrameX = cloudfallParams.RainBackgroundFrame + 2;
			if (cloudfallParams.BackgroundFrameX > 7) {
				cloudfallParams.BackgroundFrameX -= 8;
			}
		}
		else {
			cloudfallParams.ForegroundFrameX = cloudfallParams.RainForegroundFrame;
			cloudfallParams.BackgroundFrameX = cloudfallParams.RainBackgroundFrame;
		}
	}

	public override void Draw(Vector2 position, Rectangle backgroundSource, Rectangle foregroundSource, Color backgroundColor, Color foregroundColor, Vector2 origin) {
		Main.spriteBatch.Draw(Asset.Value, position, backgroundSource, backgroundColor, 0f, origin, 1f, SpriteEffects.None, 0f);
		Main.spriteBatch.Draw(Asset.Value, position, foregroundSource, foregroundColor, 0f, origin, 1f, SpriteEffects.None, 0f);
	}
}
#endregion

#region Patch
file sealed class RainingFlossPatch : ILoadable {
	public void Load(Mod mod) {
		IL_WaterfallManager.FindWaterfalls += FindCandyFlossWaterfalls;
		IL_WaterfallManager.DrawWaterfall_int_float += DrawCandyFlossWaterfalls;
	}

	/*
			// if (*(ushort*)tile.type == 196)
			IL_0395: ldloca.s 6
			IL_0397: call instance uint16& Terraria.Tile::get_type()
			IL_039c: ldind.u2
			IL_039d: ldc.i4 196
		[-]	IL_03a2: bne.un IL_0455
		[+]	IL_03a2: bne.un IL_0000

			// Tile tile5 = Main.tile[j, i + 1];
			IL_03a7: ldsflda valuetype Terraria.Tilemap Terraria.Main::tile
			IL_03ac: ldloc.s 4
			IL_03ae: ldloc.s 5
			IL_03b0: ldc.i4.1
			IL_03b1: add
			IL_03b2: call instance valuetype Terraria.Tile Terraria.Tilemap::get_Item(int32, int32)
			IL_03b7: stloc.s 10
			// if (tile5 == (ArgumentException)null)
			IL_03b9: ldloc.s 10
			IL_03bb: ldnull
			IL_03bc: call bool Terraria.Tile::op_Equality(valuetype Terraria.Tile, class [System.Runtime]System.ArgumentException)
			// (no C# code)
			IL_03c1: brfalse.s IL_03dd

			// ...

			// if (!WorldGen.SolidTile(tile5) && tile5.slope() == 0 && this.currentMax < this.qualityMax)
			IL_03dd: ldloc.s 10
			IL_03df: call bool Terraria.WorldGen::SolidTile(valuetype Terraria.Tile)
			// (no C# code)
		[-]	IL_03e4: brtrue.s IL_0455
		[+]	IL_03e4: brtrue.s IL_0000

			IL_03e6: ldloca.s 10
			IL_03e8: call instance uint8 Terraria.Tile::slope()
		[-]	IL_03ed: brtrue.s IL_0455
		[+]	IL_03ed: brtrue.s IL_0000

			IL_03ef: ldarg.0
			IL_03f0: ldfld int32 Terraria.WaterfallManager::currentMax
			IL_03f5: ldarg.0
			IL_03f6: ldfld int32 Terraria.WaterfallManager::qualityMax
		[-]	IL_03fb: bge.s IL_0455
		[+]	IL_03fb: bge.s IL_0000

			// ...

			// IL_0000:
			// our patch actually goes directly here!

			// if (*(ushort*)tile.type == 460)
			IL_0455: ldloca.s 6
			IL_0457: call instance uint16& Terraria.Tile::get_type()
			IL_045c: ldind.u2
			IL_045d: ldc.i4 460
			IL_0462: bne.un IL_0515
	 */
	private static void FindCandyFlossWaterfalls(ILContext il) {
		try {
			var waterfallsField = typeof(WaterfallManager).GetField("waterfalls", BindingFlags.Instance | BindingFlags.NonPublic);
			var currentMaxField = typeof(WaterfallManager).GetField("currentMax", BindingFlags.Instance | BindingFlags.NonPublic);
			var qualityMaxField = typeof(WaterfallManager).GetField("qualityMax", BindingFlags.Instance | BindingFlags.NonPublic);

			Debug.Assert(waterfallsField != null);
			Debug.Assert(currentMaxField != null);
			Debug.Assert(qualityMaxField != null);

			var c = new ILCursor(il);

			int xVarIndex = -1;
			int yVarIndex = -1;
			int tileVarIndex = -1;
			var gotoSnowCloudCase = default(ILLabel);

			var gotoPurpleFlossCase = c.DefineLabel();
			var gotoTrueSnowCloudCase = c.DefineLabel();

			c.GotoNext(
				i => i.MatchLdloca(out tileVarIndex),
				i => i.MatchCall<Tile>("get_type"),
				i => i.MatchLdindU2(),
				i => i.MatchLdcI4(TileID.RainCloud)
				);

			c.GotoNext(
				i => i.MatchBneUn(out gotoSnowCloudCase)
				);

			c.GotoNext(
				i => i.MatchLdsflda<Main>("tile"),
				i => i.MatchLdloc(out xVarIndex),
				i => i.MatchLdloc(out yVarIndex)
				);

			Debug.Assert(xVarIndex != -1);
			Debug.Assert(yVarIndex != -1);
			Debug.Assert(tileVarIndex != -1);
			Debug.Assert(gotoSnowCloudCase != null);

			c.GotoNext(
				i => i.MatchLdloca(tileVarIndex),
				i => i.MatchCall<Tile>("get_type"),
				i => i.MatchLdindU2(),
				i => i.MatchLdcI4(TileID.SnowCloud)
				);

			c.EmitNop();
			foreach (var label in c.IncomingLabels) {
				label.Target = c.Prev;
			}

			CopypastaPatch<BlueFairyFloss, Chocofall>(gotoPurpleFlossCase);

			c.MarkLabel(gotoPurpleFlossCase);
			CopypastaPatch<PurpleFairyFloss, Chocofall2>(gotoTrueSnowCloudCase);

			c.MarkLabel(gotoTrueSnowCloudCase);

			void CopypastaPatch<TTile, TWaterfall>(ILLabel gotoCase) where TTile : ModTile where TWaterfall : FlossWaterfallStyle {
				c.EmitLdloc(tileVarIndex);
				c.EmitDelegate(static (Tile tile) => tile.TileType == ModContent.TileType<TTile>());
				c.EmitBrfalse(gotoCase);

				c.EmitLdarg0()
					.EmitLdfld(waterfallsField)
					.EmitLdarg0()
					.EmitLdflda(currentMaxField)
					.EmitLdarg0()
					.EmitLdfld(qualityMaxField)

					.EmitLdloc(xVarIndex)
					.EmitLdloc(yVarIndex)
					.EmitDelegate(static (WaterfallManager.WaterfallData[] waterfalls, ref int currentMax, int qualityMax, int j, int i) => {
						var belowTile = Main.tile[j, i + 1];

						if (!WorldGen.SolidTile(belowTile) && belowTile.Slope == SlopeType.Solid && currentMax < qualityMax) {
							waterfalls[currentMax].type = ModContent.GetInstance<TWaterfall>().Slot;
							waterfalls[currentMax].x = j;
							waterfalls[currentMax].y = i + 1;
							currentMax++;
						}
					});
			}
		}
		catch {
			MonoModHooks.DumpIL(ModContent.GetInstance<TheConfectionRebirth>(), il);
		}
	}

	private static void DrawCandyFlossWaterfalls(ILContext il) {
		try {
			var rainyFrameBgField = typeof(WaterfallManager).GetField("rainFrameBackground", BindingFlags.Instance | BindingFlags.NonPublic);
			var rainyFrameFgField = typeof(WaterfallManager).GetField("rainFrameForeground", BindingFlags.Instance | BindingFlags.NonPublic);
			var snowyFrameFgField = typeof(WaterfallManager).GetField("snowFrameForeground", BindingFlags.Instance | BindingFlags.NonPublic);
			var waterfallDistField = typeof(WaterfallManager).GetField("waterfallDist", BindingFlags.Instance | BindingFlags.NonPublic);

			Debug.Assert(rainyFrameBgField != null && rainyFrameFgField != null);
			Debug.Assert(snowyFrameFgField != null);
			Debug.Assert(waterfallDistField != null);

			var c = new ILCursor(il);

			c.Context.Body.Variables.Add(new VariableDefinition(c.IL.Import(typeof(FlossWaterfallStyle))));
			var flossWaterfallVarIndex = c.Context.Body.Variables.Count - 1;

			int backgroundSourceVarIndex = -1, foregroundSourceVarIndex = -1;
			int backgroundColorVarIndex = -1, foregroundColorVarIndex = -1;
			int positionVarIndex = -1, originVarIndex = -1;
			int xVarIndex = -1, yVarIndex = -1;
			int foregroundXFrameVarIndex = -1, backgroundXFrameVarIndex = -1;
			var waterfallStyleVarIndex = -1;
			var waterfallLengthVarIndex = -1;
			var moveToSnowfallsCase = default(ILLabel);
			var finishDrawingCase = default(ILLabel);

			// Retrive X coordinate.
			c.GotoNext(
				i => i.MatchLdelema<WaterfallManager.WaterfallData>(),
				i => i.MatchLdfld<WaterfallManager.WaterfallData>("x")
				);
			c.GotoNext(
				i => i.MatchStloc(out xVarIndex)
				);

			Debug.Assert(xVarIndex != -1);

			// Retrive Y coordinate.
			c.GotoNext(
				i => i.MatchLdelema<WaterfallManager.WaterfallData>(),
				i => i.MatchLdfld<WaterfallManager.WaterfallData>("y")
				);
			c.GotoNext(
				i => i.MatchStloc(out yVarIndex)
				);

			Debug.Assert(yVarIndex != -1);

			// Branch into 11, 22 cases (rainfall and snowfall from cloud blocks) if waterfall style is of FlossWaterfallStyle type.
			c.GotoNext(MoveType.After,
				i => i.MatchLdloc(out waterfallStyleVarIndex),
				i => i.MatchLdcI4(11),
				i => i.MatchBeq(out moveToSnowfallsCase)
				);

			Debug.Assert(waterfallStyleVarIndex != -1);
			Debug.Assert(moveToSnowfallsCase != null);

			c.EmitLdloc(waterfallStyleVarIndex);
			c.EmitDelegate(static (int waterfallStyle) => ModContent.GetModWaterfallStyle(waterfallStyle) is FlossWaterfallStyle);
			c.EmitBrtrue(moveToSnowfallsCase);

			// Move after Main.drewLava check
			c.GotoNext(MoveType.After,
				i => i.MatchBrtrue(out _)
				);

			// Store dummy instance, so we can draw and call hooks on it.
			c.EmitLdloc(waterfallStyleVarIndex);
			c.EmitDelegate(static (int waterfallStyle) => ModContent.GetModWaterfallStyle(waterfallStyle) as FlossWaterfallStyle);
			c.EmitStloc(flossWaterfallVarIndex);

			c.GotoNext(MoveType.After,
				i => i.MatchDiv(),
				i => i.MatchStloc(out waterfallLengthVarIndex)
				);

			Debug.Assert(waterfallLengthVarIndex != -1);

			// Modify waterfall length if needed
			c.EmitLdloc(flossWaterfallVarIndex);
			c.EmitLdloc(waterfallLengthVarIndex);
			c.EmitLdarg0().EmitLdfld(waterfallDistField);
			c.EmitDelegate(static (FlossWaterfallStyle flossWaterfall, int waterfallLength, int waterfallDistance) => {
				flossWaterfall?.ModifyWaterfallLength(waterfallDistance, ref waterfallLength);
				return waterfallLength;
			});
			c.EmitStloc(waterfallLengthVarIndex);

			// Retrive foreground and background frames.
			c.GotoNext(
				i => i.MatchLdfld(rainyFrameFgField),
				i => i.MatchStloc(out foregroundXFrameVarIndex)
				);

			c.GotoNext(
				i => i.MatchLdfld(rainyFrameBgField),
				i => i.MatchStloc(out backgroundXFrameVarIndex)
				);

			Debug.Assert(foregroundXFrameVarIndex != -1);
			Debug.Assert(backgroundXFrameVarIndex != -1);
			
			// Move to background source.
			c.GotoNext(i => i.MatchLdloca(out _));

			c.EmitNop();
			foreach (var label in c.IncomingLabels) {
				label.Target = c.Prev;
			}

			// Modify parameters if needed.
			c.EmitLdloc(flossWaterfallVarIndex);
			c.EmitLdloca(xVarIndex);
			c.EmitLdloca(yVarIndex);
			c.EmitLdloca(backgroundXFrameVarIndex);
			c.EmitLdloca(foregroundXFrameVarIndex);
			c.EmitLdarg0().EmitLdflda(rainyFrameBgField);
			c.EmitLdarg0().EmitLdflda(rainyFrameFgField);
			c.EmitLdarg0().EmitLdflda(snowyFrameFgField);
			c.EmitDelegate(static (FlossWaterfallStyle flossWaterfall, ref int x, ref int y, ref int backgroundXFrame, ref int foregroundXFrame, ref int rainyBg, ref int rainyFg, ref int snowyFg) => {
				flossWaterfall?.ModifyWaterfallParams(CloudfallParams.Create(ref x, ref y, ref foregroundXFrame, ref backgroundXFrame, ref rainyBg, ref rainyFg, ref snowyFg));
			});

			// Move to background source. again.
			c.GotoNext(
				i => i.MatchLdloca(out backgroundSourceVarIndex)
				);

			Debug.Assert(backgroundSourceVarIndex != -1);

			// Move to foreground source.
			c.GotoNext(
				i => i.MatchLdloca(out foregroundSourceVarIndex)
				);

			Debug.Assert(foregroundSourceVarIndex != -1);

			// Move to origin.
			c.GotoNext(
				i => i.MatchLdloca(out originVarIndex)
				);

			Debug.Assert(originVarIndex != -1);

			// Move to position.
			c.GotoNext(
				i => i.MatchStloc(out positionVarIndex)
				);

			Debug.Assert(positionVarIndex != -1);

			// Move right before drawing.
			c.GotoNext(
				i => i.MatchLdloc(waterfallStyleVarIndex),
				i => i.MatchLdcI4(22),
				i => i.MatchBneUn(out _),

				i => i.MatchLdsfld<Main>(nameof(Main.spriteBatch))
				);

			// Store current target, so we can move back.
			var currentTargetIndex = c.Index;

			c.GotoNext(
				i => i.MatchBr(out finishDrawingCase)
				);

			Debug.Assert(finishDrawingCase != null);

			c.Index = currentTargetIndex;

			// Retrieve background and foreground colors
			c.GotoPrev(i => i.MatchStloc(out foregroundColorVarIndex));
			c.GotoPrev(i => i.MatchStloc(out backgroundColorVarIndex));

			Debug.Assert(foregroundColorVarIndex != -1);
			Debug.Assert(backgroundColorVarIndex != -1);

			c.Index = currentTargetIndex;

			// Draw our cloudfall.
			c.EmitLdloc(flossWaterfallVarIndex)
				.EmitLdloc(positionVarIndex)
				.EmitLdloc(backgroundSourceVarIndex)
				.EmitLdloc(foregroundSourceVarIndex)
				.EmitLdloc(backgroundColorVarIndex)
				.EmitLdloc(foregroundColorVarIndex)
				.EmitLdloc(originVarIndex)
				.EmitDelegate(static (FlossWaterfallStyle flossWaterfall, Vector2 position, Rectangle backgroundSource, Rectangle foregroundSource, Color backgroundColor, Color foregroundColor, Vector2 origin) => {
					if (flossWaterfall != null) {
						flossWaterfall.Draw(position, backgroundSource, foregroundSource, backgroundColor, foregroundColor, origin);
						return true;
					}

					return false;
				});
			c.EmitBrtrue(finishDrawingCase);
		}
		catch {
			MonoModHooks.DumpIL(ModContent.GetInstance<TheConfectionRebirth>(), il);
		}
	}

	public void Unload() {
	}
}
#endregion
