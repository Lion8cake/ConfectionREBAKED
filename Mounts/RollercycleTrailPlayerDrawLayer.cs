using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Mounts;

public sealed class RollercycleTrailPlayerDrawLayer : PlayerDrawLayer {
	public override Position GetDefaultPosition() {
		// We draw after `Wings` layer cause Celestial Starboard is
		// rendered here. It's also way before mount drawing.
		//
		// Celestial Starboard isn't rendered thanks to the following method:
		// * Player.ShouldDrawWingsThatAreAlwaysAnimated()
		//
		// (seeminly same applies to the rest of wings, but we don't care about other wings)

		return new AfterParent(PlayerDrawLayers.Wings);
	}

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
		// Return true only if player is on Rollercycle.

		if (!drawInfo.drawPlayer.mount.Active)
			return false;

		if (drawInfo.drawPlayer.mount.Type != ModContent.MountType<Rollercycle>())
			return false;

		return true;
	}

	protected override void Draw(ref PlayerDrawSet drawInfo) {
		float opacity = CalculateOpacity(in drawInfo);

		// No point to render if its not going to be visible anyway.
		if (opacity <= 0f)
			return;

		// Store old amount of draw data, draw the trail, update new draw data.
		int oldDrawDataCacheCount = drawInfo.DrawDataCache.Count;

		CalculateTrailDrawingValues(in drawInfo, out var commonWingPosPreFloor, out var directions);

		PlayerDrawLayers.DrawStarboardRainbowTrail(ref drawInfo, commonWingPosPreFloor, directions);

		float newDrawDataCacheCount = drawInfo.DrawDataCache.Count;

		// Modify all draw data that was drawn from DrawStarboardRainbowTrail call.
		// Have to do this for the sake of trail not appearing out of thin air.
		for (int i = oldDrawDataCacheCount; i < newDrawDataCacheCount; i++) {
			// `0f` is first trail element, `1f` is last trail element.
			float trailOrder = (i - oldDrawDataCacheCount) / (newDrawDataCacheCount - oldDrawDataCacheCount);

			var drawData = drawInfo.DrawDataCache[i];

			drawData.color *= MathHelper.SmoothStep(0f, opacity, trailOrder);

			// Change dye to mount dye. There may or may not be a dye, but it's already handled for us.
			drawData.shader = drawInfo.drawPlayer.cMount;

			drawInfo.DrawDataCache[i] = drawData;
		}
	}

	private static float CalculateOpacity(in PlayerDrawSet drawInfo) {
		// Calculate opacity for nice transition between movement.

		float speedRequiredForMaxRainbow = MphToSpeed(68);
		float playerHorizontalSpeed = Math.Abs(drawInfo.drawPlayer.velocity.X);

		float strength = Math.Clamp(playerHorizontalSpeed / speedRequiredForMaxRainbow, 0f, 1f);

		float opacity = MathHelper.SmoothStep(0f, 5f, strength) / 5f;

		return opacity;
	}

	private static void CalculateTrailDrawingValues(in PlayerDrawSet drawInfo, out Vector2 commonWingPosPreFloor, out Vector2 directions) {
		// Calculate values for drawing. Taken from PlayerDrawLayers.DrawPlayer_09_Wings

		float bodyWidth = drawInfo.drawPlayer.width / 2;
		float bodyHeight = drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height / 2 + 7f;

		commonWingPosPreFloor = drawInfo.Position - Main.screenPosition + new Vector2(bodyWidth, bodyHeight);
		directions = drawInfo.drawPlayer.Directions;
	}

	private static float MphToSpeed(int mph) {
		// https://terraria.wiki.gg/wiki/Stopwatch

		const float pixelsPerTickVelocity = 216000f / 42240f;

		return mph / pixelsPerTickVelocity;
	}
}