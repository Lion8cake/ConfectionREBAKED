using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace TheConfectionRebirth.ModSupport.Thorium.Buffs;

public sealed class RockinStar : NeapoliniteBuff {
	public const float BardDamageIncreasePerStage = 2f;
	public const float BardDamageIncreaseStage1 = BardDamageIncreasePerStage * 1f;
	public const float BardDamageIncreaseStage2 = BardDamageIncreasePerStage * 2f;
	public const float BardDamageIncreaseStage3 = BardDamageIncreasePerStage * 3f;
	public const float BardDamageIncreaseStage4 = BardDamageIncreasePerStage * 4f;
	public const float BardDamageIncreaseStage5 = BardDamageIncreasePerStage * 5f;
	public const int BardCritChanceIncreasePerStage = 2;
	public const int BardCritChanceIncreaseStage1 = BardCritChanceIncreasePerStage * 1;
	public const int BardCritChanceIncreaseStage2 = BardCritChanceIncreasePerStage * 2;
	public const int BardCritChanceIncreaseStage3 = BardCritChanceIncreasePerStage * 3;
	public const int BardCritChanceIncreaseStage4 = BardCritChanceIncreasePerStage * 4;
	public const int BardCritChanceIncreaseStage5 = BardCritChanceIncreasePerStage * 5;

	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override int DowngradeTime => 180;

	public override ref int GetCurrentStage(Player player) {
		return ref player.GetModPlayer<ThoriumDLCPlayer>().RockinStarStage;
	}

	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		base.ModifyBuffText(ref buffName, ref tip, ref rare);

		int stage = GetCurrentStage(Main.LocalPlayer);
		tip = Description.Format(GetBardDamageIncreaseByStage(stage), GetBardCritChanceIncreaseByStage(stage));
	}

	[JITWhenModsEnabled(TheConfectionRebirth.ThoriumModName)]
	public override void Update(Player player, ref int buffIndex) {
		base.Update(player, ref buffIndex);

		int stage = GetCurrentStage(player);
		player.GetDamage<BardDamage>() += GetBardDamageIncreaseByStage(stage) / 100f;
		player.GetCritChance<BardDamage>() += GetBardCritChanceIncreaseByStage(stage);
	}

	public override bool ReApply(Player player, int time, int buffIndex) {
		return false;
	}

	public static float GetBardDamageIncreaseByStage(int stage) {
		return stage switch {
			1 => BardDamageIncreaseStage1,
			2 => BardDamageIncreaseStage2,
			3 => BardDamageIncreaseStage3,
			4 => BardDamageIncreaseStage4,
			5 => BardDamageIncreaseStage5,
			_ => BardDamageIncreasePerStage * stage
		};
	}

	public static int GetBardCritChanceIncreaseByStage(int stage) {
		return stage switch {
			1 => BardCritChanceIncreaseStage1,
			2 => BardCritChanceIncreaseStage2,
			3 => BardCritChanceIncreaseStage3,
			4 => BardCritChanceIncreaseStage4,
			5 => BardCritChanceIncreaseStage5,
			_ => BardCritChanceIncreasePerStage * stage
		};
	}
}
