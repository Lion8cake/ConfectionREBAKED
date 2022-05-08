/*using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheConfectionRebirth.Tiles;
using static Terraria.ModLoader.ModContent;

namespace TheConfectionRebirth
{
    class ConfectionSpread : GlobalTile // THIS WHOLE THING IS SUPER BROKEN, DO NOT USE
    {
        int[] ConfectionSpreaders = new int[] { TileType<BlueFairyFloss>(), TileType<PinkFairyFloss>(), TileType<PurpleFairyFloss>(),
         TileType<Creamsand>(), TileType<HardenedCreamsand>(), TileType<Creamsandstone>(), TileType<Creamstone>(), TileType<BlueIce>(), TileType<CreamstoneAmethyst>(),
         TileType<CreamstoneDiamond>(), TileType<CreamstoneEmerald>(), TileType<CreamstoneRuby>(), TileType<CreamstoneRuby>(), TileType<CreamstoneTopaz>(), TileType<CreamGrass>(),
         TileType<CookieBlock>(), TileType<CreamBlock>()};

        public override void RandomUpdate(int i, int j, int type)
        {
            if ((NPC.downedPlantBoss && WorldGen.genRand.Next(2) != 0) || !Main.hardMode)
            {
                return;
            }
            if (ConfectionSpreaders.Contains(type))
            {
                int targetX = i + WorldGen.genRand.Next(-3, 4);
                int targetY = j + WorldGen.genRand.Next(-3, 4);
                int targetType = Main.tile[targetX, targetY].type;
                if ((targetType == TileID.Stone || Main.tileMoss[targetType]) && targetType != TileType<Creamstone>())
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<Creamstone>();
                } 
                else if (TileID.Sets.Conversion.Sand[Main.tile[targetX, targetY].type] == true && targetType != TileType<Creamsand>())
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<Creamsand>();
                    
                }
                else if (TileID.Sets.Conversion.Grass[Main.tile[targetX, targetY].type] == true && targetType != TileType<CreamGrass>())
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<CreamGrass>();

                }
                else if (TileID.Sets.Conversion.Ice[Main.tile[targetX, targetY].type] == true && targetType != TileType<BlueIce>())
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<BlueIce>();

                }
                else if (TileID.Sets.Conversion.HardenedSand[Main.tile[targetX, targetY].type] == true && targetType != TileType<HardenedCreamsand>())
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<HardenedCreamsand>();

                }
                else if (TileID.Sets.Conversion.Sandstone[Main.tile[targetX, targetY].type] == true && targetType != TileType<Creamsandstone>())
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<Creamsandstone>();

                }
                else if (targetType == TileID.Amethyst)
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<CreamstoneAmethyst>();

                }
                else if (targetType == TileID.Diamond)
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<CreamstoneDiamond>();

                }
                else if (targetType == TileID.Emerald)
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<CreamstoneEmerald>();

                }
                else if (targetType == TileID.Ruby)
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<CreamstoneRuby>();

                }
                else if (targetType == TileID.Sapphire)
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<CreamstoneSaphire>();

                }
                else if (targetType == TileID.Topaz)
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<CreamstoneTopaz>();

                }
                else if (targetType == TileID.Cloud)
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<PinkFairyFloss>();

                }
                else if (targetType == TileID.RainCloud)
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<PurpleFairyFloss>();

                }
                else if (targetType == TileID.SnowCloud)
                {
                    Main.tile[targetX, targetY].type = (ushort)TileType<BlueFairyFloss>();

                }
            }
        }
    }
}*/
