using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class SherbetBricks : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileBrick[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSolid[Type] = true;
			TileID.Sets.DontDrawTileSliced[Type] = true;
			TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[Type] = true;
            AddMapEntry(new Color(213, 105, 89));
			AddMapEntry(new Color(213, 136, 89));
			AddMapEntry(new Color(213, 167, 89));
			AddMapEntry(new Color(213, 198, 89));
			AddMapEntry(new Color(198, 213, 89));
			AddMapEntry(new Color(136, 213, 89));
			AddMapEntry(new Color(89, 213, 136));
			AddMapEntry(new Color(89, 198, 213));
			AddMapEntry(new Color(89, 167, 213));
			AddMapEntry(new Color(89, 136, 213));
			AddMapEntry(new Color(105, 89, 213));
			AddMapEntry(new Color(167, 89, 213));
			AddMapEntry(new Color(213, 89, 213));
			DustType = ModContent.DustType<SherbetDust>();
			HitSound = SoundID.Tink;
        }

		public override ushort GetMapOption(int i, int j)
		{
			return (ushort)((i + j) % 13);
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = TheConfectionRebirth.SherbR / 255f * 0.25f;
			g = TheConfectionRebirth.SherbG / 255f * 0.25f;
			b = TheConfectionRebirth.SherbB / 255f * 0.25f;
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			Color color = new Color(TheConfectionRebirth.SherbR, TheConfectionRebirth.SherbG, TheConfectionRebirth.SherbB, 255); //uses an IL edit due to poor programming on tmodloader's end. This is for when the DrawEffects issue is fixed
			if (drawData.tileCache.IsActuated)
			{
				color = ConfectionWorldGeneration.ActColor(color, drawData.tileCache);
			}
			drawData.finalColor = color;
		}
	}
}
