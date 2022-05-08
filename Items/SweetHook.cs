using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
    public class SweetHook : ModItem
    {   
	    
		public override void SetStaticDefaults() 
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IlluminantHook);
            Item.shootSpeed = 18f;
            Item.shoot = Mod.Find<ModProjectile>("SweetHook").Type;
        }
    }
}
