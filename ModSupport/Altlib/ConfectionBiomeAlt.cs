using AltLibrary.Common.AltBiomes;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Weapons;
using TheConfectionRebirth.Tiles;
using TheConfectionRebirth.NPCs;
using TheConfectionRebirth.Walls;
using TheConfectionRebirth.Items;
using Terraria.Localization;

namespace TheConfectionRebirth.ModSupport.Altlib
{
	[ExtendsFromMod("AltLibrary")]
	public class ConfectionBiomeAlt : AltBiome
	{
		public override string WorldIcon => "TheConfectionRebirth/Assets/WorldIcons/Confection";
		public override string IconSmall => "TheConfectionRebirth/Assets/WorldCreation/IconGoodConfection";
		public override IShoppingBiome Biome => ModContent.GetInstance<ConfectionBiome>();
		public override Color NameColor => Color.LightPink;
		public override LocalizedText DisplayName => Language.GetText(Language.GetTextValue("Mods.TheConfectionRebirth.HallowSelection.Confection.Title"));
		public override LocalizedText Description => Language.GetText(Language.GetTextValue("Mods.TheConfectionRebirth.HallowSelection.Confection.Description"));
		public override LocalizedText DryadTextDescriptor => Language.GetText(Language.GetTextValue("Mods.TheConfectionRebirth.DryadSpecialText.AltlibConfectionText"));
		public override void SetStaticDefaults()
		{
			BiomeType = AltLibrary.BiomeType.Hallow;

			AddTileConversion(ModContent.TileType<CreamGrass>(), TileID.Grass);
			AddTileConversion(ModContent.TileType<Creamstone>(), TileID.Stone);
			AddTileConversion(ModContent.TileType<Creamsand>(), TileID.Sand);
			AddTileConversion(ModContent.TileType<HardenedCreamsand>(), TileID.Sandstone);
			AddTileConversion(ModContent.TileType<Creamsandstone>(), TileID.HardenedSand);
			AddTileConversion(ModContent.TileType<BlueIce>(), TileID.IceBlock);
			
			if (ModContent.GetInstance<ConfectionServerConfig>().CookieSpread != "No Spread")
			{
				AddTileConversion(ModContent.TileType<CookieBlock>(), TileID.Dirt);
				AddTileConversion(ModContent.TileType<CreamBlock>(), TileID.SnowBlock);
				AddTileConversion(ModContent.TileType<CookiestCookieBlock>(), TileID.DirtiestBlock);
				AddTileConversion(ModContent.TileType<PinkFairyFloss>(), TileID.Cloud);
				AddTileConversion(ModContent.TileType<PurpleFairyFloss>(), TileID.RainCloud);
				AddTileConversion(ModContent.TileType<BlueFairyFloss>(), TileID.SnowCloud);
				AddTileConversion(ModContent.TileType<CreamstoneAmethyst>(), TileID.Amethyst);
				AddTileConversion(ModContent.TileType<CreamstoneTopaz>(), TileID.Topaz);
				AddTileConversion(ModContent.TileType<CreamstoneSaphire>(), TileID.Sapphire);
				AddTileConversion(ModContent.TileType<CreamstoneEmerald>(), TileID.Emerald);
				AddTileConversion(ModContent.TileType<CreamstoneRuby>(), TileID.Ruby);
				AddTileConversion(ModContent.TileType<CreamstoneDiamond>(), TileID.Diamond);
				AddTileConversion(ModContent.TileType<CreamstoneMossArgon>(), TileID.ArgonMoss);
				AddTileConversion(ModContent.TileType<CreamstoneMossBlue>(), TileID.BlueMoss);
				AddTileConversion(ModContent.TileType<CreamstoneMossBrown>(), TileID.BrownMoss);
				AddTileConversion(ModContent.TileType<CreamstoneMossGreen>(), TileID.GreenMoss);
				AddTileConversion(ModContent.TileType<CreamstoneMossHelium>(), TileID.RainbowMoss);
				AddTileConversion(ModContent.TileType<CreamstoneMossKrypton>(), TileID.KryptonMoss);
				AddTileConversion(ModContent.TileType<CreamstoneMossLava>(), TileID.LavaMoss);
				AddTileConversion(ModContent.TileType<CreamstoneMossNeon>(), TileID.VioletMoss);
				AddTileConversion(ModContent.TileType<CreamstoneMossPurple>(), TileID.PurpleMoss);
				AddTileConversion(ModContent.TileType<CreamstoneMossRed>(), TileID.RedMoss);
				AddTileConversion(ModContent.TileType<CreamstoneMossXenon>(), TileID.XenonMoss);
			}

			FountainTile = ModContent.TileType<ConfectionWaterFountain>();
			FountainTileStyle = 0;

			SeedType = ModContent.ItemType<Items.Placeable.CreamBeans>();

			BiomeChestItem = ModContent.ItemType<PopRocket>();
			BiomeChestTile = ModContent.TileType<ConfectionBiomeChestTile>();
			BiomeChestTileStyle = 1;
			BiomeKeyItem = ModContent.ItemType<Items.ConfectionBiomeKey>();

			MimicType = ModContent.NPCType<BigMimicConfection>();
			MimicKeyType = ModContent.ItemType<KeyofDelight>();
			HammerType = ModContent.ItemType<GrandSlammer>();
			//Ore is manually added
			MechDropItemType = ItemID.None;
			ModContent.GetInstance<HallowAltBiome>().MechDropItemType = ItemID.None;
			//keys
			ModContent.GetInstance<HallowAltBiome>().MimicKeyType = ItemID.LightKey;
			ModContent.GetInstance<CorruptionAltBiome>().MimicKeyType = ItemID.NightKey;
			ModContent.GetInstance<CrimsonAltBiome>().MimicKeyType = ModContent.ItemType<KeyofSpite>();

			AddWallConversions<Creamstone4Wall>(
				WallID.RocksUnsafe3
			);
			AddWallConversions<Creamstone3Wall>(
				WallID.Cave3Unsafe,
				WallID.RocksUnsafe2
			);
			AddWallConversions<Creamstone2Wall>(
				WallID.Cave4Unsafe,
				WallID.Cave5Unsafe,
				WallID.RocksUnsafe1
			);
			AddWallConversions<Creamstone5Wall>(
				WallID.Cave8Unsafe,
				WallID.RocksUnsafe4
			);
			AddWallConversions<CreamsandstoneWall>(
				WallID.Sandstone,
				WallID.CorruptSandstone,
				WallID.CrimsonSandstone,
				WallID.HallowSandstone
			);
			AddWallConversions<HardenedCreamsandWall>(
				WallID.HardenedSand,
				WallID.CorruptHardenedSand,
				WallID.CrimsonHardenedSand,
				WallID.HallowHardenedSand
			);
			AddWallConversions<CreamGrassWall>(
				WallID.GrassUnsafe,
				WallID.Grass,
				WallID.FlowerUnsafe,
				WallID.Flower
			);
			if (ModContent.GetInstance<ConfectionServerConfig>().CookieSpread != "No Spread")
			{
				AddWallConversions<CookieWall>(
					WallID.Dirt,
					WallID.DirtUnsafe
				);
				AddWallConversions<CookieStonedWall>(
					WallID.Cave6Unsafe
				);
				AddWallConversions<PinkFairyFlossWall>(
					WallID.Cloud
				);
				AddWallConversions<BlueIceWall>(
					WallID.IceUnsafe
				);
				AddWallConversions<CreamWall>(
					WallID.SnowWallUnsafe
				);
				AddWallConversions<BlueCreamyMossyWall>(
					WallID.Cave4Unsafe
				);
				AddWallConversions<BrownCreamyMossyWall>(
					WallID.Cave2Unsafe
				);
				AddWallConversions<GreenCreamyMossyWall>(
					WallID.CaveUnsafe
				);
				AddWallConversions<PurpleCreamyMossyWall>(
					WallID.Cave5Unsafe
				);
				AddWallConversions<RedCreamyMossyWall>(
					WallID.Cave3Unsafe
				);
				AddWallConversions<CreamstoneAmethystWall>(
					WallID.AmethystUnsafe
				);
				AddWallConversions<CreamstoneTopazWall>(
					WallID.TopazUnsafe
				);
				AddWallConversions<CreamstoneSapphireWall>(
					WallID.SapphireUnsafe
				);
				AddWallConversions<CreamstoneSapphireWall>(
					WallID.EmeraldUnsafe
				);
				AddWallConversions<CreamstoneRubyWall>(
					WallID.RubyUnsafe
				);
				AddWallConversions<CreamstoneDiamondWall>(
					WallID.DiamondUnsafe
				);
			}
		}
		public override AltMaterialContext MaterialContext
		{
			get
			{
				AltMaterialContext context = new();
				context.LightBar = ModContent.ItemType<Items.Placeable.NeapoliniteBar>();
				context.LightSword = ModContent.ItemType<Sucrosa>();
				context.TrueLightSword = ModContent.ItemType<TrueSucrosa>();
				context.LightResidue = ModContent.ItemType<SoulofDelight>();
				return context;
			}
		}
	}
}
