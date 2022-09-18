using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ID;

namespace TheConfectionRebirth.Items.Weapons
{
	public class NeapoliniteJoustingLance : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault(Language.GetTextValue("Build momentum to increase attack power"));

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {

            Item.DefaultToSpear(ModContent.ProjectileType<Projectiles.NeapoliniteJoustingLanceProjectile>(), 1f, 24);

            Item.DamageType = DamageClass.MeleeNoSpeed;

            Item.SetWeaponValues(80, 12f, 0);

            Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(0, 6));

            Item.channel = true;

            //Item.StopAnimationOnHurt = true;
            //^^^ This is not yet implemented for the current tmod version, uncomment on october 1st
        }

        public override bool MeleePrefix()
		{
			return true;
		}

		public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.NeapoliniteBar>(), 12).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}