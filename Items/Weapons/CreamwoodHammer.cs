using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
    public class CreamwoodHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.hammer = 45;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = 100;
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Placeable.CreamWood>(), 8)
                .AddTile(TileID.WorkBenches).Register();
        }
    }
}
