using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class GummyWispDye : ModItem
	{
		public override void SetStaticDefaults() {
			if (!Main.dedServ) {
				Ref<Effect> pixelShaderRef = Main.PixelShaderRef;
				GameShaders.Armor.BindShader(Item.type, new ArmorShaderData(pixelShaderRef, "ArmorWisp")).UseColor(0.4f, 1f, 0.5f).UseSecondaryColor(0f, 1f, 0.2f);
			}
			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults() {
			int dye = Item.dye;
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = Item.sellPrice(0, 1, 50);
			Item.rare = ItemRarityID.Orange;
			Item.dye = dye;
		}
	}
}
