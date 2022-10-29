using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
    public class IcecreamBell : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Do you scream, for Ice-cream?");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 25;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = 200000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item35;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IcecreamBall>();
            Item.shootSpeed = 7.5f;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-4f, 4f);

		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Saccharite>(50)
                .AddIngredient<SoulofDelight>(12)
                .AddIngredient(ItemID.SoulofSight, 20)
                .AddIngredient(ItemID.Bell)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}