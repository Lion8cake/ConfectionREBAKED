using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class SherbetTorch : ModTile
    {
		private Asset<Texture2D> flameTexture;

		private int tileFrame = 0;

		public override void SetStaticDefaults() {
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileWaterDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.Torch[Type] = true;

			AnimationFrameHeight = 22;

			DustType = ModContent.DustType<SherbetDust>();
			AdjTiles = new int[] { TileID.Torches };

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

			TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { 124 };
			TileObjectData.addAlternate(1);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
			TileObjectData.newAlternate.AnchorAlternateTiles = new[] { 124 };
			TileObjectData.addAlternate(2);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorWall = true;
			TileObjectData.addAlternate(0);
			TileObjectData.addTile(Type);

			LocalizedText name = CreateMapEntryName();

			AddMapEntry(new Color(253, 221, 3), name);

			if (!Main.dedServ) {
				flameTexture = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/SherbetTorch_Flame");
			}
		}

		public override void MouseOver(int i, int j) {
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Placeable.SherbetTorch>();
		}

		public override void AnimateTile(ref int frame, ref int frameCounter) {
			frameCounter++;
			if (frameCounter > 5) {
				frameCounter = 0;
				frame++;
				if (frame > 12) {
					frame = 0;
				}
			}
			tileFrame = frame;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = Main.rand.Next(1, 3);

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
			Tile tile = Main.tile[i, j];

			if (tile.TileFrameX < 66) {
				switch (tileFrame) {
					case 0:
						r = 1.92f;
						g = 0.26f;
						b = 0.26f;
						break;
					case 1:
						r = 1.93f;
						g = 0.68f;
						b = 0.26f;
						break;
					case 2:
						r = 2.53f;
						g = 0.91f;
						b = 0.03f;
						break;
					case 3:
						r = 2.52f;
						g = 1.58f;
						b = 0.03f;
						break;
					case 4:
						r = 1.99f;
						g = 1.67f;
						b = 0.15f;
						break;
					case 5:
						r = 1.04f;
						g = 1.57f;
						b = 0.15f;
						break;
					case 6:
						r = 0.23f;
						g = 1.07f;
						b = 0.29f;
						break;
					case 7:
						r = 0.23f;
						g = 1.06f;
						b = 1.06f;
						break;
					case 8:
						r = 0.29f;
						g = 0.8f;
						b = 1.31f;
						break;
					case 9:
						r = 0.37f;
						g = 0.57f;
						b = 2.27f;
						break;
					case 10:
						r = 0.66f;
						g = 0.37f;
						b = 2.26f;
						break;
					case 11:
						r = 1.01f;
						g = 0.36f;
						b = 1.62f;
						break;
					case 12:
						r = 1.62f;
						g = 0.36f;
						b = 1.58f;
						break;
				}
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {

			int offsetY = 0;

			if (WorldGen.SolidTile(i, j - 1)) {
				offsetY = 2;

				if (WorldGen.SolidTile(i - 1, j + 1) || WorldGen.SolidTile(i + 1, j + 1)) {
					offsetY = 4;
				}
			}

			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

			if (Main.drawToScreen) {
				zero = Vector2.Zero;
			}

			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);
			Color color = new Color(100, 100, 100, 0);
			int width = 20;
			int height = 20;
			var tile = Main.tile[i, j];
			int frameX = tile.TileFrameX;
			int frameY = AnimationFrameHeight * tileFrame;

			for (int k = 0; k < 7; k++) {
				float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
				float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

				spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + xx, j * 16 - (int)Main.screenPosition.Y + offsetY + yy) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default, 1f, SpriteEffects.None, 0f);
			}
		}
	}
}