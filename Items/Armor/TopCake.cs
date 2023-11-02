using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TopCake : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ItemRarityID.White;
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

		public override void UpdateArmorSet(Player player) {
			if (player.HasBuff<Buffs.RollercycleBuff>()) {
				if (ConfectionModCalling.Achievements != null) {
					ConfectionModCalling.Achievements.Call("Event", "BirthdaySuitRollerCookieRide");
				}
			}
		}

		public override void UpdateVanitySet(Player player) {
			if (player.HasBuff<Buffs.RollercycleBuff>()) {
				if (ConfectionModCalling.Achievements != null) {
					ConfectionModCalling.Achievements.Call("Event", "BirthdaySuitRollerCookieRide");
				}
			}
		}
	}
}