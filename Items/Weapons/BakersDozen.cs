using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
    public class BakersDozen : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baker's Dozen");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.damage = 32;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 14;
            Item.useStyle = 1;
            Item.useTime = 14;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.height = 38;
            Item.value = 600000;
            Item.rare = 5;
            Item.shoot = Mod.Find<ModProjectile>("BakersDozen").Type;
            Item.shootSpeed = 16f;
            Item.maxStack = 13;
        }

        public override bool CanUseItem(Player player)
        {
            int stack = Item.stack;
            bool canuse = true;
            for (int m = 0; m < 1000; m++)
            {
                if (Main.projectile[m].active && Main.projectile[m].owner == Main.myPlayer && Main.projectile[m].type == Item.shoot)
                    stack -= 1;
            }
            if (stack <= 0) canuse = false;
            return canuse;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.NeapoliniteBar>(), 15).AddIngredient(ModContent.ItemType<Items.SoulofDelight>(), 20).AddIngredient(ItemID.SoulofMight, 20).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
