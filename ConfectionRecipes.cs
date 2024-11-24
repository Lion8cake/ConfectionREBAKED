using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Armor;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Items.Placeable.Furniture;
using TheConfectionRebirth.Items.Weapons;

namespace TheConfectionRebirth
{
	public class ConfectionRecipes : ModSystem
	{
		public override void PostAddRecipes()
		{
			for (int i = 0; i < Recipe.numRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];
				//if (recipe.HasResult(ItemID.NightsEdge) && recipe.HasIngredient(ItemID.BloodButcherer))
				//{
				//	recipe.ReplaceResult(ModContent.ItemType<DeathsRaze>());
				//}
				if (recipe.HasResult(ItemID.GoldenShower) && recipe.HasIngredient(ItemID.SoulofNight))
				{
					recipe.RemoveIngredient(ItemID.SoulofNight);
					recipe.AddIngredient(ModContent.ItemType<SoulofSpite>(), 15);
				}
				if (recipe.HasResult(4142) && recipe.HasIngredient(ItemID.SoulofNight))
				{
					recipe.RemoveIngredient(ItemID.SoulofNight);
					recipe.AddIngredient(ModContent.ItemType<SoulofSpite>(), 10);
				}
				if (recipe.HasResult(ItemID.MechanicalWorm) && recipe.HasIngredient(ItemID.SoulofNight) && recipe.HasIngredient(ItemID.Vertebrae))
				{
					recipe.RemoveIngredient(ItemID.SoulofNight);
					recipe.AddIngredient(ModContent.ItemType<SoulofSpite>(), 6);
				}

				if (recipe.HasResult(ItemID.Megashark))
				{
					recipe.AddIngredient(ItemID.SoulofLight, 15);
				}
				if (recipe.HasResult(ItemID.Flamethrower))
				{
					recipe.RemoveIngredient(ItemID.IronBar);
					recipe.AddIngredient(ItemID.HallowedBar, 12);
				}
			}
		}

		public override void AddRecipeGroups()
		{
			RecipeGroup.recipeGroups[RecipeGroupID.Wood].ValidItems.Add(ModContent.ItemType<CreamWood>());
			RecipeGroup.recipeGroups[RecipeGroupID.Fruit].ValidItems.Add(ModContent.ItemType<Cherimoya>());
			RecipeGroup.recipeGroups[RecipeGroupID.Fruit].ValidItems.Add(ModContent.ItemType<CocoaBeans>());
		}

		public override void AddRecipes()
		{
			#region Vanilla Item Recipes
			Recipe recipe = Recipe.Create(ItemID.GreaterHealingPotion, 3);
			recipe.AddIngredient(ModContent.ItemType<Sprinkles>(), 3);
			recipe.AddIngredient(ModContent.ItemType<Saccharite>());
			recipe.AddIngredient(ItemID.BottledWater, 3);
			recipe.AddTile(TileID.AlchemyTable);
			recipe.SortAfterFirstRecipesOf(ItemID.GreaterHealingPotion);
			recipe.Register();

			Recipe recipe2 = Recipe.Create(ItemID.HallowedBar);
			recipe2.AddIngredient(ModContent.ItemType<HallowedOre>(), 5);
			recipe2.AddTile(TileID.AdamantiteForge);
			recipe2.SortBeforeFirstRecipesOf(ItemID.HallowedMask);
			recipe2.Register();

			Recipe recipe3 = Recipe.Create(ItemID.TerraBlade);
			recipe3.AddIngredient(ItemID.TrueNightsEdge);
			recipe3.AddIngredient(ModContent.ItemType<TrueSucrosa>());
			recipe3.AddIngredient(ItemID.BrokenHeroSword);
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.SortAfterFirstRecipesOf(ItemID.TerraBlade);
			recipe3.Register();
			#endregion

			//Confection Recipes
			//Sugar Powder
			Recipe SugarPowder = Recipe.Create(ModContent.ItemType<SugarPowder>());
			SugarPowder.AddIngredient(ModContent.ItemType<YumDrop>());
			SugarPowder.AddTile(TileID.Bottles);
			SugarPowder.SortAfterFirstRecipesOf(ItemID.ViciousPowder);
			SugarPowder.Register();

			//Choco Rabbit Cupcake
			Recipe ChocoRabbitCupcake = Recipe.Create(ModContent.ItemType<ChocoRabbitCupcake>());
			ChocoRabbitCupcake.AddIngredient<ChocolateBunny>();
			ChocoRabbitCupcake.AddTile(TileID.CookingPots);
			ChocoRabbitCupcake.SortAfterFirstRecipesOf(ItemID.PrismaticPunch);
			ChocoRabbitCupcake.Register();

			//Birdnana Pop
			Recipe BirdnanaPops = Recipe.Create(ModContent.ItemType<BirdnanaPops>());
			BirdnanaPops.AddIngredient(ModContent.ItemType<Birdnana>());
			BirdnanaPops.AddTile(TileID.CookingPots);
			BirdnanaPops.SortAfter(ChocoRabbitCupcake);
			BirdnanaPops.Register();

			//Frog In A Pond
			Recipe FrogInAPond = Recipe.Create(ModContent.ItemType<FrogInAPond>());
			FrogInAPond.AddIngredient(ModContent.ItemType<ChocolateFrog>());
			FrogInAPond.AddTile(TileID.CookingPots);
			FrogInAPond.SortAfter(BirdnanaPops);
			FrogInAPond.Register();

			//Pip Kebob
			Recipe PipKebob = Recipe.Create(ModContent.ItemType<PipKebob>());
			PipKebob.AddIngredient(ModContent.ItemType<Pip>());
			PipKebob.AddTile(TileID.CookingPots);
			PipKebob.SortAfter(FrogInAPond);
			PipKebob.Register();

			//Amethyst Creamstone Block
			Recipe AmethystCreamstoneBlock = Recipe.Create(ModContent.ItemType<AmethystCreamstoneBlock>());
			AmethystCreamstoneBlock.AddIngredient(ItemID.Amethyst);
			AmethystCreamstoneBlock.AddIngredient(ModContent.ItemType<Creamstone>());
			AmethystCreamstoneBlock.AddCondition(Condition.InGraveyard);
			AmethystCreamstoneBlock.AddTile(TileID.HeavyWorkBench);
			AmethystCreamstoneBlock.SortAfterFirstRecipesOf(ItemID.AmberStoneBlock);
			AmethystCreamstoneBlock.Register();

			//Topaz Creamstone Block
			Recipe TopazCreamstoneBlock = Recipe.Create(ModContent.ItemType<TopazCreamstoneBlock>());
			TopazCreamstoneBlock.AddIngredient(ItemID.Topaz);
			TopazCreamstoneBlock.AddIngredient(ModContent.ItemType<Creamstone>());
			TopazCreamstoneBlock.AddCondition(Condition.InGraveyard);
			TopazCreamstoneBlock.AddTile(TileID.HeavyWorkBench);
			TopazCreamstoneBlock.SortAfter(AmethystCreamstoneBlock);
			TopazCreamstoneBlock.Register();

			//Sapphire Creamstone Block
			Recipe SaphireCreamstoneBlock = Recipe.Create(ModContent.ItemType<SaphireCreamstoneBlock>());
			SaphireCreamstoneBlock.AddIngredient(ItemID.Sapphire);
			SaphireCreamstoneBlock.AddIngredient(ModContent.ItemType<Creamstone>());
			SaphireCreamstoneBlock.AddCondition(Condition.InGraveyard);
			SaphireCreamstoneBlock.AddTile(TileID.HeavyWorkBench);
			SaphireCreamstoneBlock.SortAfter(TopazCreamstoneBlock);
			SaphireCreamstoneBlock.Register();

			//Emerald Creamstone Block
			Recipe EmeraldCreamstoneBlock = Recipe.Create(ModContent.ItemType<EmeraldCreamstoneBlock>());
			EmeraldCreamstoneBlock.AddIngredient(ItemID.Emerald);
			EmeraldCreamstoneBlock.AddIngredient(ModContent.ItemType<Creamstone>());
			EmeraldCreamstoneBlock.AddCondition(Condition.InGraveyard);
			EmeraldCreamstoneBlock.AddTile(TileID.HeavyWorkBench);
			EmeraldCreamstoneBlock.SortAfter(SaphireCreamstoneBlock);
			EmeraldCreamstoneBlock.Register();

			//Ruby Creamstone Block
			Recipe RubyCreamstoneBlock = Recipe.Create(ModContent.ItemType<RubyCreamstoneBlock>());
			RubyCreamstoneBlock.AddIngredient(ItemID.Ruby);
			RubyCreamstoneBlock.AddIngredient(ModContent.ItemType<Creamstone>());
			RubyCreamstoneBlock.AddCondition(Condition.InGraveyard);
			RubyCreamstoneBlock.AddTile(TileID.HeavyWorkBench);
			RubyCreamstoneBlock.SortAfter(EmeraldCreamstoneBlock);
			RubyCreamstoneBlock.Register();

			//Diamond Creamstone Block
			Recipe DiamondCreamstoneBlock = Recipe.Create(ModContent.ItemType<DiamondCreamstoneBlock>());
			DiamondCreamstoneBlock.AddIngredient(ItemID.Diamond);
			DiamondCreamstoneBlock.AddIngredient(ModContent.ItemType<Creamstone>());
			DiamondCreamstoneBlock.AddCondition(Condition.InGraveyard);
			DiamondCreamstoneBlock.AddTile(TileID.HeavyWorkBench);
			DiamondCreamstoneBlock.SortAfter(RubyCreamstoneBlock);
			DiamondCreamstoneBlock.Register();

			//Sugar Water
			Recipe SugarWater = Recipe.Create(ModContent.ItemType<SugarWater>(), 10);
			SugarWater.AddIngredient(ModContent.ItemType<Sprinkles>(), 3);
			SugarWater.AddIngredient(ModContent.ItemType<CreamBeans>());
			SugarWater.AddIngredient(ItemID.BottledWater, 10);
			SugarWater.SortAfterFirstRecipesOf(ItemID.BloodWater);
			SugarWater.Register();

			//Pink Fairy Floss Wall
			Recipe PinkFairyFlossWall = Recipe.Create(ModContent.ItemType<PinkFairyFlossWall>(), 4);
			PinkFairyFlossWall.AddIngredient(ModContent.ItemType<PinkFairyFloss>());
			PinkFairyFlossWall.AddTile(TileID.WorkBenches);
			PinkFairyFlossWall.SortAfterFirstRecipesOf(ItemID.CloudWall);
			PinkFairyFlossWall.Register();

			Recipe PinkFairyFlossWall2 = Recipe.Create(ModContent.ItemType<PinkFairyFloss>());
			PinkFairyFlossWall2.AddIngredient(ModContent.ItemType<PinkFairyFlossWall>(), 4);
			PinkFairyFlossWall2.AddTile(TileID.WorkBenches);
			PinkFairyFlossWall2.SortAfter(PinkFairyFlossWall);
			PinkFairyFlossWall2.Register();

			//Orange Ice Wall
			Recipe OrangeIceWall = Recipe.Create(ModContent.ItemType<OrangeIceWall>(), 4);
			OrangeIceWall.AddIngredient(ModContent.ItemType<OrangeIce>());
			OrangeIceWall.AddCondition(Condition.InGraveyard);
			OrangeIceWall.AddTile(TileID.WorkBenches);
			OrangeIceWall.SortAfterFirstRecipesOf(ItemID.IceEcho);
			OrangeIceWall.Register();

			Recipe OrangeIceWall2 = Recipe.Create(ModContent.ItemType<OrangeIce>());
			OrangeIceWall2.AddIngredient(ModContent.ItemType<OrangeIceWall>(), 4);
			OrangeIceWall2.AddCondition(Condition.InGraveyard);
			OrangeIceWall2.AddTile(TileID.WorkBenches);
			OrangeIceWall2.SortAfter(OrangeIceWall);
			OrangeIceWall2.Register();

			#region Echo Dirt Wall Crafting Group
			Recipe CookieWall = Recipe.Create(ModContent.ItemType<CookieWall>(), 4);
			CookieWall.AddIngredient(ModContent.ItemType<CookieBlock>());
			CookieWall.AddCondition(Condition.InGraveyard);
			CookieWall.AddTile(TileID.WorkBenches);
			CookieWall.SortAfterFirstRecipesOf(ItemID.Dirt4Echo);
;			CookieWall.Register();

			Recipe CookieWall2 = Recipe.Create(ModContent.ItemType<CookieBlock>());
			CookieWall2.AddIngredient(ModContent.ItemType<CookieWall>(), 4);
			CookieWall2.AddTile(TileID.WorkBenches);
			CookieWall2.SortAfter(CookieWall);
			CookieWall2.Register();

			//Cookie Stoned Wall
			Recipe CookieStonedWall = Recipe.Create(ModContent.ItemType<CookieStonedWall>(), 4);
			CookieStonedWall.AddIngredient(ModContent.ItemType<CookieBlock>());
			CookieStonedWall.AddCondition(Condition.InGraveyard);
			CookieStonedWall.AddTile(TileID.WorkBenches);
			CookieStonedWall.SortAfter(CookieWall2);
			CookieStonedWall.Register();

			Recipe CookieStonedWall2 = Recipe.Create(ModContent.ItemType<CookieBlock>());
			CookieStonedWall2.AddIngredient(ModContent.ItemType<CookieStonedWall>(), 4);
			CookieStonedWall2.AddTile(TileID.WorkBenches);
			CookieStonedWall2.SortAfter(CookieStonedWall);
			CookieStonedWall2.Register();
			#endregion

			#region Echo Stone Walls Crafting Group
			//Creamstone Wall
			Recipe CreamstoneWall = Recipe.Create(ModContent.ItemType<CreamstoneWall>(), 4);
			CreamstoneWall.AddIngredient(ModContent.ItemType<Creamstone>());
			CreamstoneWall.AddCondition(Condition.InGraveyard);
			CreamstoneWall.AddTile(TileID.WorkBenches);
			CreamstoneWall.SortAfterFirstRecipesOf(ItemID.Hallow4Echo);
			CreamstoneWall.Register();

			Recipe CreamstoneWall2 = Recipe.Create(ModContent.ItemType<Creamstone>());
			CreamstoneWall2.AddIngredient(ModContent.ItemType<CreamstoneWall>(), 4);
			CreamstoneWall2.AddTile(TileID.WorkBenches);
			CreamstoneWall2.SortAfter(CreamstoneWall);
			CreamstoneWall2.Register();

			//Cracked Confection Wall
			Recipe CrackedConfectionWall = Recipe.Create(ModContent.ItemType<CrackedConfectionWall>(), 4);
			CrackedConfectionWall.AddIngredient(ModContent.ItemType<Creamstone>());
			CrackedConfectionWall.AddCondition(Condition.InGraveyard);
			CrackedConfectionWall.AddTile(TileID.WorkBenches);
			CrackedConfectionWall.SortAfter(CreamstoneWall2);
			CrackedConfectionWall.Register();

			Recipe CrackedConfectionWall2 = Recipe.Create(ModContent.ItemType<Creamstone>());
			CrackedConfectionWall2.AddIngredient(ModContent.ItemType<CrackedConfectionWall>(), 4);
			CrackedConfectionWall2.AddTile(TileID.WorkBenches);
			CrackedConfectionWall2.SortAfter(CrackedConfectionWall);
			CrackedConfectionWall2.Register();

			//Confectionary Crystals Wall
			Recipe ConfectionaryCrystalsWall = Recipe.Create(ModContent.ItemType<ConfectionaryCrystalsWall>(), 4);
			ConfectionaryCrystalsWall.AddIngredient(ModContent.ItemType<Creamstone>());
			ConfectionaryCrystalsWall.AddCondition(Condition.InGraveyard);
			ConfectionaryCrystalsWall.AddTile(TileID.WorkBenches);
			ConfectionaryCrystalsWall.SortAfter(CrackedConfectionWall2);
			ConfectionaryCrystalsWall.Register();

			Recipe ConfectionaryCrystalsWall2 = Recipe.Create(ModContent.ItemType<Creamstone>());
			ConfectionaryCrystalsWall2.AddIngredient(ModContent.ItemType<ConfectionaryCrystalsWall>(), 4);
			ConfectionaryCrystalsWall2.AddTile(TileID.WorkBenches);
			ConfectionaryCrystalsWall2.SortAfter(ConfectionaryCrystalsWall);
			ConfectionaryCrystalsWall2.Register();

			//Lined Confection Gem Wall
			Recipe LinedConfectionGemWall = Recipe.Create(ModContent.ItemType<LinedConfectionGemWall>(), 4);
			LinedConfectionGemWall.AddIngredient(ModContent.ItemType<Creamstone>());
			LinedConfectionGemWall.AddCondition(Condition.InGraveyard);
			LinedConfectionGemWall.AddTile(TileID.WorkBenches);
			LinedConfectionGemWall.SortAfter(ConfectionaryCrystalsWall2);
			LinedConfectionGemWall.Register();

			Recipe LinedConfectionGemWall2 = Recipe.Create(ModContent.ItemType<Creamstone>());
			LinedConfectionGemWall2.AddIngredient(ModContent.ItemType<LinedConfectionGemWall>(), 4);
			LinedConfectionGemWall2.AddTile(TileID.WorkBenches);
			LinedConfectionGemWall2.SortAfter(LinedConfectionGemWall);
			LinedConfectionGemWall2.Register();

			//Melting Confection Wall
			Recipe MeltingConfectionWall = Recipe.Create(ModContent.ItemType<MeltingConfectionWall>(), 4);
			MeltingConfectionWall.AddIngredient(ModContent.ItemType<Creamstone>());
			MeltingConfectionWall.AddCondition(Condition.InGraveyard);
			MeltingConfectionWall.AddTile(TileID.WorkBenches);
			MeltingConfectionWall.SortAfter(LinedConfectionGemWall2);
			MeltingConfectionWall.Register();

			Recipe MeltingConfectionWall2 = Recipe.Create(ModContent.ItemType<Creamstone>());
			MeltingConfectionWall2.AddIngredient(ModContent.ItemType<MeltingConfectionWall>(), 4);
			MeltingConfectionWall2.AddTile(TileID.WorkBenches);
			MeltingConfectionWall2.SortAfter(MeltingConfectionWall);
			MeltingConfectionWall2.Register();
			#endregion

			//Cream Wall
			Recipe CreamWall = Recipe.Create(ModContent.ItemType<CreamWall>(), 4);
			CreamWall.AddIngredient(ModContent.ItemType<CreamBlock>());
			CreamWall.AddCondition(Condition.InGraveyard);
			CreamWall.AddTile(TileID.WorkBenches);
			CreamWall.SortAfterFirstRecipesOf(ItemID.SnowWallEcho);
			CreamWall.Register();

			Recipe CreamWall2 = Recipe.Create(ModContent.ItemType<CreamBlock>());
			CreamWall2.AddIngredient(ModContent.ItemType<CreamWall>(), 4);
			CreamWall2.AddTile(TileID.WorkBenches);
			CreamWall2.SortAfter(CreamWall);
			CreamWall2.Register();

			#region Echo Sand Walls Crafting Group
			//Hardened Creamsand Wall
			Recipe CreamsandstoneWall = Recipe.Create(ModContent.ItemType<CreamsandstoneWall>(), 4);
			CreamsandstoneWall.AddIngredient(ModContent.ItemType<Creamsandstone>());
			CreamsandstoneWall.AddCondition(Condition.InGraveyard);
			CreamsandstoneWall.AddTile(TileID.WorkBenches);
			CreamsandstoneWall.SortAfterFirstRecipesOf(ItemID.HallowSandstoneWall);
			CreamsandstoneWall.Register();

			Recipe CreamsandstoneWall2 = Recipe.Create(ModContent.ItemType<Creamsandstone>());
			CreamsandstoneWall2.AddIngredient(ModContent.ItemType<CreamsandstoneWall>(), 4);
			CreamsandstoneWall2.AddTile(TileID.WorkBenches);
			CreamsandstoneWall2.SortAfter(CreamsandstoneWall);
			CreamsandstoneWall2.Register();

			//Creamsandstone Wall (yeah the names are reversed, its confused, I blame 15yr me)
			Recipe HardenedCreamsandWall = Recipe.Create(ModContent.ItemType<HardenedCreamsandWall>(), 4);
			HardenedCreamsandWall.AddIngredient(ModContent.ItemType<HardenedCreamsand>());
			HardenedCreamsandWall.AddCondition(Condition.InGraveyard);
			HardenedCreamsandWall.AddTile(TileID.WorkBenches);
			HardenedCreamsandWall.SortAfter(CreamsandstoneWall2);
			HardenedCreamsandWall.Register();

			Recipe HardenedCreamsandWall2 = Recipe.Create(ModContent.ItemType<HardenedCreamsand>());
			HardenedCreamsandWall2.AddIngredient(ModContent.ItemType<HardenedCreamsandWall>(), 4);
			HardenedCreamsandWall2.AddTile(TileID.WorkBenches);
			HardenedCreamsandWall2.SortAfter(HardenedCreamsandWall);
			HardenedCreamsandWall2.Register();
			#endregion

			#region Echo Creamy Moss Walls Crafting Group
			//Green Creamy Mossy Wall
			Recipe GreenCreamyMossyWall = Recipe.Create(ModContent.ItemType<GreenCreamyMossyWall>(), 4);
			GreenCreamyMossyWall.AddIngredient(ItemID.GreenMoss);
			GreenCreamyMossyWall.AddIngredient(ModContent.ItemType<Creamstone>());
			GreenCreamyMossyWall.AddCondition(Condition.InGraveyard);
			GreenCreamyMossyWall.AddTile(TileID.WorkBenches);
			GreenCreamyMossyWall.SortAfterFirstRecipesOf(ItemID.Cave5Echo);
			GreenCreamyMossyWall.Register();

			Recipe GreenCreamyMossyWall2 = Recipe.Create(ItemID.GreenMoss);
			GreenCreamyMossyWall2.AddIngredient(ModContent.ItemType<GreenCreamyMossyWall>(), 4);
			GreenCreamyMossyWall2.AddTile(TileID.WorkBenches);
			GreenCreamyMossyWall2.SortAfter(GreenCreamyMossyWall);
			GreenCreamyMossyWall2.Register();

			//Brown Creamy Mossy Wall
			Recipe BrownCreamyMossyWall = Recipe.Create(ModContent.ItemType<BrownCreamyMossyWall>(), 4);
			BrownCreamyMossyWall.AddIngredient(ItemID.BrownMoss);
			BrownCreamyMossyWall.AddIngredient(ModContent.ItemType<Creamstone>());
			BrownCreamyMossyWall.AddCondition(Condition.InGraveyard);
			BrownCreamyMossyWall.AddTile(TileID.WorkBenches);
			BrownCreamyMossyWall.SortAfter(GreenCreamyMossyWall2);
			BrownCreamyMossyWall.Register();

			Recipe BrownCreamyMossyWall2 = Recipe.Create(ItemID.BrownMoss);
			BrownCreamyMossyWall2.AddIngredient(ModContent.ItemType<BrownCreamyMossyWall>(), 4);
			BrownCreamyMossyWall2.AddTile(TileID.WorkBenches);
			BrownCreamyMossyWall2.SortAfter(BrownCreamyMossyWall);
			BrownCreamyMossyWall2.Register();

			//Red Creamy Mossy Wall
			Recipe RedCreamyMossyWall = Recipe.Create(ModContent.ItemType<RedCreamyMossyWall>(), 4);
			RedCreamyMossyWall.AddIngredient(ItemID.RedMoss);
			RedCreamyMossyWall.AddIngredient(ModContent.ItemType<Creamstone>());
			RedCreamyMossyWall.AddCondition(Condition.InGraveyard);
			RedCreamyMossyWall.AddTile(TileID.WorkBenches);
			RedCreamyMossyWall.SortAfter(BrownCreamyMossyWall2);
			RedCreamyMossyWall.Register();

			Recipe RedCreamyMossyWall2 = Recipe.Create(ItemID.RedMoss);
			RedCreamyMossyWall2.AddIngredient(ModContent.ItemType<RedCreamyMossyWall>(), 4);
			RedCreamyMossyWall2.AddTile(TileID.WorkBenches);
			RedCreamyMossyWall2.SortAfter(RedCreamyMossyWall);
			RedCreamyMossyWall2.Register();

			//Blue Creamy Mossy Wall
			Recipe BlueCreamyMossyWall = Recipe.Create(ModContent.ItemType<BlueCreamyMossyWall>(), 4);
			BlueCreamyMossyWall.AddIngredient(ItemID.BlueMoss);
			BlueCreamyMossyWall.AddIngredient(ModContent.ItemType<Creamstone>());
			BlueCreamyMossyWall.AddCondition(Condition.InGraveyard);
			BlueCreamyMossyWall.AddTile(TileID.WorkBenches);
			BlueCreamyMossyWall.SortAfter(RedCreamyMossyWall2);
			BlueCreamyMossyWall.Register();

			Recipe BlueCreamyMossyWall2 = Recipe.Create(ItemID.BlueMoss);
			BlueCreamyMossyWall2.AddIngredient(ModContent.ItemType<BlueCreamyMossyWall>(), 4);
			BlueCreamyMossyWall2.AddTile(TileID.WorkBenches);
			BlueCreamyMossyWall2.SortAfter(BlueCreamyMossyWall);
			BlueCreamyMossyWall2.Register();

			//Purple Creamy Mossy Wall
			Recipe PurpleCreamyMossyWall = Recipe.Create(ModContent.ItemType<PurpleCreamyMossyWall>(), 4);
			PurpleCreamyMossyWall.AddIngredient(ItemID.PurpleMoss);
			PurpleCreamyMossyWall.AddIngredient(ModContent.ItemType<Creamstone>());
			PurpleCreamyMossyWall.AddCondition(Condition.InGraveyard);
			PurpleCreamyMossyWall.AddTile(TileID.WorkBenches);
			PurpleCreamyMossyWall.SortAfter(BlueCreamyMossyWall2);
			PurpleCreamyMossyWall.Register();

			Recipe PurpleCreamyMossyWall2 = Recipe.Create(ItemID.PurpleMoss);
			PurpleCreamyMossyWall2.AddIngredient(ModContent.ItemType<PurpleCreamyMossyWall>(), 4);
			PurpleCreamyMossyWall2.AddTile(TileID.WorkBenches);
			PurpleCreamyMossyWall2.SortAfter(PurpleCreamyMossyWall);
			PurpleCreamyMossyWall2.Register();
			#endregion

			#region Echo Gem Creamstone Wall Crafting Group
			//Amethyst Creamstone Wall
			Recipe AmethystCreamstoneWall = Recipe.Create(ModContent.ItemType<AmethystCreamstoneWall>(), 4);
			AmethystCreamstoneWall.AddIngredient(ModContent.ItemType<AmethystCreamstoneBlock>());
			AmethystCreamstoneWall.AddCondition(Condition.InGraveyard);
			AmethystCreamstoneWall.AddTile(TileID.WorkBenches);
			AmethystCreamstoneWall.SortAfterFirstRecipesOf(ItemID.AmberStoneWallEcho);
			AmethystCreamstoneWall.Register();

			Recipe AmethystCreamstoneWall2 = Recipe.Create(ModContent.ItemType<AmethystCreamstoneBlock>());
			AmethystCreamstoneWall2.AddIngredient(ModContent.ItemType<AmethystCreamstoneWall>(), 4);
			AmethystCreamstoneWall2.AddTile(TileID.WorkBenches);
			AmethystCreamstoneWall2.SortAfter(AmethystCreamstoneWall);
			AmethystCreamstoneWall2.Register();

			//Topaz Creamstone Wall
			Recipe TopazCreamstoneWall = Recipe.Create(ModContent.ItemType<TopazCreamstoneWall>(), 4);
			TopazCreamstoneWall.AddIngredient(ModContent.ItemType<TopazCreamstoneBlock>());
			TopazCreamstoneWall.AddCondition(Condition.InGraveyard);
			TopazCreamstoneWall.AddTile(TileID.WorkBenches);
			TopazCreamstoneWall.SortAfter(AmethystCreamstoneWall2);
			TopazCreamstoneWall.Register();

			Recipe TopazCreamstoneWall2 = Recipe.Create(ModContent.ItemType<TopazCreamstoneBlock>());
			TopazCreamstoneWall2.AddIngredient(ModContent.ItemType<TopazCreamstoneWall>(), 4);
			TopazCreamstoneWall2.AddTile(TileID.WorkBenches);
			TopazCreamstoneWall2.SortAfter(TopazCreamstoneWall);
			TopazCreamstoneWall2.Register();

			//Sapphire Creamstone Wall
			Recipe SapphireCreamstoneWall = Recipe.Create(ModContent.ItemType<SapphireCreamstoneWall>(), 4);
			SapphireCreamstoneWall.AddIngredient(ModContent.ItemType<SaphireCreamstoneBlock>());
			SapphireCreamstoneWall.AddCondition(Condition.InGraveyard);
			SapphireCreamstoneWall.AddTile(TileID.WorkBenches);
			SapphireCreamstoneWall.SortAfter(TopazCreamstoneWall2);
			SapphireCreamstoneWall.Register();

			Recipe SapphireCreamstoneWall2 = Recipe.Create(ModContent.ItemType<SaphireCreamstoneBlock>());
			SapphireCreamstoneWall2.AddIngredient(ModContent.ItemType<SapphireCreamstoneWall>(), 4);
			SapphireCreamstoneWall2.AddTile(TileID.WorkBenches);
			SapphireCreamstoneWall2.SortAfter(SapphireCreamstoneWall);
			SapphireCreamstoneWall2.Register();

			//Emerald Creamstone Wall
			Recipe EmeraldCreamstoneWall = Recipe.Create(ModContent.ItemType<EmeraldCreamstoneWall>(), 4);
			EmeraldCreamstoneWall.AddIngredient(ModContent.ItemType<EmeraldCreamstoneBlock>());
			EmeraldCreamstoneWall.AddCondition(Condition.InGraveyard);
			EmeraldCreamstoneWall.AddTile(TileID.WorkBenches);
			EmeraldCreamstoneWall.SortAfter(SapphireCreamstoneWall2);
			EmeraldCreamstoneWall.Register();

			Recipe EmeraldCreamstoneWall2 = Recipe.Create(ModContent.ItemType<EmeraldCreamstoneBlock>());
			EmeraldCreamstoneWall2.AddIngredient(ModContent.ItemType<EmeraldCreamstoneWall>(), 4);
			EmeraldCreamstoneWall2.AddTile(TileID.WorkBenches);
			EmeraldCreamstoneWall2.SortAfter(EmeraldCreamstoneWall);
			EmeraldCreamstoneWall2.Register();

			//Ruby Creamstone Wall
			Recipe RubyCreamstoneWall = Recipe.Create(ModContent.ItemType<RubyCreamstoneWall>(), 4);
			RubyCreamstoneWall.AddIngredient(ModContent.ItemType<RubyCreamstoneBlock>());
			RubyCreamstoneWall.AddCondition(Condition.InGraveyard);
			RubyCreamstoneWall.AddTile(TileID.WorkBenches);
			RubyCreamstoneWall.SortAfter(EmeraldCreamstoneWall2);
			RubyCreamstoneWall.Register();

			Recipe RubyCreamstoneWall2 = Recipe.Create(ModContent.ItemType<RubyCreamstoneBlock>());
			RubyCreamstoneWall2.AddIngredient(ModContent.ItemType<RubyCreamstoneWall>(), 4);
			RubyCreamstoneWall2.AddTile(TileID.WorkBenches);
			RubyCreamstoneWall2.SortAfter(RubyCreamstoneWall);
			RubyCreamstoneWall2.Register();

			//Diamond Creamstone Wall
			Recipe DiamondCreamstoneWall = Recipe.Create(ModContent.ItemType<DiamondCreamstoneWall>(), 4);
			DiamondCreamstoneWall.AddIngredient(ModContent.ItemType<DiamondCreamstoneBlock>());
			DiamondCreamstoneWall.AddCondition(Condition.InGraveyard);
			DiamondCreamstoneWall.AddTile(TileID.WorkBenches);
			DiamondCreamstoneWall.SortAfter(RubyCreamstoneWall2);
			DiamondCreamstoneWall.Register();

			Recipe DiamondCreamstoneWall2 = Recipe.Create(ModContent.ItemType<DiamondCreamstoneBlock>());
			DiamondCreamstoneWall2.AddIngredient(ModContent.ItemType<DiamondCreamstoneWall>(), 4);
			DiamondCreamstoneWall2.AddTile(TileID.WorkBenches);
			DiamondCreamstoneWall2.SortAfter(DiamondCreamstoneWall);
			DiamondCreamstoneWall2.Register();
			#endregion

			#region Ore Bricks Crafting Group
			//hallowed ore bricks
			Recipe HallowedBrick = Recipe.Create(ModContent.ItemType<HallowedBrick>(), 5);
			HallowedBrick.AddIngredient(ItemID.StoneBlock, 5);
			HallowedBrick.AddIngredient<HallowedOre>();
			HallowedBrick.AddTile(TileID.Furnaces);
			HallowedBrick.SortAfterFirstRecipesOf(ItemID.TitanstoneBlockWall);
			HallowedBrick.Register();

			//Hallowed ore brick wall
			Recipe HallowedBrickWall = Recipe.Create(ModContent.ItemType<HallowedBrickWall>(), 4);
			HallowedBrickWall.AddIngredient(ModContent.ItemType<HallowedBrick>());
			HallowedBrickWall.AddTile(TileID.WorkBenches);
			HallowedBrickWall.SortAfter(HallowedBrick);
			HallowedBrickWall.Register();

			Recipe HallowedBrickWall2 = Recipe.Create(ModContent.ItemType<HallowedBrick>());
			HallowedBrickWall2.AddIngredient(ModContent.ItemType<HallowedBrickWall>(), 4);
			HallowedBrickWall2.AddTile(TileID.WorkBenches);
			HallowedBrickWall2.SortAfter(HallowedBrickWall);
			HallowedBrickWall2.Register();

			//Neapolinite ore Bricks
			Recipe NeapoliniteBrick = Recipe.Create(ModContent.ItemType<NeapoliniteBrick>(), 5);
			NeapoliniteBrick.AddIngredient(ItemID.StoneBlock, 5);
			NeapoliniteBrick.AddIngredient<NeapoliniteOre>();
			NeapoliniteBrick.AddTile(TileID.Furnaces);
			NeapoliniteBrick.SortAfter(HallowedBrickWall2);
			NeapoliniteBrick.Register();

			//Neapolinite ore Brick Wall
			Recipe NeapoliniteBrickWall = Recipe.Create(ModContent.ItemType<NeapoliniteBrickWall>(), 4);
			NeapoliniteBrickWall.AddIngredient(ModContent.ItemType<NeapoliniteBrick>());
			NeapoliniteBrickWall.AddTile(TileID.WorkBenches);
			NeapoliniteBrickWall.SortAfter(NeapoliniteBrick);
			NeapoliniteBrickWall.Register();

			Recipe NeapoliniteBrickWall2 = Recipe.Create(ModContent.ItemType<NeapoliniteBrick>());
			NeapoliniteBrickWall2.AddIngredient(ModContent.ItemType<NeapoliniteBrickWall>(), 4);
			NeapoliniteBrickWall2.AddTile(TileID.WorkBenches);
			NeapoliniteBrickWall2.SortAfter(NeapoliniteBrickWall);
			NeapoliniteBrickWall2.Register();
			#endregion

			//Shell Wall
			Recipe ShellBlockWall = Recipe.Create(ModContent.ItemType<ShellBlockWall>(), 4);
			ShellBlockWall.AddIngredient(ModContent.ItemType<ShellBlock>());
			ShellBlockWall.AddTile(TileID.WorkBenches);
			ShellBlockWall.SortAfterFirstRecipesOf(ItemID.CrystalBlockWall);
			ShellBlockWall.Register();

			Recipe ShellBlockWall2 = Recipe.Create(ModContent.ItemType<ShellBlock>());
			ShellBlockWall2.AddIngredient(ModContent.ItemType<ShellBlockWall>(), 4);
			ShellBlockWall2.AddTile(TileID.WorkBenches);
			ShellBlockWall2.SortAfter(ShellBlockWall);
			ShellBlockWall2.Register();

			//Pastry Wall
			Recipe PastryBlockWall = Recipe.Create(ModContent.ItemType<PastryBlockWall>(), 4);
			PastryBlockWall.AddIngredient(ModContent.ItemType<PastryBlock>());
			PastryBlockWall.AddTile(TileID.WorkBenches);
			PastryBlockWall.SortAfter(ShellBlockWall2);
			PastryBlockWall.Register();

			Recipe PastryBlockWall2 = Recipe.Create(ModContent.ItemType<PastryBlock>());
			PastryBlockWall2.AddIngredient(ModContent.ItemType<PastryBlockWall>(), 4);
			PastryBlockWall2.AddTile(TileID.WorkBenches);
			PastryBlockWall2.SortAfter(PastryBlockWall);
			PastryBlockWall2.Register();

			#region Neapolinite Creafting Group
			//Neapolinite Bar
			Recipe NeapoliniteBar = Recipe.Create(ModContent.ItemType<NeapoliniteBar>());
			NeapoliniteBar.AddIngredient(ModContent.ItemType<NeapoliniteOre>(), 5);
			NeapoliniteBar.AddTile(TileID.AdamantiteForge);
			NeapoliniteBar.SortAfterFirstRecipesOf(ItemID.SuperStarCannon);
			NeapoliniteBar.Register();

			//Neapolinite Mask
			Recipe NeapoliniteMask = Recipe.Create(ModContent.ItemType<NeapoliniteMask>());
			NeapoliniteMask.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			NeapoliniteMask.AddTile(TileID.MythrilAnvil);
			NeapoliniteMask.SortAfter(NeapoliniteBar);
			NeapoliniteMask.Register();

			//Neapolinite Helmet
			Recipe NeapoliniteHelmet = Recipe.Create(ModContent.ItemType<NeapoliniteHelmet>());
			NeapoliniteHelmet.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			NeapoliniteHelmet.AddTile(TileID.MythrilAnvil);
			NeapoliniteHelmet.SortAfter(NeapoliniteMask);
			NeapoliniteHelmet.Register();

			//Neapolinite Headgear
			Recipe NeapoliniteHeadgear = Recipe.Create(ModContent.ItemType<NeapoliniteHeadgear>());
			NeapoliniteHeadgear.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			NeapoliniteHeadgear.AddTile(TileID.MythrilAnvil);
			NeapoliniteHeadgear.SortAfter(NeapoliniteHelmet);
			NeapoliniteHeadgear.Register();

			//Neapolinite Hat
			Recipe NeapoliniteHat = Recipe.Create(ModContent.ItemType<NeapoliniteHat>());
			NeapoliniteHat.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			NeapoliniteHat.AddTile(TileID.MythrilAnvil);
			NeapoliniteHat.SortAfter(NeapoliniteHeadgear);
			NeapoliniteHat.Register();

			//Neapolinite Cone Mail
			Recipe NeapoliniteConeMail = Recipe.Create(ModContent.ItemType<NeapoliniteConeMail>());
			NeapoliniteConeMail.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 24);
			NeapoliniteConeMail.AddTile(TileID.MythrilAnvil);
			NeapoliniteConeMail.SortAfter(NeapoliniteHat);
			NeapoliniteConeMail.Register();

			//Neapolinite Greaves
			Recipe NeapoliniteGreaves = Recipe.Create(ModContent.ItemType<NeapoliniteGreaves>());
			NeapoliniteGreaves.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 18);
			NeapoliniteGreaves.AddTile(TileID.MythrilAnvil);
			NeapoliniteGreaves.SortAfter(NeapoliniteConeMail);
			NeapoliniteGreaves.Register();

			//Ancient Neapolinite Helmet
			Recipe AncientNeapoliniteHelmet = Recipe.Create(ModContent.ItemType<AncientNeapoliniteHelmet>());
			AncientNeapoliniteHelmet.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			AncientNeapoliniteHelmet.AddTile(TileID.DemonAltar);
			AncientNeapoliniteHelmet.SortAfter(NeapoliniteGreaves);
			AncientNeapoliniteHelmet.Register();

			//Anicent Neapolinite Headgear
			Recipe AncientNeapoliniteHeadgear = Recipe.Create(ModContent.ItemType<AncientNeapoliniteHeadgear>());
			AncientNeapoliniteHeadgear.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			AncientNeapoliniteHeadgear.AddTile(TileID.DemonAltar);
			AncientNeapoliniteHeadgear.SortAfter(AncientNeapoliniteHelmet);
			AncientNeapoliniteHeadgear.Register();

			//Anicent Neapolinite Hat
			Recipe AncientNeapoliniteHat = Recipe.Create(ModContent.ItemType<AncientNeapoliniteHat>());
			AncientNeapoliniteHat.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			AncientNeapoliniteHat.AddTile(TileID.DemonAltar);
			AncientNeapoliniteHat.SortAfter(AncientNeapoliniteHeadgear);
			AncientNeapoliniteHat.Register();

			//Drixer
			Recipe Drixer = Recipe.Create(ModContent.ItemType<Drixer>());
			Drixer.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 18);
			Drixer.AddIngredient(ItemID.SoulofSight);
			Drixer.AddIngredient(ItemID.SoulofMight);
			Drixer.AddIngredient(ItemID.SoulofFright);
			Drixer.AddTile(TileID.MythrilAnvil);
			Drixer.SortAfter(AncientNeapoliniteHat);
			Drixer.Register();

			//Pix
			Recipe Pix = Recipe.Create(ModContent.ItemType<Pix>());
			Pix.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 18);
			Pix.AddIngredient(ItemID.SoulofSight);
			Pix.AddIngredient(ItemID.SoulofMight);
			Pix.AddIngredient(ItemID.SoulofFright);
			Pix.AddTile(TileID.MythrilAnvil);
			Pix.SortAfter(Drixer);
			Pix.Register();

			//Neapolinite Repeater
			Recipe NeapoliniteRepeater = Recipe.Create(ModContent.ItemType<NeapoliniteRepeater>());
			NeapoliniteRepeater.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			NeapoliniteRepeater.AddTile(TileID.MythrilAnvil);
			NeapoliniteRepeater.SortAfter(Pix);
			NeapoliniteRepeater.Register();

			//Sucrosa
			Recipe Sucrosa = Recipe.Create(ModContent.ItemType<Sucrosa>());
			Sucrosa.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			Sucrosa.AddTile(TileID.MythrilAnvil);
			Sucrosa.SortAfter(NeapoliniteRepeater);
			Sucrosa.Register();

			//spear of cavendes
			Recipe SpearofCavendes = Recipe.Create(ModContent.ItemType<SpearofCavendes>());
			SpearofCavendes.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			SpearofCavendes.AddTile(TileID.MythrilAnvil);
			SpearofCavendes.SortAfter(Sucrosa);
			SpearofCavendes.Register();

			//Neapolinite Jousting Lance
			Recipe NeapoliniteJoustingLance = Recipe.Create(ModContent.ItemType<NeapoliniteJoustingLance>());
			NeapoliniteJoustingLance.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			NeapoliniteJoustingLance.AddTile(TileID.MythrilAnvil);
			NeapoliniteJoustingLance.SortAfter(SpearofCavendes);
			NeapoliniteJoustingLance.Register();

			//Saccharite Lash
			Recipe SacchariteLash = Recipe.Create(ModContent.ItemType<SacchariteLash>());
			SacchariteLash.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			SacchariteLash.AddTile(TileID.MythrilAnvil);
			SacchariteLash.SortAfter(NeapoliniteJoustingLance);
			SacchariteLash.Register();

			//Cosmic Cookie Cannon
			Recipe CosmicCookieCannon = Recipe.Create(ModContent.ItemType<CosmicCookieCannon>());
			CosmicCookieCannon.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			CosmicCookieCannon.AddIngredient(ItemID.StarCannon, 1);
			CosmicCookieCannon.AddTile(TileID.MythrilAnvil);
			CosmicCookieCannon.SortAfter(SacchariteLash);
			CosmicCookieCannon.Register();
			#endregion

			//Confection Torch
			Recipe ConfectionTorch = Recipe.Create(ModContent.ItemType<ConfectionTorch>(), 3);
			ConfectionTorch.AddIngredient(ItemID.Torch, 3);
			ConfectionTorch.AddIngredient(ModContent.ItemType<Creamstone>());
			ConfectionTorch.SortBeforeFirstRecipesOf(ItemID.JungleTorch);
			ConfectionTorch.Register();
			Recipe ConfectionTorch2 = Recipe.Create(ModContent.ItemType<ConfectionTorch>(), 3);
			ConfectionTorch2.AddIngredient(ItemID.Torch, 3);
			ConfectionTorch2.AddIngredient(ModContent.ItemType<HardenedCreamsand>());
			ConfectionTorch2.SortAfter(ConfectionTorch);
			ConfectionTorch2.Register();
			Recipe ConfectionTorch3 = Recipe.Create(ModContent.ItemType<ConfectionTorch>(), 3);
			ConfectionTorch3.AddIngredient(ItemID.Torch, 3);
			ConfectionTorch3.AddIngredient(ModContent.ItemType<OrangeIce>());
			ConfectionTorch3.SortAfter(ConfectionTorch2);
			ConfectionTorch3.Register();

			//Confection Campfire
			Recipe ConfectionCampfire = Recipe.Create(ModContent.ItemType<ConfectionCampfire>());
			ConfectionCampfire.AddRecipeGroup(RecipeGroupID.Wood, 10);
			ConfectionCampfire.AddIngredient(ModContent.ItemType<ConfectionTorch>(), 5);
			ConfectionCampfire.SortAfterFirstRecipesOf(ItemID.HallowedCampfire);
			ConfectionCampfire.Register();

			//Shifting Creamsand Dye
			Recipe ShiftingCreamsandsDye = Recipe.Create(ModContent.ItemType<ShiftingCreamsandsDye>());
			ShiftingCreamsandsDye.AddIngredient(ItemID.ShiftingSandsDye);
			ShiftingCreamsandsDye.AddIngredient(ModContent.ItemType<Saccharite>(), 20);
			ShiftingCreamsandsDye.AddTile(TileID.DyeVat);
			ShiftingCreamsandsDye.SortAfterFirstRecipesOf(ItemID.ShiftingPearlSandsDye);
			ShiftingCreamsandsDye.Register();

			//True Sucrosa
			Recipe TrueSucrosa = Recipe.Create(ModContent.ItemType<TrueSucrosa>());
			TrueSucrosa.AddIngredient<Sucrosa>();
			TrueSucrosa.AddIngredient(ItemID.ChlorophyteBar, 24);
			TrueSucrosa.AddTile(TileID.MythrilAnvil);
			TrueSucrosa.SortAfterFirstRecipesOf(ItemID.TrueExcalibur);
			TrueSucrosa.Register();

			#region Mech Confection Items Crafting Group
			//Trifle
			Recipe Trifle = Recipe.Create(ModContent.ItemType<Trifle>());
			Trifle.AddIngredient(ItemID.ClockworkAssaultRifle);
			Trifle.AddIngredient(ItemID.IllegalGunParts);
			Trifle.AddIngredient<SoulofDelight>(15);
			Trifle.AddIngredient<Sprinkles>(20);
			Trifle.AddIngredient(ItemID.SoulofMight, 20);
			Trifle.AddTile(TileID.MythrilAnvil);
			Trifle.SortAfterFirstRecipesOf(ItemID.FairyBell);
			Trifle.Register();

			//Bakers Dozen
			Recipe BakersDozen = Recipe.Create(ModContent.ItemType<BakersDozen>());
			BakersDozen.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 20);
			BakersDozen.AddIngredient(ModContent.ItemType<SoulofDelight>(), 15);
			BakersDozen.AddIngredient(ItemID.SoulofMight, 25);
			BakersDozen.AddTile(TileID.MythrilAnvil);
			BakersDozen.SortAfter(Trifle);
			BakersDozen.Register();

			//Candlelighter
			Recipe Candlelighter = Recipe.Create(ModContent.ItemType<Candlelighter>());
			Candlelighter.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			Candlelighter.AddIngredient(ItemID.IllegalGunParts);
			Candlelighter.AddIngredient(ItemID.SoulofFright, 20);
			Candlelighter.AddTile(TileID.MythrilAnvil);
			Candlelighter.SortAfter(BakersDozen);
			Candlelighter.Register();

			//Roller Cookie Staff
			Recipe SweetStaff = Recipe.Create(ModContent.ItemType<SweetStaff>());
			SweetStaff.AddIngredient(ModContent.ItemType<ChocolateChunk>(), 1);
			SweetStaff.AddIngredient(ModContent.ItemType<CookieDough>(), 10);
			SweetStaff.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12);
			SweetStaff.AddIngredient(ItemID.SoulofSight, 20);
			SweetStaff.AddTile(TileID.MythrilAnvil);
			SweetStaff.SortAfter(Candlelighter);
			SweetStaff.Register();

			//Icecream Bell
			Recipe IcecreamBell = Recipe.Create(ModContent.ItemType<IcecreamBell>());
			IcecreamBell.AddIngredient(ItemID.Bell);
			IcecreamBell.AddIngredient(ModContent.ItemType<Saccharite>(), 20);
			IcecreamBell.AddIngredient(ModContent.ItemType<SoulofDelight>(), 8);
			IcecreamBell.AddIngredient(ItemID.SoulofSight, 15);
			IcecreamBell.AddTile(TileID.MythrilAnvil);
			IcecreamBell.SortAfter(SweetStaff);
			IcecreamBell.Register();

			//Cream Beam
			Recipe CreamBeam = Recipe.Create(ModContent.ItemType<CreamBeam>());
			CreamBeam.AddIngredient(ModContent.ItemType<Saccharite>(), 10);
			CreamBeam.AddIngredient(ModContent.ItemType<CookieDough>(), 2);
			CreamBeam.AddIngredient(ModContent.ItemType<Sprinkles>(), 10);
			CreamBeam.AddIngredient(ModContent.ItemType<SoulofDelight>(), 8);
			CreamBeam.AddIngredient(ItemID.SoulofSight, 10);
			CreamBeam.AddTile(TileID.MythrilAnvil);
			CreamBeam.SortAfter(IcecreamBell);
			CreamBeam.Register();

			//Magical Kazoo
			Recipe MagicalKazoo = Recipe.Create(ModContent.ItemType<MagicalKazoo>());
			MagicalKazoo.AddIngredient(ModContent.ItemType<Kazoo>(), 1);
			MagicalKazoo.AddIngredient(ModContent.ItemType<Sprinkles>(), 25);
			MagicalKazoo.AddIngredient(ModContent.ItemType<SoulofDelight>(), 8);
			MagicalKazoo.AddIngredient(ItemID.SoulofSight, 15);
			MagicalKazoo.AddTile(TileID.MythrilAnvil);
			MagicalKazoo.SortAfter(CreamBeam);
			MagicalKazoo.Register();
			#endregion

			AddCreamWoodFurnitureArmorAndItems();

			//Creamwood Platform
			Recipe CreamwoodPlatform = Recipe.Create(ModContent.ItemType<CreamwoodPlatform>(), 2);
			CreamwoodPlatform.AddIngredient(ModContent.ItemType<CreamWood>(), 1);
			CreamwoodPlatform.SortAfterFirstRecipesOf(ItemID.EchoPlatform);
			CreamwoodPlatform.Register();

			Recipe CreamWood = Recipe.Create(ModContent.ItemType<CreamWood>());
			CreamWood.AddIngredient(ModContent.ItemType<CreamwoodPlatform>(), 2);
			CreamWood.SortAfter(CreamwoodPlatform);
			CreamWood.Register();

			//Trapped Creamwood Chest
			Recipe CreamwoodTrappedChest = Recipe.Create(ModContent.ItemType<CreamwoodTrappedChest>());
			CreamwoodTrappedChest.AddIngredient(ModContent.ItemType<CreamwoodChest>());
			CreamwoodTrappedChest.AddIngredient(ItemID.Wire, 10);
			CreamwoodTrappedChest.AddTile(TileID.HeavyWorkBench);
			CreamwoodTrappedChest.SortAfterFirstRecipesOf(ItemID.Fake_AshWoodChest);
			CreamwoodTrappedChest.Register();

			//Creamwood Wall
			Recipe CreamwoodWall = Recipe.Create(ModContent.ItemType<CreamwoodWall>(), 4);
			CreamwoodWall.AddIngredient(ModContent.ItemType<CreamWood>());
			CreamwoodWall.AddTile(TileID.WorkBenches);
			CreamwoodWall.SortAfterFirstRecipesOf(ItemID.AshWoodWall);
			CreamwoodWall.Register();

			Recipe CreamWood2 = Recipe.Create(ModContent.ItemType<CreamWood>());
			CreamWood2.AddIngredient(ModContent.ItemType<CreamwoodWall>(), 4);
			CreamWood2.AddTile(TileID.WorkBenches);
			CreamWood2.SortAfter(CreamWall);
			CreamWood2.Register();

			//Creamwood Fence
			Recipe CreamwoodFence = Recipe.Create(ModContent.ItemType<CreamwoodFence>(), 4);
			CreamwoodFence.AddIngredient(ModContent.ItemType<CreamWood>());
			CreamwoodFence.AddTile(TileID.WorkBenches);
			CreamwoodFence.SortAfterFirstRecipesOf(ItemID.AshWoodFence);
			CreamwoodFence.Register();

			Recipe CreamWood3 = Recipe.Create(ModContent.ItemType<CreamWood>());
			CreamWood3.AddIngredient(ModContent.ItemType<CreamwoodFence>(), 4);
			CreamWood3.AddTile(TileID.WorkBenches);
			CreamWood3.SortAfter(CreamWall);
			CreamWood3.Register();
		}

		private static void AddCreamWoodFurnitureArmorAndItems()
		{
			//Creamwood Helmet
			Recipe CreamwoodHelmet = Recipe.Create(ModContent.ItemType<CreamwoodHelmet>());
			CreamwoodHelmet.AddIngredient(ModContent.ItemType<CreamWood>(), 20);
			CreamwoodHelmet.AddTile(TileID.WorkBenches);
			CreamwoodHelmet.SortAfterFirstRecipesOf(ItemID.AshWoodToilet);
			CreamwoodHelmet.Register();

			//Creamwood Chestplate
			Recipe CreamwoodBreastplate = Recipe.Create(ModContent.ItemType<CreamwoodBreastplate>());
			CreamwoodBreastplate.AddIngredient(ModContent.ItemType<CreamWood>(), 30);
			CreamwoodBreastplate.AddTile(TileID.WorkBenches);
			CreamwoodBreastplate.SortAfter(CreamwoodHelmet);
			CreamwoodBreastplate.Register();

			//Creamwood Greaves
			Recipe CreamwoodGreaves = Recipe.Create(ModContent.ItemType<CreamwoodGreaves>());
			CreamwoodGreaves.AddIngredient(ModContent.ItemType<CreamWood>(), 25);
			CreamwoodGreaves.AddTile(TileID.WorkBenches);
			CreamwoodGreaves.SortAfter(CreamwoodBreastplate);
			CreamwoodGreaves.Register();

			//Creamwood Sword
			Recipe CreamwoodSword = Recipe.Create(ModContent.ItemType<CreamwoodSword>());
			CreamwoodSword.AddIngredient(ModContent.ItemType<CreamWood>(), 7);
			CreamwoodSword.AddTile(TileID.WorkBenches);
			CreamwoodSword.SortAfter(CreamwoodGreaves);
			CreamwoodSword.Register();

			//Creamwood Hammer
			Recipe CreamwoodHammer = Recipe.Create(ModContent.ItemType<CreamwoodHammer>());
			CreamwoodHammer.AddIngredient(ModContent.ItemType<CreamWood>(), 8);
			CreamwoodHammer.AddTile(TileID.WorkBenches);
			CreamwoodHammer.SortAfter(CreamwoodSword);
			CreamwoodHammer.Register();

			//Creamwood Bow
			Recipe CreamwoodBow = Recipe.Create(ModContent.ItemType<CreamwoodBow>());
			CreamwoodBow.AddIngredient(ModContent.ItemType<CreamWood>(), 8);
			CreamwoodBow.AddTile(TileID.WorkBenches);
			CreamwoodBow.SortAfter(CreamwoodHammer);
			CreamwoodBow.Register();

			//Creamwood Bath
			Recipe CreamwoodBath = Recipe.Create(ModContent.ItemType<CreamwoodBath>());
			CreamwoodBath.AddIngredient(ModContent.ItemType<CreamWood>(), 14);
			CreamwoodBath.AddTile(TileID.Sawmill);
			CreamwoodBath.SortAfter(CreamwoodBow);
			CreamwoodBath.Register();

			//Creamwood Bed
			Recipe CreamwoodBed = Recipe.Create(ModContent.ItemType<CreamwoodBed>());
			CreamwoodBed.AddIngredient(ModContent.ItemType<CreamWood>(), 15);
			CreamwoodBed.AddIngredient(ItemID.Silk, 5);
			CreamwoodBed.AddTile(TileID.Sawmill);
			CreamwoodBed.SortAfter(CreamwoodBath);
			CreamwoodBed.Register();

			//Creamwood Bookcase
			Recipe CreamwoodBookcase = Recipe.Create(ModContent.ItemType<CreamwoodBookcase>());
			CreamwoodBookcase.AddIngredient(ModContent.ItemType<CreamWood>(), 20);
			CreamwoodBookcase.AddIngredient(ItemID.Book, 10);
			CreamwoodBookcase.AddTile(TileID.Sawmill);
			CreamwoodBookcase.SortAfter(CreamwoodBed);
			CreamwoodBookcase.Register();

			//Creamwood Dresser
			Recipe CreamwoodDresser = Recipe.Create(ModContent.ItemType<CreamwoodDresser>());
			CreamwoodDresser.AddIngredient(ModContent.ItemType<CreamWood>(), 16);
			CreamwoodDresser.AddTile(TileID.Sawmill);
			CreamwoodDresser.SortAfter(CreamwoodBookcase);
			CreamwoodDresser.Register();

			//Creamwood Candelabra
			Recipe CreamwoodCandelabra = Recipe.Create(ModContent.ItemType<CreamwoodCandelabra>());
			CreamwoodCandelabra.AddIngredient(ModContent.ItemType<CreamWood>(), 5);
			CreamwoodCandelabra.AddIngredient(ItemID.Torch, 3);
			CreamwoodCandelabra.AddTile(TileID.WorkBenches);
			CreamwoodCandelabra.SortAfter(CreamwoodDresser);
			CreamwoodCandelabra.Register();

			//Creamwood Candle
			Recipe CreamwoodCandle = Recipe.Create(ModContent.ItemType<CreamwoodCandle>());
			CreamwoodCandle.AddIngredient(ModContent.ItemType<CreamWood>(), 4);
			CreamwoodCandle.AddIngredient(ItemID.Torch, 1);
			CreamwoodCandle.AddTile(TileID.WorkBenches);
			CreamwoodCandle.SortAfter(CreamwoodCandelabra);
			CreamwoodCandle.Register();

			//Creamwood Chair
			Recipe CreamwoodChair = Recipe.Create(ModContent.ItemType<CreamwoodChair>());
			CreamwoodChair.AddIngredient(ModContent.ItemType<CreamWood>(), 4);
			CreamwoodChair.AddTile(TileID.WorkBenches);
			CreamwoodChair.SortAfter(CreamwoodCandle);
			CreamwoodChair.Register();

			//Creamwood Chandelier
			Recipe CreamwoodChandelier = Recipe.Create(ModContent.ItemType<CreamwoodChandelier>());
			CreamwoodChandelier.AddIngredient(ModContent.ItemType<CreamWood>(), 4);
			CreamwoodChandelier.AddIngredient(ItemID.Torch, 4);
			CreamwoodChandelier.AddIngredient(ItemID.Chain, 1);
			CreamwoodChandelier.AddTile(TileID.Anvils);
			CreamwoodChandelier.SortAfter(CreamwoodChair);
			CreamwoodChandelier.Register();

			//Creamwood Chest
			Recipe CreamwoodChest = Recipe.Create(ModContent.ItemType<CreamwoodChest>());
			CreamwoodChest.AddIngredient(ModContent.ItemType<CreamWood>(), 8);
			CreamwoodChest.AddRecipeGroup(RecipeGroupID.IronBar, 2);
			CreamwoodChest.AddTile(TileID.WorkBenches);
			CreamwoodChest.SortAfter(CreamwoodChandelier);
			CreamwoodChest.Register();

			//Creamwood Clock
			Recipe CreamwoodClock = Recipe.Create(ModContent.ItemType<CreamwoodClock>());
			CreamwoodClock.AddIngredient(ModContent.ItemType<CreamWood>(), 10);
			CreamwoodClock.AddRecipeGroup(RecipeGroupID.IronBar, 3);
			CreamwoodClock.AddIngredient(ItemID.Glass, 6);
			CreamwoodClock.AddTile(TileID.Sawmill);
			CreamwoodClock.SortAfter(CreamwoodChest);
			CreamwoodClock.Register();

			//Creamwood Door
			Recipe CreamwoodDoor = Recipe.Create(ModContent.ItemType<CreamwoodDoor>());
			CreamwoodDoor.AddIngredient(ModContent.ItemType<CreamWood>(), 6);
			CreamwoodDoor.AddTile(TileID.WorkBenches);
			CreamwoodDoor.SortAfter(CreamwoodClock);
			CreamwoodDoor.Register();

			//Creamwood Lamp
			Recipe CreamwoodLamp = Recipe.Create(ModContent.ItemType<CreamwoodLamp>());
			CreamwoodLamp.AddIngredient(ModContent.ItemType<CreamWood>(), 3);
			CreamwoodLamp.AddIngredient(ItemID.Torch, 1);
			CreamwoodLamp.AddTile(TileID.WorkBenches);
			CreamwoodLamp.SortAfter(CreamwoodDoor);
			CreamwoodLamp.Register();

			//Creamwood Lantern
			Recipe CreamwoodLantern = Recipe.Create(ModContent.ItemType<CreamwoodLantern>());
			CreamwoodLantern.AddIngredient(ModContent.ItemType<CreamWood>(), 6);
			CreamwoodLantern.AddIngredient(ItemID.Torch, 1);
			CreamwoodLantern.AddTile(TileID.WorkBenches);
			CreamwoodLantern.SortAfter(CreamwoodLamp);
			CreamwoodLantern.Register();

			//Creamwood Piano
			Recipe CreamwoodPiano = Recipe.Create(ModContent.ItemType<CreamwoodPiano>());
			CreamwoodPiano.AddIngredient(ModContent.ItemType<CreamWood>(), 15);
			CreamwoodPiano.AddIngredient(ItemID.Bone, 4);
			CreamwoodPiano.AddIngredient(ItemID.Book, 1);
			CreamwoodPiano.AddTile(TileID.Sawmill);
			CreamwoodPiano.SortAfter(CreamwoodLantern);
			CreamwoodPiano.Register();

			//Creamwood Sink
			Recipe CreamwoodSink = Recipe.Create(ModContent.ItemType<CreamwoodSink>());
			CreamwoodSink.AddIngredient(ModContent.ItemType<CreamWood>(), 6);
			CreamwoodSink.AddIngredient(ItemID.WaterBucket, 1);
			CreamwoodSink.AddTile(TileID.WorkBenches);
			CreamwoodSink.SortAfter(CreamwoodPiano);
			CreamwoodSink.Register();

			//Creamwood Sofa
			Recipe CreamwoodSofa = Recipe.Create(ModContent.ItemType<CreamwoodSofa>());
			CreamwoodSofa.AddIngredient(ModContent.ItemType<CreamWood>(), 5);
			CreamwoodSofa.AddIngredient(ItemID.Silk, 2);
			CreamwoodSofa.AddTile(TileID.Sawmill);
			CreamwoodSofa.SortAfter(CreamwoodSink);
			CreamwoodSofa.Register();

			//Creamwood Table
			Recipe CreamwoodTable = Recipe.Create(ModContent.ItemType<CreamwoodTable>());
			CreamwoodTable.AddIngredient(ModContent.ItemType<CreamWood>(), 8);
			CreamwoodTable.AddTile(TileID.WorkBenches);
			CreamwoodTable.SortAfter(CreamwoodSofa);
			CreamwoodTable.Register();

			//Creamwood Workbench
			Recipe CreamwoodWorkbench = Recipe.Create(ModContent.ItemType<CreamwoodWorkbench>());
			CreamwoodWorkbench.AddIngredient(ModContent.ItemType<CreamWood>(), 10);
			CreamwoodWorkbench.SortAfter(CreamwoodTable);
			CreamwoodWorkbench.Register();

			//Creamwood Toilet
			Recipe CreamwoodToilet = Recipe.Create(ModContent.ItemType<CreamwoodToilet>());
			CreamwoodToilet.AddIngredient(ModContent.ItemType<CreamWood>(), 6);
			CreamwoodToilet.AddTile(TileID.Sawmill);
			CreamwoodToilet.SortAfter(CreamwoodWorkbench);
			CreamwoodToilet.Register();
		}
	}
}
