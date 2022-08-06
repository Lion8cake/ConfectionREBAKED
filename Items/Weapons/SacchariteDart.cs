using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
    public class SacchariteDart : ModItem
    {
        public override void SetDefaults()
        {

            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 26;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.height = 30;

            Item.shoot = Mod.Find<ModProjectile>("SacchariteDartPro").Type;
            Item.shootSpeed = 22f;
            Item.knockBack = 4;
            Item.value = 10;
            Item.rare = 3;
            Item.ammo = 283;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Saccharite Dart");
            Tooltip.SetDefault("Homes onto enemies");
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(100);
            recipe.AddIngredient(null, "Saccharite", 1);
            recipe.Register();
        }
    }
}