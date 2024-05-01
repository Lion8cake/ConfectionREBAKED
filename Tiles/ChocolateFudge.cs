using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs;

namespace TheConfectionRebirth.Tiles
{
    public class ChocolateFudge : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Tiles.CookieBlock>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Tiles.CreamGrass>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Tiles.HallowedOre>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Tiles.NeapoliniteOre>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Tiles.CreamstoneBrick>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Tiles.CreamWood>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Tiles.CreamBlock>()] = true;
            TheConfectionRebirth.tileMerge[Type, 161] = true;
            TheConfectionRebirth.tileMerge[Type, 163] = true;
            TheConfectionRebirth.tileMerge[Type, 164] = true;
            TheConfectionRebirth.tileMerge[Type, 200] = true;
            TheConfectionRebirth.tileMerge[Type, 147] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            DustType = ModContent.DustType<Dusts.FudgeDust>();
            AddMapEntry(new Color(108, 61, 49));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

		public override void NearbyEffects(int i, int j, bool closer)
        {
            Player player = Main.LocalPlayer;
            if ((int)Vector2.Distance(player.Center / 16f, new Vector2(i, j)) <= 1)
            {
                player.AddBuff(ModContent.BuffType<Fudged>(), Main.rand.Next(10, 20));
            }
        }
    }
}