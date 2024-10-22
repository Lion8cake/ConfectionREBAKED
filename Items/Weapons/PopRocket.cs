using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
    public class PopRocket : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
			Item.width = 40;
			Item.height = 18;
			Item.damage = 58;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 20);
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shootSpeed = 22f;
            Item.shoot = ModContent.ProjectileType<Projectiles.RocketPop>();
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            for (int i = -15; i <= 15; i += 10)
            {
                float j = i / 2;
                Vector2 Velocity2 = velocity.RotatedBy(MathHelper.ToRadians(j));
				Projectile.NewProjectile(source, position, Velocity2, type, damage, knockback, player.whoAmI);
            }
            return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2f, -2f);
		}
	}
}
