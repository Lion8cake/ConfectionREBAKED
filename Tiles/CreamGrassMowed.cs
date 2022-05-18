using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class CreamGrassMowed : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            this.SetModTree(new Trees.CreamTree());
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamGrass").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CookieBlock").Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBrick[base.Type] = true;
            Main.tileSolid[base.Type] = true;
            Main.tileBlockLight[base.Type] = true;
            TileID.Sets.Grass[base.Type] = true;
            TileID.Sets.ChecksForMerge[base.Type] = true;
            AddMapEntry(new Color(235, 207, 150));
            SoundType = 0;
            SoundStyle = 2;
            ItemDrop = Mod.Find<ModItem>("CookieBlock").Type;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<CookieBlock>();
            }
        }
    }
}