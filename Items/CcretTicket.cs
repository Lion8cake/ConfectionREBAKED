using TheConfectionRebirth.Mounts;
using TheConfectionRebirth.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	public class CcretTicket : ModItem
	{
		public override void SetStaticDefaults() {
		    DisplayName.SetDefault("C-cret Ticket");
			Tooltip.SetDefault("Summons a Foamin' Float to float on");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 30;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 250000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item79;
			Item.noMelee = true;
			Item.mountType = ModContent.MountType<CcretMount>();
		}
	}
}