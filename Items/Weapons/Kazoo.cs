using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Weapons
{
	public class Kazoo : ModItem
	{
		public override void SetDefaults() 
		{
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.value = 10000;
			Item.UseSound = SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Item/kazooSound");
			Item.autoReuse = true;
		}
		
		public override void SetStaticDefaults() 
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
	}
}