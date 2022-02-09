using Galactica.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Galactica.Projectiles;
using Terraria.ID;
using Galactica.NPCs.CosmicGuardians;
using System.Collections.Generic;

namespace Galactica
{
	public class ModGlobalProjectile : GlobalProjectile
	{
		public static bool first;
		public static bool first2;
		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => true;
		public override void AI(Projectile proj){
			if(!first && EXPlayer.Velocity){
				first = true;
			}
			if(!first2 && EXPlayer.TDoll && Main.rand.NextBool()){
				proj.hostile = true;
				first2 = true;
			}
			if(proj.type == ProjectileID.PhantasmalDeathray){
				proj.damage = 1000;
			}
		}
		public override Color? GetAlpha(Projectile proj, Color newColor)
		{
			if(proj.type == ProjectileID.PainterPaintball && proj.ai[0] == 6699f){

				return Main.DiscoColor;
			}
			if (proj.type == ProjectileID.SolarWhipSwordExplosion && proj.localAI[0] == 3.14f)
			{
				return Main.DiscoColor;
			}
			if(proj.type == ProjectileID.RainbowCrystalExplosion && proj.ai[0] == 1.11f)
            {
				return Main.DiscoColor;
            }
			return base.GetAlpha(proj, newColor);
		}
		public override void OnHitNPC(Projectile projectile, NPC npc, int damage, float knockback, bool crit)
		{
			Player player = new Player();
			player = Main.player[projectile.owner];
			player.GetModPlayer<StatsPlayer>().DMGXP += crit ? 2 : 1;
			if(player.GetModPlayer<StatsPlayer>().DMGXP > (float)((player.GetModPlayer<StatsPlayer>().DMGSkill * 50) + 50)){
				player.GetModPlayer<StatsPlayer>().DMGXP = 0;
				player.GetModPlayer<StatsPlayer>().DMGSkill += 1;
				Main.NewText("Damage Lvl is now " + player.GetModPlayer<StatsPlayer>().DMGSkill, Color.Green, false);
			}
			if(EXPlayer.PQuiv && projectile.ranged){
				player.statLife += (crit ? 2 : 1);
				if (Main.myPlayer == player.whoAmI)
				{
					player.HealEffect(5, true);
				}
			}
			if(EXPlayer.CMedal && ModGlobalItem.IsTHWeapon(player.HeldItem.type)){
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
			if(EXPlayer.HRing && ModGlobalItem.IsTHWeapon(player.HeldItem.type)){
				player.statLife += crit ? 2 : 1;
				player.HealEffect(crit ? 2 : 1, true);
			}
			if(projectile.type == ModContent.ProjectileType<lreaper>()){
				Main.player[projectile.owner].statLife += Main.rand.Next(1, 3);
				Main.player[projectile.owner].HealEffect(Main.rand.Next(1, 3), true);
			}
			if(projectile.type == 132){
				npc.AddBuff(ModContent.BuffType<terrainferno>(), 600, false);
			}
		}
	}
}