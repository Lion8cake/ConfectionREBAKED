using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class GummyWormWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.GummyWormWhip>(), 48, 2, 4);

			Item.shootSpeed = 4;
			Item.SetShopValues(ItemRarityColor.Pink5, Item.sellPrice(0, 2));
		}


		public override bool MeleePrefix() => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.GetModPlayer<ConfectionPlayer>().gummyWormWhipCounter++;
			if (player.GetModPlayer<ConfectionPlayer>().gummyWormWhipCounter > 4)
			{
				player.GetModPlayer<ConfectionPlayer>().gummyWormWhipCounter = 0;
			}
			int projID = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			Projectile projectile = Main.projectile[projID];
			projectile.frame = player.GetModPlayer<ConfectionPlayer>().gummyWormWhipCounter;
			return false;
		}
	}
}
