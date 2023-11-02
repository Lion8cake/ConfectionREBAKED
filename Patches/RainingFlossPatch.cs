using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using ReLogic.Content;
using System.Diagnostics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.Patches;

file interface IFlossWaterfall {
	bool UsesSnowyFrames { get; }

	void Draw(Vector2 position, Rectangle source1, Rectangle source2, Color color1, Color color2, Vector2 origin);
}

// Blue uses this
file sealed class Chocofall : ModWaterfallStyle, IFlossWaterfall {
	private static Asset<Texture2D> asset;

	public override string Texture => "TheConfectionRebirth/Assets/Chocofall";

	public bool UsesSnowyFrames => true;

	public override void Load() {
		asset = ModContent.Request<Texture2D>(Texture);
	}

	public override void Unload() {
		asset = null;
	}

	public void Draw(Vector2 position, Rectangle source1, Rectangle source2, Color color1, Color color2, Vector2 origin) {
		Main.spriteBatch.Draw(asset.Value, position, source2, color2, 0f, origin, 1f, SpriteEffects.None, 0f);
	}
}

// Purple uses this
file sealed class Chocofall2 : ModWaterfallStyle, IFlossWaterfall {
	private static Asset<Texture2D> asset;

	public override string Texture => "TheConfectionRebirth/Assets/Chocofall2";

	public bool UsesSnowyFrames => false;

	public override void Load() {
		asset = ModContent.Request<Texture2D>(Texture);
	}

	public override void Unload() {
		asset = null;
	}

	public void Draw(Vector2 position, Rectangle source1, Rectangle source2, Color color1, Color color2, Vector2 origin) {
		Main.spriteBatch.Draw(asset.Value, position, source1, color1, 0f, origin, 1f, SpriteEffects.None, 0f);
		Main.spriteBatch.Draw(asset.Value, position, source2, color2, 0f, origin, 1f, SpriteEffects.None, 0f);
	}
}

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

			CopypastaPatch<BlueFairyFloss, Chocofall>(gotoPurpleFlossCase);

			c.MarkLabel(gotoPurpleFlossCase);

			CopypastaPatch<PurpleFairyFloss, Chocofall2>(gotoSnowCloudCase);

			gotoSnowCloudCase.Target = c.Next;

			void CopypastaPatch<TTile, TWaterfall>(ILLabel gotoCase) where TTile : ModTile where TWaterfall : ModWaterfallStyle, IFlossWaterfall {
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
			var c = new ILCursor(il);

			c.Context.Body.Variables.Add(new VariableDefinition(c.IL.Import(typeof(IFlossWaterfall))));
			var flossWaterfallVarIndex = c.Context.Body.Variables.Count - 1;
			c.Context.Body.Variables.Add(new VariableDefinition(c.IL.Import(typeof(int))));
			var oldWaterfallStyleVarIndex = c.Context.Body.Variables.Count - 1;

			int positionVarIndex = -1, source1VarIndex = -1, source2VarIndex = -1, color1VarIndex = -1, color2VarIndex = -1, originVarIndex = -1;
			var waterfallStyleVarIndex = -1;
			var finishDrawingCase = default(ILLabel);
			var skipVanillaDrawingCase = c.DefineLabel();
			
			c.GotoNext(
				i => i.MatchLdcI4(11),
				i => i.MatchBeq(out _)
				);

			c.GotoNext(
				i => i.MatchLdloc(out waterfallStyleVarIndex),
				i => i.MatchLdcI4(22),
				i => i.MatchBneUn(out _)
				);

			c.GotoNext(MoveType.After,
				i => i.MatchBrtrue(out _)
				);

			Debug.Assert(waterfallStyleVarIndex != -1);

			c.EmitLdloc(waterfallStyleVarIndex);
			c.EmitDelegate(static (int waterfallStyle) => ModContent.GetModWaterfallStyle(waterfallStyle) as IFlossWaterfall);
			c.EmitStloc(flossWaterfallVarIndex);

			c.EmitLdloc(flossWaterfallVarIndex);
			c.EmitLdloca(waterfallStyleVarIndex);
			c.EmitLdloca(oldWaterfallStyleVarIndex);
			c.EmitDelegate(static (IFlossWaterfall flossWaterfall, ref int currentStyle, ref int oldStyle) => {
				if (!flossWaterfall.UsesSnowyFrames)
					return;

				oldStyle = currentStyle;
				currentStyle = 22;
			});

			c.GotoNext(
				i => i.MatchLdloc(waterfallStyleVarIndex),
				i => i.MatchLdcI4(22),
				i => i.MatchBneUn(out _),

				i => i.MatchLdsfld<Main>(nameof(Main.spriteBatch))
				);

			var currentTargetIndex = c.Index;

			c.GotoNext(i => i.MatchBr(out finishDrawingCase));
			c.GotoNext(i => i.MatchLdloc(out positionVarIndex));
			c.GotoNext(i => i.MatchLdloc(out source2VarIndex));
			c.GotoNext(i => i.MatchLdloc(out color2VarIndex));
			c.GotoNext(i => i.MatchLdloc(out originVarIndex));
			c.GotoNext(i => i.MatchLdloc(positionVarIndex));
			c.GotoNext(i => i.MatchLdloc(out source1VarIndex));
			c.GotoNext(i => i.MatchLdloc(out color1VarIndex));

			Debug.Assert(positionVarIndex != -1);
			Debug.Assert(originVarIndex != -1);
			Debug.Assert(source1VarIndex != -1);
			Debug.Assert(source2VarIndex != -1);
			Debug.Assert(color1VarIndex != -1);
			Debug.Assert(color2VarIndex != -1);
			Debug.Assert(finishDrawingCase != null);

			c.Index = currentTargetIndex;

			c.EmitLdloc(flossWaterfallVarIndex);
			c.EmitLdloca(waterfallStyleVarIndex);
			c.EmitLdloc(oldWaterfallStyleVarIndex);
			c.EmitDelegate(static (IFlossWaterfall flossWaterfall, ref int currentStyle, int oldStyle) => {
				if (!flossWaterfall.UsesSnowyFrames)
					return;

				currentStyle = oldStyle;
			});

			c.EmitLdloc(flossWaterfallVarIndex)
				.EmitLdloc(positionVarIndex)
				.EmitLdloc(originVarIndex)
				.EmitLdloc(source1VarIndex)
				.EmitLdloc(source2VarIndex)
				.EmitLdloc(color1VarIndex)
				.EmitLdloc(color2VarIndex)
				.EmitDelegate(static (IFlossWaterfall flossWaterfall, Vector2 position, Vector2 origin, Rectangle source1, Rectangle source2, Color color1, Color color2) => {
				if (flossWaterfall != null) {
					flossWaterfall.Draw(position, source1, source2, color1, color2, origin);
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
