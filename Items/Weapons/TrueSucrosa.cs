using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Weapons
{
	public class TrueSucrosa : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("True Sucrosa");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() 
		{
			Item.damage = 85;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 25;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(silver: 1000);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			// item.autoReuse = false;
			Item.shoot = Mod.Find<ModProjectile>("TrueSucrosaBolt").Type;
			Item.shootSpeed = 10f;
		}
		
		public override void AddRecipes() // thanks to foxyboy55 for this fix
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Weapons.Sucrosa>(), 1).AddIngredient(ItemID.BrokenHeroSword, 1).AddTile(TileID.MythrilAnvil).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Weapons.TrueDeathsRaze>(), 1).AddIngredient(this, 1).AddTile(TileID.MythrilAnvil).ReplaceResult(ItemID.TerraBlade);
			CreateRecipe(1).AddIngredient(675, 1).AddIngredient(this, 1).AddTile(TileID.MythrilAnvil).ReplaceResult(ItemID.TerraBlade);
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Weapons.TrueDeathsRaze>(), 1).AddIngredient(674, 1).AddTile(TileID.MythrilAnvil).ReplaceResult(ItemID.TerraBlade);
        }
	}
}