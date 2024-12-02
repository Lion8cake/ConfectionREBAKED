using Terraria.ModLoader;
using Terraria;

namespace TheConfectionRebirth.Items
{
    public class KeyofSpite : ModItem 
	{
		public override void SetStaticDefaults() 
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() 
		{
			Item.width = 14;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.useAnimation = 20;
			Item.useTime = 20;
		}
	}
}