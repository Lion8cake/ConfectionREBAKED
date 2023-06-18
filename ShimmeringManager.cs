using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Armor;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Items.Weapons;
using TheConfectionRebirth.NPCs;
using TheConfectionRebirth.NPCs.Critters;

namespace TheConfectionRebirth
{
	public class ShimmeringManager
	{
		private interface IShimmer
		{
			int Input { get; }
			int Output { get; }
			void Add();
		}

		private struct ItemShimmer : IShimmer
		{
			public int Input { get; }
			public int Output { get; }

			public ItemShimmer(int input, int output)
			{
				Input = input;
				Output = output;
			}

			public void Add() => ItemID.Sets.ShimmerTransformToItem[Input] = Output;
		}

		private struct NPCShimmer : IShimmer
		{
			public int Input { get; }
			public int Output { get; }
			private readonly bool isItem;

			public NPCShimmer(int input, int output, bool item = false)
			{
				Input = input;
				Output = output;
				isItem = item;
			}

			public void Add()
			{
				if (isItem)
				{
					NPCID.Sets.ShimmerTransformToItem[Input] = Output;
					return;
				}

				NPCID.Sets.ShimmerTransformToNPC[Input] = Output;
			}
		}

		private static List<IShimmer> shimmers = new();

		public void Load(Mod mod)
		{
			IT<SherbetTorch>(ItemID.ShimmerTorch);
			IT<CreamWood>(ItemID.Wood);
			IT<Cherimoya>(ItemID.Ambrosia);

			I<ConfectionCrate, BananaSplitCrate>();
			I<Creamsandstone, Creamsand>();
			I<HardenedCreamsand, Creamsand>();
			I<OrangeIce, CreamBlock>();
			I<ConfectionBiomeKey, ConfectionBiomeChestItem>();

			ITO<AncientNeapoliniteHat, NeapoliniteHat>();
			ITO<AncientNeapoliniteHeadgear, NeapoliniteHeadgear>();
			ITO<AncientNeapoliniteHelmet, NeapoliniteHelmet>();
			ITO<AncientCosmicCookieCannon, CosmicCookieCannon>();

			IO<CreamBeans>(ItemID.HallowedSeeds);
			IT<CreamBeans>(ItemID.HallowedSeeds);


			NT<ChocolateBunny>(NPCID.Shimmerfly);
			NT<ChocolateFrog>(NPCID.Shimmerfly);
			NT<GrumbleBee>(NPCID.Shimmerfly);
			NT<Birdnana>(NPCID.Shimmerfly);
			NT<CherryBug>(NPCID.Shimmerfly);
			NT<GummyWorm>(NPCID.Shimmerfly);
			NT<Pip>(NPCID.Shimmerfly);

			NT<SherbetSlime>(NPCID.ShimmerSlime);

			foreach (var s in shimmers)
			{
				s.Add();
			}
			shimmers.Clear();
			shimmers = null;
		}

		private static void ITO<T, F>() where T : ModItem where F : ModItem
		{
			I<T, F>();
			I<F, T>();
		}
		private static void I<T, F>() where T : ModItem where F : ModItem => I(new ItemShimmer(ModContent.ItemType<T>(), ModContent.ItemType<F>()));
		private static void IO<T>(int input) where T : ModItem => I(new ItemShimmer(input, ModContent.ItemType<T>()));
		private static void IT<T>(int output) where T : ModItem => I(new ItemShimmer(ModContent.ItemType<T>(), output));
		private static void I(ItemShimmer item) => shimmers.Add(item);

		private static void NO<T>(int input, bool item = false) where T : ModNPC => N(new NPCShimmer(input, ModContent.NPCType<T>(), item));
		private static void NT<T>(int output, bool item = false) where T : ModNPC => N(new NPCShimmer(ModContent.NPCType<T>(), output, item));
		private static void N(NPCShimmer npc) => shimmers.Add(npc);

		public ShimmeringManager() => Load(null);
	}
}
