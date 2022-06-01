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
            Main.tileMerge[Type][Mod.Find<ModTile>("CookieBlock").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamGrass").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("HallowedOre").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("NeapoliniteOre").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamstoneBrick").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamWood").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamBlock").Type] = true;
            Main.tileMerge[Type][161] = true;
            Main.tileMerge[Type][163] = true;
            Main.tileMerge[Type][164] = true;
            Main.tileMerge[Type][200] = true;
            Main.tileMerge[Type][147] = true;
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

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}