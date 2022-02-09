using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace Galactica {
    class AccPlayer : ModPlayer {
        public bool vortexScope = false;
        public bool solarFlareGlove = false;
        public bool nebulaFlower = false;
        public bool lunarGlove = false;
        public bool exoskeleton = false;
        public bool vengeance = false;
        public bool spectreLeggings = false;
        public bool intimidation = false;
        public bool threatening = false;
        public bool omniscient = false;
        public bool trueSoC = false;
        public bool gaussPack = false;
        public bool lunarBoots = false;
        public int reflect = 0;

        public int hitTimer = 0; // increases each frame and resets to 0 when the player gets hit

        public int allCirt;

        public override bool ConsumeAmmo(Item weapon, Item ammo) {
            if (vortexScope && Main.rand.Next(5) == 0) return false; // 20% chance to not consume ammo
            return true;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) {
            if (solarFlareGlove && item.melee) target.AddBuff(BuffID.Daybreak, 600); // apply daybreak on direct melee hit
            if (trueSoC && crit) target.AddBuff(BuffID.BetsysCurse, 600);
            else if (omniscient && crit) target.AddBuff(BuffID.Ichor, 600);

            if (!player.HasBuff(BuffID.MoonLeech) && ((spectreLeggings && item.magic) || exoskeleton) && !target.friendly && target.lifeMax > 10) {
                var healAmount = (int)(damage / 200f);
                if (healAmount >= 1) {
                    healAmount = Math.Min(healAmount, 2);
                    player.statLife += healAmount;
                    player.HealEffect(healAmount);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) {
            if (solarFlareGlove && proj.melee) target.AddBuff(BuffID.Daybreak, 600); // apply daybreak on melee projectile hit
            if (trueSoC && crit) target.AddBuff(BuffID.BetsysCurse, 600);
            else if (omniscient && crit) target.AddBuff(BuffID.Ichor, 600);

            if (!player.HasBuff(BuffID.MoonLeech) && ((spectreLeggings && proj.magic) || exoskeleton) && !target.friendly && target.lifeMax > 10) {
                var healAmount = (int)(damage / 200f);
                if (healAmount >= 1) {
                    healAmount = Math.Min(healAmount, 2);
                    player.statLife += healAmount;
                    player.HealEffect(healAmount);
                }
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit) {
            if (solarFlareGlove && !npc.dontTakeDamage) {
                int reflectDamage;
                if (damage < player.statLifeMax2 / 20f && npc.lifeMax <= player.statLifeMax2 * (Main.expertMode ? 1 : 0.5f)) { // very weak enemy. deal massive damage
                    reflectDamage = 999;
                } else {
                    reflectDamage = npc.damage * 5 + player.statDefense * 2; // 500% + 2 * defense
                }
                int direction = npc.position.X + npc.width / 2f > player.position.X + player.width / 2f ? 1 : -1;
                player.ApplyDamageToNPC(npc, reflectDamage, 15f, direction, false);
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
            if (lunarGlove) {
                int chance = Math.Min(hitTimer / 15, 20);
                if (Main.rand.Next(100) <= chance) {
                    player.NinjaDodge();
                    damage = 0;
                    return false;
                }
            }
            if (intimidation) {
                if (damage < player.statLifeMax2 * 2 / 25) {
                    damage = 0;
                }
            } else if (threatening) {
                if (damage < player.statLifeMax2 / 25) {
                    damage = 0;
                }
            }
            return true;
        }


        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            if (vortexScope && proj.ranged && crit) {
                float critdmg = 1f + Math.Min(hitTimer / 3000f, 0.1f);
                damage = (int)Math.Ceiling(damage * critdmg);
            }
            Mod acc = ModLoader.GetMod("UpgradedAccessories");
            if(acc != null){
                if (target.HasBuff(acc.GetBuff("Intimidated").Type)) {
                    damage = damage * 3 / 2;
                } else if (target.HasBuff(acc.GetBuff("Threatened").Type)) {
                    damage = damage * 11 / 10;
                }
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit) {
            if (vortexScope && item.ranged && crit) {
                float critdmg = 1f + Math.Min(hitTimer / 3000f, 0.1f);
                damage = (int)Math.Ceiling(damage * critdmg);
            }
            Mod acc = ModLoader.GetMod("UpgradedAccessories");
            if(acc != null){
                if (target.HasBuff(acc.GetBuff("Intimidated").Type)) {
                    damage = damage * 3 / 2;
                } else if (target.HasBuff(acc.GetBuff("Threatened").Type)) {
                    damage = damage * 11 / 10;
                }
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit) {
            if (solarFlareGlove) damage = damage * 4 / 5; // reduces damage from projectiles by 20%
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit) {
            Mod acc = ModLoader.GetMod("UpgradedAccessories");
            if(acc != null){
                if (npc.HasBuff(acc.GetBuff("Intimidated").Type)) {
                    damage /= 2;
                } else if (npc.HasBuff(acc.GetBuff("Threatened").Type)) {
                    damage = damage * 9 / 10;
                }
            }
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit) {
            hitTimer = 0;
            if (solarFlareGlove && player.whoAmI == Main.myPlayer) {
                for (int i = 0; i < 200; i++) {
                    if (Main.npc[i].active && !Main.npc[i].friendly) {
                        float distance = (Main.npc[i].Center - player.Center).Length();
                        if (distance < 1500) {
                            Main.npc[i].AddBuff(BuffID.Confused, 600);
                            Main.npc[i].AddBuff(BuffID.CursedInferno, 600);
                            Main.npc[i].AddBuff(BuffID.Ichor, 600);
                        }
                    }
                }
                Projectile.NewProjectile(player.Center.X + (float)Main.rand.Next(-40, 40), player.Center.Y - (float)Main.rand.Next(20, 60), player.velocity.X * 0.3f, player.velocity.Y * 0.3f, ProjectileID.BrainOfConfusion, 0, 0f, player.whoAmI, 0f, 0f);
            }


            if (vengeance) {
                int bees = 1;
                if (Main.rand.Next(3) == 0) bees++;
                if (Main.rand.Next(3) == 0) bees++;
                if (Main.rand.Next(3) == 0) bees++;

                for (int i = 0; i < bees; i++) {
                    float speedX = Main.rand.Next(-35, 36) * 0.02f;
                    float speedY = Main.rand.Next(-35, 36) * 0.02f;
                    Projectile.NewProjectile(player.position.X, player.position.Y, speedX, speedY, ProjectileID.GiantBee, 120/*damage*/, 0.5f/*knockback*/, Main.myPlayer, 0f, 0f);
                }
            }
        }

        float lifeCounter = 0;
        float manaCounter = 0;
        public override void PreUpdate() {
            hitTimer++;

            if (nebulaFlower) {
                float lifeBonus = Math.Min(hitTimer / 30f, 10f); // max 10 life and 20 mana per second bonus that grows 2 and 4 per second^2 and maxes out in 5 second
                float manaBonus = lifeBonus * 2;
                lifeCounter += lifeBonus;
                manaCounter += manaBonus;
            }

            while (lifeCounter > 60) {
                lifeCounter -= 60;
                if (player.statLife < player.statLifeMax2) player.statLife++;
            }
            while (manaCounter > 60) {
                manaCounter -= 60;
                if (player.statMana < player.statManaMax2) player.statMana++;
            }
        }

        public override void PreUpdateMovement() {
            if (vengeance) {
                player.allDamageMult *= 1f + ((player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2) / 2;
            }
            if(lunarBoots && player.controlDown && player.controlJump && player.wingTime > 0) {
                player.velocity.Y = 1E-05f;
                player.wingTime += 0.5f;
            }

        }


        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff) {
            if (gaussPack) {
                wallSpeedBuff = true;
                tileSpeedBuff = true;
                tileRangeBuff = true;
            }
        }

        public override void ResetEffects() {
            vortexScope = false;
            solarFlareGlove = false;
            nebulaFlower = false;
            lunarGlove = false;
            exoskeleton = false;
            vengeance = false;
            spectreLeggings = false;
            intimidation = false;
            threatening = false;
            omniscient = false;
            trueSoC = false;
            gaussPack = false;
            lunarBoots = false;
            reflect = 0;

            allCirt = 0;
        }

        public override void GetWeaponCrit(Item item, ref int crit) {
            crit += allCirt;
        }
    }
}
