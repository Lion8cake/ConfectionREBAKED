using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles.Deletion
{
	public class CreamstoneStalacmites2 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.addTile(Type);
		}
	
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 4 < 3)
			{
				spriteEffects = (SpriteEffects)1;
			}
		}
	
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 3);
		}
	
		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
		{
			frameXOffset = i % 3 * 18;
		}
		
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile tileBelow = Framing.GetTileSafely(i, j + 1);
			int type = -1;
			if (tileBelow.HasTile && !tileBelow.BottomSlope)
			{
				type = tileBelow.TileType;
			}
			if (type == ModContent.TileType<SherbetTorch>() || type == Type)
			{
				return true;
			}
			WorldGen.KillTile(i, j);
			return true;
		}
	}
}