using AltLibrary.Common.AltBiomes;
using AltLibrary;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TheConfectionRebirth.Tiles;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Weapons;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.NPCs;

namespace TheConfectionRebirth.Content
{
    internal class ConfectionBiome : AltBiome
    {		
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
			DisplayName.SetDefault("[c/d2c491:Confection]");
			Description.SetDefault("A land of where every thing looks like candy but don't be decived this is just a distraction.");
        }
		
		public override Dictionary<int, int> SpecialConversion => new Dictionary<int, int> {
			[TileID.Dirt] = ModContent.TileType<Tiles.CookieBlock>(),
			[TileID.SnowBlock] = ModContent.TileType<Tiles.CreamBlock>(),
 			[TileID.Cloud] = ModContent.TileType<Tiles.PinkFairyFloss>(),
			[TileID.RainCloud] = ModContent.TileType<Tiles.PurpleFairyFloss>(),
			[TileID.SnowCloud] = ModContent.TileType<Tiles.BlueFairyFloss>()
		};
		
		public override WallContext WallContext => new WallContext()
                .AddReplacement(28, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(1, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(48, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(49, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(50, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(51, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(52, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(53, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(54, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(55, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(56, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(57, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(58, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(188, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(189, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(190, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(191, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(192, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(193, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(194, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(195, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(61, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(185, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(212, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(213, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(214, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(215, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(3, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(200, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(201, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(202, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(203, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(83, (ushort)ModContent.WallType<Walls.CreamstoneWall>())
				.AddReplacement(63, (ushort)ModContent.WallType<Walls.CreamGrassWall>())
				.AddReplacement(65, (ushort)ModContent.WallType<Walls.CreamGrassWall>())
				.AddReplacement(66, (ushort)ModContent.WallType<Walls.CreamGrassWall>())
				.AddReplacement(68, (ushort)ModContent.WallType<Walls.CreamGrassWall>())
				.AddReplacement(69, (ushort)ModContent.WallType<Walls.CreamGrassWall>())
				.AddReplacement(70, (ushort)ModContent.WallType<Walls.CreamGrassWall>())
				.AddReplacement(81, (ushort)ModContent.WallType<Walls.CreamGrassWall>())
				.AddReplacement(59, (ushort)ModContent.WallType<Walls.CookieWall>())
				.AddReplacement(171, (ushort)ModContent.WallType<Walls.CookieWall>())
				.AddReplacement(170, (ushort)ModContent.WallType<Walls.CookieWall>())
				.AddReplacement(196, (ushort)ModContent.WallType<Walls.CookieWall>())
				.AddReplacement(198, (ushort)ModContent.WallType<Walls.CookieWall>())
				.AddReplacement(199, (ushort)ModContent.WallType<Walls.CookieWall>())
				.AddReplacement(16, (ushort)ModContent.WallType<Walls.CookieWall>())
				.AddReplacement(2, (ushort)ModContent.WallType<Walls.CookieWall>())
				.AddReplacement(216, (ushort)ModContent.WallType<Walls.CreamsandstoneWall>())
				.AddReplacement(217, (ushort)ModContent.WallType<Walls.CreamsandstoneWall>())
				.AddReplacement(218, (ushort)ModContent.WallType<Walls.CreamsandstoneWall>())
				.AddReplacement(219, (ushort)ModContent.WallType<Walls.CreamsandstoneWall>())
				.AddReplacement(197, (ushort)ModContent.WallType<Walls.HardenedCreamsandWall>())
				.AddReplacement(220, (ushort)ModContent.WallType<Walls.HardenedCreamsandWall>())
				.AddReplacement(221, (ushort)ModContent.WallType<Walls.HardenedCreamsandWall>())
				.AddReplacement(222, (ushort)ModContent.WallType<Walls.HardenedCreamsandWall>())
				.AddReplacement(40, (ushort)ModContent.WallType<Walls.CreamWall>())
				.AddReplacement(71, (ushort)ModContent.WallType<Walls.BlueIceWall>())
				.AddReplacement(73, (ushort)ModContent.WallType<Walls.PinkFairyFlossWall>());
		
        public override List<int> SpreadingTiles => new List<int> { ModContent.TileType<CreamGrass>(), ModContent.TileType<Tiles.Creamstone>(), ModContent.TileType<Tiles.Creamsand>(), ModContent.TileType<BlueIce>(), ModContent.TileType<Tiles.Creamsandstone>(), ModContent.TileType<Tiles.HardenedCreamsand>() };

        public override string WorldIcon => "TheConfectionRebirth/Assets/WorldIcons/Confection";
		
		public override string IconSmall => "TheConfectionRebirth/Biomes/BestiaryIcon1";
    }
}
