using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Pets.DudlingPet
{
	public class TaffyApple : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.LizardEgg);
			Item.shoot = ModContent.ProjectileType<DudlingPetProjectile>();
			Item.buffType = ModContent.BuffType<DudlingPet>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600);
			}
		}
	}
}
