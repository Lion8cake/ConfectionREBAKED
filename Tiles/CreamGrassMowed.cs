using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class CreamGrassMowed : ModTile
    {
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamGrass").Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CookieBlock").Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;
			TileID.Sets.Grass[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.ForcedDirtMerging[Type] = true;
			TileID.Sets.Conversion.MergesWithDirtInASpecialWay[Type] = true;
			TileID.Sets.Conversion.Grass[Type] = true;
			TileID.Sets.Conversion.GolfGrass[Type] = true;
			AddMapEntry(new Color(235, 207, 150));
			//SoundType = 0;
			//SoundStyle = 2;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CookieBlock>());
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