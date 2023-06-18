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
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CookieBlock>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamGrass>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<HallowedOre>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<NeapoliniteOre>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamstoneBrick>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<SacchariteBlock>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamWood>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamBlock>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<BlueIce>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamstoneRuby>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamstoneSaphire>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamstoneDiamond>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamstoneAmethyst>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamstoneTopaz>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamstoneEmerald>()] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            Main.tileStone[Type] = true;
            TileID.Sets.Conversion.Stone[Type] = true;
            TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
            DustType = ModContent.DustType<CreamDust>();
            ItemDrop = ModContent.ItemType<Items.Placeable.Creamstone>();
            AddMapEntry(new Color(188, 168, 120));

            HitSound = SoundID.Tink;
            MinPick = 65;
        }

        private bool Saccharite(int i, int j)
		{
            if (j > Main.rockLayer)
            {
                if (Main.tile[i, j + 1].TileType == 0 && Main.rand.Next(2) == 0)
                {
                    WorldGen.PlaceTile(i, j + 1, ModContent.TileType<SacchariteBlock>(), mute: true);
                    return true;
                }
                if (Main.tile[i, j - 1].TileType == 0 && Main.rand.Next(20) == 0)
                {
                    WorldGen.PlaceTile(i, j - 1, ModContent.TileType<SacchariteBlock>(), mute: true);
                    return true;
                }
                return false;
            }
            return true;
        }
	
		public override void RandomUpdate(int i, int j)
		{
			if (Main.rand.Next(12) == 0)
			{
				bool spawned = false;
				if (!spawned)
				{
					spawned = Saccharite(i, j);
				}
			}
		}

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}