using TheConfectionRebirth.Mounts;
using TheConfectionRebirth.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	public class PixieStick : ModItem
	{
		public override void SetStaticDefaults() {
		    DisplayName.SetDefault("Pixie Stick");
			Tooltip.SetDefault("Summons a Pixie Stick to ride on");
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
			Item.mountType = ModContent.MountType<PixieStickMount>();
		}
	}
}