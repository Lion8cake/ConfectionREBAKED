﻿using Terraria;
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
		}
	}
}
