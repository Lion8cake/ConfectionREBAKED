using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class Cherimoya : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Cherimoya");


			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(233, 217, 184),
				new Color(237, 227, 215),
				new Color(112, 115, 43)
			};

			ItemID.Sets.IsFood[Type] = true;
		}

		public override void SetDefaults() {
			Item.DefaultToFood(22, 22, BuffID.WellFed2, 18000);
			Item.value = Item.buyPrice(0, 20);
			Item.rare = ItemRarityID.Orange;
		}
	}
}