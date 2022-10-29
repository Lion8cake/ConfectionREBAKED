using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.Items.Weapons
{
    public class Pix : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Not to be confused with a Paxel'");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 5;
            Item.useAnimation = 10;
            Item.pick = 200;
            Item.axe = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(silver: 440);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
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