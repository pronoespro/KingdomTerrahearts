using KingdomTerrahearts.Extra;
using KingdomTerrahearts.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;

namespace KingdomTerrahearts.NPCs.Bosses.Org13
{
    public abstract class Base_OrgMember:ModNPC
    {

        public int curAttack = 0;
        public EntitySource_Parent s;
        public int[] attacksDamage = new int[] { 0, 50, 20, 20, 20 };
        public int attackSpeed = 1;
        public float attackSpeedMult = 1;
        public int attackCooldown = 15;
        public int nextAttack = 1;

        public bool defeated=false;
        public int defeatTime = 75;

        public int hitRecoil=0;
        public int hitCombo;

        public int weaponType;
        public int[] weaponProj;

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (projectile.type == ModContent.ProjectileType<Projectiles.Weapons.KeybladeHoldDisplay>())
            {
                NPCHit();
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            NPCHit();
        }

        public void NPCHit()
        {
            if (hitCombo < 7)
            {
                if (curAttack != 0)
                {
                    curAttack = 0;
                    CheckCurAttack();
                    attackCooldown = 15;

                    NPC.ai[1] = 0;
                }
                hitRecoil = 30;
                hitCombo++;
            }
            else
            {
                if (!SpecialNoHit())
                {
                    curAttack = 0;
                    attackCooldown = 0;

                    NPC.ai[1] = 10000000;
                    CheckCurAttack();
                    if (hitRecoil > 0)
                    {
                        RevengeValueMet();
                    }
                }
            }
            OnHitEffect();
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType =0;
        }

        public void AttackTimerCheck(int maxAI)
        {
            NPC.ai[1] += attackSpeed * attackSpeedMult;
            if (NPC.ai[1] >= maxAI)
            {
                curAttack = 0;
                CheckCurAttack();
                attackCooldown = 15;
            }
        }

        public abstract void CheckCurAttack();

        public abstract void OnHitEffect();

        public abstract Vector2 HoldWeaponPoint();

        public abstract void RevengeValueMet();

        public abstract bool SpecialNoHit();

        public override bool CheckDead()
        {
            if (!defeated)
            {
                //if (!Main.hardMode)
                {
                    if (NPC.timeLeft > 20)
                    {
                        DefeatQuote();
                    }
                }
                /*else
                {
                    NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<xion_secondPhase>(), Target: NPC.target);
                    NPC.timeLeft = (NPC.timeLeft > 5) ? 1 : NPC.timeLeft - 1;
                    defeatTime = 0;
                }*/
            }
            defeated = true;
            NPC.life = 1;
            return false;
        }

        public abstract void DefeatQuote();

        public abstract void DespawnQuote();

        public bool DespawnHandler(Player player)
        {
            if (!player.active || player.dead || player.statLife == 0)
            {
                NPC.TargetClosest(false);
                Player p = Main.player[NPC.target];
                if (!p.active || p.dead || p.statLife == 0)
                {
                    NPC.velocity = new Vector2(0, 100000);
                    NPC.Center += new Vector2(0, 100000);
                    DespawnQuote();
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 1;

                        DespawnQuote();

                        return true;
                    }
                }
            }
            return false;
        }


    }
}
