using Galactica.Items;
using Galactica.NPCs.burningdemon;
using Galactica.NPCs.cosmicorb;
using Galactica.NPCs.galactica_i;
using Galactica.NPCs.worm;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Galactica.Items.Weapons.Alchemy;
using Galactica.Items.Weapons.Alchemy.celestial;
using System.Collections.Generic;

namespace Galactica
{
	public class ModGlobalNPC : GlobalNPC
	{
		public bool tInferno;
		public bool pTised;
		public bool Electrified;
		public override void ResetEffects(NPC npc) {
			tInferno = false;
			pTised = false;
			Electrified = false;
		}
		public override void UpdateLifeRegen(NPC npc, ref int damage) {
			if (tInferno) {
				if (npc.lifeRegen > 0) {
					npc.lifeRegen = 0;
				}
				npc.lifeRegen -= 16;
				if (damage < 8) {
					damage = 8;
				}
			}
            if (Electrified)
            {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, 0, 0, 0, Color.White, Main.rand.NextFloat(1f, 2f));
				if (npc.lifeRegen > 0)
				{
					npc.lifeRegen = 0;
				}
				if (Main.GameUpdateCount % 4 == 0)
				{
					npc.life -= Main.LocalPlayer.HeldItem.damage / 16;
					CombatText.NewText(npc.Hitbox, Color.LightBlue, Main.LocalPlayer.HeldItem.damage / 16, true, true);
					npc.checkDead();
				}
			}
			if (pTised) {
				if (npc.lifeRegen > 0) {
					npc.lifeRegen = 0;
				}
				int num = 128 + (int)(npc.lifeMax * 0.001f);
				npc.lifeRegen -= 2 * num;
				if (damage < num) {
					damage = num;
				}
			}
		}


		public override void OnHitPlayer(NPC npc, Player player, int damage, bool crit)
		{
			player.GetModPlayer<StatsPlayer>().DefenseXP += (int)(damage);
			if(player.GetModPlayer<StatsPlayer>().DefenseXP > (float)((player.GetModPlayer<StatsPlayer>().DefenseSkill * 50) + (player.GetModPlayer<StatsPlayer>().DefenseSkill * player.GetModPlayer<StatsPlayer>().DefenseSkill))){
				player.GetModPlayer<StatsPlayer>().DefenseXP = 0;
				player.GetModPlayer<StatsPlayer>().DefenseSkill += 1;
				Main.NewText("Defence Lvl is now " + player.GetModPlayer<StatsPlayer>().DefenseSkill, Color.Green, false);
			}
			if(EXPlayer.nfission) Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 696, 33, 0, player.whoAmI, 0f, 0f);
			if(world.overkillmode && npc.type == NPCID.EyeofCthulhu){
				NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), NPCID.DemonEye, npc.whoAmI, 0f, 0f, 0f, 0f, 255);						
				NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), NPCID.DemonEye, npc.whoAmI, 0f, 0f, 0f, 0f, 255);						
				NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), NPCID.WanderingEye, npc.whoAmI, 0f, 0f, 0f, 0f, 255);						
				NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), NPCID.WanderingEye, npc.whoAmI, 0f, 0f, 0f, 0f, 255);						
				NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), NPCID.ServantofCthulhu, npc.whoAmI, 0f, 0f, 0f, 0f, 255);						
				NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), NPCID.ServantofCthulhu, npc.whoAmI, 0f, 0f, 0f, 0f, 255);						
			}
		}
		public override bool PreNPCLoot(NPC npc){
			NPCLoader.blockLoot.Add(ItemID.BrokenHeroSword);
			return true;
		}
		public void LegItems(NPC loot){
			if(Main.player[loot.target].ZoneDungeon && (Main.rand.Next(0, 2) == 0) && world.downedOrb){
				Item.NewItem(loot.getRect(), mod.ItemType("EnergeticPlasma"), Main.rand.Next(1, 3), false, 0, false, false);
			}
			if(Main.player[loot.target].ZoneSkyHeight && (Main.rand.Next(0, 2) == 0) && world.downedOrb){
				Item.NewItem(loot.getRect(), mod.ItemType("WindEssence"), Main.rand.Next(1, 3), false, 0, false, false);
			}
			if(Main.player[loot.target].ZoneUnderworldHeight && (Main.rand.Next(0, 2) == 0) && world.downedOrb){
				Item.NewItem(loot.getRect(), mod.ItemType("DemonicCore"), Main.rand.Next(1, 3), false, 0, false, false);
			}
			if(loot.type == mod.NPCType("galactica_ii") && Main.rand.Next(1, 100) < 5){
				Item.NewItem(loot.getRect(), mod.ItemType("Bubblegum"), 1, false, 0, false, false);
			}
			if(loot.type == mod.NPCType("awakecat") && Main.rand.Next(1, 100) < 5){
				Item.NewItem(loot.getRect(), mod.ItemType("TrueDestruction"), 1, false, 0, false, false);
			}
		}
		public override void NPCLoot(NPC npc)
		{
			if(npc.type == NPCID.DD2Betsy){
				Item.NewItem(npc.getRect(), mod.ItemType("DragonScale"), 3, false, 0, false, false);
			}
			if(npc.type == NPCID.IceQueen){
				Item.NewItem(npc.getRect(), mod.ItemType("ConcentratedIce"), 3, false, 0, false, false);
			}
			if(npc.type == NPCID.Pumpking){
				Item.NewItem(npc.getRect(), mod.ItemType("HarvestEssence"), 3, false, 0, false, false);
			}
			if(Main.eclipse){
				Item.NewItem(npc.getRect(), mod.ItemType("EclipseEssence"), 1, false, 0, false, false);
			}
			if(world.BlueMoon){
				//Item.NewItem(npc.getRect(), mod.ItemType("CoalesscedColdness"), Main.rand.Next(0, 2), false, 0, false, false);
			}
			Player player = Main.player[npc.target];
			if(player.GetModPlayer<EXPlayer>().bonusDmg < 2.50f && !npc.boss){
				player.GetModPlayer<EXPlayer>().bonusDmg+= 0.02f;
			}	
			if(player.GetModPlayer<EXPlayer>().bonusDmg < 2.50f && npc.boss){
				player.GetModPlayer<EXPlayer>().bonusDmg+= 0.5f;
			}
			LegItems(npc);
			if(npc.boss){
				int val = Main.rand.Next(50, 500);
				if(val < 1) val = 1;
				Item.NewItem(npc.getRect(), ModContent.ItemType<UltimatumFragment>(), val, false, 0, false, false);
			}
			if(npc.type == NPCID.CultistBoss){
				if(Main.rand.Next(0, 2) == 0){
					Item.NewItem(npc.getRect(), ItemID.BoringBow, 1, false, 0, false, false);
				}
			}
			if(npc.type == NPCID.WallofFlesh && !Main.expertMode){
				if(Main.rand.Next(0, 2) == 0){
					Item.NewItem(npc.getRect(), ModContent.ItemType<alchemyemblem>(), 1, false, 0, false, false);
				}
			}
			if(npc.type == NPCID.Mothron){
				if(Main.rand.NextBool()){
					Item.NewItem(npc.getRect(), ModContent.ItemType<brokenherobar>(), Main.rand.Next(5, 10), false, 0, false, false);
				}
			}
			if(npc.type == NPCID.Plantera && !Main.expertMode){
				Item.NewItem(npc.getRect(), ModContent.ItemType<terrasteel>(), Main.rand.Next(5, 16), false, 0, false, false);
			}
			if(Main.player[npc.target].GetModPlayer<EXPlayer>().kCrown == true){
				Item.NewItem(npc.getRect(), ItemID.Heart, Main.rand.Next(0, 1), false, 0, false, false);
			}
			if(Main.player[npc.target].GetModPlayer<EXPlayer>().dhorn == true){
				if(Main.rand.NextBool()){
					Item.NewItem(npc.getRect(), ItemID.SoulofLight, 1, false, 0, false, false);
				}
				else{
					Item.NewItem(npc.getRect(), ItemID.SoulofNight, 1, false, 0, false, false);
				}
			}
			if (npc.type == 439)
			{
				Item.NewItem(npc.getRect(), mod.ItemType("cultistbag"), 1, false, 0, false, false);
			}
			if(npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofWorldsHead){
				NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), NPCID.BigEater, npc.whoAmI, 0f, 0f, 0f, 0f, 255);						
			}
			if (npc.type == 398)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("meteorscroll"), 1, false, 0, false, false);
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SuperclusterFragment"), Main.rand.Next(27, 55), false, 0, false, false);
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("UltraMode"), 1, false, 0, false, false);
				Main.NewText("A cosmic energy is release across the world", Color.Red, false);
				Main.NewText("A Starlight meteor falls", Color.Purple, false);
				Main.NewText("Lunar enemies are supercharged", new Color(255, 0, 255), false);
				Main.NewText("Luminite is springing up across the world", new Color(0, 255, 123), false);
				if(!Main.gameMenu){
					MoonGen();
				}
			}
		}
		public static void MoonGen(){
				for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0005); i++)
				{
					int num = WorldGen.genRand.Next(0, Main.maxTilesX);
					int y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					if (Main.tile[num, y].active() && Main.tile[num, y].type == 1)
					{
						if(Main.rand.NextBool()){
							WorldGen.TileRunner(num, y, (double)WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), ModContent.TileType<redcosmicoretile>(), false, 0f, 0f, false, true);
						}
						else {
							WorldGen.TileRunner(num, y, (double)WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), ModContent.TileType<purplecosmicoretile>(), false, 0f, 0f, false, true);
						}
					}
				}
				for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0007); i++)
				{
					int num = WorldGen.genRand.Next(0, Main.maxTilesX);
					int y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					if (Main.tile[num, y].active() && Main.tile[num, y].type == 1)
					{
						WorldGen.TileRunner(num, y, (double)WorldGen.genRand.Next(10, 17), WorldGen.genRand.Next(10, 17), TileID.LunarOre, false, 0f, 0f, false, true);
					}
				}
				int found2 = 1;
				for(int i3 = 0; i3 != found2; i3 = i3 + 0){
					int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
					int y3 = WorldGen.genRand.Next((int)(Main.maxTilesY * 0.75f), Main.maxTilesY);
					if(Main.tile[num3, y3].active()) {
						found2 = 0;
						WorldGen.TileRunner(num3, y3, (double)WorldGen.genRand.Next(52, 75), WorldGen.genRand.Next(52 , 75), ModContent.TileType<starlightstonetile>(), false, 0f, 0f, false, true);
					}
					else{
						num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
						y3 = WorldGen.genRand.Next((int)(Main.maxTilesY * 0.75f), Main.maxTilesY);
					}
				}
				int found = 1;
				for(int i2 = 0; i2 != found; i2 = i2 + 0){
					int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
					int y3 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					if(Main.tile[num3, y3].type == ModContent.TileType<starlightstonetile>()) {
						found = 0;
						WorldGen.TileRunner(num3, y3, (double)WorldGen.genRand.Next(22, 22), WorldGen.genRand.Next(22 , 22), ModContent.TileType<starlightmeteortile>(), false, 0f, 0f, false, true);
					}
					else{
						num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
						y3 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					}
				}
		}
		OverkillAI aiOverkill = new OverkillAI();
		public override bool InstancePerEntity => true;
		public override void AI(NPC npc){
			if(pTised){
				npc.color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			}
			if(npc.type == 22 && Main.worldName == "Galactica"){
				npc.GivenName = "Luna";
			}
			
			if (world.overkillmode) aiOverkill.DoOverkillAI(npc);
		}
		public override void SetDefaults(NPC npc){
		}
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			if (type == 54)
			{
				shop.item[nextSlot].SetDefaults(225, false);
				shop.item[nextSlot].shopCustomPrice = 10;
				nextSlot++;
			}
			if (type == NPCID.Dryad)
			{
				shop.item[nextSlot].SetDefaults(ItemID.NaturesGift, false);
				shop.item[nextSlot].shopCustomPrice = 1000;
				nextSlot++;
			}
			if (type == NPCID.Merchant)
			{
				shop.item[nextSlot].SetDefaults(ItemID.GoldWatch, false);
				shop.item[nextSlot].shopCustomPrice = 10;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MinecartTrack, false);
				shop.item[nextSlot].shopCustomPrice = 1;
				nextSlot++;
			}
			if (type == 178)
			{
				shop.item[nextSlot].SetDefaults(521, false);
				shop.item[nextSlot].shopCustomPrice = 1500;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(520, false);
				shop.item[nextSlot].shopCustomPrice = 1500;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(575, false);
				shop.item[nextSlot].shopCustomPrice = 5000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(547, false);
				shop.item[nextSlot].shopCustomPrice = 5000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(548, false);
				shop.item[nextSlot].shopCustomPrice = 5000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(549, false);
				shop.item[nextSlot].shopCustomPrice = 5000;
				nextSlot++;
			}
			if (NPC.downedMoonlord && type == 108)
			{
				shop.item[nextSlot].SetDefaults(3460, false);
				nextSlot++;
			}
		}
	}
}
