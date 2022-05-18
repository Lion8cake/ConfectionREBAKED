using TheConfectionRebirth.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace TheConfectionRebirth.Items.Weapons
{
	public class SpearofCavendes : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Spear of Cavendes");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 18;
			Item.useTime = 24;
			Item.shootSpeed = 3.7f;
			Item.knockBack = 6.5f;
			Item.width = 32;
			Item.height = 32;
			Item.scale = 1f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(silver: 460);

			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true; 
			Item.noUseGraphic = true; 

			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<SpearofCavendesProj>();
			Item.shootSpeed = 10f;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SpearofCavendesProj>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SpearofCavendesBannana>(), damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes() 
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.NeapoliniteBar>(), 12).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
