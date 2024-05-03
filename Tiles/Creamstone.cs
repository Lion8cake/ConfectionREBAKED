using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class Creamstone : ModTile
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
			AddMapEntry(new Color(188, 168, 120));
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Creamstone>()] = true;
			DustType = ModContent.DustType<CreamstoneDust>();

			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 65;
		}

		public override void RandomUpdate(int i, int j) {
			Tile Blockpos = Main.tile[i, j];
			if (j > Main.rockLayer) {
				for (int NearSaccX = i + 4; NearSaccX > i - 4; --NearSaccX) {
					for (int NearSaccY = j + 4; NearSaccY > j - 4; --NearSaccY) {
						if (Main.tile[NearSaccX, NearSaccY].TileType == ModContent.TileType<SacchariteBlock>()) {
							return;
						}
					}
				}
				if (WorldGen.genRand.NextBool(40) && !Blockpos.IsHalfBlock && !Blockpos.BottomSlope && !Blockpos.LeftSlope && !Blockpos.RightSlope && !Blockpos.TopSlope) {
					if (!Main.tile[i + 1, j].HasTile && Main.tile[i + 1, j].LiquidAmount == 0) {
						WorldGen.PlaceTile(i + 1, j, ModContent.TileType<SacchariteBlock>(), mute: true);
					}
					else if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j - 1].LiquidAmount == 0) {
						WorldGen.PlaceTile(i, j - 1, ModContent.TileType<SacchariteBlock>(), mute: true);
					}
					else if (!Main.tile[i - 1, j].HasTile && Main.tile[i - 1, j].LiquidAmount == 0) {
						WorldGen.PlaceTile(i - 1, j, ModContent.TileType<SacchariteBlock>(), mute: true);
					}
				}
			}
		}

		public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
			sightColor = new Color(210, 196, 145);
			return true;
		}
	}
}