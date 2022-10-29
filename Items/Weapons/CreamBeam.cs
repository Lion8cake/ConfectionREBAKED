using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
    public class CreamBeam : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cream Beam");
            Item.staff[Item.type] = true;
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 54;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 18;
            Item.width = 25;
            Item.height = 25;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = Item.sellPrice(silver: 400);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CreamBolt>();
            Item.shootSpeed = 6f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofSight, 20)
                .AddIngredient(ModContent.ItemType<Items.SoulofDelight>(), 10)
                .AddIngredient(ModContent.ItemType<Items.Sprinkles>(), 60)
                .AddIngredient(ModContent.ItemType<Items.CookieDough>(), 6)
                .AddIngredient(ModContent.ItemType<Items.Placeable.Saccharite>(), 60)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}