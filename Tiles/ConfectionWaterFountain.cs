using Microsoft.Xna.Framework;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class ConfectionWaterFountain : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            // TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateHeights = new int[4] { 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(0, 3);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);
            TileObjectData.addTile(Type);
            AnimationFrameHeight = 72;
            AddMapEntry(new Color(188, 168, 120));
			DustType = ModContent.DustType<CreamstoneDust>();
		}

		public override void NearbyEffects(int i, int j, bool closer) {
			if (Main.tile[i, j].TileFrameX >= 36) {
				Main.SceneMetrics.ActiveFountainColor = ModContent.Find<ModWaterStyle>("TheConfectionRebirth/CreamWaterStyle").Slot;
			}
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 6)
            {
                frame = (frame + 1) % 4;
                frameCounter = 0;
            }
        }

		public override void HitWire(int i, int j) {
			Tile tile = Main.tile[i, j];
			int x = i - tile.TileFrameX / 18 % 2;
			tile = Main.tile[i, j];
			int y = j - tile.TileFrameY / 18 % 4;
			for (int m = x; m < x + 2; m++) {
				for (int n = y; n < y + 4; n++) {
					tile = Main.tile[m, n];
					if (!tile.HasTile) {
						continue;
					}
					tile = Main.tile[m, n];
					if (tile.TileFrameX < 18 * 2) {
						tile = Main.tile[m, n];
						tile.TileFrameX += (short)(18 * 2);
					}
					else {
						tile = Main.tile[m, n];
						tile.TileFrameX -= (short)(18 * 2);
					}
				}
			}
			if (!Wiring.running) {
				return;
			}
			for (int k = 0; k < 2; k++) {
				for (int l = 0; l < 4; l++) {
					Wiring.SkipWire(x + k, y + l);
				}
			}
		}

		public override bool RightClick(int i, int j) {
			HitWire(i, j);
			SoundEngine.PlaySound(SoundID.Mech, (Vector2?)new Vector2((float)(i * 16), (float)(j * 16)));
			return true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			if (frameX >= 36) {
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<Items.Placeable.ConfectionWaterFountain>());
			}
		}

		public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.ConfectionWaterFountain>();
        }
    }
}
