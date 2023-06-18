using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using AltLibrary;
using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using AltLibrary.Common.Hooks;

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
            TileMerge(Type, ModContent.TileType<CookieBlock>());
            TileMerge(Type, ModContent.TileType<CreamGrass>());
            TileMerge(Type, ModContent.TileType<HallowedOre>());
            TileMerge(Type, ModContent.TileType<NeapoliniteOre>());
            TileMerge(Type, ModContent.TileType<CreamstoneBrick>());
            TileMerge(Type, ModContent.TileType<SacchariteBlock>());
            TileMerge(Type, ModContent.TileType<CreamWood>());
            TileMerge(Type, ModContent.TileType<CreamBlock>());
            TileMerge(Type, ModContent.TileType<BlueIce>());
            TileMerge(Type, ModContent.TileType<CreamstoneRuby>());
            TileMerge(Type, ModContent.TileType<CreamstoneSaphire>());
            TileMerge(Type, ModContent.TileType<CreamstoneDiamond>());
            TileMerge(Type, ModContent.TileType<CreamstoneAmethyst>());
            TileMerge(Type, ModContent.TileType<CreamstoneTopaz>());
			TileMerge(Type, ModContent.TileType<CreamstoneEmerald>());
			TileMerge(Type, ModContent.TileType<Creamstone>());
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
			ModContent.Find<AltBiome>("TheConfectionRebirth", "ConfectionBiome").BakeTileChild(Type, tileParent, new(true, true, true));

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
