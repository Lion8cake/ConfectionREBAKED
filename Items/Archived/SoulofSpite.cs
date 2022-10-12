using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Archived
{
	public class SoulofSpite : ModItem, IArchived
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Spite");
			Tooltip.SetDefault("'The essence of bloody creatures'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void PostUpdate() => Lighting.AddLight(Item.Center, Color.Crimson.ToVector3() * 0.55f * Main.essScale);

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void SetDefaults()
		{
			Item refItem = new();
			refItem.SetDefaults(ItemID.SoulofNight);
			Item.width = refItem.width;
			Item.height = refItem.height;
			Item.value = 1000;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
		}

		public int ArchivatesTo() => ItemID.SoulofNight;
	}
}