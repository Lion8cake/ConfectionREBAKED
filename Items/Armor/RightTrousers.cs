using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class RightTrousers : ModItem
	{
		public override void SetStaticDefaults() 
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
            Item.vanity = true;
			Item.rare = ItemRarityID.White;
		}
	}
}