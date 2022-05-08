using TheConfectionRebirth.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Weapons
{
	public class Trifle : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("30% Not to consume ammo");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 23; 
			Item.DamageType = DamageClass.Ranged; 
			Item.width = 40; 
			Item.height = 20; 
			Item.useTime = 10; 
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot; 
			Item.noMelee = true; 
			Item.knockBack = 4; 
			Item.rare = ItemRarityID.Pink; 
			Item.UseSound = SoundID.Item11; 
			Item.autoReuse = true; 
			Item.shoot = 10; 
			Item.shootSpeed = 16f; 
			Item.value = 300000;
			Item.useAmmo = AmmoID.Bullet; 
			Item.useAnimation = 12;
			Item.useTime = 4;
			Item.reuseDelay = 14;
		}
		
		public override void AddRecipes() 
		{
			CreateRecipe(1).AddIngredient(ItemID.ClockworkAssaultRifle, 1).AddIngredient(ItemID.IllegalGunParts, 1).AddIngredient(ModContent.ItemType<Items.SoulofDelight>(), 15).AddIngredient(ModContent.ItemType<Items.Sprinkles>(), 20).AddIngredient(ItemID.SoulofMight, 20).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
