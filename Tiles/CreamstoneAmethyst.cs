using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamstoneAmethyst : ModTile
	{
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
			ConfectionIDs.Sets.Confection[Type] = true;
			ConfectionIDs.Sets.IsExtraConfectionTile[Type] = true;

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;

			DustType = ModContent.DustType<CreamstoneDust>();
			RegisterItemDrop(ItemID.Amethyst);
			AddMapEntry(new Color(188, 168, 120));
			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 65;
		}

		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			yield return new Item(ItemID.Amethyst, 1);
		}
	}
}
