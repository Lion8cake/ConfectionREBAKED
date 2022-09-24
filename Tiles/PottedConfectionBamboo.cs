using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles
{
    class PottedConfectionBamboo : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.Origin = new Point16(0, 3);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16 };
            TileObjectData.addTile(Type);
            AdjTiles = new int[] { 584 };
            AddMapEntry(new Color(106, 65, 51));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 32, ModContent.ItemType<Items.Placeable.PottedConfectionBamboo>());
        }
    }
}
