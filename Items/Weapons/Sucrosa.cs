using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Items.Weapons
{
    public class Sucrosa : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 51;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(silver: 460);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.SucrosaSlash>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.SucrosaSlash2>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.SucrosaSlash>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
            return false;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int num313 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<NeapoliniteDust>());
                Main.dust[num313].noGravity = true;
                Main.dust[num313].fadeIn = 1.25f;
                Main.dust[num313].velocity *= 0.25f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<NeapoliniteBar>(12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}