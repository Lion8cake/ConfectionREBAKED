using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class SacchariteBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("Creamstone").Type] = true;
            ItemDrop = ModContent.ItemType<Items.Placeable.Saccharite>();
            AddMapEntry(new Color(32, 174, 221));
            DustType = ModContent.DustType<SacchariteCrystals>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        /*private bool SpawnSaccharite(int i, int j)
	{
		if (Main.tile[i, j + 1].type == 0 && Main.rand.Next(2) == 0)
		{
			WorldGen.PlaceTile(i, j + 1, ModContent.TileType<SacchariteBlock>(), mute: true);
			return true;
		}
		if (Main.tile[i, j - 1].type == 0 && Main.rand.Next(2) == 0)
		{
			WorldGen.PlaceTile(i, j - 1, ModContent.TileType<SacchariteBlock>(), mute: true);
			return true;
		}
		return false;
	}
	
	public override void RandomUpdate(int i, int j)
	{
		if (Main.rand.Next(20) == 0)
		   {
			    bool spawned = false;
			    if (!spawned)
			    {
				    spawned = SpawnSaccharite(i, j);
			    }
		    }
	    }*/
    }
}