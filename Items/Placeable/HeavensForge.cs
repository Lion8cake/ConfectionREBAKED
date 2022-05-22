using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class HeavensForge : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 26;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.rare = 4;
            Item.value = 150;
            Item.createTile = ModContent.TileType<Tiles.HeavensForgeTile>();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heavenly Forge");
            Tooltip.SetDefault("Allows you to convert hallowed materials into their confection alternatives and vice versa\n" +
                "'A forge created from the both the lands of rainbows and candy'");
        }

        /*public override void AddRecipes()
        {
			
            CreateRecipe().AddIngredient(ItemID.CrystalShard, 10).AddIngredient(ItemID.PearlstoneBlock, 30).AddIngredient(ItemID.SoulofLight, 8).AddTile(TileID.DemonAltar).Register();

			CreateRecipe(1).AddIngredient(null, "Saccharite", 10).AddIngredient(null, "Creamstone", 30).AddIngredient(null, "SoulofDelight", 8).AddTile(TileID.DemonAltar).Register();
			
            CreateRecipe(1).AddIngredient(ItemID.CrystalShard, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "Saccharite", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.CrystalShard);
			
            CreateRecipe(1).AddIngredient(ItemID.PixieDust, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "Sprinkles", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.PixieDust);
			
            CreateRecipe(1).AddIngredient(ItemID.SoulofLight, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "SoulofDelight", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.SoulofLight);
			
            CreateRecipe(1).AddIngredient(ItemID.UnicornHorn, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "CookieDough", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.UnicornHorn);
			
            CreateRecipe(1).AddIngredient(null, "NeapoliniteBar", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.HallowedBar);
			
            CreateRecipe(1).AddIngredient(ItemID.HallowedBar, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "HallowedOre", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "NeapoliniteOre", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "HallowedBrick", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "NeapoliniteBrick", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "HallowedBrickWall", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "NeapoliniteBrickWall", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(ItemID.HallowedKey, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "ConfectionBiomeKey", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.HallowedKey);
			
            CreateRecipe(1).AddIngredient(ItemID.RainbowBrick, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "SherbetBricks", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.RainbowBrick);
			
            CreateRecipe(1).AddIngredient(ItemID.RainbowBrickWall, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "SherbetWall", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.RainbowBrickWall);
			
            CreateRecipe(1).AddIngredient(ItemID.RainbowTorch, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "SherbetTorch", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.RainbowTorch);
			
            CreateRecipe(1).AddIngredient(ItemID.Pearlwood, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "CreamWood", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.Pearlwood);
			
            CreateRecipe(1).AddIngredient(ItemID.Prismite, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "Cakekite", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.Prismite);
			
            CreateRecipe(1).AddIngredient(ItemID.PrincessFish, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "CookieCarp", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.PrincessFish);
			
            CreateRecipe(1).AddIngredient(ItemID.Pwnhammer, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "GrandSlammer", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.Pwnhammer);
			
            CreateRecipe(1).AddIngredient(ItemID.PearlstoneBlock, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "Creamstone", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.PearlstoneBlock);
			
            CreateRecipe(1).AddIngredient(ItemID.HallowedSeeds, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "CreamBeans", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.HallowedSeeds);
			
            CreateRecipe(1).AddIngredient(ItemID.PearlsandBlock, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "Creamsand", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.PearlsandBlock);
			
            CreateRecipe(1).AddIngredient(3339, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "Creamsandstone", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(3339);
			
            CreateRecipe(1).AddIngredient(3338, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "HardenedCreamsand", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(3338);
			
            CreateRecipe(1).AddIngredient(ItemID.PinkIceBlock, 1).AddTile(null, "HeavensForgeTile").ReplaceResult(null);
			
            CreateRecipe(1).AddIngredient(null, "OrangeIce", 1).AddTile(null, "HeavensForgeTile").ReplaceResult(ItemID.PinkIceBlock);
        }*/
    }
}