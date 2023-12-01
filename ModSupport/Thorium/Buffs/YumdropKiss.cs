using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace TheConfectionRebirth.ModSupport.Thorium.Buffs;

public sealed class YumdropKiss : NeapoliniteBuff {
	public const float HealerDamageIncreasePerStage = 1f;
	public const float HealerDamageIncreaseStage1 = HealerDamageIncreasePerStage * 1f;
	public const float HealerDamageIncreaseStage2 = HealerDamageIncreasePerStage * 2f;
	public const float HealerDamageIncreaseStage3 = HealerDamageIncreasePerStage * 3f;
	public const float HealerDamageIncreaseStage4 = HealerDamageIncreasePerStage * 4f;
	public const float HealerDamageIncreaseStage5 = HealerDamageIncreasePerStage * 5f;
	public const int HealerCritChanceIncreasePerStage = 2;
	public const int HealerCritChanceIncreaseStage1 = HealerCritChanceIncreasePerStage * 1;
	public const int HealerCritChanceIncreaseStage2 = HealerCritChanceIncreasePerStage * 2;
	public const int HealerCritChanceIncreaseStage3 = HealerCritChanceIncreasePerStage * 3;
	public const int HealerCritChanceIncreaseStage4 = HealerCritChanceIncreasePerStage * 4;
	public const int HealerCritChanceIncreaseStage5 = HealerCritChanceIncreasePerStage * 5;
	public const int HealerLifeBonusIncreasePerStage = 1;
	public const int HealerLifeBonusIncreaseStage1 = HealerLifeBonusIncreasePerStage * 1;
	public const int HealerLifeBonusIncreaseStage2 = HealerLifeBonusIncreasePerStage * 2;
	public const int HealerLifeBonusIncreaseStage3 = HealerLifeBonusIncreasePerStage * 3;
	public const int HealerLifeBonusIncreaseStage4 = HealerLifeBonusIncreasePerStage * 4;
	public const int HealerLifeBonusIncreaseStage5 = HealerLifeBonusIncreasePerStage * 5;

	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override ref int GetCurrentStage(Player player) {
		return ref player.GetModPlayer<ThoriumDLCPlayer>().YumdropKissStage;
	}

	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		base.ModifyBuffText(ref buffName, ref tip, ref rare);

		int stage = GetCurrentStage(Main.LocalPlayer);
		tip = Description.Format(GetHealerDamageIncreaseByStage(stage), GetHealerCritChanceIncreaseByStage(stage));
	}

	[JITWhenModsEnabled("ThoriumMod")]
	public override void Update(Player player, ref int buffIndex) {
		base.Update(player, ref buffIndex);

		int stage = GetCurrentStage(player);
		player.GetDamage<HealerDamage>() += GetHealerDamageIncreaseByStage(stage) / 100f;
		player.GetCritChance<HealerDamage>() += GetHealerCritChanceIncreaseByStage(stage);
		player.GetModPlayer<ThoriumPlayer>().healBonus += GetLifeBonusIncreaseByStage(stage);
	}

	public static float GetHealerDamageIncreaseByStage(int stage) {
		return stage switch {
			1 => HealerDamageIncreaseStage1,
			2 => HealerDamageIncreaseStage2,
			3 => HealerDamageIncreaseStage3,
			4 => HealerDamageIncreaseStage4,
			5 => HealerDamageIncreaseStage5,
			_ => HealerDamageIncreasePerStage * stage
		};
	}

	public static int GetHealerCritChanceIncreaseByStage(int stage) {
		return stage switch {
			1 => HealerCritChanceIncreaseStage1,
			2 => HealerCritChanceIncreaseStage2,
			3 => HealerCritChanceIncreaseStage3,
			4 => HealerCritChanceIncreaseStage4,
			5 => HealerCritChanceIncreaseStage5,
			_ => HealerCritChanceIncreasePerStage * stage
		};
	}

	public static int GetLifeBonusIncreaseByStage(int stage) {
		return stage switch {
			1 => HealerLifeBonusIncreaseStage1,
			2 => HealerLifeBonusIncreaseStage2,
			3 => HealerLifeBonusIncreaseStage3,
			4 => HealerLifeBonusIncreaseStage4,
			5 => HealerLifeBonusIncreaseStage5,
			_ => HealerLifeBonusIncreasePerStage * stage
		};
	}
}
