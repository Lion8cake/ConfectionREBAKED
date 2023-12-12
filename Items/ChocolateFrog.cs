using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using TheConfectionRebirth.NPCs.Critters;

namespace TheConfectionRebirth.Items
{
	[LegacyName("ChocolateFrogItem")]
	public class ChocolateFrog : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 5;
		}

		public override void SetDefaults() {
			Item.useStyle = 1;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.maxStack = 9999;
			Item.consumable = true;
			Item.width = 12;
			Item.height = 12;
			Item.noUseGraphic = true;
			Item.makeNPC = ModContent.NPCType<NPCs.Critters.ChocolateFrog>();
			Item.value = Item.sellPrice(0, 0, 10);
		}

		public override void AddRecipes() {
			Recipe.Create(ItemID.FroggleBunwich)
			   .AddIngredient(this, 2)
			   .AddTile(TileID.CookingPots)
			   .Register();
		}
	}
}