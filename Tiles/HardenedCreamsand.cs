using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class HardenedCreamsand : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamstoneBrick>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamWood>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Creamsandstone>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Creamsand>()] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            ItemDrop = ModContent.ItemType<Items.Placeable.HardenedCreamsand>();
            AddMapEntry(new Color(108, 61, 49));
            TileID.Sets.Conversion.HardenedSand[Type] = true;
            TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}