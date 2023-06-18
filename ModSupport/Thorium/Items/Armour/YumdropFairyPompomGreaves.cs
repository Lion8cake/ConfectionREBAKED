using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Armour 
{
	[ExtendsFromMod("ThoriumMod")]
	[AutoloadEquip(EquipType.Legs)]
	public class YumdropFairyPompomGreaves : ThoriumMod.Items.ThoriumItem {
		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}

		public override void SetStaticDefaults() {
			isHealer = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 12;
		}

		public override void AddRecipes() {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod mod)) {
				CreateRecipe()
				.AddIngredient(mod.Find<ModItem>("LifeCell"), 2)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			}
		}

		public override void UpdateEquip(Player player) {
			player.moveSpeed += 0.06f;
			if (ModLoader.TryGetMod("ThoriumMod", out Mod source) && source.TryFind("HealerDamage", out DamageClass damageClass)) {
				player.manaCost -= 0.1f;
				player.GetDamage(damageClass) += 0.1f;
			}
		}
	}
}