using TheConfectionRebirth;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items.Weapons
{
public class CreamBeam : ModItem
{
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Cream Beam");
		Item.staff[Item.type] = true;
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.damage = 54;
		Item.DamageType = DamageClass.Magic;
		Item.mana = 18;
		Item.width = 25;
		Item.height = 25;
		Item.useTime = 14;
		Item.useAnimation = 14;
		Item.useStyle = 5;
		Item.noMelee = true;
		Item.knockBack = 8f;
		Item.value = Item.sellPrice(silver: 400);
		Item.rare = ItemRarityID.Pink;
		Item.UseSound = SoundID.Item72;
		Item.autoReuse = true;
		Item.shoot = ModContent.ProjectileType<CreamBolt>();
		Item.shootSpeed = 6f;
	}
	
	public override void AddRecipes() {
			CreateRecipe(1).AddIngredient(ItemID.SoulofSight, 20).AddIngredient(ModContent.ItemType<Items.SoulofDelight>(), 10).AddIngredient(ModContent.ItemType<Items.Sprinkles>(), 60).AddIngredient(ModContent.ItemType<Items.CookieDough>(), 6).AddIngredient(ModContent.ItemType<Items.Placeable.Saccharite>(), 60).AddTile(TileID.MythrilAnvil).Register();
		}
}
}