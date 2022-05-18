using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class CreamBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamGrass").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("Creamstone").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamWood").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("BlueIce").Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            TileID.Sets.Snow[Type] = true;
            DustType = ModContent.DustType<CreamSnowDust>();
            ItemDrop = ModContent.ItemType<Items.Placeable.CreamBlock>();
            AddMapEntry(new Color(219, 223, 234));
            this.SetModTree(new Trees.CreamSnowTree());
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}