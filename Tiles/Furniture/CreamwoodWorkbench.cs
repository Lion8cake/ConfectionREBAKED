using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles.Furniture
{
    public class CreamwoodWorkbench : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileTable[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;

            DustType = ModContent.DustType<Dusts.ChipDust>();
            AdjTiles = new int[] { TileID.WorkBenches };

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
            TileObjectData.newTile.CoordinateHeights = new[] { 18 };
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            LocalizedText name = CreateMapEntryName();
            //name.SetDefault("Work Bench");
            AddMapEntry(new Color(106, 65, 51), name);
        }

		public override void NumDust(int x, int y, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override bool CanDrop(int i, int j) {
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Placeable.Furniture.CreamwoodWorkbench>());
			return false;
		}
	}
}