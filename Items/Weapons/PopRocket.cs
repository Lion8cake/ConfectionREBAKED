using TheConfectionRebirth.Tiles;
using TheConfectionRebirth.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Weapons
{
	public class PopRocket : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Fires 4 Icy-Rockets after your enemys.");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 58; 
			Item.DamageType = DamageClass.Magic;
		    Item.mana = 55;
			Item.width = 40; 
			Item.height = 20; 
			Item.useTime = 20; 
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot; 
			Item.noMelee = true; 
			Item.knockBack = 4; 
			Item.rare = ItemRarityID.Yellow; 
			Item.UseSound = SoundID.Item11; 
			Item.value = Item.sellPrice(silver: 700);
			Item.autoReuse = true; 
			Item.shoot = 10; 
			Item.shootSpeed = 16f; 
			Item.shoot = Mod.Find<ModProjectile>("PopRocket").Type;
			Item.useAnimation = 12;
		}
	}
}
