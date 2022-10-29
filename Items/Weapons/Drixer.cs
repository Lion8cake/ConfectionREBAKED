using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.Items.Weapons
{
    public class Drixer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Not to be confused with a Drax'");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 12;
            Item.useTime = 5;
            Item.useAnimation = 35;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.axe = 22;
            Item.pick = 200;
            Item.tileBoost++;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(silver: 440);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item23;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Drixer>();
            Item.shootSpeed = 40f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<NeapoliniteBar>(18)
                .AddIngredient(ItemID.SoulofSight)
                .AddIngredient(ItemID.SoulofMight)
                .AddIngredient(ItemID.SoulofFright)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}