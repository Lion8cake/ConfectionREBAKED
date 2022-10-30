using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Mounts;

namespace TheConfectionRebirth.Items
{
    public class DriversPermint : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 250000;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item79;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<Rollercycle>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<KeyofDelight>()
                .AddIngredient<NeapoliniteBar>(30)
                .AddIngredient<Sprinkles>(80)
                .AddIngredient<CookieDough>(20)
                .AddIngredient<Saccharite>(100)
                .AddIngredient<SoulofDelight>(30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}