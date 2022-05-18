using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
    public class CosmicCookieCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Cookie Cannon");
            Tooltip.SetDefault("'But wait? doesn't it shoot cosmic brownies instead?'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 16;
            Item.value = Item.sellPrice(silver: 400);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.CosmicCookie>();
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.FallenStar;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CosmicBlueTrail>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CosmicCookie>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes() 
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.NeapoliniteBar>(), 12).AddIngredient(ItemID.StarCannon, 1).AddTile(TileID.MythrilAnvil).Register();
		}
    }
}