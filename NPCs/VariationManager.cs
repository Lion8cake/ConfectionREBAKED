using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.NPCs
{
	public static class VariationManager<T> where T : ModNPC
	{
		private static readonly Func<bool> AlwaysTrue = () => true;
		private static Dictionary<string, VariationGroup> groups = new();
		private static List<VariationGroup> groups2 = new();
		private static List<string> groups3 = new();
		private static List<string> groupsThatForNormal = new();
		public static int Count => groups.Count;

		public static void AddGroup(string groupName, Asset<Texture2D> asset, Func<bool> condition = null)
		{
			if (groups == null)
			{
				groups = new();
				groups2 = new();
				groups3 = new();
			}

			if (groups?.ContainsKey(groupName) == false && !groups.Any(x => x.Value.Index == Count))
			{
				if (condition == null)
				{
					condition ??= AlwaysTrue;
					if (groupsThatForNormal == null)
						groupsThatForNormal = new();

					groupsThatForNormal.Add(groupName);
				}

				VariationGroup group = new(groupName, condition, asset)
				{
					Index = (sbyte)Count
				};
				if (group.Index == -1)
					throw new NotFiniteNumberException($"Too many variations have been added to {typeof(T).Name}!");
				groups.Add(groupName, group);
				groups2.Add(group);
				groups3.Add(groupName);
			}
			else if (asset != null && groups3.Contains(groupName))
			{
				groups?[groupName].Add(asset);
				groups2?[groups3.IndexOf(groupName)].Add(asset);
			}
		}

		public static (VariationGroup group, string groupName, Asset<Texture2D> asset) GetRandom()
		{
			List<VariationGroup> normalGroups = new();
			List<VariationGroup> otherGroups = new();
			foreach (var g in groupsThatForNormal)
				normalGroups.Add(groups[g]);
			if (normalGroups.Count == 0)
				throw new InvalidOperationException();
			else if (normalGroups.Count == Count)
				goto skip;

			foreach (var g in groups)
			{
				if (groupsThatForNormal.Contains(g.Key))
					continue;

				if (g.Value)
					otherGroups.Add(g.Value);
			}

		skip:
			VariationGroup ourGroup;
			if (otherGroups.Count > 0)
			{
				ourGroup = Main.rand.Next(otherGroups);
				goto ret;
			}

			ourGroup = Main.rand.Next(normalGroups);

		ret:
			return (ourGroup, ourGroup, ourGroup.Get());
		}

		public static VariationGroup GetRandomGroup() => GetRandom().group;

		public static Asset<Texture2D> GetRandomAsset() => GetRandom().asset;

		public static VariationGroup GetByIndex(int index) => groups2[index];

		public static void Clear()
		{
			groupsThatForNormal?.Clear();
			groupsThatForNormal = null;
			groups?.Clear();
			groups = null;
			groups2?.Clear();
			groups2 = null;
			groups3?.Clear();
			groups3 = null;
		}
	}

	public struct VariationGroup
	{
		private readonly string name;
		private Asset<Texture2D> assets;
		private readonly Func<bool> condition;

		public sbyte Index { get; internal set; }

		public static readonly VariationGroup Empty = default;

		public VariationGroup(string name, Func<bool> condition, Asset<Texture2D> asset)
		{
			Index = -1;
			this.name = name;
			assets = asset;
			this.condition = condition;
		}

		public void Add(Asset<Texture2D> asset) => assets = asset;

		public Asset<Texture2D> Get() => assets;

		public void Clear()
		{
			Index = 0;
			assets = null;
		}

		public override bool Equals([NotNullWhen(true)] object obj) => obj is VariationGroup other && other.Index == Index;

		public override int GetHashCode() => HashCode.Combine(name, assets, condition, Index);

		public static implicit operator int(VariationGroup group) => group.Index;
		public static implicit operator bool(VariationGroup group) => group.condition();
		public static implicit operator string(VariationGroup group) => group.name;

		public static bool operator ==(VariationGroup left, VariationGroup right) => left.Equals(right);

		public static bool operator !=(VariationGroup left, VariationGroup right) => !(left == right);
	}
}
