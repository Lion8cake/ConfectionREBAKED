using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace TheConfectionRebirth
{
	using static SummonersShineCompat;
	using static SummonersShineCompat.MinionPowerCollection;
	// Copy this file for your mod and change the namespace above to yours
	/// <summary>
	/// Central file used for mod.Call wrappers.
	/// </summary>
	
	internal class SummonersShineCompat : ModSystem
	{
		internal static readonly Version apiVersion = new Version(0, 0, 1);

		internal static Mod SummonersShine
		{
			get
			{
				if (summonersShine == null && ModLoader.TryGetMod("SummonersShine", out var mod))
				{
					summonersShine = mod;
				}
				return summonersShine;
			}
		}
		private static Mod summonersShine;

		public override void Load()
		{
		}

		public override void Unload()
		{
			summonersShine = null;
		}

		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Changes a projectile config setting in the config.
		/// </summary>
		/// <param name="ConfigType">The Projectile Config Tyoe</param>
		/// <param name="ProjectileType">The Projectile Type</param>
		internal static void AddModdedConfig(ProjectileConfig ConfigType, int ProjectileType)
		{
			SummonersShine.Call(0, (int)ConfigType + 1, ProjectileType);
		}
		/// <summary>
		/// Call this in <see cref="ModItem.SetStaticDefaults"/>. Changes an item config setting in the config.
		/// </summary>
		/// <param name="ConfigType">The Item Config Tyoe</param>
		/// <param name="ItemType">The Item Type</param>
		internal static void AddModdedConfig(ItemConfig ConfigType, int ItemType)
		{
			int configType = 0;
			switch (ConfigType)
			{
				case ItemConfig.ItemIgnoresCustomSpecialPower:
					configType = 0;
					break;
				case ItemConfig.ItemNonPrefixable:
					configType = 7;
					break;
				case ItemConfig.ItemRetainManaCost:
					configType = 10;
					break;
				case ItemConfig.ItemCountAsWhip:
					configType = 14;
					break;
			}
			SummonersShine.Call(0, configType, ItemType);
		}

		/// <summary>
		/// Call this in <see cref="ModBuff.SetStaticDefaults"/>. Causes a buff to display the energy of all summoned minions associated with an item.
		/// </summary>
		/// <param name="BuffType">The Buff Type</param>
		/// <param name="ItemType">The Item Type</param>
		internal static void SetBuffDisplayMinionEnergy(int BuffType, params int[] ItemType)
		{
			SummonersShine.Call(0, 11, BuffType, ItemType);
		}

		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Changes the outgoing damage modifier.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type</param>
		/// <param name="DamageModifier">The Damage Modifier. Ranges from 0 to 1.</param>
		internal static void SetProjectileOutgoingDamageModifier(int ProjectileType, float DamageModifier)
		{

			SummonersShine.Call(0, 12, ProjectileType, DamageModifier);
		}

		/// <summary>
		/// Call this in <see cref="ModItem.SetStaticDefaults"/>. Changes the available default special abilities for the item.
		/// </summary>
		/// <param name="ItemType">The Item Type</param>
		/// <param name="enabledData">The enabled array of special abilities. Refer to the Spoilers thread for default special ability ID information.</param>
		internal static void SetItemDefaultSpecialAbilityValid(int ItemType, params BitsByte[] enabledData)
		{

			SummonersShine.Call(0, 13, ItemType, enabledData);
		}

		/// <summary>
		/// Call this in <see cref="ModBuff.SetStaticDefaults"/>. Overrides the buff minion energy display data, allowing you to display arbitrary data<br />
		/// Return false in Item1 if you want to skip drawing altogether.
		/// </summary>
		/// <param name="BuffType">The Buff Type</param>
		/// <param name="GetPinPositionsOverride">The function to get the pin positions denoting the highest and lowest energy percentage recharge progress.<br />
		/// Highest pin is ReturnTuple.Item2<br />
		/// Lowest pin is ReturnTuple.Item3<br />
		/// Highest and Lowest range from 0 to 1.<br />
		/// delegate Tuple&lt;bool, float, float&gt; GetPinPositions(List&lt;Projectile&gt; Valid, int BuffType, int ItemType);
		/// </param>
		internal static void SetBuffDisplayPinPositionsOverride(int BuffType, Func<int, int, List<Projectile>, Tuple<bool, float, float>> GetPinPositionsOverride)
		{
			SummonersShine.Call(17, BuffType, 0, GetPinPositionsOverride);
		}
		public delegate Tuple<bool, float, float> GetPinPositions(List<Projectile> Valid, int BuffType, int ItemType);

		/// <summary>
		/// Call this in <see cref="ModBuff.SetStaticDefaults"/>. Allows you to assign multiple minions to a singular buff<br />
		/// Return false in Item1 if you want to skip drawing altogether.
		/// </summary>
		/// <param name="BuffType">The Buff Type</param>
		/// <param name="GetLinkedItemsOverride">The function to get all linked item types related to this projectile.<br />
		/// delegate int[] GetLinkedItemsOverride(int BuffType);
		/// </param>
		internal static void SetBuffDisplayLinkedItemsOverride(int BuffType, Func<int, int[]> GetLinkedItemsOverride)
		{
			SummonersShine.Call(17, BuffType, 1, GetLinkedItemsOverride);
		}

		/// <summary>
		/// Call this in <see cref="ModItem.SetStaticDefaults"/>. Changes the source item ID of a projectile on-load.
		/// </summary>
		/// <param name="ProjType">The Projectile Type</param>
		/// <param name="ItemType">The Item Type</param>
		internal static void SetProjSourceItemType(int ProjType, int ItemType)
		{
			SummonersShine.Call(0, 8, ProjType, ItemType);
		}


		/// <summary>
		/// Call this in <see cref="Mod.Load"/> or <see cref="Mod.PostSetupContent"/>. Allows projectiles in your mod to ignore the setting "Disable Modded Minion Relative Velocity and Aimbot
		/// </summary>
		/// <param name="Mod">The Mod Instance of your mod</param>
		/// <param name="type">The type of whitelist</param>
		internal static void WhitelistMod(Mod mod, WhitelistModType type = WhitelistModType.all)
		{
			SummonersShine.Call(0, 6, mod, type - 1);
		}

		public enum WhitelistModType
		{
			all,
			tracking,
			damage,
		}

		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the Max Energy of all copies of the projectile upon creation.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MaxEnergy">The number of game ticks the staff takes to recharge. 1 second = 60 game ticks</param>
		internal static void ProjectileOnCreate_SetMaxEnergy(int ProjectileType, float MaxEnergy)
		{
			SummonersShine.Call(1, ProjectileType, 0, MaxEnergy);
		}
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the Minion Tracking Acceleration of all copies of the projectile upon creation.<br />
		/// When tracking a target, accelerates the projectile towards the target until the projectile matches the target's velocity.<br />
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionTrackingAcceleration">The magnitude of velocity to accelerate every tick</param>
		internal static void ProjectileOnCreate_SetMinionTrackingAcceleration(int ProjectileType, float MinionTrackingAcceleration)
		{
			SummonersShine.Call(1, ProjectileType, 1, MinionTrackingAcceleration);
		}
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the Minion Tracking Imperfection of all copies of the projectile upon creation.<br />
		/// When tracking a target, subtracts this amount from the magnitude of the target's working velocity. This ensures that the minion still tails behind the target to a certain extent.<br />
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionTrackingImperfection">The magnitude of velocity to subtract from the target's working velocity</param>
		internal static void ProjectileOnCreate_SetMinionTrackingImperfection(int ProjectileType, float MinionTrackingImperfection)
		{
			SummonersShine.Call(1, ProjectileType, 2, MinionTrackingImperfection);
		}

		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the Armor Ignore Percentage of all copies of the projectile upon creation.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="ArmorIgnoredPerc">The % of armor to ignore. Defaults to 0.</param>
		internal static void ProjectileOnCreate_SetArmorIgnoredPerc(int ProjectileType, float ArmorIgnoredPerc)
		{
			SummonersShine.Call(1, ProjectileType, 26, ArmorIgnoredPerc);
		}
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the OnPostCreation hook of all copies of the projectile upon creation.<br />
		/// This hook is called during the very first AI loop at the start of PreAI. This only runs once.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="OnPostCreation">
		/// The function to run<br />
		/// delegate void OnPostCreationFunction(Projectile Projectile, Player Owner)
		/// </param>
		internal static void ProjectileOnCreate_SetOnPostCreation(int ProjectileType, Action<Projectile, Player> OnPostCreation)
		{
			SummonersShine.Call(1, ProjectileType, 3, OnPostCreation);
		}
		internal delegate void OnPostCreationFunction(Projectile Projectile, Player Owner);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionOnSpecialAbilityUsed hook of all copies of the projectile upon creation.<br />
		/// This hook is called when a projectile is selected to cast their special ability.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionOnSpecialAbilityUsed">
		/// The function to run<br />
		/// delegate void MinionOnSpecialAbilityUsedFunction(Projectile Projectile, Entity Target, int CastingSpecialAbilityType, bool FromServer)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionOnSpecialAbilityUsed(int ProjectileType, Action<Projectile, Entity, int, bool> MinionOnSpecialAbilityUsed)
		{
			SummonersShine.Call(1, ProjectileType, 4, MinionOnSpecialAbilityUsed);
		}
		internal delegate void MinionOnSpecialAbilityUsedFunction(Projectile Projectile, Entity Target, int CastingSpecialAbilityType, bool FromServer);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionTerminateSpecialAbility hook of all copies of the projectile upon creation.<br />
		/// This hook is called when a projectile terminates their special ability via. a config change or by being killed.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionTerminateSpecialAbility">
		/// The function to run<br />
		/// delegate void MinionTerminateSpecialAbilityFunction(Projectile Projectile, Player Owner);
		/// </param>
		internal static void ProjectileOnCreate_SetMinionTerminateSpecialAbility(int ProjectileType, Action<Projectile, Player> MinionTerminateSpecialAbility)
		{
			SummonersShine.Call(1, ProjectileType, 5, MinionTerminateSpecialAbility);
		}
		internal delegate void MinionTerminateSpecialAbilityFunction(Projectile Projectile, Player Owner);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionSummonEffect hook of all copies of the projectile upon creation.<br />
		/// This hook is called on the very first tick of PostAI, after all other mod's PostAI code has been run but before the rest of Summoner's Shine PostAI code.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionSummonEffect">
		/// The function to run<br />
		/// delegate void MinionSummonEffectFunction(Projectile Projectile, Player Owner)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionSummonEffect(int ProjectileType, Action<Projectile, Player> MinionSummonEffect)
		{
			SummonersShine.Call(1, ProjectileType, 6, MinionSummonEffect);
		}
		internal delegate void MinionSummonEffectFunction(Projectile Projectile, Player Owner);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionDespawnEffect hook of all copies of the projectile upon creation.<br />
		/// This hook is called when the projectile is killed, and is called after MinionTerminateSpecialAbility.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionDespawnEffect">
		/// The function to run<br />
		/// delegate void MinionDespawnEffect(Projectile Projectile, Player Owner)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionDespawnEffect(int ProjectileType, Action<Projectile, Player> MinionDespawnEffect)
		{
			SummonersShine.Call(1, ProjectileType, 7, MinionDespawnEffect);
		}
		internal delegate void MinionDespawnEffectFunction(Projectile Projectile, Player Owner);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionPreAI hook of all copies of the projectile upon creation.<br />
		/// This hook is called in PreAI, assuming this tick will not be skipped due to the minion having a low speed.<br />
		/// This hook is also called after the player has been comparatively slowed for Projectile AI working purposes.<br />
		/// The player's velocity is slowed during the AI to ensure that minion AI which relies upon player velocity do not overshoot or undershoot due to having extra ticks or skipping ticks.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionPreAI">
		/// The function to run<br />
		/// delegate void MinionPreAIFunction(Projectile Projectile)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionPreAI(int ProjectileType, Action<Projectile> MinionPreAI)
		{
			SummonersShine.Call(1, ProjectileType, 8, MinionPreAI);
		}
		internal delegate void MinionPreAIFunction(Projectile Projectile);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionCustomAI hook of all copies of the projectile upon creation.<br />
		/// This hook is called after MinionPreAI. Return false to skip all other vanilla and modded PreAI, AI, PostAI functions and go straight to Summoner's Shine PostAI.<br />
		/// There is no need to use this, truth be told.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionCustomAI">
		/// The function to run<br />
		/// delegate bool MinionCustomAIFunction(Projectile Projectile)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionCustomAI(int ProjectileType, Func<Projectile, bool> MinionCustomAI)
		{
			SummonersShine.Call(1, ProjectileType, 9, MinionCustomAI);
		}
		internal delegate bool MinionCustomAIFunction(Projectile Projectile);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionEndOfAI hook of all copies of the projectile upon creation.<br />
		/// This hook is called at the start of Summoner's Shine's PostAI after buffs have been updated.
		/// This hook is only called if the tick is not skipped.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionEndOfAI">
		/// The function to run<br />
		/// delegate void MinionEndOfAIFunction(Projectile Projectile)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionEndOfAI(int ProjectileType, Action<Projectile> MinionEndOfAI)
		{
			SummonersShine.Call(1, ProjectileType, 10, MinionEndOfAI);
		}
		internal delegate void MinionEndOfAIFunction(Projectile Projectile);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionPostAI hook of all copies of the projectile upon creation.<br />
		/// This hook is called right after MinionEndOfAI, regardless if the tick has been skipped.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionPostAI">
		/// The function to run<br />
		/// delegate void MinionPostAIFunction(Projectile Projectile)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionPostAI(int ProjectileType, Action<Projectile> MinionPostAI)
		{
			SummonersShine.Call(1, ProjectileType, 11, MinionPostAI);
		}
		internal delegate void MinionPostAIFunction(Projectile Projectile);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionPostAI hook of all copies of the projectile upon creation.<br />
		/// This hook is called after MinionPostAI and after the working velocity has been modified to fit the projectile's speed modifier.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionPostAIPostRelativeVelocity">
		/// The function to run<br />
		/// delegate void MinionPostAIPostRelativeVelocityFunction(Projectile Projectile);
		/// </param>
		internal static void ProjectileOnCreate_SetMinionPostAIPostRelativeVelocity(int ProjectileType, Action<Projectile> MinionPostAIPostRelativeVelocity)
		{
			SummonersShine.Call(1, ProjectileType, 12, MinionPostAIPostRelativeVelocity);
		}
		internal delegate void MinionPostAIPostRelativeVelocityFunction(Projectile Projectile);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionOnCreation hook of all copies of the projectile upon creation.<br />
		/// This hook is called after Summoner's Shine's OnSpawn and all the OnSpawn things, such as dynamic minion cooldown, attack speed, crit chance, have been set up.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionOnCreation">
		/// The function to run<br />
		/// delegate void MinionOnCreationFunction(Projectile Projectile, Player Owner)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionOnCreation(int ProjectileType, Action<Projectile, Player> MinionOnCreation)
		{
			SummonersShine.Call(1, ProjectileType, 13, MinionOnCreation);
		}
		internal delegate void MinionOnCreationFunction(Projectile Projectile, Player Owner);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionOnMovement hook of all copies of the projectile upon creation.<br />
		/// This hook is called after the projectile updates its position based on velocity.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionOnMovement">
		/// The function to run<br />
		/// delegate void MinionOnMovementFunction(Projectile Projectile)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionOnMovement(int ProjectileType, Action<Projectile> MinionOnMovement)
		{
			SummonersShine.Call(1, ProjectileType, 14, MinionOnMovement);
		}
		internal delegate void MinionOnMovementFunction(Projectile Projectile);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionOnTileCollide hook of all copies of the projectile upon creation.<br />
		/// This is just an OnTileCollide hook. It's exactly like tModLoader's hook.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionOnTileCollide">
		/// The function to run<br />
		/// delegate bool MinionOnTileCollideFunction(Projectile Projectile, Vector2 OldVelocity)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionOnTileCollide(int ProjectileType, Func<Projectile, Vector2, bool> MinionOnTileCollide)
		{
			SummonersShine.Call(1, ProjectileType, 15, MinionOnTileCollide);
		}
		internal delegate bool MinionOnTileCollideFunction(Projectile Projectile, Vector2 OldVelocity);

		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionOnSlopeCollide hook of all copies of the projectile upon creation.<br />
		/// This is called every time the minion collides with a sloped block.<br />
		/// Note that this is called before the new position and velocity is applied.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionOnSlopeCollide">
		/// The function to run<br />
		/// delegate void MinionOnSlopeCollideFunction(Projectile Projectile, Vector4 NewPositionAndVelocity)<br />
		/// NewPositionAndVelocity.X - New Position X<br />
		/// NewPositionAndVelocity.Y - New Position Y<br />
		/// NewPositionAndVelocity.Z - New Velocity X<br />
		/// NewPositionAndVelocity.W - New Velocity Y
		/// </param>
		internal static void ProjectileOnCreate_SetMinionOnSlopeCollide(int ProjectileType, Action<Projectile, Vector4> MinionOnSlopeCollide)
		{
			SummonersShine.Call(1, ProjectileType, 16, MinionOnSlopeCollide);
		}
		internal delegate void MinionOnSlopeCollideFunction(Projectile Projectile, Vector4 NewPositionAndVelocity);

		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionOnShootProjectile hook of all copies of the projectile upon creation.<br />
		/// This is called every time the minion shoots a projectile.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionOnShootProjectile">
		/// The function to run<br />
		/// delegate void MinionOnShootProjectileFunction(Projectile NewProjectile, Projectile ThisShooter)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionOnShootProjectile(int ProjectileType, Action<Projectile, Projectile> MinionOnShootProjectile)
		{
			SummonersShine.Call(1, ProjectileType, 17, MinionOnShootProjectile);
		}
		internal delegate void MinionOnShootProjectileFunction(Projectile NewProjectile, Projectile ThisShooter);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionOnPlayerHitNPCWithProj hook of all copies of the projectile upon creation.<br />
		/// This is called every time the player hits an NPC with any projectile
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionOnPlayerHitNPCWithProj">
		/// The function to run<br />
		/// delegate void MinionOnPlayerHitNPCWithProjFunction(Projectile ThisProjectile, Projectile ProjectileThatHit, NPC target, int Damage, float Knockback, bool Crit)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionOnPlayerHitNPCWithProj(int ProjectileType, Action<Projectile, Projectile, NPC, int, float, bool> MinionOnPlayerHitNPCWithProj)
		{
			SummonersShine.Call(1, ProjectileType, 18, MinionOnPlayerHitNPCWithProj);
		}
		internal delegate void MinionOnPlayerHitNPCWithProjFunction(Projectile ThisProjectile, Projectile ProjectileThatHit, NPC target, int Damage, float Knockback, bool Crit);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionOnHitNPC hook of all copies of the projectile upon creation.<br />
		/// This is called for the owner when the projectile hits an NPC
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionOnHitNPC">
		/// The function to run<br />
		/// delegate void OnHitNPCHook(Projectile projectile, Entity target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionOnHitNPC(int ProjectileType, Func<Projectile, Entity, int, float, bool, int, Tuple<int, float, bool, int>> MinionOnHitNPC)
		{
			SummonersShine.Call(1, ProjectileType, 19, MinionOnHitNPC);
		}
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionOnHitNPC_EffectOnly hook of all copies of the projectile upon creation.<br />
		/// This is called on every other client when the projectile hits an NPC
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionOnHitNPC_EffectOnly">
		/// The function to run<br />
		/// delegate Tuple&lt;int, float, bool, int&gt; OnHitNPCHook(Projectile projectile, Entity target, int damage, float knockback, bool crit, int hitDirection)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionOnHitNPC_EffectOnly(int ProjectileType, Func<Projectile, Entity, int, float, bool, int, Tuple<int, float, bool, int>> MinionOnHitNPC_EffectOnly)
		{
			SummonersShine.Call(1, ProjectileType, 20, MinionOnHitNPC_EffectOnly);
		}

		internal delegate Tuple<int, float, bool, int> OnHitNPCHook(Projectile projectile, Entity target, int damage, float knockback, bool crit, int hitDirection);

		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionModifyColor hook of all copies of the projectile upon creation.<br />
		/// Modifies the color before drawing. I highly suggest you use PreDraw instead.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionModifyColor">
		/// The function to run<br />
		/// delegate Color MinionModifyColorHook(Projectile projectile, Color lightColor)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionModifyColor(int ProjectileType, Func<Projectile, Color, Color> MinionModifyColor)
		{
			SummonersShine.Call(1, ProjectileType, 27, MinionModifyColor);
		}

		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionCustomPreDraw hook of all copies of the projectile upon creation.<br />
		/// This is just a tModLoader PreDraw hook
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionCustomPreDraw">
		/// The function to run<br />
		/// delegate bool PreDrawHook(Projectile projectile, Color lightColor)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionCustomPreDraw(int ProjectileType, Func<Projectile, Color, bool> MinionCustomPreDraw)
		{
			SummonersShine.Call(1, ProjectileType, 21, MinionCustomPreDraw);
		}

		internal delegate bool PreDrawHook(Projectile projectile, Color lightColor);

		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the MinionCustomPostDraw hook of all copies of the projectile upon creation.<br />
		/// This is just a tModLoader PostDraw hook
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionCustomPostDraw">
		/// The function to run<br />
		/// delegate void PostDrawHook(Projectile projectile, Color lightColor)
		/// </param>
		internal static void ProjectileOnCreate_SetMinionCustomPostDraw(int ProjectileType, Action<Projectile, Color> MinionCustomPostDraw)
		{
			SummonersShine.Call(1, ProjectileType, 22, MinionCustomPostDraw);
		}
		internal delegate void PostDrawHook(Projectile projectile, Color lightColor);
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the Minion Speed Modifier Type of all copies of the projectile upon creation.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionSpeedModType">The minion speed modifier type.<br />
		/// Normal - Works for basically all projectiles.<br />
		/// Stepped - Removes phantom updates (affects velocity but prevents velocity from updating position) from the minion.<br />
		/// Only use Stepped if you know what you are doing and have code in your AI to make up for lost fractions of ticks.<br />
		/// None - Minion speed doesn't affect this<br />
		/// LetOthersUpdate - Basically removes the AI entirely. Do not use.
		/// </param>
		internal static void ProjectileOnCreate_SetMinionSpeedModType(int ProjectileType, MinionSpeedModifier MinionSpeedModType)
		{
			SummonersShine.Call(1, ProjectileType, 23, MinionSpeedModType);
		}
		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the Minion Tracking State of all copies of the projectile upon creation.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionTrackingState">The minion tracking state.<br />
		/// Normal - If the projectile is chasing an enemy or is just idling.<br />
		/// Retreating - If the projectile is retreating to the player.<br />
		/// NoTracking - Removes minion tracking<br />
		/// XOnly -For your walking minions
		/// </param>
		internal static void ProjectileOnCreate_SetMinionTrackingState(int ProjectileType, MinionTracking_State MinionTrackingState)
		{
			SummonersShine.Call(1, ProjectileType, 24, MinionTrackingState);
		}

		/// <summary>
		/// Call this in <see cref="ModProjectile.SetStaticDefaults"/>. Sets the Minion Flickering Threshold of all copies of the projectile upon creation.
		/// </summary>
		/// <param name="ProjectileType">The Projectile Type of the projectile</param>
		/// <param name="MinionFlickeringThreshold">The magnitude of the X-velocity required before the sprite is allowed to flip.
		/// </param>
		internal static void ProjectileOnCreate_SetMinionFlickeringThreshold(int ProjectileType, float MinionFlickeringThreshold)
		{
			SummonersShine.Call(1, ProjectileType, 25, MinionFlickeringThreshold);
		}
		internal enum ProjectileConfig
		{
			BlacklistedProjectiles,
			NoTrackingProjectiles,
			MinionsIgnoreTracking,
			ProjectilesNotCountedAsMinion,
			ProjectilesCountedAsMinion,
		}
		internal enum ItemConfig
		{
			ItemIgnoresCustomSpecialPower,
			ItemNonPrefixable,
			ItemRetainManaCost,
			ItemCountAsWhip,
		}
		internal enum MinionTracking_State
		{
			Normal,
			Retreating,
			NoTracking,
			XOnly
		}

		internal enum MinionCDType
		{
			idStaticNPCHitCooldown,
			localNPCHitCooldown,
			noCooldown
		}
		internal enum MinionSpeedModifier
		{
			Normal,
			Stepped,
			None,
			Letothersupdate,
		}

		internal enum ProjMinionRelation
		{
			notMinion,
			isMinion,
			fromMinion,
			isWhip,
			fromWhip,
		}

		internal enum DrawBehindType
		{
			normal,
			none,
			npc_tiles,
			npc,
			projectiles,
			players,
			wires
		}


		/// <summary>
		/// Call this in <see cref="ModItem.SetStaticDefaults"/>. Sets the various item-related hooks Summoner's Shine uses to cast Right-Click Special Abilities.
		/// </summary>
		/// <param name="ItemType">The Item Type of the item</param>
		/// <param name="SpecialAbilityFindTarget">The function called when trying to find the target<br />
		/// delegate Entity SpecialAbilityFindTargetHook(Player Owner, Vector2 ClickPosition).<br />
		/// Return any kind of Entity - a Projectile, a Player, an NPC, a dummy Entity with only Position and Velocity values, or even Null.</param>
		/// <param name="SpecialAbilityFindMinions">The function called when trying to find valid minions<br />
		/// delegate List&lt;Projectile&gt; SpecialAbilityFindMinionsHook(Player Owner, Item item, List&lt;Projectile&gt; ViableMinions)<br />
		/// Returns a list of valid minions. If an empty list or null is returned the special ability will not be cast.</param>
		/// <param name="minionPowers">The minion power values used for the special ability.</param>
		/// <param name="minionPowerRechargeTime">Cooldown of the minion power in ticks. 1 second = 60 ticks.</param>
		/// <param name="specialActive">Removes the default passive special ability from the item and projectile</param>
		internal static void ModSupport_AddItemStatics(int ItemType, Func<Player, Vector2, Entity> SpecialAbilityFindTarget, Func<Player, Item, List<Projectile>, List<Projectile>> SpecialAbilityFindMinions, MinionPowerCollection minionPowers, int minionPowerRechargeTime, bool specialActive)
		{
			SummonersShine.Call(2, ItemType, SpecialAbilityFindTarget, SpecialAbilityFindMinions, minionPowers.BakeToTupleArray(), minionPowerRechargeTime, specialActive);
		}

		internal class MinionPowerCollection
		{
			List<Tuple<float, int, int, bool>> minionPowers = new();

			/// <summary>
			/// Call this to feed data into ModSupport_AddItemStatics. Adds a Minion Power to the Minion Power Collection
			/// </summary>
			/// <param name="power">The base number of the minion power</param>
			/// <param name="scalingType">How the minion power will scale with ability power modifiers</param>
			/// <param name="roundingType">How much to round the ability power value to</param>
			/// <param name="DifficultyScale">If true, halves this in Journey, doubles this in Expert, triples this in Master.</param>
			/// 
			public void AddMinionPower(float power, MinionPowerScalingType scalingType = MinionPowerScalingType.multiply, MinionPowerRoundingType roundingType = MinionPowerRoundingType.dp2, bool DifficultyScale = false)
			{
				minionPowers.Add(new Tuple<float, int, int, bool>
				(
					power,
					(int)scalingType,
					(int)roundingType,
					DifficultyScale
				));
			}

			public Tuple<float, int, int, bool>[] BakeToTupleArray()
			{
				return minionPowers.ToArray();
			}
			internal enum MinionPowerScalingType
			{
				add,
				subtract,
				multiply,
				divide,
			}
			internal enum MinionPowerRoundingType
			{
				dp2,
				integer,
			}
		}

		internal delegate Entity SpecialAbilityFindTargetHook(Player Owner, Vector2 ClickPosition);
		internal delegate List<Projectile> SpecialAbilityFindMinionsHook(Player Owner, Item sourceItem, List<Projectile> ViableMinions);

		/// <summary>
		/// Gets the cute little thought bubble image that is displayed every time a projectile fully recharges their special ability.
		/// </summary>
		/// <param name="SpecialPowerDisplayData">delegate Tuple&lt;Texture2D, Rectangle&gt; SpecialPowerDisplayData(int ItemType, int Frame)<br />
		/// ItemType is your summon item Type
		/// Frame is 1/2 for a single minion fully recharging and 4/5 for when all minions are fully recharged. Frames 0/3 should be empty because the bubble is opening/closing.
		/// </param>
		internal static void ModSupport_AddSpecialPowerDisplayData(Func<int, int, Tuple<Texture2D, Rectangle>> SpecialPowerDisplayData)
		{
			SummonersShine.Call(3, SpecialPowerDisplayData);
		}

		internal delegate Tuple<Texture2D, Rectangle> SpecialPowerDisplayData(int ItemType, int Frame);


		/// <summary>
		/// Returns a variable in the Item's ReworkMinion_Item
		/// </summary>
		/// <param name="item">The item </param>
		/// <param name="ProjectileFuncsVariableType">The variable to change<br />
		/// PrefixMinionPower - Expects float<br />
		/// UsingSpecialAbility - Expects bool. Set it to true in SpecialAbilityFindMinions when you want to activate the special ability without relying on a fully-ready minion (e.g. Dastardly Doubloon, Ancient Guardian minion summoning)<br />
		/// </param>
		internal static void ModSupport_SetVariable_ItemFuncs(Item item, ItemFuncsVariableType ItemFuncsVariableType, object value)
		{
			SummonersShine.Call(15, item, ItemFuncsVariableType, value);
		}

		/// <summary>
		/// Sets a variable in the Item's ReworkMinion_Item
		/// </summary>
		/// <param name="item">The item </param>
		/// <param name="ProjectileFuncsVariableType">The variable to change<br />
		/// PrefixMinionPower - Expects float<br />
		/// UsingSpecialAbility - Expects bool. Set it to true in SpecialAbilityFindMinions when you want to activate the special ability without relying on a fully-ready minion (e.g. Dastardly Doubloon, Ancient Guardian minion summoning)<br />
		/// </param>
		/// <param name="value">The new value</param>
		internal static object ModSupport_GetVariable_ItemFuncs(Item item, ItemFuncsVariableType ItemFuncsVariableType)
		{
			return SummonersShine.Call(16, item, ItemFuncsVariableType);
		}

		/// <summary>
		/// Sets a variable in the projectile's ReworkMinionProjectile
		/// </summary>
		/// <param name="Projectile">The Projectile </param>
		/// <param name="ProjectileFuncsVariableType">The variable to change<br />
		/// ProjectileCrit - Expects int<br />
		/// MinionASMod - Expects float. Default is 1<br />
		/// IsMinion - Expects ProjMinionRelation<br />
		/// killed - Expect bool<br />
		/// killedTicks - Expect byte<br />
		/// LimitedLife - Expect bool<br />
		/// originalNPCHitCooldown - Expect int<br />
		/// minionCDType - Expect MinionCDType<br />
		/// drawBehindType - Expect DrawBehindType<br />
		/// ArmorIgnoredPerc - Expects float. Default is 0.<br />
		/// PrefixMinionPower - Expects float<br />
		/// </param>
		/// <param name="value">The new value</param>
		internal static void ModSupport_SetVariable_ProjFuncs(Projectile Projectile, ProjectileFuncsVariableType ProjectileFuncsVariableType, object value)
		{
			SummonersShine.Call(4, Projectile, ProjectileFuncsVariableType, value);
		}
		/// <summary>
		/// Returns a variable in the projectile's ReworkMinionProjectile
		/// </summary>
		/// <param name="Projectile">The Projectile </param>
		/// <param name="ProjectileFuncsVariableType">The variable to change<br />
		/// ProjectileCrit - Expects int<br />
		/// MinionASMod - Expects float. Default is 1<br />
		/// IsMinion - Expects ProjMinionRelation<br />
		/// killed - Expect bool<br />
		/// killedTicks - Expect byte<br />
		/// LimitedLife - Expect bool<br />
		/// originalNPCHitCooldown - Expect int<br />
		/// minionCDType - Expect MinionCDType<br />
		/// drawBehindType - Expect DrawBehindType<br />
		/// ArmorIgnoredPerc - Expects float. Default is 0.<br />
		/// PrefixMinionPower - Expects float<br />
		/// </param>
		internal static object ModSupport_GetVariable_ProjFuncs(Projectile Projectile, ProjectileFuncsVariableType ProjectileFuncsVariableType)
		{
			return SummonersShine.Call(6, Projectile, ProjectileFuncsVariableType);
		}

		/// <summary>
		/// Sets a variable in the projectile's MinionProjectileData
		/// </summary>
		/// <param name="Projectile">The Projectile </param>
		/// <param name="ProjectileDataVariableType">The variable to change<br />
		/// castingSpecialAbilityType - Expects int<br />
		/// energyRegenRateMult - Expects float<br />
		/// energy - Expects float<br />
		/// maxEnergy - Expects float<br />
		/// energyRegenRate - Expects float<br />
		/// castingSpecialAbilityTime - Expects int<br />
		/// specialCastTarget - Expects NPC<br />
		/// specialCastPosition - Expects Vector2<br />
		/// cancelSpecialNextFrame - Expects bool<br />
		/// minionFlickeringThreshold - Expects float<br />
		/// [UNUSED - WILL THROW ERRORS!} prefixMinionPower - Expects float<br />
		/// minionTrackingAcceleration - Expects float<br />
		/// minionTrackingImperfection - Expects float<br />
		/// trackingState - Expects MinionTracking_State<br />
		/// minionSpeedModType - Expects MinionSpeedModifier<br />
		/// actualMinionAttackTargetNPC - Expects int<br />
		/// moveTarget - Expects Entity<br />
		/// currentTick - Expects float<br />
		/// nextTicks - Expects float<br />
		/// lastSimRateInv - Expects float<br />
		/// updatedSim - Expects bool<br />
		/// isRealTick - Expects bool<br />
		/// lastRelativeVelocity - Expects Vector2<br />
		/// alphaOverride - Expects int<br />
		/// isTeleportFrame - Expects bool<br />
		/// </param>
		internal static void ModSupport_SetVariable_ProjData(Projectile Projectile, ProjectileDataVariableType ProjectileDataVariableType, object value)
		{
			SummonersShine.Call(5, Projectile, ProjectileDataVariableType, value);
		}

		/// <summary>
		/// Returns a variable in the projectile's MinionProjectileData
		/// </summary>
		/// <param name="Projectile">The Projectile </param>
		/// <param name="ProjectileDataVariableType">The variable to change<br />
		/// castingSpecialAbilityType - Expects int<br />
		/// energyRegenRateMult - Expects float<br />
		/// energy - Expects float<br />
		/// maxEnergy - Expects float<br />
		/// energyRegenRate - Expects float<br />
		/// castingSpecialAbilityTime - Expects int<br />
		/// specialCastTarget - Expects NPC<br />
		/// specialCastPosition - Expects Vector2<br />
		/// cancelSpecialNextFrame - Expects bool<br />
		/// minionFlickeringThreshold - Expects float<br />
		/// [UNUSED - WILL THROW ERRORS!} prefixMinionPower - Expects float<br />
		/// minionTrackingAcceleration - Expects float<br />
		/// minionTrackingImperfection - Expects float<br />
		/// trackingState - Expects MinionTracking_State<br />
		/// minionSpeedModType - Expects MinionSpeedModifier<br />
		/// actualMinionAttackTargetNPC - Expects int<br />
		/// moveTarget - Expects Entity<br />
		/// currentTick - Expects float<br />
		/// nextTicks - Expects float<br />
		/// lastSimRateInv - Expects float<br />
		/// updatedSim - Expects bool<br />
		/// isRealTick - Expects bool<br />
		/// lastRelativeVelocity - Expects Vector2<br />
		/// alphaOverride - Expects int<br />
		/// isTeleportFrame - Expects bool<br />
		/// <param name="value">The new value</param>
		/// </param>
		internal static object ModSupport_GetVariable_ProjData(Projectile Projectile, ProjectileDataVariableType ProjectileDataVariableType)
		{
			return SummonersShine.Call(7, Projectile, ProjectileDataVariableType);
		}
		/// <summary>
		/// Returns a variable in the player's ReworkMinion_Player
		/// </summary>
		/// <param name="Player">The Player</param>
		/// <param name="PlayerFuncsVariableType">The variable to change<br />
		/// minionPower - Expects float<br />
		/// energyRestoreRate - Expects float<br />
		/// minionAS - Expects float<br />
		/// minionASRetreating - Expects float<br />
		/// minionASNonPrimaryWeapon - Expects float<br />
		/// <param name="value">The new value</param>
		/// </param>
		internal static void ModSupport_SetVariable_PlayerFuncs(Player Player, PlayerFuncsVariableType PlayerFuncsVariableType, object value)
		{
			SummonersShine.Call(8, Player, PlayerFuncsVariableType, value);
		}
		/// <summary>
		/// Returns a variable in the player's ReworkMinion_Player
		/// </summary>
		/// <param name="Player">The Player</param>
		/// <param name="PlayerFuncsVariableType">The variable to change<br />
		/// minionPower - Expects float<br />
		/// energyRestoreRate - Expects float<br />
		/// minionAS - Expects float<br />
		/// minionASRetreating - Expects float<br />
		/// minionASNonPrimaryWeapon - Expects float<br />
		/// </param>
		internal static object ModSupport_GetVariable_PlayerFuncs(Player Player, PlayerFuncsVariableType PlayerFuncsVariableType)
		{
			return SummonersShine.Call(9, Player, PlayerFuncsVariableType);
		}
		internal enum ItemFuncsVariableType
		{
			PrefixMinionPower,
			UsingSpecialAbility,
		}
		internal enum PlayerFuncsVariableType
		{
			minionPower,
			energyRestoreRate,
			minionAS,
			minionASRetreating,
			minionASNonPrimaryWeapon,
		}
		internal enum ProjectileFuncsVariableType
		{
			ProjectileCrit,
			MinionASMod,
			IsMinion,
			killed,
			killedTicks,
			LimitedLife,
			originalNPCHitCooldown,
			minionCDType,
			drawBehindType,
			ArmorIgnoredPerc,
			PrefixMinionPower,
		}

		internal enum ProjectileDataVariableType
		{
			castingSpecialAbilityType,
			energyRegenRateMult,
			energy,
			maxEnergy,
			energyRegenRate,
			castingSpecialAbilityTime,
			specialCastTarget,
			specialCastPosition,
			cancelSpecialNextFrame,
			minionFlickeringThreshold,
			prefixMinionPower,
			minionTrackingAcceleration,
			minionTrackingImperfection,
			trackingState,
			minionSpeedModType,
			actualMinionAttackTargetNPC,
			moveTarget,
			currentTick,
			nextTicks,
			lastSimRateInv,
			updatedSim,
			isRealTick,
			lastRelativeVelocity,
			alphaOverride,
			isTeleportFrame
		}

		/// <summary>
		/// Adds a modded projectile buff to a projectile. It is recommended to make a class to store the buff data. Will not add duplicates (same source, same mod instance, same modded buff ID)
		/// </summary>
		/// <param name="SourceProj">The source projectile</param>
		/// <param name="Proj">The projectile to apply</param>
		/// <param name="ModInstance">Your mod instance. Used to check for equality.</param>
		/// <param name="ModdedBuffID">The ID of your modded buff. Used to check for equality.</param>
		/// <param name="UpdateHook">Called every frame to update the projectile buff. Return false to kill the buff.<br />
		/// delegate bool ModdedProjectileBuffUpdateHook(Projectile projectile)</param>
		/// <param name="GetAttackSpeedBuffHook">Gets the speed multiplier of the minion.<br />
		/// delegate float ModdedProjectileBuffGetAttackSpeedBuffHook(Projectile projectile)</param>
		/// <param name="PreDrawHook">Draws stuff behind the projectile<br />
		/// delegate Color ModdedProjectileBuffDrawHook(Projectile projectile)</param>
		/// <param name="PostDrawHook">Draws stuff in front of the projectile<br />
		/// delegate Color ModdedProjectileBuffDrawHook(Projectile projectile)</param>
		internal void AddModdedProjectileBuff(Projectile SourceProj, Projectile Proj, Mod ModInstance, int ModdedBuffID,
			Func<Projectile, bool> UpdateHook,
			Func<Projectile, float> GetAttackSpeedBuffHook,
			Func<Projectile, Color> PreDrawHook,
			Func<Projectile, Color> PostDrawHook)
		{
			SummonersShine.Call(11, SourceProj, Proj, ModInstance, ModdedBuffID, UpdateHook, GetAttackSpeedBuffHook, PreDrawHook, PostDrawHook);
		}
		internal delegate bool ModdedProjectileBuffUpdateHook(Projectile projectile);
		internal delegate float ModdedProjectileBuffGetAttackSpeedBuffHook(Projectile projectile);
		internal delegate Color ModdedProjectileBuffDrawHook(Projectile projectile);


		/// <summary>
		/// Removes a modded projectile buff from a projectile.
		/// </summary>
		/// <param name="SourceProj">The source projectile</param>
		/// <param name="Proj">The projectile to apply</param>
		/// <param name="ModInstance">Your mod instance. Used to check for equality.</param>
		/// <param name="ModdedBuffID">The ID of your modded buff. Used to check for equality.</param>
		internal void RemoveModdedProjectileBuff(Projectile SourceProj, Projectile Proj, Mod ModInstance, int ModdedBuffID)
		{
			SummonersShine.Call(12, SourceProj, Proj, ModInstance, ModdedBuffID);
		}
		/// <summary>
		/// Adds a Summoner's Shine projectile buff to a projectile. Will not add duplicates (same source, same buff ID)
		/// </summary>
		/// <param name="SourceProj">The source projectile</param>
		/// <param name="Proj">The projectile to apply</param>
		/// <param name="ProjectileBuffID">The Vanilla Projectile Buff ID. Do NOT set to ModdedBuff - use AddModdedProjectileBuff instead.</param>
		internal void AddProjectileBuff(Projectile SourceProj, Projectile Proj, ProjectileBuffIDs ProjectileBuffID)
		{
			SummonersShine.Call(13, SourceProj, Proj, (int)ProjectileBuffID);
		}
		/// <summary>
		/// Removes a Summoner's Shine projectile buff from a projectile.
		/// </summary>
		/// <param name="SourceProj">The source projectile</param>
		/// <param name="Proj">The projectile to apply</param>
		/// <param name="ProjectileBuffID">The Vanilla Projectile Buff ID. Do NOT set to ModdedBuff - use AddModdedProjectileBuff instead.</param>
		internal void RemoveProjectileBuff(Projectile SourceProj, Projectile Proj, ProjectileBuffIDs ProjectileBuffID)
		{
			SummonersShine.Call(14, SourceProj, Proj, (int)ProjectileBuffID);
		}
		public enum ProjectileBuffIDs
		{
			None,
			ModdedBuff,
			FlinxyFuryBuff,
			ReactionEnrageBuff,
			InstantAttackBuff,
		}
		/// <summary>
		/// Checks if the player or server disabled this item's custom minion power.
		/// </summary>
		/// <param name="ItemID">The item ID</param>
		public static bool IsItemMinionPowerEnabled(int ItemID)
		{
			return (bool)SummonersShine.Call(10, 12, ItemID);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>. Alters the position and ai[0] of the Multishot projectile.<br />
		///  Use if you need to fix your projectile not properly working with Multishot.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="handler">The multishot handler<br />
		/// Return position is ReturnTuple.Item1<br />
		/// Return velocity is ReturnTuple.Item2<br />
		/// Return ai[0] is ReturnTuple.Item3<br />
		/// Return ai[1] is ReturnTuple.Item4<br />
		/// delegate Tuple&lt;Vector2, Vector2, float, float&gt; MultishotDefaultSpecialPreShoot(Vector2 position, Vector2 velocity, Projectile template, Projectile source, NPC target);
		/// </param>
		public static void HookMultishotDefaultSpecialPreShoot(Mod mod, Func<Vector2, Vector2, Projectile, Projectile, NPC, Tuple<Vector2, Vector2, float, float>> handler)
		{
			SummonersShine.Call(18, mod, 0, handler);
		}
		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>. Allows you to modify the multishot projectile post-creation.<br />
		///  Use if you need to fix your projectile not properly working with Multishot.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="handler">The multishot handler<br />
		/// delegate void MultishotDefaultSpecialPostShoot(Projectile newProjectile, Projectile source, NPC target);
		/// </param>
		public static void HookMultishotDefaultSpecialPostShoot(Mod mod, Action<Projectile, Projectile, NPC> handler)
		{
			SummonersShine.Call(18, mod, 1, handler);
		}
		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>. Allows you to modify the instastrike projectile post-teleport.<br />
		///  Use if you need to fix your projectile not properly working with Instastrike (Melee).
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="handler">The instastrike handler<br />
		/// delegate void InstastrikeDefaultSpecialPostTeleport(Projectile minion, NPC target);
		/// </param>
		public static void HookMeleeInstastrikeDefaultSpecialPostTeleport(Mod mod, Action<Projectile, NPC> handler)
		{
			SummonersShine.Call(18, mod, 2, handler);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Allows you to modify if a projectile is detected as ranged or melee.<br />
		///  Use if you need to make Instastrike register your projectile as ranged or melee.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="handler">The instastrike handler<br />
		/// delegate bool DefaultSpecialDetectRanged(Projectile minion);
		/// </param>
		public static void HookDefaultSpecialAbilityDetectRanged(Mod mod, Func<Projectile, bool> handler)
		{
			SummonersShine.Call(18, mod, 3, handler);
		}



		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>Adds or modifies a custom default special ability.<br />
		/// Handles the custom addition of a minion power through config. Required.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate Tuple&lt;float, int, int, bool&gt; GetMinionPower(float MinionPower0, float MinionPower1);<br />
		/// Returns a MinionPowerCollection baked to Tuple Array
		/// </param>
		public static void HookCustomDefaultSpecialAbility_GetMinionPower(Mod mod, string specialName, Func<float, float, Tuple<float, int, int, bool>[]> hook)
		{
			SummonersShine.Call(19, mod, specialName, 0, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom default special ability.<br />
		/// Handles the default RNG addition of a minion power. Required.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate Tuple&lt;float, int, int, bool&gt; GetRandomMinionPower(Random RNG);<br />
		/// Returns a MinionPowerCollection baked to Tuple Array
		/// </param>
		public static void HookCustomDefaultSpecialAbility_GetRandomMinionPower(Mod mod, string specialName, Func<Random, Tuple<float, int, int, bool>[]> hook)
		{
			SummonersShine.Call(19, mod, specialName, 1, hook);
		}


		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom default special ability.<br />
		/// Returns true if the item is valid for this minion power. Required.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate bool GetValidForItem(Item testItem, Projectile testProjectile);<br />
		/// Returns true if the item is valid for this minion power
		/// </param>
		public static void HookCustomDefaultSpecialAbility_GetValidForItem(Mod mod, string specialName, Func<Item, Projectile, bool> hook)
		{
			SummonersShine.Call(19, mod, specialName, 2, hook);
		}


		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom default special ability.<br />
		/// Handles the pin position data of the custom default special ability.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate Tuple&lt;bool, float, float&gt; GetArbitraryPinPositions(List&lt;Projectile&gt; linkedProjectiles);<br />
		/// Highest pin is ReturnTuple.Item2<br />
		/// Lowest pin is ReturnTuple.Item3<br />
		/// If Item1 is false, don't display pins.<br />
		/// Highest and Lowest range from 0 to 1.<br />
		/// </param>
		public static void HookCustomDefaultSpecialAbility_GetArbitraryPinPositions(Mod mod, string specialName, Func<List<Projectile>, Tuple<bool, float, float>> hook)
		{
			SummonersShine.Call(19, mod, specialName, 3, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom default special ability.<br />
		/// Called when a projectile is created.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate void HookMinionPower(Projectile projectile, float MinionPower0, float MinionPower1);<br />
		/// </param>
		public static void HookCustomDefaultSpecialAbility_HookMinionPower(Mod mod, string specialName, Action<Projectile, float, float> hook)
		{
			SummonersShine.Call(19, mod, specialName, 4, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom default special ability.<br />
		/// Called when config is changed or a projectile dies.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate void UnhookMinionPower(Projectile projectile, float MinionPower0, float MinionPower1);<br />
		/// </param>
		public static void HookCustomDefaultSpecialAbility_UnhookMinionPower(Mod mod, string specialName, Action<Projectile, float, float> hook)
		{
			SummonersShine.Call(19, mod, specialName, 5, hook);
		}
		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom default special ability.<br />
		/// Called when the item is used
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate bool OnItemUsed(Player player, Item item);<br />
		/// Just return false on this one. Returning true will make the game engine think it is a whip with regards to whip special abilities.
		/// </param>
		public static void HookCustomDefaultSpecialAbility_OnItemUsed(Mod mod, string specialName, Func<Player, Item, bool> hook)
		{
			SummonersShine.Call(19, mod, specialName, 6, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>Adds or modifies a custom whip default special ability.<br />
		/// Handles the custom addition of a minion power through config. Required.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate Tuple&lt;float, int, int, bool&gt; GetMinionPower(float MinionPower0, float MinionPower1);<br />
		/// Returns a MinionPowerCollection baked to Tuple Array
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_GetMinionPower(Mod mod, string specialName, Func<float, float, Tuple<float, int, int, bool>[]> hook)
		{
			SummonersShine.Call(20, mod, specialName, 0, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Handles the default RNG addition of a minion power. Required.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate Tuple&lt;float, int, int, bool&gt; GetRandomMinionPower(Random RNG);<br />
		/// Returns a MinionPowerCollection baked to Tuple Array
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_GetRandomMinionPower(Mod mod, string specialName, Func<Random, Tuple<float, int, int, bool>[]> hook)
		{
			SummonersShine.Call(20, mod, specialName, 1, hook);
		}


		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Returns true if the item is valid for this minion power. Required.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate bool GetValidForItem(Item testItem, Projectile testProjectile);<br />
		/// Returns true if the item is valid for this minion power
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_GetValidForItem(Mod mod, string specialName, Func<Item, Projectile, bool> hook)
		{
			SummonersShine.Call(20, mod, specialName, 2, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Use this function to pass in any arbitrary data to be stored..
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate object GetMinionPower(float MinionPower0, float MinionPower1, object ArbitraryData);<br />
		/// Returns arbitrary data to be stored.
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_GetArbitraryData(Mod mod, string specialName, Func<float, float, object> hook)
		{
			SummonersShine.Call(20, mod, specialName, 3, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Use this function to set the maximum duration of the special ability. Required.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="maxDuration">The maximum duration
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_SetMaxDuration(Mod mod, string specialName, int maxDuration)
		{
			SummonersShine.Call(20, mod, specialName, 4, maxDuration);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Called when the whip special ability is initialized and gives you the set-duration and force-net-update functions. Required.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function
		/// delegate void ReceiveCallbacks(float MinionPower0, float MinionPower1, object ArbitraryData, Action&lt;int&gt; SetRemainingDurationFunction, Action ForceNetUpdateFunction)
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_ReceiveCallbacks(Mod mod, string specialName, Action<float, float, object, Action<int>, Action> hook)
		{
			SummonersShine.Call(20, mod, specialName, 5, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Called every tick
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate bool Update(float MinionPower0, float MinionPower1, object ArbitraryData);
		/// Return true to not kill the whip special ability. Return false to kill the whip special ability. Return null otherwise.
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_Update(Mod mod, string specialName, Func<float, float, object, bool?> hook)
		{
			SummonersShine.Call(20, mod, specialName, 6, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Called if the whip special ability naturally expires or is replaced with another whip special ability
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate void Update(float MinionPower0, float MinionPower1, object ArbitraryData);
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_Kill(Mod mod, string specialName, Action<float, float, object> hook)
		{
			SummonersShine.Call(20, mod, specialName, 7, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Returns the minion armor negation percentage. Called every time a minion attacks an enemy.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate float GetMinionArmorNegationPerc(NPC enemy, float MinionPower0, float MinionPower1, object ArbitraryData);<br />
		/// Returns the percentage of armor to ignore.
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_GetMinionArmorNegationPerc(Mod mod, string specialName, Func<NPC, float, float, object, float> hook)
		{
			SummonersShine.Call(20, mod, specialName, 8, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Returns the inverted minion attack speed modifier.
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate float GetMinionAttackSpeed(float MinionPower0, float MinionPower1, object ArbitraryData);<br />
		/// Returns the inverted minion attack speed modifier.
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_GetMinionAttackSpeed(Mod mod, string specialName, Func<float, float, object, float> hook)
		{
			SummonersShine.Call(20, mod, specialName, 9, hook);
		}

		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Returns the whip range modifier
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate float GetWhipRange(float MinionPower0, float MinionPower1, object ArbitraryData);<br />
		/// Returns the whip range modifier
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_GetWhipRange(Mod mod, string specialName, Func<float, float, object, float> hook)
		{
			SummonersShine.Call(20, mod, specialName, 10, hook);
		}


		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Called when an enemy is whipped by this weapon
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate void OnWhippedEnemy(NPC target, float MinionPower0, float MinionPower1, object ArbitraryData);
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_OnWhippedEnemy(Mod mod, string specialName, Action<NPC, float, float, object> hook)
		{
			SummonersShine.Call(20, mod, specialName, 11, hook);
		}
		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Called when the whip is cracked
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate void OnWhipUsed(Item whip, float MinionPower0, float MinionPower1, object ArbitraryData);
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_OnWhipUsed(Mod mod, string specialName, Action<Item, float, float, object> hook)
		{
			SummonersShine.Call(20, mod, specialName, 12, hook);
		}
		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Called when multiplayer data is being received
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate void LoadNetData_Extra(BinaryReader reader, float MinionPower0, float MinionPower1, object ArbitraryData);
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_LoadNetData_Extra(Mod mod, string specialName, Action<BinaryReader, float, float, object> hook)
		{
			SummonersShine.Call(20, mod, specialName, 13, hook);
		}
		/// <summary>
		/// Call this in <see cref="Mod.PostSetupContent"/>.Adds or modifies a custom whip default special ability.<br />
		/// Called when multiplayer data is being sent
		/// </summary>
		/// <param name="mod">Your mod</param>
		/// <param name="specialName">Your default special ability's name</param>
		/// <param name="hook">The function<br />
		/// delegate void SaveNetData_extra(ModPacket packet, float MinionPower0, float MinionPower1, object ArbitraryData);
		/// </param>
		public static void HookCustomDefaultWhipSpecialAbility_SaveNetData_extra(Mod mod, string specialName, Action<ModPacket, float, float, object> hook)
		{
			SummonersShine.Call(20, mod, specialName, 14, hook);
		}
	}

	internal static class SummonersShineGenerics
	{

		/// <summary>
		/// Sets the move target of this projectile for the purposes of aimbot and chasing ability
		/// </summary>
		/// <param name="MoveTarget">The targeted entity</param>
		public static void SummonersShine_SetMoveTarget(this Projectile projectile, Entity MoveTarget)
		{
			SummonersShine.Call(10, 0, projectile, MoveTarget);
		}
		/// <summary>
		/// Sets the move target of this projectile for the purposes of aimbot and chasing ability
		/// </summary>
		/// <param name="MoveTarget">The targetedNPC whoAmI</param>
		public static void SummonersShine_SetMoveTarget_FromID(this Projectile projectile, int MoveTarget)
		{
			SummonersShine.Call(10, 1, projectile, MoveTarget);
		}
		/// <summary>
		/// Checks if this is on the very last projectile tick of the current frame and that it's not undergoing a vanilla extra update.
		/// </summary>
		public static bool SummonersShine_IsOnRealTick(this Projectile projectile)
		{
			return (bool)SummonersShine.Call(10, 2, projectile);
		}
		/// <summary>
		/// Gets the minion power value of the projectile, after all modifications.
		/// </summary>
		/// <param name="index">The minion power's index</param>
		public static float SummonersShine_GetMinionPower(this Projectile projectile, int index)
		{
			return (float)SummonersShine.Call(10, 3, projectile, index);
		}
		/// <summary>
		/// Gets the minion power value of the item, after all modifications.
		/// </summary>
		/// <param name="owner">The owner of the item</param>
		/// <param name="index">The minion power's index</param>
		public static float SummonersShine_GetMinionPower(this Item item, Player owner, int index)
		{
			return (float)SummonersShine.Call(10, 16, owner, item, index);
		}
		/// <summary>
		/// Gets the minion power value of the projectile, after all modifications.
		/// Also gets all associated minion power data, such as the original minion power value, the scaling-type, the rounding-type, and if it scales with difficulty
		/// </summary>
		/// <param name="index">The minion power's index</param>
		/// <param name="original">The original minion power value</param>
		/// <param name="mpScalingType">The minion power's scaling type</param>
		/// <param name="mpRoudingType">The minion power's rounding type</param>
		/// <param name="difficultyScale">Whether or not the minion power scales with difficulty</param>
		public static float SummonersShine_GetAllMinionPowerData(this Projectile projectile, int index, out float original, out MinionPowerScalingType mpScalingType, out MinionPowerRoundingType mpRoudingType, out bool difficultyScale)
		{
			Tuple<float, float, int, int, bool> rv = (Tuple<float, float, int, int, bool>)SummonersShine.Call(10, 10, projectile, index);
			original = rv.Item2;
			mpScalingType = (MinionPowerScalingType)rv.Item3;
			mpRoudingType = (MinionPowerRoundingType)rv.Item4;
			difficultyScale = rv.Item5;
			return rv.Item1;
		}
		/// <summary>
		/// Gets the minion power value of the projectile, after all modifications.
		/// Also gets all associated minion power data, such as the original minion power value, the scaling-type, the rounding-type, and if it scales with difficulty
		/// </summary>
		/// <param name="index">The minion power's index</param>
		/// <param name="original">The original minion power value</param>
		/// <param name="mpScalingType">The minion power's scaling type</param>
		/// <param name="mpRoudingType">The minion power's rounding type</param>
		/// <param name="difficultyScale">Whether or not the minion power scales with difficulty</param>
		public static float SummonersShine_GetAllMinionPowerData(this Item item, Player owner, int index, out float original, out MinionPowerScalingType mpScalingType, out MinionPowerRoundingType mpRoudingType, out bool difficultyScale)
		{
			Tuple<float, float, int, int, bool> rv = (Tuple<float, float, int, int, bool>)SummonersShine.Call(10, 17, owner, item, index);
			original = rv.Item2;
			mpScalingType = (MinionPowerScalingType)rv.Item3;
			mpRoudingType = (MinionPowerRoundingType)rv.Item4;
			difficultyScale = rv.Item5;
			return rv.Item1;
		}
		/// <summary>
		/// Gets the inverse simulation rate of a projectile. E.g. will return 0.5f if it's 2x faster, 0.25f if it's 4x faster, etc.
		/// </summary>
		public static float SummonersShine_GetSpeed(this Projectile projectile)
		{
			return (float)SummonersShine.Call(10, 4, projectile);
		}
		/// <summary>
		/// Gets the simulation rate of a projectile.
		/// </summary>
		public static float SummonersShine_GetSimulationRate(this Projectile projectile)
		{
			return (float)SummonersShineCompat.SummonersShine.Call(10, 5, projectile);
		}
		/// <summary>
		/// Use this to get how far to move your projectile per AI call, if you are using Stepped minion speed modifier.
		/// </summary>
		public static float SummonersShine_GetInternalSimRate(this Projectile projectile)
		{
			return (float)SummonersShineCompat.SummonersShine.Call(10, 6, projectile);
		}
		/// <summary>
		/// Gets the aimbotted velocity of a shot projectile.
		/// </summary>
		/// <param name="Vel">The original velocity</param>
		/// <param name="NumUpdates">The projectile's extraUpdates + 1</param>
		/// <param name="Npc">The targeted NPC</param>
		public static Vector2 GetTotalProjectileVelocity(this Projectile projectile, Vector2 Vel, float NumUpdates, NPC Npc)
		{
			return (Vector2)SummonersShineCompat.SummonersShine.Call(10, 7, Vel, NumUpdates, projectile, Npc);
		}
		/// <summary>
		/// Checks if the projectile is currently casting a special ability
		/// </summary>
		/// <param name="SourceItemID">The projectile's source item</param>
		public static bool Projectile_IsCastingSpecialAbility(this Projectile projectile, int SourceItemID)
		{
			return (bool)SummonersShineCompat.SummonersShine.Call(10, 8, projectile, SourceItemID);
		}

		/// <summary>
		/// Increments the special ability timer, automatically resetting CastingSpecialAbilityTime, EnergyRegenRateMult, and calling MinionTerminateSpecialAbility if MaxTicks is reached.
		/// </summary>
		/// <param name="SourceItemID">The projectile's source item</param>
		public static bool IncrementSpecialAbilityTimer(this Projectile projectile, int MaxTicks, float DefaultEnergyRegenMult = 1)
		{
			return (bool)SummonersShineCompat.SummonersShine.Call(10, 9, projectile, MaxTicks, DefaultEnergyRegenMult);
		}
		
		/// <summary>
		 /// Increments the special ability timer, automatically resetting CastingSpecialAbilityTime, EnergyRegenRateMult, and calling MinionTerminateSpecialAbility if MaxTicks is reached.
		 /// </summary>
		 /// <param name="SourceItemID">The projectile's source item</param>
		public static void OverrideSourceItem(this Projectile projectile, int sourceItemID)
		{
			SummonersShine.Call(10, 11, projectile, sourceItemID);
		}

		/// <summary>
		/// Checks if the player or server disabled this projectile's special ability
		/// </summary>
		public static bool IsProjectileMinionPowerEnabled(this Projectile projectile)
		{
			return (bool)SummonersShine.Call(10, 13, projectile);
		}

		/// <summary>
		/// Gets the list of projectiles attached to an item, and every minion's special power recharge progress fraction (ranges from 0-1) respectively
		/// Tuple.Item1: List of all projectiles with item as source
		/// Tuple.Item2: All minions' special power recharge progress fraction (ranges from 0-1)
		/// </summary>
		/// <param name="itemID">The projectile's source item</param>
		public static Tuple<List<Projectile>, List<float>> GetPlayerMinionCollectionData(this Player player, int itemID) {

			return (Tuple<List<Projectile>, List<float>>)SummonersShine.Call(10, 14, player, itemID);
		}

		/// <summary>
		/// Forcibly uses an item's special ability at the player's cursor. Call only on owning player.
		/// </summary>
		/// <param name="owner">The item's owner</param>
		public static void ForciblyUseSpecialAbility(this Item item, Player owner)
        {
			SummonersShine.Call(10, 15, item, owner);
		}

		/// <summary>
		/// Forcibly displays a finished-cooldown thought bubble for the player
		/// </summary>
		/// <param name="itemID">The item ID of the minion</param>
		/// <param name="playerHasMultipleMinions">Set this to true unless the player only has 1 minion, or it will cause it to bug out</param>
		/// <param name="golden">Whether to display a golden thought bubble</param>
		public static void ForciblyDisplayFinishedCooldown(this Player player, int itemID, bool playerHasMultipleMinions, bool golden)
		{
			SummonersShine.Call(10, 18, player, itemID, playerHasMultipleMinions, golden);
		}

		public static IEntitySource GetRespawnMinionEntitySource(this Projectile minion)
		{
			return (IEntitySource)SummonersShine.Call(10, 19, minion);
		}
	}
}
