using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheConfectionRebirth.Items.Accessories
{
    [AutoloadEquip(new EquipType[] { EquipType.Wings })] 
	public class WildAiryBlue : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(145, 14f, 2.5f);
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) {
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.135f;
		}
		
		public override bool WingUpdate(Player player, bool inUse)
    	{
	    	int WingTicks = ((!inUse) ? 8 : 6);
	    	if (player.velocity.Y != 0f)
    		{
    			player.wingFrameCounter++;
    			if (player.wingFrameCounter > WingTicks)
    			{
	    			player.wingFrame++;
	    			player.wingFrameCounter = 0;
	     			if (player.wingFrame >= 3)
	    			{
	    				player.wingFrame = 0;
	     			}
	    		}
	    	}
	    	else
	     	{
	    		player.wingFrame = 4;
	    	}
	    	return true;
	    }
	}
}