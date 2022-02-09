using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Galactica.Buffs;
using Galactica.Items;
using Galactica.Projectiles;

namespace Galactica
{
	public class EXPlayer : ModPlayer
	{
		public int DarkHits;
		public float bonusDmg;
		public static bool LoveStruck;
		public static bool Stellae;
		public static bool NHeal;
		public static bool Machina;
		public static bool MEnhcance;
		public static bool FStrike;
		public static bool Inferno;
		public static bool Imposter;
		public static bool Supernovae;
		public static bool Reaped;
		public static bool CMedal;
		public static bool LSheath;
		public static bool TDoll;
		public static bool MDagger;
		public static bool Velocity;
		public int lesslife;
		public static bool CrystAcc;
		public bool LCandy;
		public static bool TFlask;
		public bool Feather;
		public static bool PQuiv;
		public bool eFlames;
		public bool badHeal;		
		public bool aurealisbonus;
		public bool Acorn;
		public bool Sentry;
		public static bool cotu;
		public bool CSentry;
		public bool univbonus;
		public static bool HRing;
		public static bool SSheath;
		public int healHurt;
		public bool bonecharm;
		public bool radiation;
		public bool radiation2;
		public static int strength = 1;
		public bool ewire;
		public bool zoneStar;
		public bool zoneFlesh;
		public bool kCrown;
		public static bool tmaid;
		public static bool nfission;
		public bool car;
		public bool mcharm;
		public bool cosmicdiamond;
		public bool Overkill;
		public static bool mconv;
		public bool dhorn;
		public static bool crushing;
		public const int maxcosmicheart = 10;
		public int cosmicheart = 0;
		public const int maxgalacticheart = 10;
		public int galacticheart = 0;
		public int glitchheart = 0;
		public static int DCore = 0;
		public const int maxcrystal = 10;
		public int crystal = 0;
		public int demon = 0;
		public const int maxdemon = 1;
		public bool ccharm;
		public static bool darkheart;
		public override void ModifyNursePrice(NPC nurse, int health, bool removeDebuffs, ref int price){
			if(NHeal) price = 1;
		}
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (player.HeldItem.summon && Main.rand.NextBool(2))
			{
				CombatText.NewText(target.getRect(), Color.Red, damage, true, true);
				target.StrikeNPC(damage, knockback, hitDirection);
			}
        }
        public override void ProcessTriggers(TriggersSet triggersSet) {
			if (Galactica.Teleport.JustPressed && cotu) {
				player.Spawn();
			}
            
		}
		
		public override void ResetEffects()
		{
			Foul = false;
			zoneFlesh = false;
			hemorrhage = false;
			headeache = false;
			Stellae = false;
			NHeal = false;
			Machina = false;
			MEnhcance = false;
			FStrike = false;
			CMedal = false;
			PQuiv = false;
			Imposter = false;
			Reaped = false;
			Sorcering = false;
			Inferno = false;
			SSheath = false;
			Supernovae = false;
			TDoll = false;
			HRing = false;
			LSheath = false;
			if (LCandy)
			{
				player.extraAccessorySlots = 1;
			}
			if (LCandy && player.extraAccessory && (Main.expertMode || Main.gameMenu))
			{
				player.extraAccessorySlots = 2;
			}
			BallAcc = false;
			LoveStruck = false;
			MDagger = false;	
			mconv = false;
			Velocity = false;
			crushing = false;
			cotu = false;
			Feather = false;
			Acorn = false;
			Sentry = false;
			CSentry = false;
			tmaid = false;
			eFlames = false;
			kCrown = false;
			bonecharm = false;
			badHeal = false;
			nfission = false;
			ewire = false;
			TFlask = false;
			ccharm = false;
			CrystAcc = false;
			mcharm = false;
			aurealisbonus = false;
			univbonus = false;
			dhorn = false;
			car = false;
			radiation = false;
			radiation2 = false;
			healHurt = 0;
			darkheart = false;
			cosmicdiamond = false;
			player.statLifeMax2 += cosmicheart * 5;	
			player.statLifeMax2 += glitchheart * 175;	
			player.statLifeMax2 += crystal * 5;
			player.statLifeMax2 += galacticheart * 5;
			player.statManaMax2 += DCore * 75;
			player.statLifeMax2 += demon * 75;
		}
		public override void UpdateBiomes() {
			zoneStar = world.starTiles > 50;
			zoneFlesh = world.fleshTiles > 50;
			player.ZoneCrimson = world.fleshTiles > 50;
			if(world.BlueMoon){
				player.AddBuff(mod.BuffType("FrigidDefeat"), 5);
			}
			if(player.statLife < 0){
				player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " Tried to break the rules"), 999999, 0, false);
			}
		}
		public override bool CustomBiomesMatch(Player other) {
			EXPlayer modOther = other.GetModPlayer<EXPlayer>();
			return zoneStar == modOther.zoneStar;
		}
		public override void CopyCustomBiomesTo(Player other) {
			EXPlayer modOther = other.GetModPlayer<EXPlayer>();
			modOther.zoneStar = zoneStar;
		}

		public override void SendCustomBiomes(BinaryWriter writer) {
			BitsByte flags = new BitsByte();
			flags[0] = zoneStar;
			writer.Write(flags);
		}
		public override void ReceiveCustomBiomes(BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			zoneStar = flags[0];
		}
		public override void UpdateBiomeVisuals() {
			bool wet = player.wet && !player.honeyWet && !player.lavaWet;
			if (wet && zoneStar){
				player.AddBuff(ModContent.BuffType<radiationbuff>(), 2, true);
			}
			if (wet && zoneFlesh)
			{
				player.AddBuff(BuffID.Lovestruck, 2, true);
			}
			if (world.radTiles > 2){
				player.AddBuff(ModContent.BuffType<radbuff>(), 2, true);
			}
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
			{
				Sorcering = false;
			}
		}
		public override void UpdateBadLifeRegen() {
			if (radiation) {
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
				if (player.lifeRegen > 0) {
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second.
				player.lifeRegen -= 16;
			}
			if (radiation2) {
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
				if (player.lifeRegen > 0) {
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second.
				player.lifeRegen -= 16;
			}
			if (eFlames) {
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
				if (player.lifeRegen > 0) {
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second.
				player.lifeRegen -= 8;
			}
			if (hemorrhage)
			{
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second.
				player.lifeRegen -= 32;
			}
			if (crushing) {
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
				if (player.lifeRegen > 0) {
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second.
				player.lifeRegen -= 128;
			}
			if (Reaped) {
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
				if (player.lifeRegen > 0) {
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second.
				player.lifeRegen -= 45;
			}
			if (Inferno) {
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
				if (player.lifeRegen > 0) {
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second.
				player.lifeRegen -= 38;
			}
			if (healHurt > 0) {
				if (player.lifeRegen > 0) {
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				player.lifeRegen -= 120 * healHurt;
			}
		}
		public int dripTimer;
		public void Drip()
		{
			if (dripTimer > 0) dripTimer--;
			if (dripTimer <= 0)
			{
				for (int i = 0; i < 3; i++)
				{
					Projectile.NewProjectile(player.Center + new Vector2(Main.rand.Next(-25, 25), Main.rand.Next(-25, 25)), new Vector2(0, 5) * Main.rand.NextFloat(0.5f, 2.5f), mod.ProjectileType("BloodFriendly"), 200, 0, player.whoAmI);
					Projectile.NewProjectile(player.Center + new Vector2(Main.rand.Next(-25, 25), Main.rand.Next(-25, 25)), new Vector2(0, 5) * Main.rand.NextFloat(0.5f, 2.5f), mod.ProjectileType("SteamFriendly"), 200, 0, player.whoAmI);
				}
				dripTimer = 26;
			}
		}
		public override void PostUpdate()
        {
			if(BallAcc)Drip();
        }
        public override void OnEnterWorld(Player player)
		{
			string[] source = new string[9]
			{
				"cringe cat",
				"goliathgamer",
				"larry koopa",
				"sodium 5\ufffd",
				"spy_kid",
				"daniel the angel",
				"rowing!",
				"p l a n t",
				"bean boi"
			};
			string[] darkNames = new string[3]{
				"dark",
				"luna",
				"darkreality"
			};
			lesslife = player.statLifeMax - 75;
			if (world.overkillmode)
			{
				player.potionDelay = 90;
			}
			else
			{
				player.endurance = 1f;
				player.lifeRegen = 15;
				player.potionDelay = 6;
				player.allDamage = 1f;
			}
			if (source.Contains(player.name.ToLower()))
			{
				Main.NewText("Thank You", Microsoft.Xna.Framework.Color.Purple, false);
			}
			if (darkNames.Contains(player.name.ToLower()))
			{
				Main.NewText("Stop Impersonating Dark", Microsoft.Xna.Framework.Color.Pink, false);
			}
			Mod[] LoadedMods = new Mod[]{
				ModLoader.GetMod("UpgradedAccessories"),
				ModLoader.GetMod("CalamityMod"),
				ModLoader.GetMod("FargowiltasSouls"),
				ModLoader.GetMod("ThoriumMod"),
				ModLoader.GetMod("Redemption"),
				ModLoader.GetMod("ExampleMod"),
				ModLoader.GetMod("TerrariaOverhaul")
			};
			string[] Text = new string[]{
				"Even more accessories than before!",
				"Did you see a worm floating around here somewhere?",
				"Dont embrace eternity",
				"What do dreams even taste like",
				"Ew are you infected?",
				"Did you really enable example mod and Galactica?",
				"Galactica Overhaul: Now Fireproof!"
			};
			Color[] Colours = new Color[]{
				Color.LightBlue,
				Color.OrangeRed,
				new Color(0, 255, 151),
				Color.Purple,
				Color.Green,
				Color.White,
				Color.Red
			};
			int i = 0;
			foreach(Mod loadedMod in LoadedMods){
				if (loadedMod != null)
				{
					Main.NewText(Text[i], Colours[i], false);
				}
				i++;
			}
			player.statLifeMax2 += cosmicheart * 5;
			player.statLifeMax2 += crystal * 5;
			player.statLifeMax2 += galacticheart * 5;
			player.statLifeMax2 += demon * 75;
			player.statLifeMax2 += glitchheart * 175;
		}

		public override void UpdateDead()
		{
			Sorcering = false;
			Velocity = false;
			crushing = false;
			eFlames = false;
			radiation = false;
			radiation2 = false;
			badHeal = false;
			car = false;
		}

		public override TagCompound Save()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			TagCompound val = new TagCompound();
			val.Add("cosmicheart", (object)cosmicheart);
			val.Add("galacticheart", (object)galacticheart);
			val.Add("DCore", (object)DCore);
			val.Add("crystal", (object)crystal);
			val.Add("demon", (object)demon);
			val.Add("glitchheart", (object)glitchheart);
			val.Add("LCandy", (object)LCandy);
			return val;
		}
		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath) {
			Item item = new Item();
			item.SetDefaults(ModContent.ItemType<starterbag>());
			item.stack = 1;
			items.Add(item);
		}

		public override void Load(TagCompound tag)
		{
			cosmicheart = tag.GetInt("cosmicheart");
			galacticheart = tag.GetInt("galacticheart");
			DCore = tag.GetInt("DCore");
			crystal = tag.GetInt("crystal");
			demon = tag.GetInt("demon");
			glitchheart = tag.GetInt("glitchheart");
			LCandy = tag.GetBool("LCandy");
		}
		public static Texture2D dHeartText;
		public static Texture2D cHeartText;
		public static Texture2D gHeartText;
		public static Texture2D crHeartText;
		public static Texture2D glHeartText;
		public static Texture2D origHeartText;
		public static Texture2D origHeartText2;
		public static Texture2D heart;
		public static Texture2D heart2;
        internal static float timer;

		public bool Sorcering;
        internal bool univbonusS;

        public Item AlchemyItem { get; set; }
        public bool hemorrhage { get; internal set; }
        public bool headeache { get; internal set; }
        public bool BallAcc { get; internal set; }
        public bool Foul { get; internal set; }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if(Reaped){
				r = 167;
				g = 255;
				b = 191;
			}
			if(!Main.gameMenu){
				dHeartText = ModContent.GetTexture("Galactica/ExtraTextures/dheart");
				cHeartText = ModContent.GetTexture("Galactica/ExtraTextures/cheart");
				gHeartText = ModContent.GetTexture("Galactica/ExtraTextures/gheart");
				crHeartText = ModContent.GetTexture("Galactica/ExtraTextures/crheart");
				glHeartText = ModContent.GetTexture("Galactica/ExtraTextures/glheart");
				heart = ModContent.GetTexture("Terraria/Heart");
				heart2 = ModContent.GetTexture("Terraria/Heart2");
				if(demon != 0 && !(demon <= cosmicheart)){
					Main.heartTexture = dHeartText;
				}
				else if(cosmicheart != 0 && !(cosmicheart <= galacticheart)){
					Main.heart2Texture =  cHeartText;
				}
				else if(galacticheart != 0 && !(galacticheart <= crystal)){
					Main.heart2Texture =  gHeartText;
				}
				else if(crystal != 0 && !(crystal <= glitchheart)){
					Main.heart2Texture = crHeartText;
				}
				else if(glitchheart > 0){
					Main.heart2Texture =  glHeartText;
				}
				else{
					Main.heartTexture =  heart;
					Main.heart2Texture =  heart2;
				}
			}
			if(Main.gameMenu){
				Main.heartTexture = ModContent.GetTexture("Terraria/Heart");
				Main.heart2Texture = ModContent.GetTexture("Terraria/Heart2");
			}
		}

		public override void LoadLegacy(BinaryReader reader)
		{
			reader.ReadInt32();
		}
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
			if (Foul)
			{
				for (int i = 0; i < 5; i++)
				{
					Vector2 pos = target.Center + new Vector2(Main.rand.Next(-75, 75), -520);
					Vector2 vel = Vector2.Normalize(target.Center - pos) * 7;
					Projectile.NewProjectile(pos, vel, Main.rand.NextBool() ? ProjectileID.IchorSplash : mod.ProjectileType("SteamFriendly"), 50, 0, player.whoAmI);
				}
			}
		}
        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
			if (world.ultramode) player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " Failed"), 1, 0);
        }
		public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
		{
			if (world.ultramode) player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " Failed"), 1, 0);
		}
		public override void OnHitNPCWithProj(Projectile projectile, NPC target, int damage, float knockback, bool crit){
			if(LoveStruck && projectile.type != 370){
				Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, 370, projectile.damage, 0, player.whoAmI, 0f, 0f);
			}
			if(MEnhcance && player.HeldItem.magic && crit){
				player.statMana += (int)(0.25f * player.HeldItem.mana);
				CombatText.NewText(player.Hitbox, Microsoft.Xna.Framework.Color.Blue, (int)(0.25f * player.HeldItem.mana), true, false);
			}
			if(FStrike && player.HeldItem.melee && damage > (int)(target.lifeMax * 0.03f)){
				player.AddBuff(198, 10000, true);
			}
			if(aurealisbonus == true){
				target.AddBuff(BuffID.Frostburn, 300, true);
			}
			if(ewire == true){
				target.AddBuff(BuffID.Electrified, 300, true);
			}
			if(tmaid || projectile.type == ModContent.ProjectileType<terradiskproj>() || projectile.type == ModContent.ProjectileType<terraflame>() || projectile.type == ModContent.ProjectileType<terraknife>() || projectile.type == 132) target.AddBuff(ModContent.BuffType<terrainferno>(), 300, true);
			if(cotu){
				target.AddBuff(BuffID.Frostburn, 300, true);
				target.AddBuff(BuffID.OnFire, 300, true);
				target.AddBuff(BuffID.Bleeding, 300, true);
				target.AddBuff(BuffID.Ichor, 300, true);
				target.AddBuff(mod.BuffType("terrainferno"), 300, true);
				target.AddBuff(39, 300, true);
			}
            if (Foul && projectile.type != mod.ProjectileType("BloodFriendly") && projectile.type != mod.ProjectileType("SteamFriendly"))
            {
				for (int i = 0; i < 5; i++)
				{
					Vector2 pos = target.Center + new Vector2(Main.rand.Next(-75, 75), -520);
					Vector2 vel = Vector2.Normalize(target.Center - pos) * Main.rand.Next(7, 15);
					Projectile.NewProjectile(pos, vel, Main.rand.NextBool() ? mod.ProjectileType("BloodFriendly") : mod.ProjectileType("SteamFriendly"), 50, 0, projectile.owner);
				}
			}
		} 	
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			if (world.ultramode) return true;
			if(univbonus && player.FindBuffIndex(ModContent.BuffType<univcooldown>()) < 0){
				player.statLife = (int)(0.3f *(player.statLifeMax + player.statLifeMax2));
				player.AddBuff(ModContent.BuffType<univcooldown>(), 7200);
				Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color(3, 202, 252);
				CombatText.NewText(player.Hitbox, color2, ((int)(0.3f *(player.statLifeMax + player.statLifeMax2))), true, false);
				return false;
			}
			if(Machina && player.FindBuffIndex(ModContent.BuffType<univcooldown>()) < 0){
				player.statLife = (int)(0.3f *(player.statLifeMax + player.statLifeMax2));
				player.AddBuff(ModContent.BuffType<univcooldown>(), 7200);
				Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color(3, 202, 252);
				CombatText.NewText(player.Hitbox, color2, ((int)(0.3f *(player.statLifeMax + player.statLifeMax2))), true, false);
				player.position += new Vector2(Main.rand.Next(-1280, 1280),Main.rand.Next(-1280, 1280));
				return false;
			}
			if(mconv && player.FindBuffIndex(ModContent.BuffType<univcooldown>()) < 0){
				if(player.statMana > 1){
					player.statLife += (int)(player.statMana / 2);
					player.AddBuff(ModContent.BuffType<univcooldown>(), 18000);
					Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(3, 252, 198);
					CombatText.NewText(player.Hitbox, color, ((int)(player.statMana / 2)), true, false);
					player.statMana = 0;
					return false;
				}
			}
			if(ModContent.GetInstance<GalacticaConfig>().Souls) Item.NewItem(new Vector2(Main.spawnTileX * 16f, Main.spawnTileY * 16f), Vector2.One * 32f, mod.ItemType("GreaterSoulEssence"), Main.rand.Next(3, 7));
			return true;
		}
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
			if(damage > 50 && player.statLife > damage && ModContent.GetInstance<GalacticaConfig>().Souls)
            {
				Item.NewItem(player.position, Vector2.One * 64f, mod.ItemType("LesserSoulEssence"), Main.rand.Next(1, 3));
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }
        public void Hurt(Player player)
		{
			if(NPC.AnyNPCs(mod.NPCType("awakecat")) || NPC.AnyNPCs(mod.NPCType("phase2"))){
				DarkHits += 1;
			}
			if(nfission) Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 696, 100, 0, player.whoAmI, 0f, 0f);

		}
	}
}
