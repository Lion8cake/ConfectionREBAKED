using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport.Thorium.Dusts;

public sealed class NeapoliniteChocolateDust : ModDust {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override void OnSpawn(Dust dust) {
        dust.noLight = true;
		dust.velocity *= 0.4f;
		dust.scale *= 0.9f;
    }
}