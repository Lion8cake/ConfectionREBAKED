using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class GreenCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.Green;
		public override int tileParent => TileID.GreenMoss;
	}

	public class BrownCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.Brown;
		public override int tileParent => TileID.BrownMoss;
	}

	public class RedCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.Red;
		public override int tileParent => TileID.RedMoss;
	}
	public class BlueCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.Blue;
		public override int tileParent => TileID.BlueMoss;
	}
	public class PurpleCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.Purple;
		public override int tileParent => TileID.PurpleMoss;
	}
	public class LavaCreamMoss : CreamMoss
	{
		public override Color mossColor => new Color(225, 88, 31);
		public override int tileParent => TileID.LavaMoss;
	}
	public class KryptonCreamMoss : CreamMoss
	{
		public override Color mossColor => new Color(198, 248, 72);
		public override int tileParent => TileID.KryptonMoss;
	}
	public class XenomCreamMoss : CreamMoss
	{
		public override Color mossColor => new Color(60, 206, 216);
		public override int tileParent => TileID.XenonMoss;
	}
	public class ArgonCreamMoss : CreamMoss
	{
		public override Color mossColor => new Color(233, 98, 183);
		public override int tileParent => TileID.ArgonMoss;
	}
	public abstract class CreamMoss : ModTile
    {
        public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public virtual Color mossColor => Color.White;
		public virtual int tileParent => 0;

		static void TileMerge(int thisTile, int other)
		{
			Main.tileMerge[thisTile][other] = true;
			Main.tileMerge[other][thisTile] = true;
		}
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            TileMerge(Type, Mod.Find<ModTile>("CookieBlock").Type);
            TileMerge(Type, Mod.Find<ModTile>("CreamGrass").Type);
            TileMerge(Type, Mod.Find<ModTile>("HallowedOre").Type);
            TileMerge(Type, Mod.Find<ModTile>("NeapoliniteOre").Type);
            TileMerge(Type, Mod.Find<ModTile>("CreamstoneBrick").Type);
            TileMerge(Type, Mod.Find<ModTile>("SacchariteBlock").Type);
            TileMerge(Type, Mod.Find<ModTile>("CreamWood").Type);
            TileMerge(Type, Mod.Find<ModTile>("CreamBlock").Type);
            TileMerge(Type, Mod.Find<ModTile>("BlueIce").Type);
            TileMerge(Type, Mod.Find<ModTile>("CreamstoneRuby").Type);
            TileMerge(Type, Mod.Find<ModTile>("CreamstoneSaphire").Type);
            TileMerge(Type, Mod.Find<ModTile>("CreamstoneDiamond").Type);
            TileMerge(Type, Mod.Find<ModTile>("CreamstoneAmethyst").Type);
            TileMerge(Type, Mod.Find<ModTile>("CreamstoneTopaz").Type);
			TileMerge(Type, Mod.Find<ModTile>("CreamstoneEmerald").Type);
			TileMerge(Type, Mod.Find<ModTile>("Creamstone").Type);
			Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            DustType = ModContent.DustType<CreamDust>();
            ItemDrop = ModContent.ItemType<Items.Placeable.Creamstone>();
            AddMapEntry(mossColor);

			foreach(KeyValuePair<int, Color> entry in TilePostDraws.Moss.MossColor)
			{
				TileMerge(Type, entry.Key);
			}
			TilePostDraws.Moss.MossColor.Add(Type, mossColor);

			for (int x = 0; x < TileID.Sets.Stone.Length; x++)
			{
				if (Main.tileMerge[TileID.Stone][x])
					TileMerge(Type, x);
			}
			TileMerge(Type, TileID.Stone);

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
		if (Main.tile[i, j - 1].type == 0 && Main.rand.Next(20) == 0)
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
    }
}
