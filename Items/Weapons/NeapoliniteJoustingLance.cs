using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ID;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.Items.Weapons
{
	public class NeapoliniteJoustingLance : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToSpear(ModContent.ProjectileType<Projectiles.NeapoliniteJoustingLanceProjectile>(), 1f, 24);

            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.SetWeaponValues(80, 12f, 0);
            Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(0, 6));
            Item.channel = true;
            Item.StopAnimationOnHurt = true;
        }

		public override bool MeleePrefix() => true;

		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<NeapoliniteBar>(12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}