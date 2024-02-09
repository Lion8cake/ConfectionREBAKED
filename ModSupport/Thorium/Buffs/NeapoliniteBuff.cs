using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport.Thorium.Buffs;

public abstract class NeapoliniteBuff : ModBuff {
	private Asset<Texture2D> _iconTextureCache;

	public static Asset<Texture2D> NeapoliniteBuffOverlay { get; private set; }

	public sealed override string Texture => "TheConfectionRebirth/Assets/EmptyBuff";

	public virtual int StageCount => 5;
	public virtual int DowngradeTime => 480;

	public virtual string IconTexture => base.Texture;

	public override void Load() {
		if (Main.dedServ)
			return;

		_iconTextureCache = ModContent.Request<Texture2D>(IconTexture);
		NeapoliniteBuffOverlay = Mod.Assets.Request<Texture2D>("ModSupport/Thorium/Buffs/NeapoliniteBuffOverlay");
	}

	public override void Unload() {
		_iconTextureCache = null;
	}

	public override void SetStaticDefaults() {
		Main.pvpBuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}

	public abstract ref int GetCurrentStage(Player player);

	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		int currentStage = GetCurrentStage(Main.LocalPlayer);

		buffName = DisplayName.Format(currentStage.ToRoman());
	}

	public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams) {
		int stage = GetCurrentStage(Main.LocalPlayer) - 1;

		var sourceRect = new Rectangle(32 * stage, 0, 32, 32);

		spriteBatch.Draw(_iconTextureCache.Value, drawParams.Position, sourceRect, drawParams.DrawColor);
		spriteBatch.Draw(NeapoliniteBuffOverlay.Value, drawParams.Position, sourceRect, drawParams.DrawColor);
		return false;
	}

	public override bool ReApply(Player player, int time, int buffIndex) {
		ref int currentStage = ref GetCurrentStage(player);
		currentStage = Math.Min(currentStage + 1, StageCount);

		return false;
	}

	public override void Update(Player player, ref int buffIndex) {
		ref int currentStage = ref GetCurrentStage(player);

		if (currentStage == 0) {
			currentStage = 1;
		}

		if (player.buffTime[buffIndex] == 1) {
			currentStage -= 2;

			if (currentStage > 0) {
				player.buffTime[buffIndex] = DowngradeTime;
			}
			else {
				currentStage = 0;

				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}
