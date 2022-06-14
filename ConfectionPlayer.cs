using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheConfectionRebirth.Biomes;

namespace TheConfectionRebirth
{
    public class ConfectionPlayer : ModPlayer
    {
        public bool RollerCookiePet;
        public bool CreamsandWitchPet;
        public bool minitureCookie;
        public bool littleMeawzer;
        public bool miniGastropod;
        public bool flyingGummyFish;
        public bool birdnanaLightPet;
        public bool MeawzerPet;
        public bool DudlingPet;
        public bool FoxPet;

        public Projectile DimensionalWarp;

        public override void ResetEffects()
        {
            RollerCookiePet = false;
            CreamsandWitchPet = false;
            minitureCookie = false;
            littleMeawzer = false;
            miniGastropod = false;
            flyingGummyFish = false;
            birdnanaLightPet = false;
            MeawzerPet = false;
            DudlingPet = false;
            FoxPet = false;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (damageSource.SourceCustomReason == "DimensionSplit")
            {
                WeightedRandom<string> deathmessage = new WeightedRandom<string>();
                deathmessage.Add(Player.name + " got lost in a rift.");
                deathmessage.Add(Player.name + " was split like a banana.");
                deathmessage.Add(Player.name + " tried to banana all the time.");
                deathmessage.Add(Player.name + " got split between dimensions.");
                damageSource = PlayerDeathReason.ByCustomReason(deathmessage);
                return true;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }

        //The is a debug that says what background the world has
        public override void OnEnterWorld(Player player)
        {
            if (ConfectionWorld.ConfectionSurfaceBG == -1)
            {
                Main.NewText("The world's background didn't update");
            }
            if (ConfectionWorld.ConfectionSurfaceBG == 0)
            {
                Main.NewText("This world has background 1");
            }
            if (ConfectionWorld.ConfectionSurfaceBG == 1)
            {
                Main.NewText("This world has background 2");
            }
            if (ConfectionWorld.ConfectionSurfaceBG == 2)
            {
                Main.NewText("This world has background 3");
            }
        }

        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            bool inWater = !attempt.inLava && !attempt.inHoney;
            bool inConfectionSurfaceBiome = Player.InModBiome(ModContent.GetInstance<ConfectionBiomeSurface>());

            if (inWater && inConfectionSurfaceBiome && attempt.crate)
            {
                if (!attempt.veryrare && !attempt.legendary && attempt.rare)
                {
                    itemDrop = ModContent.ItemType<Items.Placeable.BananaSplitCrate>();
                }
                if (!attempt.veryrare && !attempt.legendary && attempt.rare && Main.hardMode)
                {
                    itemDrop = ModContent.ItemType<Items.Placeable.ConfectionCrate>();
                }
            }
        }
    }
}