using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Armor.BirthdayOutfit
{
    [AutoloadEquip(EquipType.Head)]
    public class TopCake : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<BirthdaySuit>() && legs.type == ModContent.ItemType<RightTrousers>();
		}

		/*public override void UpdateArmorSet(Player player) {
			if (player.HasBuff<Buffs.RollercycleBuff>()) {
				if (ConfectionModCalling.Achievements != null) {
					ConfectionModCalling.Achievements.Call("Event", "BirthdaySuitRollerCookieRide");
				}
			}
		}*/
	}
}