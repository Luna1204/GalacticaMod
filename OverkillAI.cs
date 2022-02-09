using Galactica.Items;
using Galactica.Projectiles.Boss;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Galactica
{
    class OverkillAI
    {
        public void DoOverkillAI(NPC npc)
        {
            if (npc.type == NPCID.KingSlime) KSAI(npc);
            if (npc.type == NPCID.EyeofCthulhu) EYEAI(npc);
        }
        public bool ksDash;
        public int attackTimer;
        public int attackTimer2;
        public bool summonLens;
        public int dashTimer;
        public void KSAI(NPC npc)
        {
            if (npc.life < 50)
            {
                npc.defense = 9999;
            }
            Player target = Main.player[npc.target];
            if (npc.Distance(target.position) > 800 && !ksDash)
            {
                npc.velocity = npc.DirectionTo(target.position) * 30;
                for (int i = 0; i < 20; i++)
                {
                    int projectile3 = Projectile.NewProjectile(npc.Center, Vector2.One.RotatedBy((MathHelper.TwoPi / 20) * i) * 5, ProjectileID.RubyBolt, 22, 0, target.whoAmI, 0f, 0f);
                    Main.projectile[projectile3].friendly = false;
                    Main.projectile[projectile3].hostile = true;
                }
                npc.noTileCollide = true;
                Main.PlaySound(SoundID.Roar, npc.position);
                ksDash = true;
            }
            else
            {
                npc.noTileCollide = false;
                ksDash = false;
            }
            if (attackTimer > 0)
            {
                attackTimer--;
            }
            if (attackTimer <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int projectile3 = Projectile.NewProjectile(npc.Center, Vector2.One.RotatedBy((MathHelper.TwoPi / 10) * i) * 5, ProjectileID.RubyBolt, 22, 0, target.whoAmI, 0f, 0f);
                    Main.projectile[projectile3].friendly = false;
                    Main.projectile[projectile3].hostile = true;
                }
                attackTimer = 200;
                npc.netUpdate = true;
            }
            if (attackTimer2 > 0)
            {
                attackTimer2--;
            }
            if (attackTimer2 <= 0)
            {
                float extraRot = Main.rand.NextFloat(-MathHelper.TwoPi, 0);
                for (int i = 0; i < 4; i++)
                {
                    float rot = ((MathHelper.TwoPi / 4) * i) + extraRot;
                    int projectile3 = Projectile.NewProjectile(target.Center + Vector2.One.RotatedBy(rot) * 160, Vector2.One.RotatedBy(rot) * -2, ModContent.ProjectileType<CrystalizedSlime>(), 22, 0, target.whoAmI, 0f, 0f);
                    Main.projectile[projectile3].friendly = false;
                    Main.projectile[projectile3].hostile = true;
                }
                attackTimer2 = 150;
                npc.netUpdate = true;
            }
        }
        public bool circle;
        public void EYEAI(NPC npc)
        {
            npc.rotation = npc.velocity.ToRotation() - MathHelper.PiOver2;
            Player target = Main.player[npc.target];
            dashTimer--;
            if (dashTimer <= 0)
            {
                Main.PlaySound(SoundID.Roar, npc.position);
                npc.velocity = npc.DirectionTo(target.position) * 20;
                dashTimer = 350;
            }
            float num = Math.Abs(npc.velocity.Length());
            if (num > 10)
            {
                if (!circle)
                {
                    for(int i = 0; i < 10; i++)
                    {
                        Projectile.NewProjectile(npc.position, Vector2.One.RotatedBy((MathHelper.TwoPi / 10) * i), ProjectileID.EyeLaser, 50, 0);
                    }
                    circle = true;
                }
            }
            else
            {
                circle = false;
            }
        }
    }
}
