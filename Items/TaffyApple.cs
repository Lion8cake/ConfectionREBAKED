using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Newtonsoft.Json.Linq;

namespace TheConfectionRebirth.Items
{
	public class TaffyApple : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.useStyle = 1;
			Item.width = 16;
			Item.height = 30;
			Item.UseSound = SoundID.Item2;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.Orange;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 2);
			Item.shoot = ModContent.ProjectileType<Projectiles.DudlingPet>();
			Item.buffType = ModContent.BuffType<Buffs.DudlingPet>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600);
			}
		}
	}
}
