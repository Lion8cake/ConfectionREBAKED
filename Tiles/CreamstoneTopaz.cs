using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class CreamstoneTopaz : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CookieBlock").Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamGrass").Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("HallowedOre").Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("NeapoliniteOre").Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneBrick").Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("SacchariteBlock").Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamWood").Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamBlock").Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("BlueIce").Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("Creamstone").Type] = true;
			Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
			RegisterItemDrop(ItemID.Topaz);
            DustType = ModContent.DustType<CreamDust>();
            AddMapEntry(new Color(188, 168, 120));

            HitSound = SoundID.Tink;
            MinPick = 65;
        }

        /*private bool SpawnRocks(int i, int j)
	{
		if (Main.tile[i, j - 1].type == 0 && Main.tile[i, j - 2].type == 0 && Main.rand.Next(6) == 0)
		{
			WorldGen.PlaceTile(i, j - 1, ModContent.TileType<CreamstoneStalagmites>(), mute: true);
			return true;
		}
		if (Main.tile[i, j + 1].type == 0 && Main.tile[i, j + 2].type == 0 && Main.rand.Next(4) == 0)
		{
			WorldGen.PlaceTile(i, j + 1, ModContent.TileType<CreamstoneStalacmites>(), mute: true);
			return true;
		}
		if (Main.tile[i, j - 1].type == 0 && Main.rand.Next(6) == 0)
		{
			WorldGen.PlaceTile(i, j - 1, ModContent.TileType<CreamstoneStalagmites2>(), mute: true);
			return true;
		}
		if (Main.tile[i, j + 1].type == 0 && Main.rand.Next(4) == 0)
		{
			WorldGen.PlaceTile(i, j + 1, ModContent.TileType<CreamstoneStalacmites2>(), mute: true);
			return true;
		}
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
		if (Main.rand.Next(8) == 0)
		   {
			    bool spawned = false;
			    if (!spawned)
			    {
				    spawned = SpawnRocks(i, j);
			    }
		    }
	    }*/

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

		public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
			sightColor = new Color(210, 196, 145);
			return true;
		}
	}
}