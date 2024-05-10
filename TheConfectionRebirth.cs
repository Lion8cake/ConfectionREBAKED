using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;
using Terraria.ID;
using MonoMod.Cil;
using System.Reflection;
using Microsoft.CodeAnalysis.Emit;
using Terraria.ModLoader.Config;

namespace TheConfectionRebirth
{
	public class TheConfectionRebirth : Mod
	{
		//Edit the following
		//Player.cs
		//DoBootsEffect_PlaceFlowersOnTile //Needs foliage to be done
		//MowGrassTile //Needs lawnmower grass to be done, this should be the FINAL thing to do with that grass
		//PlaceThing_Tiles_PlaceIt_KillGrassForSolids (done)
		//DoesPickTargetTransformOnKill (done)
		//Liquid.cs
		//DelWater (done)
		//SmartCursorHelper.cs
		//Step_LawnMower
		//WorldGen.cs
		//IsFitToPlaceFlowerIn
		//PlantCheck

		public override void Load() {
			On_Player.PlaceThing_Tiles_PlaceIt_KillGrassForSolids += KillConjoinedGrass_PlaceThing;
			On_Player.DoesPickTargetTransformOnKill += PickaxeKillTile;
			IL_Liquid.DelWater += BurnGrass;
			IL_WorldGen.PlantCheck += PlantTileFrameIL;
		}

		public override void Unload() {
			On_Player.PlaceThing_Tiles_PlaceIt_KillGrassForSolids -= KillConjoinedGrass_PlaceThing;
			On_Player.DoesPickTargetTransformOnKill -= PickaxeKillTile;
			IL_Liquid.DelWater -= BurnGrass;
			IL_WorldGen.PlantCheck -= PlantTileFrameIL;
		}

		private void PlantTileFrameIL(ILContext il) {
			ILCursor c = new(il);
			ILLabel IL_0433 = c.DefineLabel();
			if (!c.TryGotoNext(
				MoveType.Before,
				i => i.MatchLdcI4(0),
				i => i.MatchStloc2(),
				i => i.MatchLdloc1(),
				i => i.MatchLdcI4(3),
				i => i.MatchBeq(out _)
				)) {
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Plant Check's main tile framing could not be found");
				return;
			}
			c.MarkLabel(IL_0433);
			if (!c.TryGotoPrev(
				MoveType.Before,
				i => i.MatchLdloca(5),
				i => i.MatchCall<Tile>("get_type"),
				i => i.MatchPop(),
				i => i.MatchLdloc1(),
				i => i.MatchLdcI4(3),
				i => i.MatchBneUn(out _)
				)) {
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Plant Check's massive if statement instructions could not found");
				return;
			}
			if (!c.TryGotoPrev(
				MoveType.Before,
				i => i.MatchLdloca(5),
				i => i.MatchCall<Tile>("nactive"),
				i => i.MatchBrfalse(out _)
				)) {
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Plant Check's before massive if statement could not be found");
				return;
			}
			if (IL_0433 == null) {
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Plant Check's main tile framing IlLable could not be found");
				return;
			}
			c.EmitLdloc1(); //num2
			c.EmitLdloc0(); //num
			c.EmitDelegate((int num2, int num) => {
				return (!(num2 != ModContent.TileType<CreamGrass_Foliage>() || num == ModContent.TileType<CreamGrass>() || num == ModContent.TileType<CreamGrassMowed>()));
			});
			c.EmitBrtrue(IL_0433);
			
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(0),
				i => i.MatchStloc2()
				);
			c.EmitLdloc0(); //num
			c.EmitLdloca(1); //num2
			c.EmitDelegate((int num, ref int num2) => {
				if (num == ModContent.TileType<CreamGrass>() || num == ModContent.TileType<CreamGrassMowed>()) 
				{
					num2 = ModContent.TileType<CreamGrass_Foliage>();
				}
			});
		}

		private void BurnGrass(ILContext il) {
			ILCursor c = new(il);
			if (!c.TryGotoNext(
				MoveType.After,
				i => i.MatchLdloca(10),
				i => i.MatchCall<Tile>("active"), //Gets the if statement checking if the tile at tile5 is active
				i => i.MatchBrfalse(out _))) //aka this places this BEFORE the check for normal grasses (normal, hallowed, corruption, (anything that uses dirt)
			{
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("The Confection REBAKED: lava tile burning instructions not found");
				return;
			}
			c.EmitLdloca(10); //ref tile5
			c.EmitLdloc(8); //i
			c.EmitLdloc(9); //j
			c.EmitLdloc(0); //num
			c.EmitLdloc(1); //num2
			c.EmitDelegate((ref Tile tile5, int i, int j, int num, int num2) => {
				if (tile5.TileType == ModContent.TileType<CreamGrass>()) { //Turns Creamgrass into Cookie block when lava is near
					tile5.TileType = (ushort)ModContent.TileType<CookieBlock>();
					WorldGen.SquareTileFrame(i, j);
					if (Main.netMode == NetmodeID.Server) {
						NetMessage.SendTileSquare(-1, num, num2, 3);
					}
				}
			});
		}

		private bool PickaxeKillTile(On_Player.orig_DoesPickTargetTransformOnKill orig, Player self, HitTile hitCounter, int damage, int x, int y, int pickPower, int bufferIndex, Tile tileTarget) {
			if (hitCounter.AddDamage(bufferIndex, damage, updateAmount: false) >= 100 && tileTarget.TileType == ModContent.TileType<CreamGrass>()) {
				return true;
			}
			else {
				return orig.Invoke(self, hitCounter, damage, x, y, pickPower, bufferIndex, tileTarget);
			}
		}

		private void KillConjoinedGrass_PlaceThing(On_Player.orig_PlaceThing_Tiles_PlaceIt_KillGrassForSolids orig, Player self) {
			orig.Invoke(self);
			for (int i = Player.tileTargetX - 1; i <= Player.tileTargetX + 1; i++) {
				for (int j = Player.tileTargetY - 1; j <= Player.tileTargetY + 1; j++) {
					Tile tile = Main.tile[i, j];
					if (!tile.HasTile || self.inventory[self.selectedItem].createTile == tile.TileType || tile.TileType != ModContent.TileType<CreamGrass>()) {
						continue;
					}
					bool flag = true;
					for (int k = i - 1; k <= i + 1; k++) {
						for (int l = j - 1; l <= j + 1; l++) {
							if (!WorldGen.SolidTile(k, l)) {
								flag = false;
							}
						}
					}
					if (flag) {
						WorldGen.KillTile(i, j, fail: true);
						if (Main.netMode == NetmodeID.MultiplayerClient) {
							NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 1f);
						}
					}
				}
			}
		}
	}
}
