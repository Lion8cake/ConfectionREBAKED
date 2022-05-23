using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs;

namespace TheConfectionRebirth.Tiles
{
	public class ChocolateFudge : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<Tiles.CookieBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Tiles.CreamGrass>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Tiles.HallowedOre>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Tiles.NeapoliniteOre>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Tiles.CreamstoneBrick>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Tiles.CreamWood>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Tiles.CreamBlock>()] = true;
            Main.tileMerge[Type][161] = true;
            Main.tileMerge[Type][163] = true;
            Main.tileMerge[Type][164] = true;
            Main.tileMerge[Type][200] = true;
            Main.tileMerge[Type][147] = true;
            Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			DustType = ModContent.DustType<Dusts.FudgeDust>();
			ItemDrop = ModContent.ItemType<Items.Placeable.ChocolateFudge>();
			AddMapEntry(new Color(108, 61, 49));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
		
		public override void NearbyEffects(int i, int j, bool closer)
		{
			Player player = Main.LocalPlayer;
			if ((int)Vector2.Distance(player.Center / 16f, new Vector2((float)i, (float)j)) <= 1)
			{
				player.AddBuff(ModContent.BuffType<Fudged>(), Main.rand.Next(10, 20));
			}
		}
	}
}