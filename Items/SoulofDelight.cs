using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	public class SoulofDelight : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Soul of Delight");
			Tooltip.SetDefault("'The essence of delight creatures'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults() 
		{
		    Item refItem = new Item();
			refItem.SetDefaults(ItemID.SoulofLight);
			Item.width = refItem.width;
			Item.height = refItem.height;
			Item.value = 1000;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
		}
	    
		public override void AddRecipes() // thanks to foxyboy55 for this fix
		{
														
 		 CreateRecipe(1).AddIngredient(38, 3).AddIngredient(22, 5).AddIngredient(null, "SoulofDelight", 6).AddTile(134).ReplaceResult(544);
														
 		 CreateRecipe(1).AddIngredient(38, 3).AddIngredient(704, 5).AddIngredient(null, "SoulofDelight", 6).AddTile(134).ReplaceResult(544);
														
 		 CreateRecipe(1).AddIngredient(154, 30).AddIngredient(22, 5).AddIngredient(null, "SoulofDelight", 3).AddIngredient(521, 3).AddTile(134).ReplaceResult(557);
														
 		 CreateRecipe(1).AddIngredient(154, 30).AddIngredient(704, 5).AddIngredient(null, "SoulofDelight", 3).AddIngredient(521, 3).AddTile(134).ReplaceResult(557);
														
 		 CreateRecipe(1).AddIngredient(117, 20).AddIngredient(null, "Sprinkles", 10).AddIngredient(null, "SoulofDelight", 10).AddTile(134).ReplaceResult(2750);
		}
	}
}