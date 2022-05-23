using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items
{
    class DimensionSplit : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dimensional Split");
            Tooltip.SetDefault("When used places a portal and when clicked on a npc teleports it to the first portal");
        }
        public override void SetDefaults()
        {
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.width = 30;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ModContent.ProjectileType<DimWarp>();
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ConfectionPlayer modPlayer = player.GetModPlayer<ConfectionPlayer>();
            int tileX = (int)((Main.mouseX + Main.screenPosition.X) / 16);
            int tileY = (int)((Main.mouseY + Main.screenPosition.Y) / 16);
            if (!Main.tile[tileX, tileY].HasTile || !Main.tileSolid[Main.tile[tileX, tileY].TileType])
            {
                if (modPlayer.DimensionalWarpIndex < 0)
                {
                    int index = Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, damage, knockback, player.whoAmI);
                    modPlayer.DimensionalWarp = Main.projectile[index];
                }
                else
                {
                    Projectile.NewProjectile(source, Main.MouseWorld, velocity, ModContent.ProjectileType<DimWarp2>(), 1, knockback, player.whoAmI);
                }
            }
            return false;
        }
    }
}
