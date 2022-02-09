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
using Galactica.Items.Weapons.Throwing;

namespace Galactica
{
	public class StatsPlayer : ModPlayer
	{
		public int DMGSkill;
		public float DMGXP;
		public int DefenseSkill;
		public float DefenseXP;
		public int SpeedSkill;
		public float SpeedXP;
		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath) {
			DMGSkill = 0;
			DMGXP = 0f;
		}
		public override TagCompound Save()
		{
			TagCompound val = new TagCompound();
			val.Add("DMGSkill", DMGSkill);
			val.Add("DMGXP", DMGXP);
			val.Add("DefenseSkill", DefenseSkill);
			val.Add("DefenseXP", DefenseXP);
			val.Add("SpeedSkill", SpeedSkill);
			val.Add("SpeedXP", SpeedXP);
			return val;
		}
		public override void Load(TagCompound tag)
		{
			DMGSkill = tag.GetInt("DMGSkill");
			DMGXP = tag.GetFloat("DMGXP");
			DefenseSkill = tag.GetInt("DefenseSkill");
			DefenseXP = tag.GetFloat("DefenseXP");
			SpeedSkill = tag.GetInt("SpeedSkill");
			SpeedXP = tag.GetFloat("SpeedXP");
		}

		public override void LoadLegacy(BinaryReader reader)
		{
			reader.ReadInt32();
		}
		public override void OnEnterWorld(Player player)
		{
		}
		public override void PostUpdateMiscEffects()
		{
			player.thrownDamage += player.GetModPlayer<THPlayer>().THDMult;
			player.thrownCrit += player.GetModPlayer<THPlayer>().THCrit;
			player.allDamage += (float)((float)DMGSkill / 200);
			player.allDamage += player.GetModPlayer<EXPlayer>().bonusDmg;
			player.endurance += (float)((float)DefenseSkill / 400);
			SpeedBuff();
		}
		public override void PostUpdateRunSpeeds()
		{
			float speedMult = 1f + (float)(SpeedSkill / 50);
			player.maxRunSpeed *= speedMult;
		}
		public void SpeedBuff(){
			if(player.velocity.X > 0){
				SpeedXP += (player.velocity.X) / 15f;
				if(player.GetModPlayer<StatsPlayer>().SpeedXP > (float)((player.GetModPlayer<StatsPlayer>().SpeedSkill * 150) + player.GetModPlayer<StatsPlayer>().SpeedSkill)){
					player.GetModPlayer<StatsPlayer>().SpeedXP = 0;
					player.GetModPlayer<StatsPlayer>().SpeedSkill += 1;
					Main.NewText("Speed Lvl is now " + player.GetModPlayer<StatsPlayer>().SpeedSkill, Color.Green, false);
				}
			}
			if(player.velocity.X < 0){
				SpeedXP += (player.velocity.X) / -15f;
				if(player.GetModPlayer<StatsPlayer>().SpeedXP > (float)((player.GetModPlayer<StatsPlayer>().SpeedSkill * 150) + player.GetModPlayer<StatsPlayer>().SpeedSkill)){
					player.GetModPlayer<StatsPlayer>().SpeedXP = 0;
					player.GetModPlayer<StatsPlayer>().SpeedSkill += 1;
					Main.NewText("Speed Lvl is now " + player.GetModPlayer<StatsPlayer>().SpeedSkill, Color.Green, false);
				}
			}
			if(player.velocity.Y > 0){
				SpeedXP += (player.velocity.Y) / 15f;
				if(player.GetModPlayer<StatsPlayer>().SpeedXP > (float)((player.GetModPlayer<StatsPlayer>().SpeedSkill * 150) + player.GetModPlayer<StatsPlayer>().SpeedSkill)){
					player.GetModPlayer<StatsPlayer>().SpeedXP = 0;
					player.GetModPlayer<StatsPlayer>().SpeedSkill += 1;
					Main.NewText("Speed Lvl is now " + player.GetModPlayer<StatsPlayer>().SpeedSkill, Color.Green, false);
				}
			}
			if(player.velocity.Y < 0){
				SpeedXP += (player.velocity.Y) / -15f;
				if(player.GetModPlayer<StatsPlayer>().SpeedXP > (float)((player.GetModPlayer<StatsPlayer>().SpeedSkill * 150) + player.GetModPlayer<StatsPlayer>().SpeedSkill)){
					player.GetModPlayer<StatsPlayer>().SpeedXP = 0;
					player.GetModPlayer<StatsPlayer>().SpeedSkill += 1;
					Main.NewText("Speed Lvl is now " + player.GetModPlayer<StatsPlayer>().SpeedSkill, Color.Green, false);
				}
			}
		}
	}
}