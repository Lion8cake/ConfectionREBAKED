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
	[AutoloadEquip(EquipType.Body)]
    public class RockCandyConemail : ThoriumMod.Items.BardItem
    {
		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}

		public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetBardDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 10000;
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 16;
        }

		public override void AddRecipes() {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod mod)) {
				CreateRecipe()
				.AddIngredient(mod.Find<ModItem>("StrangePlating"), 5)
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			}
		}

		public override void UpdateEquip(Player player)
        {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod source) && source.TryFind("BardDamage", out DamageClass damageClass)) {
				player.GetCritChance(damageClass) += 0.05f;
			}
        }
    }
}