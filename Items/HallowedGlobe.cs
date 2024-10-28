using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class HallowedGlobe : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
	
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 8f;
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<Projectiles.HallowedGlobe>();
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.UseSound = SoundID.Item106;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.noMelee = true;
			Item.value = 200;
		}
	}
}
