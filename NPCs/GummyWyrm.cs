using Terraria.Audio;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using ReLogic.Content;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items.Banners;

namespace TheConfectionRebirth.NPCs {
	internal class GummyWyrmHead : ConfectionWorm {

		public sealed override WormSegmentType SegmentType => WormSegmentType.Head;

		public static Asset<Texture2D> KissyHeadTexture { get; private set; }

		/// <summary>
		/// The NPCID or ModContent.NPCType for the body segment NPCs.<br/>
		/// This property is only used if <see cref="HasCustomBodySegments"/> returns <see langword="false"/>.
		/// </summary>
		public int BodyType = ModContent.NPCType<GummyWyrmBody>();

		/// <summary>
		/// The NPCID or ModContent.NPCType for the tail segment NPC.<br/>
		/// This property is only used if <see cref="HasCustomBodySegments"/> returns <see langword="false"/>.
		/// </summary>
		public int TailType = ModContent.NPCType<GummyWyrmTail>();

		/// <summary>
		/// The minimum amount of segments expected, including the head and tail segments
		/// </summary>
		public int MinSegmentLength { get; set; }

		/// <summary>
		/// The maximum amount of segments expected, including the head and tail segments
		/// </summary>
		public int MaxSegmentLength { get; set; }

		/// <summary>
		/// Whether the NPC ignores tile collision when attempting to "dig" through tiles, like how Wyverns work.
		/// </summary>
		public bool CanFly { get; set; }

		/// <summary>
		/// The maximum distance in <b>pixels</b> within which the NPC will use tile collision, if <see cref="CanFly"/> returns <see langword="false"/>.<br/>
		/// Defaults to 1000 pixels, which is equivalent to 62.5 tiles.
		/// </summary>
		public virtual int MaxDistanceForUsingTileCollision => 1000;

		/// <summary>
		/// Whether the NPC uses 
		/// </summary>
		public virtual bool HasCustomBodySegments => false;

		/// <summary>
		/// If not <see langword="null"/>, this NPC will target the given world position instead of its player target
		/// </summary>
		public Vector2? ForcedTargetPosition { get; set; }

		/// <summary>
		/// Override this method to use custom body-spawning code.<br/>
		/// This method only runs if <see cref="HasCustomBodySegments"/> returns <see langword="true"/>.
		/// </summary>
		/// <param name="segmentCount">How many body segments are expected to be spawned</param>
		/// <returns>The whoAmI of the most-recently spawned NPC, which is the result of calling <see cref="NPC.NewNPC(Terraria.DataStructures.IEntitySource, int, int, int, int, float, float, float, float, int)"/></returns>
		public virtual int SpawnBodySegments(int segmentCount)
		{
			return NPC.whoAmI;
		}

		/// <summary>
		/// Spawns a body or tail segment of the worm.
		/// </summary>
		/// <param name="source">The spawn source</param>
		/// <param name="type">The ID of the segment NPC to spawn</param>
		/// <param name="latestNPC">The whoAmI of the most-recently spawned segment NPC in the worm, including the head</param>
		/// <returns></returns>
		protected int SpawnSegment(IEntitySource source, int type, int latestNPC)
		{
			int oldLatest = latestNPC;
			latestNPC = NPC.NewNPC(source, (int)NPC.Center.X, (int)NPC.Center.Y, type, NPC.whoAmI, 0, latestNPC, ai3: (Main.npc[oldLatest].ai[3] + 7) % 361);

			Main.npc[oldLatest].ai[0] = latestNPC;

			NPC latest = Main.npc[latestNPC];
			latest.realLife = NPC.whoAmI;

			return latestNPC;
		}

		internal sealed override void HeadAI()
		{
			HeadAI_SpawnSegments();

			bool collision = HeadAI_CheckCollisionForDustSpawns();

			HeadAI_CheckTargetDistance(ref collision);

			HeadAI_Movement(collision);
		}

		private void HeadAI_SpawnSegments()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				bool hasFollower = NPC.ai[0] > 0;
				if (!hasFollower)
				{
					NPC.realLife = NPC.whoAmI;
					int latestNPC = NPC.whoAmI;

					int randomWormLength = Main.rand.Next(MinSegmentLength, MaxSegmentLength + 1);

					int distance = randomWormLength - 2;

					IEntitySource source = NPC.GetSource_FromAI();

					if (HasCustomBodySegments)
					{
						latestNPC = SpawnBodySegments(distance);
					}
					else
					{
						while (distance > 0)
						{
							latestNPC = SpawnSegment(source, BodyType, latestNPC);
							distance--;
						}
					}

					SpawnSegment(source, TailType, latestNPC);

					NPC.netUpdate = true;

					int count = 0;
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC n = Main.npc[i];

						if (n.active && (n.type == Type || n.type == BodyType || n.type == TailType) && n.realLife == NPC.whoAmI)
							count++;
					}

					if (count != randomWormLength)
					{
						for (int i = 0; i < Main.maxNPCs; i++)
						{
							NPC n = Main.npc[i];

							if (n.active && (n.type == Type || n.type == BodyType || n.type == TailType) && n.realLife == NPC.whoAmI)
							{
								n.active = false;
								n.netUpdate = true;
							}
						}
					}

					NPC.TargetClosest(true);
				}
			}
		}

		private bool HeadAI_CheckCollisionForDustSpawns()
		{
			int minTilePosX = (int)(NPC.Left.X / 16) - 1;
			int maxTilePosX = (int)(NPC.Right.X / 16) + 2;
			int minTilePosY = (int)(NPC.Top.Y / 16) - 1;
			int maxTilePosY = (int)(NPC.Bottom.Y / 16) + 2;

			if (minTilePosX < 0)
				minTilePosX = 0;
			if (maxTilePosX > Main.maxTilesX)
				maxTilePosX = Main.maxTilesX;
			if (minTilePosY < 0)
				minTilePosY = 0;
			if (maxTilePosY > Main.maxTilesY)
				maxTilePosY = Main.maxTilesY;

			bool collision = false;

			for (int i = minTilePosX; i < maxTilePosX; ++i)
			{
				for (int j = minTilePosY; j < maxTilePosY; ++j)
				{
					Tile tile = Main.tile[i, j];

					if (tile.HasUnactuatedTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType] && tile.TileFrameY == 0) || tile.LiquidAmount > 64)
					{
						Vector2 tileWorld = new Point16(i, j).ToWorldCoordinates(0, 0);

						if (NPC.Right.X > tileWorld.X && NPC.Left.X < tileWorld.X + 16 && NPC.Bottom.Y > tileWorld.Y && NPC.Top.Y < tileWorld.Y + 16)
						{
							collision = true;

							if (Main.rand.NextBool(100))
								WorldGen.KillTile(i, j, fail: true, effectOnly: true, noItem: false);
						}
					}
				}
			}

			return collision;
		}

		private void HeadAI_CheckTargetDistance(ref bool collision)
		{
			if (!collision)
			{
				Rectangle hitbox = NPC.Hitbox;

				int maxDistance = MaxDistanceForUsingTileCollision;

				bool tooFar = true;

				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Rectangle areaCheck;

					Player player = Main.player[i];

					if (ForcedTargetPosition is Vector2 target)
						areaCheck = new Rectangle((int)target.X - maxDistance, (int)target.Y - maxDistance, maxDistance * 2, maxDistance * 2);
					else if (player.active && !player.dead && !player.ghost)
						areaCheck = new Rectangle((int)player.position.X - maxDistance, (int)player.position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
					else
						continue;

					if (hitbox.Intersects(areaCheck))
					{
						tooFar = false;
						break;
					}
				}

				if (tooFar)
					collision = true;
			}
		}

		private void HeadAI_Movement(bool collision)
		{
			float speed = MoveSpeed;
			float acceleration = Acceleration;

			float targetXPos, targetYPos;

			Player playerTarget = Main.player[NPC.target];

			Vector2 forcedTarget = ForcedTargetPosition ?? playerTarget.Center;
			(targetXPos, targetYPos) = (forcedTarget.X, forcedTarget.Y);

			Vector2 npcCenter = NPC.Center;

			float targetRoundedPosX = (float)((int)(targetXPos / 16f) * 16);
			float targetRoundedPosY = (float)((int)(targetYPos / 16f) * 16);
			npcCenter.X = (float)((int)(npcCenter.X / 16f) * 16);
			npcCenter.Y = (float)((int)(npcCenter.Y / 16f) * 16);
			float dirX = targetRoundedPosX - npcCenter.X;
			float dirY = targetRoundedPosY - npcCenter.Y;

			float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

			if (!collision && !CanFly)
				HeadAI_Movement_HandleFallingFromNoCollision(dirX, speed, acceleration);
			else
			{
				HeadAI_Movement_PlayDigSounds(length);

				HeadAI_Movement_HandleMovement(dirX, dirY, length, speed, acceleration);
			}

			HeadAI_Movement_SetRotation(collision);
		}

		private void HeadAI_Movement_HandleFallingFromNoCollision(float dirX, float speed, float acceleration)
		{
			NPC.TargetClosest(true);

			NPC.velocity.Y += 0.11f;

			if (NPC.velocity.Y > speed)
				NPC.velocity.Y = speed;

			if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.4f)
			{
				if (NPC.velocity.X < 0.0f)
					NPC.velocity.X -= acceleration * 1.1f;
				else
					NPC.velocity.X += acceleration * 1.1f;
			}
			else if (NPC.velocity.Y == speed)
			{
				if (NPC.velocity.X < dirX)
					NPC.velocity.X += acceleration;
				else if (NPC.velocity.X > dirX)
					NPC.velocity.X -= acceleration;
			}
			else if (NPC.velocity.Y > 4)
			{
				if (NPC.velocity.X < 0)
					NPC.velocity.X += acceleration * 0.9f;
				else
					NPC.velocity.X -= acceleration * 0.9f;
			}
		}

		private void HeadAI_Movement_PlayDigSounds(float length)
		{
			if (NPC.soundDelay == 0)
			{
				float num1 = length / 40f;

				if (num1 < 10)
					num1 = 10f;

				if (num1 > 20)
					num1 = 20f;

				NPC.soundDelay = (int)num1;

				SoundEngine.PlaySound(SoundID.WormDig, NPC.position);
			}
		}

		private void HeadAI_Movement_HandleMovement(float dirX, float dirY, float length, float speed, float acceleration)
		{
			float absDirX = Math.Abs(dirX);
			float absDirY = Math.Abs(dirY);
			float newSpeed = speed / length;
			dirX *= newSpeed;
			dirY *= newSpeed;

			if ((NPC.velocity.X > 0 && dirX > 0) || (NPC.velocity.X < 0 && dirX < 0) || (NPC.velocity.Y > 0 && dirY > 0) || (NPC.velocity.Y < 0 && dirY < 0))
			{
				if (NPC.velocity.X < dirX)
					NPC.velocity.X += acceleration;
				else if (NPC.velocity.X > dirX)
					NPC.velocity.X -= acceleration;

				if (NPC.velocity.Y < dirY)
					NPC.velocity.Y += acceleration;
				else if (NPC.velocity.Y > dirY)
					NPC.velocity.Y -= acceleration;

				if (Math.Abs(dirY) < speed * 0.2 && ((NPC.velocity.X > 0 && dirX < 0) || (NPC.velocity.X < 0 && dirX > 0)))
				{
					if (NPC.velocity.Y > 0)
						NPC.velocity.Y += acceleration * 2f;
					else
						NPC.velocity.Y -= acceleration * 2f;
				}

				if (Math.Abs(dirX) < speed * 0.2 && ((NPC.velocity.Y > 0 && dirY < 0) || (NPC.velocity.Y < 0 && dirY > 0)))
				{
					if (NPC.velocity.X > 0)
						NPC.velocity.X = NPC.velocity.X + acceleration * 2f;
					else
						NPC.velocity.X = NPC.velocity.X - acceleration * 2f;
				}
			}
			else if (absDirX > absDirY)
			{
				if (NPC.velocity.X < dirX)
					NPC.velocity.X += acceleration * 1.1f;
				else if (NPC.velocity.X > dirX)
					NPC.velocity.X -= acceleration * 1.1f;

				if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
				{
					if (NPC.velocity.Y > 0)
						NPC.velocity.Y += acceleration;
					else
						NPC.velocity.Y -= acceleration;
				}
			}
			else
			{
				if (NPC.velocity.Y < dirY)
					NPC.velocity.Y += acceleration * 1.1f;
				else if (NPC.velocity.Y > dirY)
					NPC.velocity.Y -= acceleration * 1.1f;

				if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
				{
					if (NPC.velocity.X > 0)
						NPC.velocity.X += acceleration;
					else
						NPC.velocity.X -= acceleration;
				}
			}
		}

		private void HeadAI_Movement_SetRotation(bool collision)
		{
			NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;

			if (collision)
			{
				if (NPC.localAI[0] != 1)
					NPC.netUpdate = true;

				NPC.localAI[0] = 1f;
			}
			else
			{
				if (NPC.localAI[0] != 0)
					NPC.netUpdate = true;

				NPC.localAI[0] = 0f;
			}

			if (((NPC.velocity.X > 0 && NPC.oldVelocity.X < 0) || (NPC.velocity.X < 0 && NPC.oldVelocity.X > 0) || (NPC.velocity.Y > 0 && NPC.oldVelocity.Y < 0) || (NPC.velocity.Y < 0 && NPC.oldVelocity.Y > 0)) && !NPC.justHit)
				NPC.netUpdate = true;
		}

		public override void Load() {
			if (!Main.dedServ) {
				KissyHeadTexture = Mod.Assets.Request<Texture2D>("NPCs/GummyWyrmHead_Kissy");
			}
		}

		public override void SetStaticDefaults() {
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				CustomTexturePath = "TheConfectionRebirth/NPCs/GummyWyrm_Bestiary",
				Position = new Vector2(30f, 28f),
				PortraitPositionXOverride = 0f,
				PortraitPositionYOverride = 20f,
				PortraitScale = 1.25f,
				Scale = 1.25f
			});
		}

		public override void SetDefaults() {
			NPC.width = 30;
			NPC.height = 30;
			NPC.aiStyle = -1;
			NPC.netAlways = true;
			NPC.damage = 70;
			NPC.defense = 34;
			NPC.lifeMax = 1000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0f;
			NPC.behindTiles = true;
			NPC.scale = 1f;
			NPC.value = 300f;
			SpawnModBiomes = new int[1] { ModContent.GetInstance<ConfectionUndergroundBiome>().Type };
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<GummyWyrmBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("Mods.TheConfectionRebirth.Bestiary.GummyWyrm")
			});
		}

		protected override void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 screenPos, Color drawColor) {
			if (NPC.Center.DistanceSQ(Main.player[NPC.target].Center) < (15 * 16) * (15 * 16)) {
				texture = KissyHeadTexture.Value;
			}

			base.Draw(spriteBatch, texture, screenPos, drawColor);
		}

		public override void Init() {
			NPC.ai[3] = Main.rand.Next(361);
			MinSegmentLength = 12;
			MaxSegmentLength = 24;

			CommonWormInit(this);
		}

		internal static void CommonWormInit(ConfectionWorm worm) {
			worm.MoveSpeed = 5.5f;
			worm.Acceleration = 0.045f;
		}

		public override void HitEffect(NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server) {
				return;
			}

			if (NPC.life > 0)
			{
				for (int num556 = 0; (double)num556 < hit.Damage / (double)NPC.lifeMax * 50.0; num556++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int num557 = 0; num557 < 10; num557++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * (float)hit.HitDirection, -2.5f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GummyWyrmHeadGore").Type);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) 
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 5, 9));
			npcLoot.Add(ItemDropRule.Common(ItemID.WhoopieCushion, 100));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.GummyWormWhip>(), 100));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return ConfectionGlobalNPC.SpawnNPC_ConfectionNPC(spawnInfo, Type);
		}
	}

	public class GummyWyrmBody : ConfectionWorm {

		public sealed override WormSegmentType SegmentType => WormSegmentType.Body;

		internal override void BodyTailAI()
		{
			CommonAI_BodyTail(this);
		}

		internal static void CommonAI_BodyTail(ConfectionWorm worm)
		{
			if (!worm.NPC.HasValidTarget)
				worm.NPC.TargetClosest(true);

			if (Main.player[worm.NPC.target].dead && worm.NPC.timeLeft > 30000)
				worm.NPC.timeLeft = 10;

			NPC following = worm.NPC.ai[1] >= Main.maxNPCs ? null : worm.FollowingNPC;
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (following is null || !following.active || following.friendly || following.townNPC || following.lifeMax <= 5)
				{
					worm.NPC.life = 0;
					worm.NPC.HitEffect(0, 10);
					worm.NPC.active = false;
				}
			}

			if (following is not null)
			{
				float dirX = following.Center.X - worm.NPC.Center.X;
				float dirY = following.Center.Y - worm.NPC.Center.Y;
				worm.NPC.rotation = (float)Math.Atan2(dirY, dirX) + MathHelper.PiOver2;
				float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
				float dist = (length - worm.NPC.width) / length;
				float posX = dirX * dist;
				float posY = dirY * dist;

				worm.NPC.velocity = Vector2.Zero;
				worm.NPC.position.X += posX;
				worm.NPC.position.Y += posY;
			}
		}

		public override void SetStaticDefaults() 
		{
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true
			});
		}

		public override void SetDefaults() {
			NPC.width = 22;
			NPC.height = 22;
			NPC.aiStyle = -1;
			NPC.netAlways = true;
			NPC.damage = 40;
			NPC.defense = 44;
			NPC.lifeMax = 1000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0f;
			NPC.behindTiles = true;
			NPC.scale = 1f;
			NPC.value = 300f;
			NPC.dontCountMe = true;
			Banner = ModContent.NPCType<GummyWyrmHead>();
			BannerItem = ModContent.ItemType<GummyWyrmBanner>();
		}

		public override void Init() {
			GummyWyrmHead.CommonWormInit(this);
		}

		public override void HitEffect(NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server) {
				return;
			}

			if (NPC.life > 0)
			{
				for (int num556 = 0; (double)num556 < hit.Damage / (double)NPC.lifeMax * 50.0; num556++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int num557 = 0; num557 < 10; num557++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * (float)hit.HitDirection, -2.5f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GummyWyrmBodyGore").Type);
			}
		}
	}

	internal class GummyWyrmTail : ConfectionWorm {
		public sealed override WormSegmentType SegmentType => WormSegmentType.Tail;

		internal override void BodyTailAI()
		{
			GummyWyrmBody.CommonAI_BodyTail(this);
		}
		public override void SetStaticDefaults() {
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true
			});
		}

		public override void SetDefaults() {
			NPC.width = 22;
			NPC.height = 22;
			NPC.aiStyle = -1;
			NPC.netAlways = true;
			NPC.damage = 30;
			NPC.defense = 54;
			NPC.lifeMax = 1000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0f;
			NPC.behindTiles = true;
			NPC.scale = 1f;
			NPC.value = 300f;
			NPC.dontCountMe = true;
			Banner = ModContent.NPCType<GummyWyrmHead>();
			BannerItem = ModContent.ItemType<GummyWyrmBanner>();
		}

		public override void Init() {
			GummyWyrmHead.CommonWormInit(this);
		}

		public override void HitEffect(NPC.HitInfo hit) {
			if (Main.netMode == NetmodeID.Server) {
				return;
			}

			if (NPC.life > 0)
			{
				for (int num556 = 0; (double)num556 < hit.Damage / (double)NPC.lifeMax * 50.0; num556++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, hit.HitDirection, -1f);
				}
			}
			else
			{
				for (int num557 = 0; num557 < 10; num557++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * (float)hit.HitDirection, -2.5f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GummyWyrmTailGore").Type);
			}
		}
	}

	//literally feel free to edit past here because I dont think the confection will get any more worms other than the wyrm

	public enum WormSegmentType {
		/// <summary>
		/// The head segment for the worm.  Only one "head" is considered to be active for any given worm
		/// </summary>
		Head,
		/// <summary>
		/// The body segment.  Follows the segment in front of it
		/// </summary>
		Body,
		/// <summary>
		/// The tail segment.  Has the same AI as the body segments.  Only one "tail" is considered to be active for any given worm
		/// </summary>
		Tail
	}

	/// <summary>
	/// The base class for non-separating Worm enemies.
	/// </summary>
	public abstract class ConfectionWorm : ModNPC {
		/*  ai[] usage:
		 *  
		 *  ai[0] = "follower" segment, the segment that's following this segment
		 *  ai[1] = "following" segment, the segment that this segment is following
		 *  ai[2] = Unused.
		 *  ai[3] = Hue color. Ranges from 0 to 360.
		 *  
		 *  localAI[0] = used when syncing changes to collision detection
		 *  localAI[1] = checking if Init() was called
		 */

		/// <summary>
		/// Which type of segment this NPC is considered to be
		/// </summary>
		public abstract WormSegmentType SegmentType { get; }

		/// <summary>
		/// The maximum velocity for the NPC
		/// </summary>
		public float MoveSpeed { get; set; }

		/// <summary>
		/// The rate at which the NPC gains velocity
		/// </summary>
		public float Acceleration { get; set; }

		/// <summary>
		/// The NPC instance of the head segment for this worm.
		/// </summary>
		public NPC HeadSegment => Main.npc[NPC.realLife];

		/// <summary>
		/// The NPC instance of the segment that this segment is following (ai[1]).  For head segments, this property always returns <see langword="null"/>.
		/// </summary>
		public NPC FollowingNPC => SegmentType == WormSegmentType.Head ? null : Main.npc[(int)NPC.ai[1]];

		/// <summary>
		/// The NPC instance of the segment that is following this segment (ai[0]).  For tail segment, this property always returns <see langword="null"/>.
		/// </summary>
		public NPC FollowerNPC => SegmentType == WormSegmentType.Tail ? null : Main.npc[(int)NPC.ai[0]];

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) {
			return SegmentType == WormSegmentType.Head ? null : false;
		}

		private bool startDespawning;

		public sealed override bool PreAI() {
			if (NPC.localAI[1] == 0) {
				NPC.localAI[1] = 1f;
				Init();
			}

			if (SegmentType == WormSegmentType.Head) {
				HeadAI();

				if (!NPC.HasValidTarget) {
					NPC.TargetClosest(true);

					if (!NPC.HasValidTarget && NPC.boss) {
						NPC.velocity.Y += 8f;

						MoveSpeed = 1000f;

						if (!startDespawning) {
							startDespawning = true;

							NPC.timeLeft = 90;
						}
					}
				}
			}
			else {
				BodyTailAI();
			}

			return true;
		}

		internal virtual void HeadAI() { }

		internal virtual void BodyTailAI() { }

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			TheConfectionRebirth.GummyWyrmShaderData.Shader.Parameters["uColor"].SetValue(drawColor.ToVector3());
			TheConfectionRebirth.GummyWyrmShaderData.Shader.Parameters["uHue"].SetValue(NPC.ai[3] / 360f);
			TheConfectionRebirth.GummyWyrmShaderData.Apply();

			Draw(spriteBatch, TextureAssets.Npc[Type].Value, screenPos, drawColor);
			return false;
		}

		protected virtual void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 screenPos, Color drawColor) {
			if (NPC.IsABestiaryIconDummy)
			{
				return;
			}

			var drawData = new DrawData {
				texture = texture,
				position = NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY),
				sourceRect = NPC.frame,
				color = drawColor,
				rotation = NPC.rotation,
				origin = NPC.frame.Size() / 2f,
				scale = new(NPC.scale),
				effect = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally
			};

			Main.EntitySpriteDraw(drawData);
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
		}

		public abstract void Init();
	}
}