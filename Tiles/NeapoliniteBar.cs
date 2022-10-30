using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles
{
    public class NeapoliniteBar : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            // TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(186, 134, 75), CreateMapEntryName());
        }

        /*public override bool Drop(int i, int j)
		{
			Tile t = Main.tile[i, j];
			int style = t.frameX / 18;
			if (style == 0)
			{
				Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Placeable.NeapoliniteBar>());
			}
			return base.Drop(i, j);
		}*/
    }
}