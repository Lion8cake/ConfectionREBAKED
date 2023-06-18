using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class BlueFairyFloss : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamGrass>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Creamstone>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamWood>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<PurpleFairyFloss>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<PinkFairyFloss>()] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            ItemDrop = ModContent.ItemType<Items.Placeable.BlueFairyFloss>();
            AddMapEntry(new Color(78, 191, 252));
        }
    }
}