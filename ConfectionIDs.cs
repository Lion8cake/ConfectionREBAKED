using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;
using Terraria.ID;
using TheConfectionRebirth.Items.Weapons.Minions.Gastropod;

namespace TheConfectionRebirth {
	public class ConfectionIDs
    {
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
				ModContent.TileType<ArgonCreamMoss>(),
				ModContent.TileType<BlueCreamMoss>(),
				ModContent.TileType<BrownCreamMoss>(),
				ModContent.TileType<GreenCreamMoss>(),
				ModContent.TileType<KryptonCreamMoss>(),
				ModContent.TileType<LavaCreamMoss>(),
				ModContent.TileType<PurpleCreamMoss>(),
				ModContent.TileType<RedCreamMoss>(),
				ModContent.TileType<XenomCreamMoss>() };
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
		}
    }
}
