using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth {
	public class ConfectionIDs {
		public class Sets {
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
				ModContent.TileType<CreamstoneMossHelium>()
			};

			/// <summary>
			/// The blacklists confection items from appearing in recipes for certain items.
			/// <br/>Example: SoulofLightOnlyItem, items inside this blacklist will ONLY be created with souls of light
			/// <br/>This is so modded (and vanilla) items cannot be crafted by confection items if specifically for the hallow
			/// </summary>
			public class RecipeBlacklist {
				public static bool[] SoulofLightOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.LightKey, ItemID.SoulBottleLight, ItemID.Chik, ItemID.Megashark, ItemID.LightDisc, ItemID.FairyBell, ItemID.RainbowRod, ItemID.AngelWings, ItemID.CrystalStorm, ItemID.DaoofPow);

				public static bool[] SoulofNightOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.NightKey, ItemID.SoulBottleNight, ItemID.DaoofPow, ItemID.CursedFlames, ItemID.FleshCloningVaat, ItemID.MechanicalWorm);

				public static bool[] DarkShardOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.DaoofPow);

				public static bool[] PixieDustOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.HolyWater, ItemID.HolyArrow, ItemID.FairyBell, ItemID.RainbowRod, ItemID.FairyWings);

				public static bool[] UnicornHornOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.HolyArrow, ItemID.RainbowRod);

				public static bool[] CrystalShardOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.CrystalDart, ItemID.CrystalBullet, ItemID.CrystalBlock, ItemID.Chik, ItemID.MagicalHarp, ItemID.RainbowRod, ItemID.ShiftingPearlSandsDye, ItemID.CrystalStorm);

				public static bool[] HallowedBarOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.AncientHallowedGreaves, ItemID.AncientHallowedPlateMail, ItemID.AncientHallowedHeadgear, ItemID.AncientHallowedHeadgear, ItemID.AncientHallowedHelmet, ItemID.AncientHallowedHood, ItemID.HallowedGreaves, ItemID.HallowedPlateMail, ItemID.HallowedHeadgear, ItemID.HallowedHeadgear, ItemID.HallowedHelmet, ItemID.HallowedHood, ItemID.Drax, ItemID.PickaxeAxe, ItemID.HallowedRepeater, ItemID.Excalibur, ItemID.Gungnir, ItemID.HallowJoustingLance, ItemID.SwordWhip, ItemID.SuperStarCannon, ItemID.OpticStaff, ItemID.LightDisc);

				public static bool[] PrincessFishOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.SeafoodDinner);

				public static bool[] PrismiteOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.SeafoodDinner);

				public static bool[] ChaosFishOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.SeafoodDinner);

				public static bool[] HallowedSeedsOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.HolyWater);

				public static bool[] PearlstoneOnlyItem = ItemID.Sets.Factory.CreateBoolSet(ItemID.HallowedTorch, ItemID.PearlstoneBrick, ItemID.PearlstoneEcho, ItemID.Hallow1Echo, ItemID.Hallow2Echo, ItemID.Hallow3Echo, ItemID.Hallow4Echo);
			}

			/// <summary>
			/// Used to add extra tiles to the Confection's worldgen, such as modded grass tiles converting to confection grass tiles upon hardmode
			/// <br/> Example:
			/// <br/> ConfectionIDs.Sets.ConvertsToConfection[Type] = TileID.Clay;
			/// <br/> This will turn the modded tile of choice into clay blocks if the confection's worldgen hits directly ontop of the modded tile
			/// </summary>
			public static int[] ConvertsToConfection = TileID.Sets.Factory.CreateIntSet(-1);

			/// <summary>
			/// Whether or not the tile type can grow saccharite off of it. 
			/// <br/> This does NOT include Saccharite itself as it has special generation for it growing Enchanted Saccharite and its general movement
			/// </summary>
			public static bool[] CanGrowSaccharite = TileID.Sets.Factory.CreateBoolSet();

			/// <summary>
			/// Whether or not the tile type has both biome sight and shares the same biomesight color
			/// </summary>
			public static bool[] ConfectionBiomeSight = TileID.Sets.Factory.CreateBoolSet();

			/// <summary>
			/// Whether or not the tile draws like a creamstone moss.
			/// <br/> The color contained is the color the moss will draw with.
			/// <br/> Set the Color's Alpha parameter to 0 if want it to glow.
			/// <br/> Doesn't react well with tiles that have a tile sheet different that a normal tiles that connects with dirt.
			/// <br/> Defaults to Null.
			/// </summary>
			public static Color?[] IsTileCreamMoss = TileID.Sets.Factory.CreateCustomSet<Color?>(null);
		}
	}
}
