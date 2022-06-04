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
            MechDropItemType = ModContent.ItemType<Items.Placeable.NeapoliniteBar>();
            BiomeChestItem = ModContent.ItemType<PopRocket>();
            BiomeChestTile = ModContent.TileType<Tiles.ConfectionBiomeChestTile>();
            BiomeChestTileStyle = 1;
            BiomeMowedGrass = ModContent.TileType<CreamGrassMowed>();
            MimicKeyType = ModContent.ItemType<KeyofDelight>();
            MimicType = ModContent.NPCType<BigMimicConfection>();
            DisplayName.SetDefault("Confection");
            Description.SetDefault("A land of where every thing looks like candy but don't be decived this is just a distraction.");

            BakeTileChild(ModContent.TileType<CookieBlock>(), TileID.Dirt, new(true, true, true));
            BakeTileChild(ModContent.TileType<CreamBlock>(), TileID.SnowBlock, new(true, true, true));
            BakeTileChild(ModContent.TileType<PinkFairyFloss>(), TileID.Cloud, new(true, true, true));
            BakeTileChild(ModContent.TileType<PurpleFairyFloss>(), TileID.RainCloud, new(true, true, true));
            BakeTileChild(ModContent.TileType<BlueFairyFloss>(), TileID.SnowCloud, new(true, true, true));

            /*WallContext = new WallContext()
                .AddReplacement<Walls.CreamstoneWall>(28, 1, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 188, 189, 190, 191, 192, 193, 194, 195, 61, 185, 212, 213, 214, 215, 3, 200, 201, 202, 203, 83)
                .AddReplacement<Walls.CreamGrassWall>(63, 65, 66, 68, 69, 70, 81)
                .AddReplacement<Walls.CookieWall>(59, 171, 170, 196, 198, 199, 16, 2)
                .AddReplacement<Walls.CreamsandstoneWall>(216, 217, 218, 219)
                .AddReplacement<Walls.HardenedCreamsandWall>(197, 220, 221, 222)
                .AddReplacement(40, (ushort)ModContent.WallType<Walls.CreamWall>())
                .AddReplacement(71, (ushort)ModContent.WallType<Walls.BlueIceWall>())
                .AddReplacement(73, (ushort)ModContent.WallType<Walls.PinkFairyFlossWall>());*/
        }

        public override Dictionary<int, int> SpecialConversion => new()
        {
            [TileID.Dirt] = ModContent.TileType<Tiles.CookieBlock>(),
            [TileID.SnowBlock] = ModContent.TileType<Tiles.CreamBlock>(),
            [TileID.Cloud] = ModContent.TileType<Tiles.PinkFairyFloss>(),
            [TileID.RainCloud] = ModContent.TileType<Tiles.PurpleFairyFloss>(),
            [TileID.SnowCloud] = ModContent.TileType<Tiles.BlueFairyFloss>()
        };

        public override List<int> SpreadingTiles => new() { ModContent.TileType<CreamGrass>(), ModContent.TileType<Tiles.Creamstone>(), ModContent.TileType<Tiles.Creamsand>(), ModContent.TileType<BlueIce>(), ModContent.TileType<Tiles.Creamsandstone>(), ModContent.TileType<Tiles.HardenedCreamsand>() };

        public override string WorldIcon => "TheConfectionRebirth/Assets/WorldIcons/Confection";

        public override string IconSmall => "TheConfectionRebirth/Biomes/BestiaryIcon1";
    }
}
