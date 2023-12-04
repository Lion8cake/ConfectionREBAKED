using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.ModSupport.Thorium.Projectiles;
using ThoriumMod;
using ThoriumMod.Items;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Weapons;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class NeapoliniteScepter : ThoriumItem {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void SetStaticDefaults() {
		Item.staff[Type] = true;
		isHealer = true;
	}

	public override void SetDefaults() {
		Item.width = 30;
		Item.height = 30;

		Item.useTime = 22;
		Item.useAnimation = 22;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.Item43;

		Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
		Item.damage = 33;
		Item.knockBack = 3f;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.shoot = ModContent.ProjectileType<NeapoliniteScepterVanilla>();
		Item.shootSpeed = 15f;

		Item.mana = 12;
		Item.value = Item.sellPrice(gold: 4, silver: 80);
		Item.rare = ItemRarityID.Pink;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		var muzzleOffset = Vector2.Normalize(velocity) * 25f;
		if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
			position += muzzleOffset;
		}

		Projectile.NewProjectile(source, position.X, position.Y, velocity.X + 2, velocity.Y + 2, ModContent.ProjectileType<NeapoliniteScepterStrawberry>(), damage, knockback, player.whoAmI);
		Projectile.NewProjectile(source, position.X, position.Y, velocity.X - 2, velocity.Y - 2, ModContent.ProjectileType<NeapoliniteScepterChocolate>(), damage, knockback, player.whoAmI);
		Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
		return false;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<NeapoliniteBar>(12)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
