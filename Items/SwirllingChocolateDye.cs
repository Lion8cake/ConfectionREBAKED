using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
	public class SwirllingChocolateDye : ModItem
	{
		public override void SetStaticDefaults() {
			if (!Main.dedServ) {
				GameShaders.Armor.BindShader(
					Item.type,
					new ArmorShaderData(
						new Ref<Effect>(Mod.Assets.Request<Effect>("Shaders/SwirlsandDye",
						AssetRequestMode.ImmediateLoad).Value
					), "SwirlsandDyeShaderPass")
					.UseColor(new Color(1.0f, 0.65f, 0.45f))
				);
			}
			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = Item.sellPrice(0, 1, 50);
			Item.rare = ItemRarityID.Orange;
		}
	}
}
