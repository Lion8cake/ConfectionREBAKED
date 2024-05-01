using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles
{
    public class ConfectionUGMusicBox : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            // TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(133, 87, 50), name);
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Placeable.ConfectionUGMusicBox>();
        }

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData) {
			if (Main.tile[i, j].TileFrameX == 36 && (int)Main.timeForVisualEffects % 7 == 0 && Main._rand.NextBool(3)) {
				int MusicNote = Main._rand.Next(570, 573);
				Vector2 SpawnPosition = new((float)(i * 16 + 8), (float)(j * 16 - 8));
				Vector2 NoteMovement = new(Main.WindForVisuals * 2f, -0.5f);
				NoteMovement.X *= 1f + (float)Main._rand.Next(-50, 51) * 0.01f;
				NoteMovement.Y *= 1f + (float)Main._rand.Next(-50, 51) * 0.01f;
				if (MusicNote == 572) {
					SpawnPosition.X -= 8f;
				}
				if (MusicNote == 571) {
					SpawnPosition.X -= 4f;
				}
				Gore.NewGore(new EntitySource_Misc(""), SpawnPosition, NoteMovement, MusicNote, 0.8f);
			}
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			if (frameX >= 36) {
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<Items.Placeable.ConfectionUGMusicBox>());
			}
		}
	}
}
