using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Tiles
{
    public class Creamsand : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSand[Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamGrass").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneBrick").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamWood").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("HardenedCreamsand").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("Creamsandstone").Type] = true;
			TileID.Sets.Suffocate[Type] = true;
            TileID.Sets.Conversion.Sand[Type] = true;
            TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true;
            TileID.Sets.Falling[Type] = true;
            AddMapEntry(new Color(99, 57, 46));
			TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.FallingBlockProjectile[Type] = new TileID.Sets.FallingBlockProjectileInfo(ModContent.ProjectileType<CreamsandProjectile>());
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			TileID.Sets.GeneralPlacementTiles[Type] = false;
			TileID.Sets.ChecksForMerge[Type] = true;
		}

		public override bool HasWalkDust() => Main.rand.NextBool(3);

		public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color) {
			dustType = DustType;
		}

		public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
			sightColor = new Color(210, 196, 145);
			return true;
		}
	}
}