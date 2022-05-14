using TheConfectionRebirth.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles.Trees;
using Terraria.DataStructures;

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
		
		public override void RandomUpdate(int i, int j)
        {
            if (!Main.tile[i, j + 1].HasTile)
            {
                WorldGen.KillTile(i, j, noItem: true);
                Projectile.NewProjectile(new EntitySource_Misc(""),  new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<CreamsandProjectile>(), 50, 0f, Main.myPlayer);
            }
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            if (!Main.tile[i, j + 1].HasTile)
            {
                WorldGen.KillTile(i, j, noItem: true);
                Projectile.NewProjectile(new EntitySource_Misc(""),  new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<CreamsandProjectile>(), 50, 0f, Main.myPlayer);
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Vector2 entityCoord = new Vector2(i, j) * 16 + new Vector2(8, 8);
            if (!Main.tile[i, j + 1].HasTile)
            {
                WorldGen.KillTile(i, j, noItem: true);
                Projectile.NewProjectile(new EntitySource_Misc(""), entityCoord, Vector2.Zero, ModContent.ProjectileType<CreamsandProjectile>(), 50, 0f, Main.myPlayer);
            }
        }

		/*public override int SaplingGrowthType(ref int style) {
			style = 1;
			return ModContent.TileType<CreamSapling>();
		}*/

		public override bool HasWalkDust()
		{
			return Main.rand.NextBool(3);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
	}
}