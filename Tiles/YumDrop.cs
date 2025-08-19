using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class YumDrop : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileCut[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoFail[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.SwaysInWindBasic[Type] = true;
            DustType = ModContent.DustType<YumDust>();
            HitSound = SoundID.Grass;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 20 };
			TileObjectData.addTile(Type);
            AddMapEntry(new Color(64, 133, 44));
			RegisterItemDrop(ModContent.ItemType<Items.YumDrop>());
		}

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            int type = -1;
            if (tileBelow.HasTile && !tileBelow.BottomSlope)
            {
                type = tileBelow.TileType;
            }
            if (type == ModContent.TileType<CreamGrass>() || type == Type)
            {
                return true;
            }
            WorldGen.KillTile(i, j);
            return true;
        }
    }
}
