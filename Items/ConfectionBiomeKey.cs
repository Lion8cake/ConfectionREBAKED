using Microsoft.Xna.Framework; // thanks to foxyboy55 for this fix
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	class ConfectionBiomeKey : ModItem
	{   
	    public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Confection Key");
            Tooltip.SetDefault("Unlocks a Confection Chest in the dungeon");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
		
		public override void SetDefaults()
        {
			Item.width = 14;
			Item.height = 20;
			Item.maxStack = 99;
            Item.rare = 8;
        }
	}
}