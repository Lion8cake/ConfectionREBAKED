using AltLibrary;
using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using AltLibrary.Common.Hooks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Weapons;
using TheConfectionRebirth.NPCs;
using TheConfectionRebirth.Tiles;
using Terraria;

namespace TheConfectionRebirth
{
    internal class ConfectionBiome : AltBiome
    {
        public override Color NameColor => new(210, 196, 145);

        public override void SetStaticDefaults()
        {
            BiomeType = BiomeType.Hallow;
            BiomeGrass = ModContent.TileType<CreamGrass>();
            BiomeStone = ModContent.TileType<Tiles.Creamstone>();
            BiomeSand = ModContent.TileType<Tiles.Creamsand>();
            BiomeIce = ModContent.TileType<BlueIce>();
            BiomeSandstone = ModContent.TileType<Tiles.Creamsandstone>();
            BiomeHardenedSand = ModContent.TileType<Tiles.HardenedCreamsand>();
            HammerType = ModContent.ItemType<GrandSlammer>();
            MechDropItemType = ModContent.ItemType<Items.Placeable.NeapoliniteBar>();
            BiomeChestItem = ModContent.ItemType<PopRocket>();
            BiomeChestTile = ModContent.TileType<Tiles.ConfectionBiomeChestTile>();
            BiomeChestTileStyle = 1;
            BiomeMowedGrass = ModContent.TileType<CreamGrassMowed>();
            MimicKeyType = ModContent.ItemType<KeyofDelight>();
            MimicType = ModContent.NPCType<BigMimicConfection>();

            BakeTileChild(ModContent.TileType<CookieBlock>(), TileID.Dirt, new(true, true, true));
            BakeTileChild(ModContent.TileType<CreamBlock>(), TileID.SnowBlock, new(true, true, true));
            BakeTileChild(ModContent.TileType<PinkFairyFloss>(), TileID.Cloud, new(true, true, true));
            BakeTileChild(ModContent.TileType<PurpleFairyFloss>(), TileID.RainCloud, new(true, true, true));
            BakeTileChild(ModContent.TileType<BlueFairyFloss>(), TileID.SnowCloud, new(true, true, true));
            BakeTileChild(ModContent.TileType<CreamstoneAmethyst>(), TileID.Amethyst, new(true, true, true));
            BakeTileChild(ModContent.TileType<CreamstoneDiamond>(), TileID.Diamond, new(true, true, true));
            BakeTileChild(ModContent.TileType<CreamstoneEmerald>(), TileID.Emerald, new(true, true, true));
            BakeTileChild(ModContent.TileType<CreamstoneRuby>(), TileID.Ruby, new(true, true, true));
            BakeTileChild(ModContent.TileType<CreamstoneSaphire>(), TileID.Sapphire, new(true, true, true));
            BakeTileChild(ModContent.TileType<CreamstoneTopaz>(), TileID.Topaz, new(true, true, true));
            //BakeTileChild(ModContent.TileType<CookiestCookieBlock>(), TileID.DirtiestBlock, new(true, true, true));

            WallContext = new WallContext()
                .AddReplacement<Walls.CreamstoneWall>(28, 1, 54, 55, 61, 3, 83) //Normal
                .AddReplacement<Walls.Creamstone2Wall>(50, 51, 52, 189, 193, 214, 201) //Cracking Confection Wall
                .AddReplacement<Walls.Creamstone3Wall>(56, 188, 192, 213, 202) //Confectionary Cryatal Wall
                .AddReplacement<Walls.Creamstone4Wall>(48, 49, 53, 57, 58, 190, 194, 212, 203) //Lined Confection Gem Wall
                .AddReplacement<Walls.Creamstone5Wall>(191, 195, 185, 215, 200, 83) //Melting Confection Wall
                .AddReplacement<Walls.CreamGrassWall>(63, 65, 66, 68, 69, 70, 81)
                .AddReplacement<Walls.CookieWall>(59, 171, 170, 196, 198, 199, 16, 2)
                .AddReplacement<Walls.CreamsandstoneWall>(216, 217, 218, 219)
                .AddReplacement<Walls.HardenedCreamsandWall>(197, 220, 221, 222)
                .AddReplacement(40, (ushort)ModContent.WallType<Walls.CreamWall>())
                .AddReplacement(71, (ushort)ModContent.WallType<Walls.BlueIceWall>())
                .AddReplacement(73, (ushort)ModContent.WallType<Walls.PinkFairyFlossWall>());
        }

        public override int GetAltBlock(int BaseBlock, int posX, int posY)
        {
            int grass = ModContent.TileType<CreamGrass>();
            Tile tile = Main.tile[posX, posY];
            if (tile.TileType == 59 && (Main.tile[posX - 1, posY].TileType == grass || Main.tile[posX + 1, posY].TileType == grass || Main.tile[posX, posY - 1].TileType == grass || Main.tile[posX, posY + 1].TileType == grass))
            {
                return ModContent.TileType<CookieBlock>();
            }
            return base.GetAltBlock(BaseBlock, posX, posY);
        }

        public override Dictionary<int, int> SpecialConversion => new()
        {
            [TileID.Dirt] = ModContent.TileType<Tiles.CookieBlock>(),
            [TileID.SnowBlock] = ModContent.TileType<Tiles.CreamBlock>(),
            [TileID.Cloud] = ModContent.TileType<Tiles.PinkFairyFloss>(),
            [TileID.RainCloud] = ModContent.TileType<Tiles.PurpleFairyFloss>(),
            [TileID.SnowCloud] = ModContent.TileType<Tiles.BlueFairyFloss>(),
            [TileID.Amethyst] = ModContent.TileType<Tiles.CreamstoneAmethyst>(),
            [TileID.Diamond] = ModContent.TileType<Tiles.CreamstoneDiamond>(),
            [TileID.Emerald] = ModContent.TileType<Tiles.CreamstoneEmerald>(),
            [TileID.Ruby] = ModContent.TileType<Tiles.CreamstoneRuby>(),
            [TileID.Sapphire] = ModContent.TileType<Tiles.CreamstoneSaphire>(),
            [TileID.Topaz] = ModContent.TileType<Tiles.CreamstoneTopaz>()
        };

        public override List<int> SpreadingTiles => new() { ModContent.TileType<CreamGrass>(), ModContent.TileType<Tiles.Creamstone>(), ModContent.TileType<Tiles.Creamsand>(), ModContent.TileType<BlueIce>(), ModContent.TileType<Tiles.Creamsandstone>(), ModContent.TileType<Tiles.HardenedCreamsand>() };

        public override string WorldIcon => "TheConfectionRebirth/Assets/WorldIcons/Confection";

        public override string IconSmall => "TheConfectionRebirth/Biomes/BestiaryIcon1";
    }
}
