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
	[LegacyName("GrumbleBeeItem")]
	public class GrumbleBee : ModItem
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
			Item.bait = 40;
			Item.makeNPC = ModContent.NPCType<NPCs.Critters.GrumbleBee>();
			Item.value = Item.sellPrice(0, 0, 5);
		}
	}
}