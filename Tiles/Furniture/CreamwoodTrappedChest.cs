using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles.Furniture
{
    public class CreamwoodTrappedChest : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileOreFinderPriority[Type] = 500;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileSpelunker[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.BasicChestFake[Type] = true;
            TileID.Sets.AvoidedByNPCs[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;
            TileID.Sets.IsATrigger[Type] = true;

            DustType = ModContent.DustType<CreamwoodDust>();
            AdjTiles = new int[] { 441 };

            AddMapEntry(new Color(106, 65, 51), CreateMapEntryName());

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[2] { 16, 18 };
			TileObjectData.newTile.AnchorInvalidTiles = new int[5] { 127, 138, 664, 665, 484 };
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			if (tileFrameY == 18)
			{
				height = 18;
			}
			int num22 = tileFrameX % 36;
			int num33 = tileFrameY % 38;
			if (Animation.GetTemporaryFrame(i - num22 / 18, j - num33 / 18, out var frameData))
			{
				tileFrameY = (short)(38 * frameData + num33);
			}
		}

		public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile4 = Main.tile[i, j];
            int left = i;
            int top = j;
            if (tile4.TileFrameX % 36 != 0)
            {
                left--;
            }

            if (tile4.TileFrameY != 0)
            {
                top--;
            }

			int num3;
			for (num3 = tile4.TileFrameX / 18; num3 > 1; num3 -= 2)
			{
			}
			num3 = i - num3;
			int num4 = j - tile4.TileFrameY / 18;
			Animation.NewTemporaryAnimation(2, tile4.TileType, num3, num4);
			NetMessage.SendTemporaryAnimation(-1, 2, tile4.TileType, num3, num4);
			//Wiring.HitSwitch(i, j);
			HitWire(i, j);
			NetMessage.SendData(MessageID.HitSwitch, -1, -1, null, i, j);
			return true;
        }

		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			tile = Main.tile[i, j];
			int num = tile.TileFrameX / 18 * -1;
			tile = Main.tile[i, j];
			int num2 = tile.TileFrameY / 18 * -1;
			num %= 4;
			if (num < -1)
			{
				num += 2;
			}
			num += i;
			num2 += j;
			SoundEngine.PlaySound(SoundID.Mech, new Vector2(i * 16, j * 16));
			Wiring.TripWire(num, num2, 2, 2);
		}

		public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int num22 = i;
			int num33 = j;
			if (tile.TileFrameX % 36 != 0)
			{
				num22--;
			}
			if (tile.TileFrameY % 36 != 0)
			{
				num33--;
			}
			player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.CreamwoodChest>();
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}
    }
}