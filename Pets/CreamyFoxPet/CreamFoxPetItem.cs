using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Pets.CreamyFoxPet
{
	public class CreamFoxPetItem : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			DisplayName.SetDefault("Fox Cookie");
			Tooltip.SetDefault("Summons a Creamy Fox"
				+"\nGreat for impersonating Devs");
		}

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.DogWhistle);
			Item.value = Item.buyPrice(platinum: 1);
			Item.shoot = ModContent.ProjectileType<FoxPetProjectile>();
			Item.buffType = ModContent.BuffType<FoxPet>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600);
			}
		}
	}
}
