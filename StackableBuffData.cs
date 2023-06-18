using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;

namespace TheConfectionRebirth {
	public class StackableBuffData {
		public static StackableBuffData SwirlySwarm;
		public static StackableBuffData ChocolateCharge;
		public static StackableBuffData StrawberryStrike;
		public static StackableBuffData VanillaValor;

		public class StackableBuffData_Loader : ILoadable {
			public void Load(Mod mod) {
			}

			public void Unload() {
				SwirlySwarm = null;
				ChocolateCharge = null;
				StrawberryStrike = null;
				VanillaValor = null;
			}
		}

		public static void PostSetupContent() {
			SwirlySwarm = new(
				ModContent.BuffType<SwirlySwarmI>(),
				ModContent.BuffType<SwirlySwarmII>(),
				ModContent.BuffType<SwirlySwarmIII>(),
				ModContent.BuffType<SwirlySwarmIV>(),
				ModContent.BuffType<SwirlySwarmV>());
			ChocolateCharge = new(
				ModContent.BuffType<ChocolateChargeI>(),
				ModContent.BuffType<ChocolateChargeII>(),
				ModContent.BuffType<ChocolateChargeIII>(),
				ModContent.BuffType<ChocolateChargeIV>(),
				ModContent.BuffType<ChocolateChargeV>());
			StrawberryStrike = new(
				ModContent.BuffType<StrawberryStrikeI>(),
				ModContent.BuffType<StrawberryStrikeII>(),
				ModContent.BuffType<StrawberryStrikeIII>(),
				ModContent.BuffType<StrawberryStrikeIV>(),
				ModContent.BuffType<StrawberryStrikeV>());
			VanillaValor = new(
				ModContent.BuffType<VanillaValorI>(),
				ModContent.BuffType<VanillaValorII>(),
				ModContent.BuffType<VanillaValorIII>(),
				ModContent.BuffType<VanillaValorIV>(),
				ModContent.BuffType<VanillaValorV>());
		}

		int[] BuffIDs;
		Dictionary<int, byte> IsBuff;
		int lastRank = -1;
		public StackableBuffData(params int[] buffs) {
			BuffIDs = buffs;
			IsBuff = new();
			for (byte x = 0; x < buffs.Length; x++) {
				IsBuff.Add(buffs[x], (byte)(x + 1));
			}
		}

		public void AscendBuff(Player player, int rank, int time, bool refresh = true) {
			int pos = FindBuff(player, out byte buffRank);
			int refreshTime = refresh ? 2 : time;
			if (rank == -1) {
				if (lastRank != -1 && lastRank == buffRank - 1)
					player.buffTime[pos] = time;
				lastRank = rank;
				return;
			}
			if (rank >= buffRank) {
				if (pos == -1)
					player.AddBuff(BuffIDs[rank], refreshTime);
				else {
					player.buffTime[pos] = refreshTime;
					player.buffType[pos] = BuffIDs[rank];
				}
			}
			else if (rank == buffRank - 1) {
				if (refresh || player.buffTime[pos] == 1)
					player.buffTime[pos] = 2;
			}
			else if (lastRank == buffRank - 1)
				player.buffTime[pos] = time;

			lastRank = rank;
		}

		public void DeleteBuff(Player player) {
			int buffPos = FindBuff(player, out byte _);
			if (buffPos == -1)
				return;
			player.DelBuff(buffPos);
		}
		//1-indexed
		public int FindBuff(Player player, out byte rank) {
			for (int i = 0; i < Player.MaxBuffs; i++) {
				if (player.buffTime[i] >= 1 && IsBuff.TryGetValue(player.buffType[i], out byte rankTemp)) {
					rank = rankTemp;
					return i;
				}
			}
			rank = 0;
			return -1;
		}
	}
}
