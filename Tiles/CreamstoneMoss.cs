using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamstoneMossGreen : ModTile
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public override void SetStaticDefaults() 
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			ConfectionIDs.Sets.IsTileCreamMoss[Type] = new Color(49, 242, 151);

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = DustID.GreenMoss;
			AddMapEntry(new Color(49, 134, 114));
			HitSound = SoundID.Dig;
			MineResist = 2f;
			MinPick = 65;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Creamstone>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<Creamstone>();
			}
		}
	}

	public class CreamstoneMossBrown : ModTile
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			ConfectionIDs.Sets.IsTileCreamMoss[Type] = Color.SandyBrown;

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = DustID.BrownMoss;
			AddMapEntry(new Color(126, 134, 49));
			HitSound = SoundID.Dig;
			MineResist = 2f;
			MinPick = 65;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Creamstone>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<Creamstone>();
			}
		}
	}

	public class CreamstoneMossRed : ModTile
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			ConfectionIDs.Sets.IsTileCreamMoss[Type] = Color.IndianRed;

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = DustID.RedMoss;
			AddMapEntry(new Color(134, 59, 49));
			HitSound = SoundID.Dig;
			MineResist = 2f;
			MinPick = 65;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Creamstone>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<Creamstone>();
			}
		}
	}

	public class CreamstoneMossBlue : ModTile
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			ConfectionIDs.Sets.IsTileCreamMoss[Type] = Color.CornflowerBlue;

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = DustID.BlueMoss;
			AddMapEntry(new Color(43, 86, 140));
			HitSound = SoundID.Dig;
			MineResist = 2f;
			MinPick = 65;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Creamstone>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<Creamstone>();
			}
		}
	}

	public class CreamstoneMossPurple : ModTile
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			ConfectionIDs.Sets.IsTileCreamMoss[Type] = Color.Purple;

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = DustID.PurpleMoss;
			AddMapEntry(new Color(121, 49, 134));
			HitSound = SoundID.Dig;
			MineResist = 2f;
			MinPick = 65;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Creamstone>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<Creamstone>();
			}
		}
	}

	public class CreamstoneMossLava : ModTile
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			ConfectionIDs.Sets.IsTileCreamMoss[Type] = new Color(Color.DarkOrange.R, Color.DarkOrange.G, Color.DarkOrange.B, 0);

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = DustID.LavaMoss;
			AddMapEntry(new Color(254, 121, 2));
			HitSound = SoundID.Dig;
			MineResist = 2f;
			MinPick = 65;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Creamstone>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<Creamstone>();
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.25f;
			g = 0.1f;
			b = 0f;
		}
	}

	public class CreamstoneMossKrypton : ModTile
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			ConfectionIDs.Sets.IsTileCreamMoss[Type] = new Color(Color.Lime.R, Color.Lime.G, Color.Lime.B, 0);

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = DustID.KryptonMoss;
			AddMapEntry(new Color(114, 254, 2));
			HitSound = SoundID.Dig;
			MineResist = 2f;
			MinPick = 65;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Creamstone>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<Creamstone>();
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0f;
			g = 0.25f;
			b = 0f;
		}
	}

	public class CreamstoneMossXenon : ModTile
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			ConfectionIDs.Sets.IsTileCreamMoss[Type] = new Color(145, 241, 247, 0);

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = DustID.XenonMoss;
			AddMapEntry(new Color(0, 197, 208));
			HitSound = SoundID.Dig;
			MineResist = 2f;
			MinPick = 65;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Creamstone>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<Creamstone>();
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0f;
			g = 0.16f;
			b = 0.34f;
		}
	}

	public class CreamstoneMossArgon : ModTile
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			ConfectionIDs.Sets.IsTileCreamMoss[Type] = new Color(252, 135, 211, 0);

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = DustID.ArgonMoss;
			AddMapEntry(new Color(208, 0, 126));
			HitSound = SoundID.Dig;
			MineResist = 2f;
			MinPick = 65;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Creamstone>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<Creamstone>();
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.3f;
			g = 0f;
			b = 0.17f;
		}
	}

	public class CreamstoneMossNeon : ModTile
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			ConfectionIDs.Sets.IsTileCreamMoss[Type] = new Color(223, 149, 251, 0);

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = DustID.VioletMoss;
			AddMapEntry(new Color(220, 12, 237));
			HitSound = SoundID.Dig;
			MineResist = 2f;
			MinPick = 65;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Creamstone>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<Creamstone>();
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.3f;
			g = 0f;
			b = 0.35f;
		}
	}

	public class CreamstoneMossHelium : ModTile
	{
		public override string Texture => "TheConfectionRebirth/Tiles/Creamstone";

		public override void SetStaticDefaults()
		{
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileStone[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			ConfectionIDs.Sets.IsTileCreamMoss[Type] = new Color(255, 255, 255, 0); //Gets RGB'd in the moss's drawing (im hardcoding because uh uh uhhhhh nunyabisness

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			ConfectionGlobalTile.MergesWithCreamGems(Type);

			DustType = DustID.RainbowMk2;
			AddMapEntry(new Color(255, 0, 0));
			HitSound = SoundID.Dig;
			MineResist = 2f;
			MinPick = 65;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.Creamstone>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail && !effectOnly)
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<Creamstone>();
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = (float)Main.DiscoR / 255f * 0.25f;
			g = (float)Main.DiscoG / 255f * 0.25f;
			b = (float)Main.DiscoB / 255f * 0.25f;
		}
	}
}
