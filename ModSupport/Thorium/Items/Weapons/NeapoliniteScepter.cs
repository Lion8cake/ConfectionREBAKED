using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Weapons
{
	[ExtendsFromMod("ThoriumMod")]

	public class NeapoliniteScepter : ThoriumMod.Items.ThoriumItem {

		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}

		public override void SetStaticDefaults() {
			Item.staff[Type] = true;
		}

		public override void SetDefaults() {
			Item.mana = 12;
			Item.DamageType = ((DamageClass)(object)ThoriumMod.ThoriumDamageBase<ThoriumMod.HealerDamage>.Instance);
			Item.damage = 33;
			isHealer = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 3f;
			Item.value = Item.sellPrice(0, 4, 80, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.NeapoliniteScepterVanilla>();
			Item.shootSpeed = 15f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X + 2, velocity.Y + 2, ModContent.ProjectileType<Projectiles.NeapoliniteScepterStrawberry>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X - 2, velocity.Y - 2, ModContent.ProjectileType<Projectiles.NeapoliniteScepterChocolate>(), damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes() {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod mod)) {
				CreateRecipe()
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			}
		}
	}
}
