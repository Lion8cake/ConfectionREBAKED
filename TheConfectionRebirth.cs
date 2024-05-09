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
			//On_WorldGen.PlantCheck += PlantTileFrameDetour;
		}

		public override void Unload() {
			On_Player.PlaceThing_Tiles_PlaceIt_KillGrassForSolids -= KillConjoinedGrass_PlaceThing;
			On_Player.DoesPickTargetTransformOnKill -= PickaxeKillTile;
			IL_Liquid.DelWater -= BurnGrass;
			IL_WorldGen.PlantCheck -= PlantTileFrameIL;
			//On_WorldGen.PlantCheck -= PlantTileFrameDetour;
		}

		private void PlantTileFrameDetour(On_WorldGen.orig_PlantCheck orig, int x, int y) {
			//Orig-less detour, it gives an example of what the IL edit is supposed to do
			x = Utils.Clamp(x, 1, Main.maxTilesX - 2);
			y = Utils.Clamp(y, 1, Main.maxTilesY - 2);
			for (int i = x - 1; i <= x + 1; i++) {
				for (int j = y - 1; j <= y + 1; j++) {
					if (Main.tile[i, j] == null) {
						return;
					}
				}
			}
			int num = -1;
			Tile tile = Main.tile[x, y];
			int num2 = tile.TileType;
			_ = Main.maxTilesX;
			if (y + 1 >= Main.maxTilesY) {
				num = num2;
			}
			if (x - 1 >= 0 && Main.tile[x - 1, y] != null) {
				tile = Main.tile[x - 1, y];
				if (tile.HasUnactuatedTile) {
					tile = Main.tile[x - 1, y];
					_ = ref tile.TileType;
				}
			}
			if (x + 1 < Main.maxTilesX && Main.tile[x + 1, y] != null) {
				tile = Main.tile[x + 1, y];
				if (tile.HasUnactuatedTile) {
					tile = Main.tile[x + 1, y];
					_ = ref tile.TileType;
				}
			}
			if (y - 1 >= 0 && Main.tile[x, y - 1] != null) {
				tile = Main.tile[x, y - 1];
				if (tile.HasUnactuatedTile) {
					tile = Main.tile[x, y - 1];
					_ = ref tile.TileType;
				}
			}
			if (y + 1 < Main.maxTilesY && Main.tile[x, y + 1] != null) {
				tile = Main.tile[x, y + 1];
				if (tile.HasUnactuatedTile) {
					tile = Main.tile[x, y + 1];
					if (!tile.IsHalfBlock) {
						tile = Main.tile[x, y + 1];
						if (tile.Slope == 0) {
							tile = Main.tile[x, y + 1];
							num = tile.TileType;
						}
					}
				}
			}
			if (x - 1 >= 0 && y - 1 >= 0 && Main.tile[x - 1, y - 1] != null) {
				tile = Main.tile[x - 1, y - 1];
				if (tile.HasUnactuatedTile) {
					tile = Main.tile[x - 1, y - 1];
					_ = ref tile.TileType;
				}
			}
			if (x + 1 < Main.maxTilesX && y - 1 >= 0 && Main.tile[x + 1, y - 1] != null) {
				tile = Main.tile[x + 1, y - 1];
				if (tile.HasUnactuatedTile) {
					tile = Main.tile[x + 1, y - 1];
					_ = ref tile.TileType;
				}
			}
			if (x - 1 >= 0 && y + 1 < Main.maxTilesY && Main.tile[x - 1, y + 1] != null) {
				tile = Main.tile[x - 1, y + 1];
				if (tile.HasUnactuatedTile) {
					tile = Main.tile[x - 1, y + 1];
					_ = ref tile.TileType;
				}
			}
			if (x + 1 < Main.maxTilesX && y + 1 < Main.maxTilesY && Main.tile[x + 1, y + 1] != null) {
				tile = Main.tile[x + 1, y + 1];
				if (tile.HasUnactuatedTile) {
					tile = Main.tile[x + 1, y + 1];
					_ = ref tile.TileType;
				}
			}
			//if (num2 == confection_foliage && num != CreamGrass)
			//	goto IL_0510;
			if ((num2 != 3 || num == 2 || num == 477 || num == 78 || num == 380 || num == 579) && (num2 != 73 || num == 2 || num == 477 || num == 78 || num == 380 || num == 579) && (num2 != 24 || num == 23 || num == 661) && (num2 != 61 || num == 60) && (num2 != 74 || num == 60) && (num2 != 71 || num == 70) && (num2 != 110 || num == 109 || num == 492) && (num2 != 113 || num == 109 || num == 492) && (num2 != 201 || num == 199 || num == 662) && (num2 != 637 || num == 633) && (num2 != ModContent.TileType<CreamGrass_Foliage>() || num == ModContent.TileType<CreamGrass>() || num == ModContent.TileType<CreamGrassMowed>())) {
				return;
			}
			bool flag = false;
			if (num2 == 3 || num2 == 110 || num2 == 24) {
				tile = Main.tile[x, y];
				flag = tile.TileFrameX == 144;
			}
			if (num2 == 201) {
				tile = Main.tile[x, y];
				flag = tile.TileFrameX == 270;
			}
			if ((num2 == 3 || num2 == 73) && num != 2 && num != 477) {
				tile = Main.tile[x, y];
				if (tile.TileFrameX >= 162) {
					tile = Main.tile[x, y];
					tile.TileFrameX = 126;
				}
			}
			if (num2 == 74 && num != 60) {
				tile = Main.tile[x, y];
				if (tile.TileFrameX >= 162) {
					tile = Main.tile[x, y];
					tile.TileFrameX = 126;
				}
			}
			switch (num) {
				case 23:
				case 661:
					num2 = 24;
					tile = Main.tile[x, y];
					if (tile.TileFrameX >= 162) {
						tile = Main.tile[x, y];
						tile.TileFrameX = 126;
					}
					break;
				case 2:
				case 477:
					num2 = ((num2 != 113) ? 3 : 73);
					break;
				case 109:
				case 492:
					num2 = ((num2 != 73) ? 110 : 113);
					break;
				case 199:
				case 662:
					num2 = 201;
					break;
				case 60:
					num2 = 61;
					while (true) {
						tile = Main.tile[x, y];
						if (tile.TileFrameX > 126) {
							tile = Main.tile[x, y];
							tile.TileFrameX -= 126;
							continue;
						}
						break;
					}
					break;
				case 70:
					num2 = 71;
					while (true) {
						tile = Main.tile[x, y];
						if (tile.TileFrameX <= 72) {
							break;
						}
						tile = Main.tile[x, y];
						tile.TileFrameX -= 72;
					}
					break;
			}
			int num3 = num2;
			if (num == ModContent.TileType<CreamGrass>() || num == ModContent.TileType<CreamGrassMowed>()) {
				num2 = ((num2 != 73) ? ModContent.TileType<CreamGrass_Foliage>() : 113);
			}
			tile = Main.tile[x, y];
			if (num3 != tile.TileType) {
				tile = Main.tile[x, y];
				tile.TileType = (ushort)num2;
				if (flag) {
					tile = Main.tile[x, y];
					tile.TileFrameX = 144;
					if (num2 == 201) {
						tile = Main.tile[x, y];
						tile.TileFrameX = 270;
					}
				}
			}
			else {
				WorldGen.KillTile(x, y);
			}
		}

		private void PlantTileFrameIL(ILContext il) {
			ILCursor c = new(il);
			ILLabel IL_0433 = null;
			if (!c.TryGotoNext(
				MoveType.After,
				i => i.MatchLdloc0(),
				i => i.MatchLdcI4(662),
				i => i.MatchBneUn(out IL_0433)
				)) {
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("The Confection REBAKED: Plant Check massive if statement instructions was not found");
				return;
			}
			if (IL_0433 == null)
				return;
			c.EmitLdloc1(); //num2
			c.EmitLdloc0(); //num
			c.EmitDelegate((int num2, int num) => {
				return (num2 != ModContent.TileType<CreamGrass_Foliage>() || num == ModContent.TileType<CreamGrass>() || num == ModContent.TileType<CreamGrassMowed>());
			});
			c.EmitBrtrue(IL_0433);
			
			c.GotoNext(
				MoveType.Before,
				i => i.MatchLdloc2(),
				i => i.MatchBrfalse(out _)
				);
			c.EmitLdloc0(); //num
			c.EmitLdloca(1); //num2
			c.EmitDelegate((int num, ref int num2) => {
				if (num == ModContent.TileType<CreamGrass>() || num == ModContent.TileType<CreamGrassMowed>()) 
				{
					num2 = ((num2 != 73) ? ModContent.TileType<CreamGrass_Foliage>() : 113);
				}
			});
			MonoModHooks.DumpIL(ModContent.GetInstance<TheConfectionRebirth>(), il);
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
