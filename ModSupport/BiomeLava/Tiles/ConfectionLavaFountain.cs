using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.ModSupport.BiomeLava.Tiles
{
	public class ConfectionLavaFountain : ModTile
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ConfectionModCalling.BiomeLava != null;
		}

		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Origin = new Point16(1, 3);
			TileObjectData.newTile.CoordinateHeights = new int[4] { 16, 16, 16, 16 };
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			AnimationFrameHeight = 72;
			AddMapEntry(new Color(91, 53, 49));
			DustType = ModContent.DustType<CreamsandDust>();
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Tile tile2 = Main.tile[i, j];
				if (tile2.TileFrameY >= 72)
				{
					ConfectionModCalling.BiomeLava.Call("SetActiveLavaFountainColor", ModContent.GetInstance<TheConfectionRebirth>(), "ChocolateLavaStyle");
				}
			}
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
		}

		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			if (Main.tile[i, j].TileFrameY >= 72)
			{
				frameYOffset = Main.tileFrame[type];
				int num43 = i;
				if (Main.tile[i, j].TileFrameX % 36 != 0)
				{
					num43--;
				}
				frameYOffset += num43 % 6;
				if (frameYOffset >= 6)
				{
					frameYOffset -= 6;
				}
				frameYOffset *= 72;
			}
			else
			{
				frameYOffset = 0;
			}
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 4)
			{
				frameCounter = 0;
				frame++;
				if (frame >= 6)
				{
					frame = 0;
				}
			}
		}

		public override void HitWire(int i, int j)
		{
			SwitchLavaFountain(i, j);
		}

		public static void SwitchLavaFountain(int i, int j)
		{
			int num = i;
			int num2 = j;
			Tile tile = Main.tile[i, j];
			int num3;
			for (num3 = tile.TileFrameX / 18; num3 >= 2; num3 -= 2)
			{
			}
			tile = Main.tile[i, j];
			int num4 = tile.TileFrameY / 18;
			if (num4 >= 4)
			{
				num4 -= 4;
			}
			num = i - num3;
			num2 = j - num4;
			for (int k = num; k < num + 2; k++)
			{
				for (int l = num2; l < num2 + 4; l++)
				{
					tile = Main.tile[k, l];
					if (!tile.HasTile)
					{
						continue;
					}
					if (tile.TileFrameY < 72)
					{
						tile.TileFrameY += 72;
					}
					else
					{
						tile.TileFrameY -= 72;
					}
				}
			}
			if (Wiring.running)
			{
				Wiring.SkipWire(num, num2);
				Wiring.SkipWire(num, num2 + 1);
				Wiring.SkipWire(num, num2 + 2);
				Wiring.SkipWire(num, num2 + 3);
				Wiring.SkipWire(num + 1, num2);
				Wiring.SkipWire(num + 1, num2 + 1);
				Wiring.SkipWire(num + 1, num2 + 2);
				Wiring.SkipWire(num + 1, num2 + 3);
			}
			NetMessage.SendTileSquare(-1, num, num2, 2, 4);
		}


		public override bool RightClick(int i, int j)
		{
			SwitchLavaFountain(i, j);
			SoundEngine.PlaySound(SoundID.Mech, (Vector2?)new Vector2((float)(i * 16), (float)(j * 16)));
			return true;
		}

		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			yield return new Item(ModContent.ItemType<Items.ConfectionLavaFountain>());
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.ConfectionLavaFountain>();
		}
	}
}
