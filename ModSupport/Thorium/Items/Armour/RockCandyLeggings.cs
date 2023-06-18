using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Armour 
{
	[ExtendsFromMod("ThoriumMod")]
	[AutoloadEquip(EquipType.Legs)]
	public class RockCandyLeggings : ThoriumMod.Items.BardItem {
		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}

		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetBardDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 14;
		}

		public override void AddRecipes() {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod mod)) {
				CreateRecipe()
				.AddIngredient(mod.Find<ModItem>("StrangePlating"), 4)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 8)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			}
		}

		public override void UpdateEquip(Player player) {
			player.moveSpeed += 0.07f;
			if (ModLoader.TryGetMod("ThoriumMod", out Mod source) && source.TryFind("BardDamage", out DamageClass damageClass)) {
				player.GetDamage(damageClass) += 0.05f;
			}
		}
	}
}