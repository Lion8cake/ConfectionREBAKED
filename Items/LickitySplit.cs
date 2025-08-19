﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items
{
	class LickitySplit : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.width = 30;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<LickWarp>();
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
        }

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, new Vector3(2.39f, 1.64f, 0.05f) * 0.22f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor) {
			return Color.White;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ConfectionPlayer modPlayer = player.GetModPlayer<ConfectionPlayer>();
            int tileX = (int)((Main.mouseX + Main.screenPosition.X) / 16);
            int tileY = (int)((Main.mouseY + Main.screenPosition.Y) / 16);
            if (modPlayer.DimensionalWarp == null && (!Main.tile[tileX, tileY].HasTile || !Main.tileSolid[Main.tile[tileX, tileY].TileType]))
            {
                Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, damage, knockback, player.whoAmI);
            }
            else if (modPlayer.DimensionalWarp != null && player.ownedProjectileCounts[ModContent.ProjectileType<LickWarp2>()] == 0)
            {
                Projectile.NewProjectile(source, Main.MouseWorld, velocity, ModContent.ProjectileType<LickWarp2>(), 1, knockback, player.whoAmI);
            }
            return false;
        }
    }
}
