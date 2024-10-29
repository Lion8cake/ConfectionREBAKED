using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class ShiftingCreamsandsDye : ModItem
	{
		public override void SetStaticDefaults() {
			if (!Main.dedServ) {
				GameShaders.Armor.BindShader(
					Item.type,
					new ArmorShaderData(Main.PixelShaderRef, "ArmorShiftingSands")
				).UseImage("Images/Misc/noise")
				.UseColor(1f, 0.65f, 0.45f)
				.UseSecondaryColor(0.6f, 0.35f, 0.27f);
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
