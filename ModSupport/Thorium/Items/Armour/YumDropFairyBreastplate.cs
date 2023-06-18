using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Armour
{
	[ExtendsFromMod("ThoriumMod")]
	[AutoloadEquip(new EquipType[]
	{
		EquipType.Body
	})]
	public class YumdropFairyBreastplate : ThoriumMod.Items.ThoriumItem
    {
		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}

		private int backSlot = -1;

		public override void Load() {
			if (!Main.dedServ) {
				backSlot = EquipLoader.AddEquipTexture(Mod, Texture + "_Back", (EquipType)5, null, Item.Name + "_Back", null);
			}
		}

		public override void SetStaticDefaults()
        {
			isHealer = true;
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ArmorIDs.Body.Sets.IncludedCapeBack[Item.bodySlot] = backSlot;
			ArmorIDs.Body.Sets.IncludedCapeBackFemale[Item.bodySlot] = backSlot;
		}

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 20;
            Item.value = 10000;
            Item.rare = ItemRarityID.Pink;
            Item.defense = 17;
        }

		public override void AddRecipes() {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod mod)) {
				CreateRecipe()
				.AddIngredient(mod.Find<ModItem>("LifeCell"), 3)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 16)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			}
		}

		public override void UpdateEquip(Player player)
        {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod source) && source.TryFind("HealerDamage", out DamageClass damageClass)) {
				player.GetCritChance(damageClass) += 0.06f;
				player.GetDamage(damageClass) += 0.15f;
				player.manaCost -= 0.1f;
			}
        }
    }
}