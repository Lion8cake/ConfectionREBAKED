using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.ModSupport.Thorium.Items;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Items;
using Terraria.Localization;

namespace TheConfectionRebirth.ModSupport.Thorium
{
	[ExtendsFromMod("ThoriumMod")]
	internal class ThoriumItemCrafting : ModSystem
    {
		[JITWhenModsEnabled("ThoriumMod")]
		public override void AddRecipes()
        {
			/*?if (ModLoader.TryGetMod("ThoriumMod", out Mod mod)) {
				Recipe recipe = Recipe.Create(mod.Find<ModItem>("WhiteKnightLeggings").Type);
				recipe.AddIngredient(ModContent.ItemType<ConfectedCharm>(), 14)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe1 = Recipe.Create(mod.Find<ModItem>("WhiteKnightTabard").Type);
				recipe1.AddIngredient(ModContent.ItemType<ConfectedCharm>(), 18)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe2 = Recipe.Create(mod.Find<ModItem>("WhiteKnightMask").Type);
				recipe2.AddIngredient(ModContent.ItemType<ConfectedCharm>(), 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe3 = Recipe.Create(mod.Find<ModItem>("Zunpet").Type);
				recipe3.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddIngredient(ModContent.ItemType<ConfectedCharm>(), 10)
				.AddIngredient(ItemID.LightShard)
				.AddIngredient(ItemID.DarkShard)
				//.AddCondition(NetworkText.FromKey("Toggle Recipe in Config"), r => mod.Find<>(ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleZunpet))
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe4 = Recipe.Create(mod.Find<ModItem>("TunePlayerDamage").Type);
				recipe4.AddIngredient(mod.Find<ModItem>("MusicPlayerDamage"))
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 5)
				.AddIngredient(ItemID.Cog, 5)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
				Recipe recipe5 = Recipe.Create(mod.Find<ModItem>("TunePlayerDamageReduction").Type);
				recipe5.AddIngredient(mod.Find<ModItem>("MusicPlayerDamageReduction"))
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 5)
				.AddIngredient(ItemID.Cog, 5)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
				Recipe recipe6 = Recipe.Create(mod.Find<ModItem>("TunePlayerLifeRegen").Type);
				recipe6.AddIngredient(mod.Find<ModItem>("MusicPlayerLifeRegen"))
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 5)
				.AddIngredient(ItemID.Cog, 5)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
				Recipe recipe7 = Recipe.Create(mod.Find<ModItem>("TunePlayerMovementSpeed").Type);
				recipe7.AddIngredient(mod.Find<ModItem>("MusicPlayerMovementSpeed"))
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 5)
				.AddIngredient(ItemID.Cog, 5)
				.AddTile(TileID.TinkerersWorkbench)
				.Register(); 
				/*Recipe recipe8 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleBloodyPaganStaff, mod.Find<ModItem>("BloodyPaganStaff").Type, 1);
				recipe8.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 20)
				.AddIngredient(mod.Find<ModItem>("CursedCloth"), 20)
				.AddIngredient(ItemID.SunStone)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe9 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleEmperorsWill, mod.Find<ModItem>("EmperorsWill").Type, 1);
				recipe9.AddIngredient(ItemID.IllegalGunParts)
				.AddIngredient(ItemID.FragmentStardust, 12)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddIngredient(ItemID.CowboyHat)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
				Recipe recipe10 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.togglePlasmaStaff, mod.Find<ModItem>("PlasmaStaff").Type, 1);
				recipe10.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddIngredient(mod.Find<ModItem>("LifeCell"))
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe11 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.togglePortableWintergatan, mod.Find<ModItem>("PortableWintergatan").Type, 1);
				recipe11.AddIngredient(ModContent.ItemType<CreamWood>(), 30)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe12 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleRudeWand, mod.Find<ModItem>("RudeWand").Type, 1);
				recipe12.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddIngredient(ItemID.Nanites, 10)
				.AddIngredient(ItemID.Silk, 6)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe13 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleShockbuster, mod.Find<ModItem>("Shockbuster").Type, 1);
				recipe13.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddIngredient(ItemID.Cog, 10)
				.AddRecipeGroup(RecipeGroupID.Wood, 40)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe14 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleSmitingHammer, mod.Find<ModItem>("SmitingHammer").Type, 1);
				recipe14.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddIngredient(mod.Find<ModItem>("SoulofPlight"), 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe15 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleStaticProd, mod.Find<ModItem>("StaticProd").Type, 1);
				recipe15.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddIngredient(ItemID.Cog, 10)
				.AddRecipeGroup(RecipeGroupID.Wood, 20)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe16 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleValkyrieBlade, mod.Find<ModItem>("ValkyrieBlade").Type, 1);
				recipe16.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 14)
				.AddIngredient(ItemID.SoulofFright, 6)
				.AddIngredient(ItemID.SoulofSight, 6)
				.AddIngredient(ItemID.SoulofMight, 6)
				.AddIngredient(ItemID.SoulofNight, 6)
				.AddIngredient(ModContent.ItemType<SoulofDelight>(), 6)
				.AddTile(TileID.MythrilAnvil)
				.Register();*/
				/*Recipe recipe17 = Recipe.Create(mod.Find<ModItem>("AncientTome").Type);
				recipe17.AddIngredient(ItemID.SpellTome)
				.AddIngredient(mod.Find<ModItem>("SolarPebble"), 10)
				.AddIngredient(mod.Find<ModItem>("PurityShards"), 10)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe18 = Recipe.Create(mod.Find<ModItem>("MartyrChalice").Type);
				recipe18.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddIngredient(ItemID.Ectoplasm, 4)
				.AddIngredient(ModContent.ItemType<Saccharite>(), 15)
				.AddTile(TileID.Anvils)
				.Register();
				Recipe recipe19 = Recipe.Create(mod.Find<ModItem>("MindMelter").Type);
				recipe19.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 10)
				.AddIngredient(mod.Find<ModItem>("UnholyShards"), 12)
				.AddIngredient(ItemID.SoulofFright, 6)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe20 = Recipe.Create(mod.Find<ModItem>("WhisperingDagger").Type);
				recipe20.AddIngredient(mod.Find<ModItem>("VoidHeart"))
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 15)
				.AddIngredient(mod.Find<ModItem>("UnholyShards"), 15)
				.AddIngredient(ItemID.SoulofNight, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe21 = Recipe.Create(mod.Find<ModItem>("ArenaMastersBrazier").Type);
				recipe21.AddIngredient(ItemID.Campfire)
				.AddIngredient(ItemID.HeartLantern)
				.AddIngredient(ItemID.StarinaBottle)
				.AddIngredient(ItemID.Sunflower)
				.AddIngredient(ItemID.HoneyBucket, 2)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 2)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe22 = Recipe.Create(mod.Find<ModItem>("SoulForge").Type);
				recipe22.AddIngredient(ItemID.AdamantiteForge)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 8)
				.AddIngredient(ItemID.SoulofFright, 5)
				.AddIngredient(ItemID.SoulofSight, 5)
				.AddIngredient(ItemID.SoulofMight, 5)				
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe23 = Recipe.Create(mod.Find<ModItem>("SoulForge").Type);
				recipe23.AddIngredient(ItemID.TitaniumForge)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 8)
				.AddIngredient(ItemID.SoulofFright, 5)
				.AddIngredient(ItemID.SoulofSight, 5)
				.AddIngredient(ItemID.SoulofMight, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe24 = Recipe.Create(mod.Find<ModItem>("TerrariumCore").Type, 2);
				recipe24.AddRecipeGroup(ThoriumMod.ThoriumRecipes.AnyCopperBarGroup)
				.AddRecipeGroup(RecipeGroupID.IronBar)
				.AddRecipeGroup(ThoriumMod.ThoriumRecipes.AnySilverBarGroup)
				.AddRecipeGroup(ThoriumMod.ThoriumRecipes.AnyGoldBarGroup)
				.AddRecipeGroup(ThoriumMod.ThoriumRecipes.AnyDemoniteBarGroup)
				.AddIngredient(ItemID.MeteoriteBar)
				.AddIngredient(ItemID.HellstoneBar)
				.AddRecipeGroup(ThoriumMod.ThoriumRecipes.AnyCobaltBarGroup)
				.AddRecipeGroup(ThoriumMod.ThoriumRecipes.AnyMythrilBarGroup)
				.AddRecipeGroup(ThoriumMod.ThoriumRecipes.AnyAdamantiteBarGroup)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>())
				.AddIngredient(ItemID.ChlorophyteBar)
				.AddIngredient(ItemID.Ectoplasm)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
				Recipe recipe25 = Recipe.Create(mod.Find<ModItem>("RiftTearer").Type);
				recipe25.AddIngredient(mod.Find<ModItem>("VoidHeart"))
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 14)
				.AddIngredient(ItemID.SoulofNight, 8)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe26 = Recipe.Create(mod.Find<ModItem>("InspirationReachPotion").Type);
				recipe26.AddIngredient(ItemID.BottledWater)
				.AddIngredient(ItemID.Shiverthorn)
				.AddIngredient(ModContent.ItemType<Saccharite>())
				.AddTile(TileID.AlchemyTable)
				.Register();
				/*Recipe recipe27 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleLadysLight, mod.Find<ModItem>("LadyLight").Type, 1);
				recipe27.AddIngredient(ModContent.ItemType<Sprinkles>(), 25)
				.AddIngredient(ModContent.ItemType<Saccharite>(), 8)
				.AddIngredient(ModContent.ItemType<SoulofDelight>(), 4)
				.AddIngredient(ModContent.ItemType<CookieDough>(), 2)
				.AddTile(TileID.Anvils)
				.Register();
				Recipe recipe28 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleMastersLibram, mod.Find<ModItem>("MastersLibram").Type, 1);
				recipe28.AddIngredient(ItemID.SpellTome)
				.AddIngredient(ItemID.Ichor, 10)
				.AddIngredient(ModContent.ItemType<Saccharite>(), 12)
				.AddIngredient(ItemID.GuideVoodooDoll)
				.AddIngredient(ItemID.Bone, 10)
				.AddIngredient(ItemID.WaterCandle)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe29 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleMastersLibram, mod.Find<ModItem>("MastersLibram").Type, 1); 
				recipe29.AddIngredient(ItemID.SpellTome)
				.AddIngredient(ItemID.CursedFlame, 10)
				.AddIngredient(ModContent.ItemType<Saccharite>(), 12)
				.AddIngredient(ItemID.GuideVoodooDoll)
				.AddIngredient(ItemID.Bone, 10)
				.AddIngredient(ItemID.WaterCandle)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe30 = ThoriumMod.Items.ThoriumItem.GetDonatorRecipe(() => ThoriumMod.ThoriumConfigServer.Instance.donatorWeapons.toggleNebulasReflection, mod.Find<ModItem>("NebulaReflection").Type, 1);
				recipe30.AddIngredient(ItemID.Glass, 10)
				.AddIngredient(ModContent.ItemType<Saccharite>(), 12)
				.AddIngredient(ItemID.FragmentNebula, 16)
				.AddTile(TileID.LunarCraftingStation)
				.Register();*/
				/*Recipe recipe31 = Recipe.Create(mod.Find<ModItem>("MidnightStaff").Type);
				recipe31.AddIngredient(ItemID.SoulofNight, 10)
				.AddIngredient(ModContent.ItemType<Saccharite>(), 6)
				.AddIngredient(ItemID.DarkShard)
				.AddTile(TileID.Anvils)
				.Register();
				Recipe recipe32 = Recipe.Create(mod.Find<ModItem>("NullZoneStaff").Type);
				recipe32.AddRecipeGroup(ThoriumMod.ThoriumRecipes.AnySilverBarGroup, 10)
				.AddIngredient(mod.Find<ModItem>("PurityShards"), 5)
				.AddIngredient(ModContent.ItemType<Saccharite>(), 8)
				.AddIngredient(ItemID.LightShard)
				.AddTile(TileID.Anvils)
				.Register();
				Recipe recipe33 = Recipe.Create(mod.Find<ModItem>("CyanPhasesaber").Type);
				recipe33.AddIngredient(mod.Find<ModItem>("CyanPhaseblade"))
				.AddIngredient(ModContent.ItemType<Saccharite>(), 50)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe34 = Recipe.Create(mod.Find<ModItem>("PinkPhasesaber").Type);
				recipe34.AddIngredient(mod.Find<ModItem>("PinkPhaseblade"))
				.AddIngredient(ModContent.ItemType<Saccharite>(), 50)
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe35 = Recipe.Create(mod.Find<ModItem>("SorcerersMirror").Type);
				recipe35.AddIngredient(ItemID.MagicMirror)
				.AddIngredient(ModContent.ItemType<Saccharite>(), 12)
				.AddIngredient(ModContent.ItemType<Sprinkles>(), 30)
				.AddTile(mod.Find<ModTile>("SoulForge"))
				.Register();
				Recipe recipe36 = Recipe.Create(mod.Find<ModItem>("SorcerersMirror").Type);
				recipe36.AddIngredient(ItemID.IceMirror)
				.AddIngredient(ModContent.ItemType<Saccharite>(), 12)
				.AddIngredient(ModContent.ItemType<Sprinkles>(), 30)
				.AddTile(mod.Find<ModTile>("SoulForge"))
				.Register();
				Recipe recipe37 = Recipe.Create(mod.Find<ModItem>("FallingChandelier").Type, 10);
				recipe37.AddRecipeGroup(ThoriumMod.ThoriumRecipes.AnyGoldBarGroup, 4)
				.AddIngredient(ItemID.Torch, 4)
				.AddIngredient(ModContent.ItemType<Saccharite>())
				.AddTile(TileID.MythrilAnvil)
				.Register();
				Recipe recipe38 = Recipe.Create(mod.Find<ModItem>("AphrodisiacVial").Type, 150);
				recipe38.AddIngredient(ItemID.Bottle, 10)
				.AddIngredient(ModContent.ItemType<Saccharite>())
				.AddIngredient(mod.Find<ModItem>("LifeQuartz"))
				.AddTile(TileID.Anvils)
				.Register();
				Recipe recipe39 = Recipe.Create(mod.Find<ModItem>("CombustionFlask").Type, 150);
				recipe39.AddIngredient(ItemID.Bottle, 10)
				.AddIngredient(ModContent.ItemType<Saccharite>())
				.AddIngredient(ItemID.HellstoneBar, 2)
				.AddTile(TileID.Anvils)
				.Register();
				Recipe recipe40 = Recipe.Create(mod.Find<ModItem>("CorrosionBeaker").Type, 150);
				recipe40.AddIngredient(ItemID.Bottle, 10)
				.AddIngredient(ModContent.ItemType<Saccharite>())
				.AddIngredient(ItemID.SpiderFang)
				.AddTile(TileID.Anvils)
				.Register();
				/*Recipe recipe41 = Recipe.Create(mod.Find<ModItem>("CrystalBalloon").Type, 100);
				recipe41.AddIngredient(mod.Find<ModItem>("BenignBalloon"))
				.AddIngredient(ModContent.ItemType<Saccharite>(), 3)
				.AddTile(TileID.Anvils)
				.Register();*/
				/*Recipe recipe42 = Recipe.Create(mod.Find<ModItem>("GasContainer").Type, 150);
				recipe42.AddIngredient(ItemID.Bottle, 10)
				.AddIngredient(ModContent.ItemType<Saccharite>())
				.AddIngredient(ItemID.JungleSpores, 2)
				.AddTile(TileID.Anvils)
				.Register();
				Recipe recipe43 = Recipe.Create(mod.Find<ModItem>("NitrogenVial").Type, 150);
				recipe43.AddIngredient(ItemID.Bottle, 10)
				.AddIngredient(ModContent.ItemType<Saccharite>())
				.AddIngredient(mod.Find<ModItem>("IcyShard"), 3)
				.AddTile(TileID.Anvils)
				.Register();
				Recipe recipe44 = Recipe.Create(mod.Find<ModItem>("PlasmaVial").Type, 150);
				recipe44.AddIngredient(ItemID.Bottle, 10)
				.AddIngredient(ModContent.ItemType<Saccharite>())
				.AddIngredient(mod.Find<ModItem>("SolarPebble"), 2)
				.AddTile(TileID.Anvils)
				.Register();
				Recipe recipe45 = Recipe.Create(ItemID.SnowGlobe);
				recipe45.AddIngredient(ItemID.Glass, 10)
				.AddIngredient(ItemID.SnowBlock, 25)
				.AddIngredient(ModContent.ItemType<SoulofDelight>(), 2)
				.AddIngredient(ItemID.SoulofLight, 2)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			}*/
        }
    }
}