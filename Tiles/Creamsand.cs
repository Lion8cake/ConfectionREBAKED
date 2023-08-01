using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
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
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (WorldGen.noTileActions)
                return true;

            Tile above = Main.tile[i, j - 1];
            Tile below = Main.tile[i, j + 1];
            bool canFall = true;

            if (below == null || below.HasTile)
                canFall = false;

            if (above.HasTile && (TileID.Sets.BasicChest[above.TileType] || TileID.Sets.BasicChestFake[above.TileType] || above.TileType == TileID.PalmTree || above.TileType == TileID.Cactus || TileID.Sets.BasicDresser[above.TileType]))
                canFall = false;

            if (canFall)
            {
                int projectileType = ModContent.ProjectileType<CreamsandProjectile>();
                float positionX = i * 16 + 8;
                float positionY = j * 16 + 8;

                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.tile[i, j].ClearTile();
                    int proj = Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, projectileType, 50, 0f, Main.myPlayer);
                    Main.projectile[proj].ai[0] = 1f;
                    WorldGen.SquareTileFrame(i, j);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    bool spawnProj = true;

                    for (int k = 0; k < 1000; k++)
                    {
                        Projectile otherProj = Main.projectile[k];

                        if (otherProj.active && otherProj.owner == Main.myPlayer && otherProj.type == projectileType && Math.Abs(otherProj.timeLeft - 3600) < 60 && otherProj.Distance(new Vector2(positionX, positionY)) < 4f)
                        {
                            spawnProj = false;
                            break;
                        }
                    }

                    if (spawnProj)
                    {
                        int proj = Projectile.NewProjectile(new EntitySource_Misc(""), new Vector2(i, j) * 16 + new Vector2(8, 8), Vector2.Zero, projectileType, 50, 0f, Main.myPlayer);
                        Main.projectile[proj].velocity.Y = 0.5f;
                        Main.projectile[proj].position.Y += 2f;
                        Main.projectile[proj].netUpdate = true;
                    }

                    NetMessage.SendTileSquare(-1, i, j, 1);
                    WorldGen.SquareTileFrame(i, j);
                }
                return false;
            }
            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    }
}