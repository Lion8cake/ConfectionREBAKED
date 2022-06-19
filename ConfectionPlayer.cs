using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;
using TheConfectionRebirth.Util;

namespace TheConfectionRebirth
{
    public enum TimerDataType
    {
        MeleeDamage,
        MagicManaRegeneration,
    }
    public struct TimerData
    {
        public uint endTime;
        public int value;
        public TimerDataType type;

        public TimerData(int value, uint duration, TimerDataType type)
        {
            this.type = type;
            endTime = Main.GameUpdateCount + duration;
            this.value = value;
        }
        public static bool Comparer(TimerData first, TimerData second) {

            //overflow checks
            if (Main.GameUpdateCount > first.endTime && Main.GameUpdateCount < second.endTime) return true;
            if (Main.GameUpdateCount > second.endTime && Main.GameUpdateCount < first.endTime) return false;
            return first.endTime <= second.endTime;
        }
    }
    public class StackableBuffData
    {
        public static StackableBuffData SwirlySwarm;
        public static StackableBuffData ChocolateCharge;
        public static StackableBuffData StrawberryStrike;
        public static StackableBuffData VanillaValor;

        public class StackableBuffData_Loader : ILoadable
        {
            public void Load(Mod mod)
            {
            }

            public void Unload()
            {
                SwirlySwarm = null;
                ChocolateCharge = null;
                StrawberryStrike = null;
                VanillaValor = null;
            }
        }

        public static void PostSetupContent()
        {
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
            for (byte x = 0; x < buffs.Length; x++)
            {
                IsBuff.Add(buffs[x], (byte)(x + 1));
            }
        }

        public void AscendBuff(Player player, int rank, int time, bool refresh = true) {
            int pos = FindBuff(player, out byte buffRank);
            if (rank >= buffRank)
            {
                if (pos == -1)
                    player.AddBuff(BuffIDs[rank], time);
                else
                {
                    player.buffTime[pos] = 2;
                    player.buffType[pos] = BuffIDs[rank];
                }
            }
            else if (rank == buffRank - 1)
            {
                if (refresh)
                    player.buffTime[pos] = 2;
            }
            else if(lastRank == buffRank - 1)
                player.buffTime[pos] = time;

            lastRank = rank;
        }

        public void DeleteBuff(Player player) {
            player.DelBuff(FindBuff(player, out byte _));
        }
        public int FindBuff(Player player, out byte rank)
        {
            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                if (player.buffTime[i] >= 1 && IsBuff.TryGetValue(player.buffType[i], out byte rankTemp))
                {
                    rank = rankTemp;
                    return i;
                }
            }
            rank = 0;
            return -1;
        }
    }
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
        public bool NeapoliniteMagicSet;
        public bool NeapoliniteSummonerSet;

        public Projectile DimensionalWarp;

        public float neapoliniteSummonTimer;

        public BinaryHeap<TimerData> Timer;
        public int VanillaValorDamageDealt;
        public int ManaConsumed;

        public override void OnEnterWorld(Player player)
        {
            Timer = new(TimerData.Comparer);
            VanillaValorDamageDealt = 0;
            ManaConsumed = 0;
        }
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
            NeapoliniteMagicSet = false;
            NeapoliniteSummonerSet = false;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (damageSource.SourceCustomReason == "DimensionSplit")
            {
                WeightedRandom<string> deathmessage = new();
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
        /*public override void OnEnterWorld(Player player)
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
        }*/

        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            bool inWater = !attempt.inLava && !attempt.inHoney;
            bool inConfectionSurfaceBiome = Player.InModBiome(ModContent.GetInstance<ConfectionBiomeSurface>());

            if (inWater && inConfectionSurfaceBiome && attempt.crate)
            {
                if (!attempt.veryrare && !attempt.legendary && attempt.rare)
                {
                    itemDrop = !Main.hardMode ? ModContent.ItemType<Items.Placeable.BananaSplitCrate>() : ModContent.ItemType<Items.Placeable.ConfectionCrate>();
                }
            }
        }

        public override void PostUpdate()
        {
            if (NeapoliniteSummonerSet)
            {
                neapoliniteSummonTimer++;
                float progress = (neapoliniteSummonTimer / (8 * 60));
                int rank = (int)progress;
                int timer = rank == 5 ? 2 : (int)(8 * 60 * (1 - progress % 1));
                if (rank != 0) //max byte
                    StackableBuffData.SwirlySwarm.AscendBuff(Player, rank - 1, timer, rank == 5);
                if (neapoliniteSummonTimer >= 2400)
                {
                    neapoliniteSummonTimer = 2400;
                }
                if (NeapoliniteSummonerSet == false)
                {
                    neapoliniteSummonTimer = 0;
                }
            }
            while (Timer.items.Count > 0 && Timer.items[0].endTime == Main.GameUpdateCount)
            {
                TimerData top = Timer.Pop();
                switch (top.type)
                {
                    case TimerDataType.MeleeDamage:
                        VanillaValorDamageDealt -= top.value;
                        break;
                    case TimerDataType.MagicManaRegeneration:
                        ManaConsumed -= top.value;
                        break;
                }
            }
            //Main.NewText(VanillaValorDamageDealt);
        }

        public override void OnConsumeMana(Item item, int manaConsumed)
        {
            if (item.CountsAsClass(DamageClass.Magic))
            {
                this.VanillaValorDamageDealt += manaConsumed;
                Timer.Add(new(manaConsumed, 180, TimerDataType.MagicManaRegeneration));
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (item.DamageType.CountsAsClass(DamageClass.Melee))
                AddDamage(damage);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (proj.DamageType.CountsAsClass(DamageClass.Melee))
                AddDamage(damage);
        }

        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            if (item.DamageType.CountsAsClass(DamageClass.Melee))
                AddDamage(damage);
        }

        public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)
        {
            if (proj.DamageType.CountsAsClass(DamageClass.Melee))
                AddDamage(damage);
        }

        void AddDamage(int damage)
        {
            this.VanillaValorDamageDealt += damage;
            Timer.Add(new(damage, 300, TimerDataType.MeleeDamage));
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            neapoliniteSummonTimer = 0;
        }
    }
}
