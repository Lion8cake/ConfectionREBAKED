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
		public override Color mossColor => Color.DarkGreen;
		public override int tileParent => TileID.GreenMoss;
	}

	public class BrownCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.Brown;
		public override int tileParent => TileID.BrownMoss;
	}

	public class RedCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.DarkRed;
		public override int tileParent => TileID.RedMoss;
	}
	public class BlueCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.DarkBlue;
		public override int tileParent => TileID.BlueMoss;
	}
	public class PurpleCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.Purple;
		public override int tileParent => TileID.PurpleMoss;
	}
	public class LavaCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.OrangeRed;
		public override int tileParent => TileID.LavaMoss;
	}
	public class KryptonCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.Lime;
		public override int tileParent => TileID.KryptonMoss;
	}
	public class XenomCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.LightBlue;
		public override int tileParent => TileID.XenonMoss;
	}
	public class ArgonCreamMoss : CreamMoss
	{
		public override Color mossColor => Color.MediumOrchid;
		public override int tileParent => TileID.ArgonMoss;
	}
	public abstract class CreamMoss : ModTile
    {
        public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public virtual Color mossColor => Color.White;
		public virtual int tileParent => 0;
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CookieBlock").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamGrass").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("HallowedOre").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("NeapoliniteOre").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamstoneBrick").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("SacchariteBlock").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamWood").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamBlock").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("BlueIce").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamstoneRuby").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamstoneSaphire").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamstoneDiamond").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamstoneAmethyst").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamstoneTopaz").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamstoneEmerald").Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            DustType = ModContent.DustType<CreamDust>();
            ItemDrop = ModContent.ItemType<Items.Placeable.Creamstone>();
            HitSound = SoundID.Tink;
            AddMapEntry(mossColor);

		foreach(KeyValuePair<int, Color> entry in TilePostDraws.Moss.MossColor)
		{
			Main.tileMerge[Type][entry.Key] = true;
			Main.tileMerge[entry.Key][Type] = true;
		}
		TilePostDraws.Moss.MossColor.Add(Type, mossColor);
		ModContent.Find<AltBiome>("TheConfectionRebirth", "ConfectionBiome").BakeTileChild(Type, tileParent, new(true, true, true));
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
