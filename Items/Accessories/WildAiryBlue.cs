using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings )]
    public class WildAiryBlue : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 8f, 1f, hasHoldDownHoverFeatures: true, 10f, 10f);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 400000;
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            maxAscentMultiplier = 2.5f;
			if (player.TryingToHoverDown && !player.controlLeft && !player.controlRight)
			{
				player.wingTime += 0.5f; //-= 1 is normally applied, but we want -= 0.5f, so we add += 0.5f instead to combat -= 1
			}
            //Hovering itself is found in ConfectionPlayer.PreUpdateMovement
		}

        public override bool WingUpdate(Player player, bool inUse)
        {
			bool newInUse = (player.controlJump && player.TryingToHoverDown && player.wingTime > 0f) ? true : inUse;
			player.wingFrameCounter++;
            if (player.wingFrameCounter > ((!newInUse) ? 8 : 6))
            {
                player.wingFrame++;
                player.wingFrameCounter = 0;
                if (player.wingFrame >= 3)
                {
                    player.wingFrame = 0;
                }
            }
			//Manual rendering is split off into a detour in TheConfectionRebirth.cs and PlayerDrawLayer class, ConfectionWingRenderer
			return true;
        }
    }
}