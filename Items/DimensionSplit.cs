using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items
{
	class DimensionSplit : ModItem
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
            if (modPlayer.DimensionalWarp == null && (!Main.tile[tileX, tileY].HasTile || !Main.tileSolid[Main.tile[tileX, tileY].TileType]))
            {
                Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, damage, knockback, player.whoAmI);
            }
            else if (modPlayer.DimensionalWarp != null && player.ownedProjectileCounts[ModContent.ProjectileType<DimWarp2>()] == 0)
            {
                Projectile.NewProjectile(source, Main.MouseWorld, velocity, ModContent.ProjectileType<DimWarp2>(), 1, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ModContent.ItemType<BananawarpPeel>())
                .AddIngredient(ModContent.ItemType<CookieDough>(), 2)
                .AddIngredient(ModContent.ItemType<Placeable.Saccharite>(), 6)
                .AddIngredient(ItemID.Ectoplasm, 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe(1)
                .AddIngredient(ModContent.ItemType<BananawarpPeel>())
                .AddIngredient(ModContent.ItemType<CookieDough>(), 2)
                .AddIngredient(ModContent.ItemType<Placeable.Saccharite>(), 6)
                .AddIngredient(ItemID.SoulofLight, 8)
                .AddCondition(Language.GetOrRegister("CelebrationMK10"), () => Main.tenthAnniversaryWorld)
				.AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(1)
                .AddIngredient(ModContent.ItemType<BananawarpPeel>())
                .AddIngredient(ModContent.ItemType<CookieDough>(), 2)
                .AddIngredient(ModContent.ItemType<Placeable.Saccharite>(), 6)
                .AddIngredient(ModContent.ItemType<SoulofDelight>(), 8)
                .AddCondition(Language.GetOrRegister("CelebrationMK10"), () => Main.tenthAnniversaryWorld)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
