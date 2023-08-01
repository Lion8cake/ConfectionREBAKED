using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles.Furniture
{
    public class SacchariteBath : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

            DustType = ModContent.DustType<SacchariteCrystals>();
            AdjTiles = new int[] { TileID.Beds };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(32, 174, 221), name);
        }


        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }
    }
}
