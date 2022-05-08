using TheConfectionRebirth.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles.Trees;

namespace TheConfectionRebirth.Tiles
{
	public class Creamsand : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSand[Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("CreamstoneBrick").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("CreamWood").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("HardenedCreamsand").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("Creamsandstone").Type] = true;
			TileID.Sets.Conversion.Sand[Type] = true; 
			TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true; 
			AddMapEntry(new Color(99, 57, 46));
			ItemDrop = ModContent.ItemType<Items.Placeable.Creamsand>();
			SetModCactus(new SprinkleCactus());
			SetModPalmTree(new CreamPalmTree());
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
	}
}