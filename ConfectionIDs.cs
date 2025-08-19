using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth {
	public class ConfectionIDs {
		public static class Sets {
			public static List<int> ConfectCountCollection = new List<int> {
				ModContent.TileType<Creamstone>(),
				ModContent.TileType<CreamGrass>(),
				ModContent.TileType<CreamGrassMowed>(),
				ModContent.TileType<Creamsand>(),
				ModContent.TileType<BlueIce>(),
				ModContent.TileType<Creamsandstone>(),
				ModContent.TileType<HardenedCreamsand>(),
				ModContent.TileType<CookieBlock>(),
				ModContent.TileType<CreamBlock>(),
				ModContent.TileType<PinkFairyFloss>(),
				ModContent.TileType<PurpleFairyFloss>(),
				ModContent.TileType<BlueFairyFloss>(),
				ModContent.TileType<CreamstoneAmethyst>(),
				ModContent.TileType<CreamstoneSaphire>(),
				ModContent.TileType<CreamstoneTopaz>(),
				ModContent.TileType<CreamstoneRuby>(),
				ModContent.TileType<CreamstoneDiamond>(),
				ModContent.TileType<CreamstoneEmerald>(),
				ModContent.TileType<CreamstoneMossGreen>(),
				ModContent.TileType<CreamstoneMossBrown>(),
				ModContent.TileType<CreamstoneMossRed>(),
				ModContent.TileType<CreamstoneMossBlue>(),
				ModContent.TileType<CreamstoneMossPurple>(),
				ModContent.TileType<CreamstoneMossLava>(),
				ModContent.TileType<CreamstoneMossKrypton>(),
				ModContent.TileType<CreamstoneMossXenon>(),
				ModContent.TileType<CreamstoneMossArgon>(),
				ModContent.TileType<CreamstoneMossNeon>(),
				ModContent.TileType<CreamstoneMossHelium>(),
				ModContent.TileType<CreamVines>(),
				ModContent.TileType<CreamGrass_Foliage>()
			};

			/// <summary>
			/// Used for generating walls in the confection hardmode V
			/// </summary>
			public static bool[] Confection = TileID.Sets.Factory.CreateNamedSet("Confection")
				.Description("Used for generating walls in the confection hardmode V")
				.RegisterBoolSet();

			/// <summary>
			/// Whether a tile is a normal confection tile <br/>
			/// E.G. Creamstone, Creamgrass, etc
			/// </summary>
			public static bool[] IsNaturalConfectionTile = TileID.Sets.Factory.CreateNamedSet("IsNaturalConfectionTile")
				.Description("Whether a tile is a normal confection tile. E.G. Creamstone, Creamgrass, etc")
				.RegisterBoolSet();

			/// <summary>
			/// Whether a tile is an extra confection tile <br/>
			/// E.G. Cookie Block, Cream Block, etc
			/// </summary>
			public static bool[] IsExtraConfectionTile = TileID.Sets.Factory.CreateNamedSet("IsExtraConfectionTile")
				.Description("Whether a tile is an extra confection tile. E.G. Cookie Block, Cream Block, etc")
				.RegisterBoolSet();

			/// <summary>
			/// Whether a tile is a normal confection tile <br/>
			/// E.G. Creamstone Wall, Creamgrass Wall, etc
			/// </summary>
			public static bool[] IsNaturalConfectionWall = WallID.Sets.Factory.CreateNamedSet("IsNaturalConfectionWall")
				.Description("Whether a tile is a normal confection tile. E.G. Creamstone Wall, Creamgrass Wall, etc")
				.RegisterBoolSet();

			/// <summary>
			/// Whether a tile is an extra confection tile <br/>
			/// E.G. Cookie Wall, Cream Wall, etc
			/// </summary>
			public static bool[] IsExtraConfectionWall = WallID.Sets.Factory.CreateNamedSet("IsExtraConfectionWall")
				.Description("Whether a tile is an extra confection tile. E.G. Cookie Wall, Cream Wall, etc")
				.RegisterBoolSet();

			/// <summary>
			/// The blacklists confection items from appearing in recipes for certain items.
			/// <br/>Example: SoulofLightOnlyItem, items inside this blacklist will ONLY be created with souls of light
			/// <br/>This is so modded (and vanilla) items cannot be crafted by confection items if specifically for the hallow
			/// </summary>
			public class RecipeBlacklist {
				public static bool[] SoulofLightOnlyItem = ItemID.Sets.Factory.CreateNamedSet("SoulofLightOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.LightKey, ItemID.SoulBottleLight, ItemID.Chik, ItemID.Megashark, ItemID.LightDisc, ItemID.FairyBell, ItemID.RainbowRod, ItemID.AngelWings, ItemID.CrystalStorm, ItemID.DaoofPow);

				public static bool[] SoulofNightOnlyItem = ItemID.Sets.Factory.CreateNamedSet("SoulofNightOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.NightKey, ItemID.SoulBottleNight, ItemID.DaoofPow, ItemID.CursedFlames, ItemID.FleshCloningVaat, ItemID.MechanicalWorm);

				public static bool[] DarkShardOnlyItem = ItemID.Sets.Factory.CreateNamedSet("DarkShardOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.DaoofPow);

				public static bool[] PixieDustOnlyItem = ItemID.Sets.Factory.CreateNamedSet("PixieDustOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.HolyWater, ItemID.HolyArrow, ItemID.FairyBell, ItemID.RainbowRod, ItemID.FairyWings);

				public static bool[] UnicornHornOnlyItem = ItemID.Sets.Factory.CreateNamedSet("UnicornHornOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.HolyArrow, ItemID.RainbowRod);

				public static bool[] CrystalShardOnlyItem = ItemID.Sets.Factory.CreateNamedSet("CrystalShardOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.CrystalDart, ItemID.CrystalBullet, ItemID.CrystalBlock, ItemID.Chik, ItemID.MagicalHarp, ItemID.RainbowRod, ItemID.ShiftingPearlSandsDye, ItemID.CrystalStorm);

				public static bool[] HallowedBarOnlyItem = ItemID.Sets.Factory.CreateNamedSet("HallowedBarOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.AncientHallowedGreaves, ItemID.AncientHallowedPlateMail, ItemID.AncientHallowedHeadgear, ItemID.AncientHallowedHeadgear, ItemID.AncientHallowedHelmet, ItemID.AncientHallowedHood, ItemID.HallowedGreaves, ItemID.HallowedPlateMail, ItemID.HallowedHeadgear, ItemID.HallowedMask, ItemID.HallowedHelmet, ItemID.HallowedHood, ItemID.Drax, ItemID.PickaxeAxe, ItemID.HallowedRepeater, ItemID.Excalibur, ItemID.Gungnir, ItemID.HallowJoustingLance, ItemID.SwordWhip, ItemID.SuperStarCannon, ItemID.OpticStaff, ItemID.LightDisc);

				public static bool[] PrincessFishOnlyItem = ItemID.Sets.Factory.CreateNamedSet("PrincessFishOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.SeafoodDinner);

				public static bool[] PrismiteOnlyItem = ItemID.Sets.Factory.CreateNamedSet("PrismiteOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.SeafoodDinner);

				public static bool[] ChaosFishOnlyItem = ItemID.Sets.Factory.CreateNamedSet("ChaosFishOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.SeafoodDinner);

				public static bool[] HallowedSeedsOnlyItem = ItemID.Sets.Factory.CreateNamedSet("HallowedSeedsOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.HolyWater);

				public static bool[] PearlstoneOnlyItem = ItemID.Sets.Factory.CreateNamedSet("PearlstoneOnlyItem").Description("The blacklists confection items from appearing in recipes for certain items.").RegisterBoolSet(ItemID.HallowedTorch, ItemID.PearlstoneBrick, ItemID.PearlstoneEcho, ItemID.Hallow1Echo, ItemID.Hallow2Echo, ItemID.Hallow3Echo, ItemID.Hallow4Echo);
			}

			/// <summary>
			/// Used to add extra tiles to the Confection's worldgen, such as modded grass tiles converting to confection grass tiles upon hardmode
			/// <br/> Example:
			/// <br/> ConfectionIDs.Sets.ConvertsToConfection[Type] = TileID.Clay;
			/// <br/> This will turn the modded tile of choice into clay blocks if the confection's worldgen hits directly ontop of the modded tile
			/// </summary>
			public static int[] ConvertsToConfection = TileID.Sets.Factory.CreateNamedSet("ConvertsToConfection")
				.Description("Used to add extra tiles to the Confection's worldgen, such as modded grass tiles converting to confection grass tiles upon hardmode")
				.RegisterIntSet();

			/// <summary>
			/// Whether or not the tile type can grow saccharite off of it. 
			/// <br/> This does NOT include Saccharite itself as it has special generation for it growing Enchanted Saccharite and its general movement
			/// </summary>
			public static bool[] CanGrowSaccharite = TileID.Sets.Factory.CreateNamedSet("CanGrowSaccharite")
				.Description("Whether or not the tile type can grow saccharite off of it.")
				.RegisterBoolSet();

			/// <summary>
			/// Whether or not the tile type has both biome sight and shares the same biomesight color
			/// </summary>
			public static bool[] ConfectionBiomeSight = TileID.Sets.Factory.CreateNamedSet("ConfectionBiomeSight")
				.Description("Whether or not the tile type has both biome sight and shares the same biomesight color")
				.RegisterBoolSet();

			/// <summary>
			/// Whether or not the tile draws like a creamstone moss.
			/// <br/> The color contained is the color the moss will draw with.
			/// <br/> Set the Color's Alpha parameter to 0 if want it to glow.
			/// <br/> Doesn't react well with tiles that have a tile sheet different that a normal tiles that connects with dirt.
			/// <br/> Defaults to Null.
			/// </summary>
			public static Color?[] IsTileCreamMoss = TileID.Sets.Factory.CreateNamedSet("IsTileCreamMoss")
				.Description("Whether or not the tile draws like a creamstone moss. The color contained is the color the moss will draw with. Defaults to Null.")
				.RegisterCustomSet<Color?>(null);

			/// <summary>
			/// Immunity for certain NPCs who shouldn't be effected by the Vanilla Valor defence ignoring capabilities.
			/// </summary>
			public static bool[] IsEnemyVanillaCritImmune = NPCID.Sets.Factory.CreateNamedSet("IsEnemyVanillaCritImmune")
				.Description("Immunity for certain NPCs who shouldn't be effected by the Vanilla Valor defence ignoring capabilities.")
				.RegisterBoolSet(NPCID.DungeonGuardian);
		}
	}
}
