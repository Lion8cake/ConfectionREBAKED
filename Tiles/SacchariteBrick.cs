using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class SacchariteBrick : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
			TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[Type] = true;
			TileID.Sets.ForcedDirtMerging[Type] = true;
			TileID.Sets.GemsparkFramingTypes[Type] = Type;
			AddMapEntry(new Color(145, 241, 247));
            DustType = ModContent.DustType<SacchariteDust>();
        }

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
			return false;
		}
	}
}