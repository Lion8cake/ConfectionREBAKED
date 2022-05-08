using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Weapons
{
	public class DeathsRaze : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Death's Raze");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() 
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 54000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			// item.autoReuse = false;
		}
		
		public override void AddRecipes() 
		{
			CreateRecipe(1).AddIngredient(ItemID.BloodButcherer, 1).AddIngredient(ItemID.Muramasa, 1).AddIngredient(ItemID.BladeofGrass, 1).AddIngredient(ItemID.FieryGreatsword, 1).AddTile(TileID.DemonAltar).Register();
		}
	}
}