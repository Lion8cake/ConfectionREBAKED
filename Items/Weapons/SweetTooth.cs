using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Channels;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
    public class SweetTooth : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 58;
            Item.height = 26;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.SweetTooth>();
            Item.shootSpeed = 10f;
            Item.value = 400000;
            Item.useAmmo = AmmoID.Arrow;
			Item.noUseGraphic = true;
			Item.channel = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.SweetTooth>(), damage, knockback, player.whoAmI);
            return false;
		}
	}
}
