using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Placeable
{
	public class CreamBeans : ModItem
	{
	    public override void SetStaticDefaults() 
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

	    public override void SetDefaults()
	    {
	    	Item.width = 16;
	    	Item.height = 16;
	    	Item.maxStack = 999;
	    	Item.useStyle = 1;
	    	Item.useAnimation = 15;
	    	Item.useTime = 10;
	    	Item.autoReuse = true;
	    	Item.useTurn = true;
	    	Item.createTile = Mod.Find<ModTile>("CreamGrass").Type;
	    	Item.consumable = true;
	    }

	    public override bool? UseItem(Player player)
	    {
		    WorldGen.destroyObject = false;
		    TileID.Sets.BreakableWhenPlacing[0] = false;
			return false;
		}
	}
}
