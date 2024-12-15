using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class SoulofSpite : ModItem {
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));

			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;

			Item.ResearchUnlockCount = 25;
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 2);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
		}

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, new Vector3(1.52f, 0.21f, 0.37f) * 0.22f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor) {
			return new Color(255, 255, 255, byte.MaxValue - 50);
		}
	}
}