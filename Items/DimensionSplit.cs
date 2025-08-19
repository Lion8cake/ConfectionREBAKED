using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items
{
	public class DimensionSplit : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<LickitySplit>();
		}
        public override void SetDefaults()
        {
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.width = 30;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<DimensionalWarp>();
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
            if (!Main.tile[tileX, tileY].HasTile || !Main.tileSolid[Main.tile[tileX, tileY].TileType])
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<DimensionalWarp>()] < 1)
                {
                    Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, 0, 0, player.whoAmI, 0);
                }
                else if (player.ownedProjectileCounts[ModContent.ProjectileType<DimensionalWarp>()] < 2)
                {
                    Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, 0, 0, player.whoAmI, 1);
                }
				else
				{
					for (int i = 0; i < Main.maxProjectiles; i++)
					{
						Projectile projectile = Main.projectile[i];
						if (projectile.active && (projectile.ai[0] == 1 || projectile.ai[0] == 3) && projectile.type == ModContent.ProjectileType<DimensionalWarp>())
						{
							projectile.Kill();
							Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, 0, 0, player.whoAmI, 1);
						}
					}
				}
			}
            return false;
        }
    }
}
