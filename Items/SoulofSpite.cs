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
			Item refItem = new();
			refItem.SetDefaults(ItemID.SoulofNight);
			Item.width = refItem.width;
			Item.height = refItem.height;
			Item.value = Item.sellPrice(silver: 2);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
		}

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, new Vector3(1.52f, 0.21f, 0.37f) * 0.22f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor) {
			return Color.White;
		}

		public override void AddRecipes()
		{
			Recipe.Create(ModContent.ItemType<KeyofSpite>())
				.AddIngredient<SoulofSpite>(15)
				.AddTile(TileID.WorkBenches)
				.Register();

			Recipe.Create(ItemID.CoolWhip)
				.AddIngredient(ItemID.SoulofLight, 8)
				.AddIngredient<SoulofSpite>(8)
				.AddIngredient(ItemID.FrostCore, 2)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			Recipe.Create(ItemID.CoolWhip)
				.AddIngredient<SoulofDelight>(8)
				.AddIngredient<SoulofSpite>(8)
				.AddIngredient(ItemID.FrostCore, 2)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			Recipe.Create(ItemID.SpiritFlame)
				.AddIngredient(ItemID.DjinnLamp)
				.AddIngredient(ItemID.AncientBattleArmorMaterial, 2)
				.AddIngredient<SoulofSpite>(12)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			Recipe.Create(ItemID.MagicalHarp)
				.AddIngredient(ItemID.Harp)
				.AddIngredient(ItemID.CrystalShard, 20)
				.AddIngredient<SoulofSpite>(8)
				.AddIngredient(ItemID.SoulofSight, 15)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			Recipe.Create(ItemID.DemonWings)
				.AddIngredient(ItemID.Feather, 10)
				.AddIngredient(ItemID.SoulofFlight, 20)
				.AddIngredient<SoulofSpite>(15)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			Recipe.Create(ItemID.MechanicalSkull)
				.AddIngredient(ItemID.Bone, 30)
				.AddRecipeGroup(RecipeGroupID.IronBar, 5)
				.AddIngredient<SoulofDelight>(3)
				.AddIngredient<SoulofSpite>(3)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			Recipe.Create(ItemID.MechanicalSkull)
				.AddIngredient(ItemID.Bone, 30)
				.AddRecipeGroup(RecipeGroupID.IronBar, 5)
				.AddIngredient(ItemID.SoulofLight, 3)
				.AddIngredient<SoulofSpite>(3)
				.AddTile(TileID.MythrilAnvil)
				.Register();

			Recipe.Create(ItemID.SoulofNight)
				.AddIngredient<SoulofSpite>()
				.AddTile(TileID.DemonAltar)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.SoulofNight)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}