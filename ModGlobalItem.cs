using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Galactica.Items.Weapons;
using Galactica.NPCs.CosmicGuardians;
using Galactica.Projectiles;
using Galactica.Buffs;
using Galactica.Reforges;
using Galactica.Projectiles.Summon;
using Galactica.Items.Weapons.Throwing;
namespace Galactica
{
	public class ModGlobalItem : GlobalItem
	{
		public float speed = new float();
		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;

        public static Dictionary<int, string> NarratorTexts { get; set; }

        public override bool ConsumeItem(Item item, Player player){
			if(EXPlayer.LSheath && IsTHWeapon(item.type)){
				return Main.rand.NextFloat() >= 0.10f;
			}
			if(EXPlayer.SSheath && IsTHWeapon(item.type)){
				return Main.rand.NextFloat() >= 0.25f;
			}
			return true;
		}
		public override bool ConsumeAmmo(Item item, Player player){
			if(EXPlayer.PQuiv){
				return Main.rand.NextFloat() >= 0.75f;
			}
			return true;
		}
		public override void SetDefaults(Item item)
		{

			if(item.useTime == 0)  item.useTime = 1;	
			if(item.useAnimation == 0)  item.useAnimation = 1;
			if(item.shoot == 0 && item.damage > 0 && !(item.pick > 0) && !(item.hammer > 0) && !(item.axe > 0)) item.shoot = mod.ProjectileType("blank");
			if(throwingGlobalWeapons.Contains(item.type)){
				item.thrown = false;
				speed = item.shootSpeed;
			}
			if(item.type == ItemID.BoringBow){
				item.damage = 156;
				item.ranged = true;
				item.width = 40;
				item.height = 20;
				item.useTime = 15;
				item.useAnimation = 15;
				item.useStyle = 5;
				item.noMelee = true;
				item.knockBack = 2f;
				item.value = 1000000;
				item.rare = 4;
				item.UseSound = SoundID.Item11;
				item.autoReuse = true;
				item.shoot = 1;
				item.shootSpeed = 8f;
				item.useAmmo = AmmoID.Arrow;
				item.crit = 27;
			}
			for(int i = 1; i < 99; i++){
				if(item.type == ItemID.CopperShortsword && EXPlayer.darkheart){
					item.damage = 10000;
				}
			}
		}
		public static List<int> items = new List<int>(){
			ItemID.Shuriken
		}; 
		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			if (context == "bossBag" && arg == ItemID.KingSlimeBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("kingscrown"), 1);
			}
			if (context == "bossBag" && arg == ItemID.EyeOfCthulhuBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("eyestooth"), 1);
			}
			if (context == "bossBag" && arg == ItemID.BrainOfCthulhuBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("crimsonvile"), 1);
			}
			if (context == "bossBag" && arg == ItemID.EaterOfWorldsBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("corruptvile"), 1);
			}
			if (context == "bossBag" && arg == ItemID.QueenBeeBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("royaljelly"), 1);
			}
			if (context == "bossBag" && arg == ItemID.SkeletronBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("charmofbones"), 1);
			}
			if (context == "bossBag" && arg == ItemID.WallOfFleshBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("demonheart"), 1);
			}
			if (context == "bossBag" && arg == ItemID.WallOfFleshBossBag)
			{
				int choice = Main.rand.Next(0, 3);
				if(choice == 0){
					player.QuickSpawnItem(mod.ItemType("alchemyemblem"), 1);
				}
				if(choice == 1){
					player.QuickSpawnItem(mod.ItemType("ThrowerEmblem"), 1);
				}
			}
			if (context == "bossBag" && arg == ItemID.TwinsBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("lasereye"), 1);
			}
			if (context == "bossBag" && arg == ItemID.DestroyerBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("hallowedvile"), 1);
			}
			if (context == "bossBag" && arg == ItemID.SkeletronPrimeBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("mechanicalcharm"), 1);
			}
			if (context == "bossBag" && arg == ItemID.PlanteraBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("shieldoflife"), 1);
			}
			if (context == "bossBag" && arg == ItemID.PlanteraBossBag)
			{
				player.QuickSpawnItem(mod.ItemType("terrasteel"), Main.rand.Next(5, 11));
			}
			if (context == "bossBag" && arg == ItemID.GolemBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("coreofthejungle"), 1);
			}
			if (context == "bossBag" && arg == ItemID.MoonLordBossBag && world.overkillmode)
			{
				player.QuickSpawnItem(mod.ItemType("championartefact"), 1);
				player.QuickSpawnItem(mod.ItemType("LunarCandy"), 1);
			}
		}
		public override void OnHitNPC(Item item, Player player, NPC npc, int damage, float knockback, bool crit)
		{
			if(EXPlayer.LoveStruck){
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, 370, item.damage, 0, player.whoAmI, 0f, 0f);
			}
			if(EXPlayer.FStrike && player.HeldItem.melee && damage > (int)(npc.lifeMax * 0.03f)){
				player.AddBuff(198, 10000, true);
			}
			if(EXPlayer.CMedal && IsTHWeapon(player.HeldItem.type)){
				int choice = Main.rand.Next(0, 3);
				switch(choice) {
					default:
						npc.AddBuff(BuffID.OnFire, 600, false);
						break;
					case(0):
						npc.AddBuff(BuffID.Frostburn, 600, false);
						break;
					case(1):
						npc.AddBuff(BuffID.OnFire, 600, false);
						break;
					case(2):
						break;

				}
			}
			if(EXPlayer.HRing && IsTHWeapon(player.HeldItem.type)){
				player.statLife += crit ? 2 : 1;
				player.HealEffect(crit ? 2 : 1, true);
			}
			player.GetModPlayer<StatsPlayer>().DMGXP += crit ? 2 : 1;
			if(player.GetModPlayer<StatsPlayer>().DMGXP > (float)((player.GetModPlayer<StatsPlayer>().DMGSkill * 50) + 50)){
				player.GetModPlayer<StatsPlayer>().DMGXP = 0;
				player.GetModPlayer<StatsPlayer>().DMGSkill += 1;
				Main.NewText("Damage Lvl is now " + player.GetModPlayer<StatsPlayer>().DMGSkill, Color.Green, false);
			}
			if(EXPlayer.tmaid || item.type == ItemID.TerraBlade) npc.AddBuff(ModContent.BuffType<terrainferno>(), 300, true);
			if(player.GetModPlayer<EXPlayer>().ewire == true){
				npc.AddBuff(mod.BuffType("Electrified"), 300, true);
			
			}
		}
		public List<int> Consumables = new List<int>();
		public int shoot; //funny
		public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int[] poss = new int[5]{ 64, -32, 0, 32, 64 };
			if(EXPlayer.Stellae && Main.rand.Next(0, 100) < (player.HeldItem.useTime * 2)){
				for(int i = 0; i < 5; i++){
					Vector2 position1 = Main.MouseWorld;
					position1.Y -= 400;
					position1.X += poss[i];
					Projectile.NewProjectile(position1.X, position1.Y, 0, 10, mod.ProjectileType("energybeam"), damage, knockBack, player.whoAmI, 0f, 0f);
				}
			}

			if(EXPlayer.MDagger && IsTHWeapon(item.type) && (item.stack > (item.maxStack * (4f / 5f)))){
				Consumables.Add(item.type);
				item.consumable = false;
			}
			if(IsTHWeapon(item.type) && Consumables.Contains(item.type) && !(item.stack > (item.maxStack * (4f / 5f)))){
				item.consumable = true;
			}
			if(EXPlayer.CrystAcc && item.summon && !item.melee && (player.ownedProjectileCounts[ModContent.ProjectileType<NatureCrystal>()] <= player.maxMinions)){
				Vector2 position1 = Main.MouseWorld;
				Projectile.NewProjectile(position1.X, position1.Y, speedX + Main.rand.Next(-5, 5), speedY + Main.rand.Next(-5, 5),mod.ProjectileType("NatureCrystal"), damage, knockBack, player.whoAmI, 0f, 0f);
			}
			if(EXPlayer.darkheart && !item.summon){
					Projectile.NewProjectile(position.X, position.Y, speedX + Main.rand.Next(-5, 5), speedY + Main.rand.Next(-5, 5), type, damage, knockBack, player.whoAmI, 0f, 0f);
					return true;
			}
			if(EXPlayer.Supernovae && !item.summon && IsTHWeapon(item.type) && (Main.rand.Next(0, 4) == 0)){
					Projectile.NewProjectile(position.X, position.Y, speedX + Main.rand.Next(-5, 5), speedY + Main.rand.Next(-5, 5), type, damage, knockBack, player.whoAmI, 0f, 0f);
					return true;
			}
			if(item.type == ItemID.BoringBow){
				type = ModContent.ProjectileType<boringarrow>();
			}
			if(player.GetModPlayer<EXPlayer>().bonecharm == true){
				if(Main.rand.Next(1, 3) == 2){
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 126, damage, knockBack, player.whoAmI, 0f, 0f);
					return true;
				}
				return true;
			}
			if(player.GetModPlayer<EXPlayer>().mcharm == true){
				if(Main.rand.Next(0, 10) > 5){
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 126, damage, knockBack, player.whoAmI, 0f, 0f);
					return true;
				}
				else{
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.RubyBolt, damage, knockBack, player.whoAmI, 0f, 0f);
					return true;
				}
				return true;
			}
			if(player.GetModPlayer<EXPlayer>().ccharm == true){
				if(Main.rand.Next(0, 10) > 5){
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.AmethystBolt, damage, knockBack, player.whoAmI, 0f, 0f);
					return true;
				}
				else{
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.RubyBolt, damage, knockBack, player.whoAmI, 0f, 0f);
					return true;
				}
				return true;
			}
			return true;
		}
		public static List<int> alchemyWeapons = new List<int>();
		public static List<int> throwingWeapons = new List<int>();
		public static List<int> throwingGlobalWeapons = new List<int>();
		public static bool IsAlcWeapon(int type)
		{
			return alchemyWeapons.Contains(type);
		}
		public void RegAlcWeapon(ModItem modItem){
			Item item = new Item();
			int type = item.type;
			if (!alchemyWeapons.Contains(type))
			{
				alchemyWeapons.Add(type);
			}
		}
		public static bool IsTHWeapon(int type)
		{
			return throwingWeapons.Contains(type);
		}
		public void RegTHWeapon(ModItem modItem){
			Item item = new Item();
			int type = item.type;
			if (!throwingWeapons.Contains(type))
			{
				throwingWeapons.Add(type);
			}
			foreach(int i in items){
				if(!throwingGlobalWeapons.Contains(i)){
					throwingGlobalWeapons.Add(type);
				}
			}
		}
		public int susCount = 0;
		public bool susDone;
		public override bool CanUseItem(Item item, Player player)
		{
			if (player.altFunctionUse == 2 && item.summon)
			{
				item.useTime = 20;
				item.useAnimation = 20;
				return true;
			}
			if (!(player.altFunctionUse == 2) && item.summon)
			{
				item.useTime = 1;
				item.useAnimation = 1;
				 return true;
			}
			if(EXPlayer.Imposter && Main.rand.Next(0, 10) == 0){
				player.statLife -= 33;
				return true;
			}
			return true;
		}
		public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat) {
			if(throwingGlobalWeapons.Contains(item.type)){
				add += THPlayer.ModPlayer(player).THD;
				mult *= THPlayer.ModPlayer(player).THDMult;
			}
		}
		public override void GetWeaponKnockback(Item item, Player player, ref float knockback) {
			// Adds knockback bonuses
			if(throwingGlobalWeapons.Contains(item.type)){
				knockback = 0 + THPlayer.ModPlayer(player).THKnockback;
				item.shootSpeed = THPlayer.ModPlayer(player).THS + speed;
			}
		}

		public override void GetWeaponCrit(Item item, Player player, ref int crit) {
			// Adds crit bonuses
			if(throwingGlobalWeapons.Contains(item.type)){
				crit += THPlayer.ModPlayer(player).THCrit;
			}
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if(item.type == ModContent.ItemType<ram>() || item.type == ModContent.ItemType<graphicscard>()){
				foreach (TooltipLine line2 in tooltips) {
					if (line2.text.Contains("melee damage")) {
						line2.text = item.damage + "gigabytes";
					}
				}
			}
			if(item.type == ModContent.ItemType<cheatweapon>()){
				foreach (TooltipLine line2 in tooltips) {
					if (line2.text.Contains("damage")) {
						line2.text = "∞ damage";
					}
				}
			}
			if(item.type == mod.ItemType("Stelae")){
				foreach (TooltipLine line2 in tooltips) {
					if (line2.text.Contains("Every Shot")) {
						if(Main.LocalPlayer.HeldItem.useTime * 2 < 100){
							line2.text = $"Every shot has a {Main.LocalPlayer.HeldItem.useTime * 2}% chance to fire a devastating blast cosmic rays down from the sky";
						}
						else line2.text = "Every shot has a 100% chance to fire a devastating blast cosmic rays down from the sky";
					}
				}
			}
			
			if(item.type == ModContent.ItemType<handoffate>() || item.type == ModContent.ItemType<coreoftruemelee>() || item.type == ModContent.ItemType<coreofbadaiming>()){
				foreach (TooltipLine line2 in tooltips) {
					if (line2.text.Contains("melee damage")) {
						line2.text = item.damage + " damage";
					}
				}
			}
			if(item.type == ItemID.BoringBow){
				foreach (TooltipLine line2 in tooltips) {
					if (line2.mod == "Terraria" && line2.Name == "ItemName") {
						line2.text = "Boring Bow";

					}

				}
			}
			if(throwingGlobalWeapons.Contains(item.type)){
				TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
				if (tt != null && items.Contains(item.type)) {
					// We want to grab the last word of the tooltip, which is the translated word for 'damage' (depending on what language the player is using)
					// So we split the string by whitespace, and grab the last word from the returned arrays to get the damage word, and the first to get the damage shown in the tooltip
					string[] splitText = tt.text.Split(' ');
					string damageValue = splitText.First();
					string damageWord = splitText.Last();
					// Change the tooltip text
					tt.text = damageValue + " test " + damageWord; //1.4 protection
				}
			}
			if(world.overkillmode && item.expert == true){
				foreach (TooltipLine line2 in tooltips) {
					if (line2.mod == "Terraria" && line2.Name == "ItemName") {
						line2.overrideColor = new Color(62, 0, 0);
					}
				}
			}
			if (NarratorTexts.ContainsKey(item.type))
			{
				tooltips.Add(new TooltipLine(mod, "NarratorText", "[c/bf00ff:Maybe the Narrator would like this]"));
			}
		}
	}
}
