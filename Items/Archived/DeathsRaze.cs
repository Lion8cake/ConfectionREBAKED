using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Archived
{
	public class DeathsRaze : ModItem {
		public override void SetStaticDefaults() {
			Terraria.ID.ItemID.Sets.Deprecated[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 54000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
		}
	}
}