using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class BlueIce : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CookieBlock>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamGrass>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<HallowedOre>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<NeapoliniteOre>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamstoneBrick>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamWood>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamBlock>()] = true;
            TheConfectionRebirth.tileMerge[Type, 161] = true;
            TheConfectionRebirth.tileMerge[Type, 163] = true;
            TheConfectionRebirth.tileMerge[Type, 164] = true;
            TheConfectionRebirth.tileMerge[Type, 200] = true;
            TheConfectionRebirth.tileMerge[Type, 147] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            TileID.Sets.Ices[Type] = true;
            TileID.Sets.IcesSlush[Type] = true;
            TileID.Sets.IcesSnow[Type] = true;
            TileID.Sets.Conversion.Ice[Type] = true;
            DustType = ModContent.DustType<OrangeIceDust>();
            ItemDrop = ModContent.ItemType<Items.Placeable.OrangeIce>();
            AddMapEntry(new Color(237, 145, 103));

            HitSound = SoundID.Tink;
        }

        public override void FloorVisuals(Player player)
        {
            player.slippy = true;
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