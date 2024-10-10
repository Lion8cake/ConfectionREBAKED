using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Items.Weapons;

namespace TheConfectionRebirth
{
	public class ConfectionRecipes : ModSystem
	{
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
		}
	}
}
