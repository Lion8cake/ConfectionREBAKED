using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Armor;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
[AutoloadEquip(EquipType.Body)]
public class YumdropFairyBreastplate : ThoriumItem {
	public const float HealerDamageIncrease = 15f;
	public const int HealerCritChanceIncrease = 6;
	public const float ManaCostReduction = 10f;

	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(HealerDamageIncrease, HealerCritChanceIncrease, ManaCostReduction);

	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	private int backSlot = -1;

	public override void Load() {
		if (!Main.dedServ) {
			backSlot = EquipLoader.AddEquipTexture(Mod, Texture + "_Back", EquipType.Back, null, Item.Name + "_Back", null);
		}
	}

	public override void SetStaticDefaults() {
		ArmorIDs.Body.Sets.IncludedCapeBack[Item.bodySlot] = backSlot;
		ArmorIDs.Body.Sets.IncludedCapeBackFemale[Item.bodySlot] = backSlot;

		isHealer = true;
	}

	public override void SetDefaults() {
		Item.width = 38;
		Item.height = 20;

		Item.defense = 17;

		Item.value = Item.sellPrice(gold: 1);
		Item.rare = ItemRarityID.Pink;
	}

	public override void UpdateEquip(Player player) {
		player.GetDamage<HealerDamage>() += HealerDamageIncrease / 100f;
		player.GetCritChance<HealerDamage>() += HealerCritChanceIncrease;
		player.manaCost = ManaCostReduction / 100f;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<LifePoweredEnergyCell>(3)
			.AddIngredient<NeapoliniteBar>(16)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}