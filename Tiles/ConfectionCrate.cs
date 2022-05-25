using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace TheConfectionRebirth.Tiles          
{
    public class ConfectionCrate : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = true;     
            Main.tileFrameImportant[Type] = true;
            Main.tileTable[Type] = true;     
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            AddMapEntry(new Color(133, 87, 50)); 
            TileObjectData.addTile(Type);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<Items.Placeable.ConfectionCrate>());
        }
    }
}