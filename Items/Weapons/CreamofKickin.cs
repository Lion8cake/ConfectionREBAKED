using TheConfectionRebirth.Projectiles;
using TheConfectionRebirth.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class CreamofKickin : ModItem
	{
		public override void SetStaticDefaults() {
			SacrificeTotal = 1;
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 288);
			Item.rare = ItemRarityID.Pink;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.knockBack = 4f;
			Item.damage = 45;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<CreamofKickinMeatball>();
			Item.shootSpeed = 15.1f;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 15;
			Item.channel = true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ModContent.ProjectileType<CreamofKickinCookie>())
			{
				damage = (int)(damage / 2.5f);
			}
		}

		public override void AddRecipes() 
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Items.CreamPuff>(), 1)
				.AddIngredient(ItemID.DarkShard, 1)
				.AddIngredient(ModContent.ItemType<Items.SoulofDelight>(), 7)
				.AddIngredient(ItemID.SoulofNight, 7)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
		
		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public override bool CanUseItem(Player player) {
			if (player.altFunctionUse == 2) {
				Item.shoot = ModContent.ProjectileType<CreamofKickinCookie>();
			}
			else {
				Item.shoot = ModContent.ProjectileType<CreamofKickinMeatball>();
			}
			return base.CanUseItem(player);
		}
	}
}