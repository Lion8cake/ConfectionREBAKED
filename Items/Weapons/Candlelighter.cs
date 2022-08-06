using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class Candlelighter : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Yes, it's the tricky kind'\nUses gel as ammo\nEnemys will always be on fire unless they fall into water.");
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 50;
			Item.height = 18;
			Item.useTime = 4;
			Item.useAnimation = 20; 
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true; 
			Item.knockBack = 2; 
			Item.value = 500000;
			Item.rare = ItemRarityID.Pink; 
			Item.UseSound = SoundID.Item34;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CandleFlames>();
			Item.shootSpeed = 9f; 
			Item.useAmmo = AmmoID.Gel; 
		}

		public override bool CanUseItem(Player player)
		{
			return !player.wet;
		}

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return player.itemAnimation >= player.itemAnimationMax - 4;
		}


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 54f; 
			if (Collision.CanHit(position, 6, 6, position + muzzleOffset, 6, 6))
			{
				position += muzzleOffset;
			}
			return true;
        }
        public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, -2);
		}
		
		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.NeapoliniteBar>(), 12);
			recipe.AddIngredient(ItemID.IllegalGunParts, 1);
			recipe.AddIngredient(ItemID.SoulofSight, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
