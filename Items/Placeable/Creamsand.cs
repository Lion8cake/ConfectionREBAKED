using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
	public class Creamsand : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 100;
			ItemID.Sets.SandgunAmmoProjectileData[Type] = new(ModContent.ProjectileType<Projectiles.CreamsandSandGunProjectile>(), 10);
			ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 1, ItemID.SandBlock, 1);
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Creamsand>();
			Item.width = 12;
			Item.height = 12;
			Item.ammo = AmmoID.Sand;
			Item.notAmmo = true;
		}
	}
}
