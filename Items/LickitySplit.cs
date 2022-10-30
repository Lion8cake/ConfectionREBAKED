using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items
{
	class LickitySplit : DimensionSplit
    {
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ConfectionPlayer modPlayer = player.GetModPlayer<ConfectionPlayer>();
            int tileX = (int)((Main.mouseX + Main.screenPosition.X) / 16);
            int tileY = (int)((Main.mouseY + Main.screenPosition.Y) / 16);
            if (modPlayer.DimensionalWarp == null && (!Main.tile[tileX, tileY].HasTile || !Main.tileSolid[Main.tile[tileX, tileY].TileType]))
            {
                Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, damage, knockback, player.whoAmI, ai1: 1);
            }
            else if (modPlayer.DimensionalWarp != null)
            {
                Projectile.NewProjectile(source, Main.MouseWorld, velocity, ModContent.ProjectileType<DimWarp2>(), 1, knockback, player.whoAmI, ai1: 1);
            }
            return false;
        }
    }
}
