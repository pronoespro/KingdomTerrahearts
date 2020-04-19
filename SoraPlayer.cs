using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace KingdomTerrahearts
{

    class SoraPlayer:ModPlayer
    {

        public bool canGlide = false;
        public float glideTime = 0;
        public float glideFallSpeed=10;
        
        public bool canDash = false;
        public float dashSpeed=0;
        public int dashReloadSpeed = 100;
        public bool canDashMidAir = false;

        public bool canDoubleJump = false;
        public float doubleJumpHeight = 0;
        public int doubleJumpQuantity = 0;

        public bool hasSecondChance = false;
        public int secondChanceInvulnerability = 0;
        public int secondChanceReload = 1000;
        public bool hasAutoHP = false;
        public int autoHPReload = 1000;
        public int autoHPRecover = 0;

        public int tpFallImmunity = 0;

        public bool orgCoatAccesory=false;
        public bool orgCoatHideVanity = false;
        public bool orgCoatForceBanity = false;

        public bool canCastHeal = false;
        public int castHealInvulnerabilityTime = 0;
        public int castHealAmount = 0;
        public int castHealCost = 1000;

        float curGlideTime = 0;
        bool tapped = false;
        float tapTime = 0;
        float reTapTime = 0;
        int lastPress = 0;
        bool jumped = false;
        int jumpCount = 0;
        int curDashReload = 100;
        int SCcurReload = 0;
        int AHPcurReload = 0;
        int curInvulnerabilityFrames = 0;

        public void AddInvulnerability(int time)
        {
            curInvulnerabilityFrames += (curInvulnerabilityFrames > time) ? 0 : time;
        }

        public void ResetTimers()
        {
            SCcurReload = 0;
            AHPcurReload = 0;
            curInvulnerabilityFrames = 0;
        }

        public override void ResetEffects()
        {
            canGlide = false;
            glideTime = 0;
            glideFallSpeed = 10;

            canDash = false;
            dashSpeed = 0;
            canDashMidAir = false;

            canDoubleJump = false;
            doubleJumpHeight = 0;
            doubleJumpQuantity = 0;
            
            hasSecondChance = false;
            secondChanceReload = 1000;
            hasAutoHP = false;
            autoHPReload = 1000;
            autoHPRecover = 0;

            canCastHeal = false;
            castHealInvulnerabilityTime = 0;
            castHealAmount = 0;
            castHealCost = 1000;

            orgCoatAccesory = false;
            orgCoatHideVanity = false;
            orgCoatForceBanity = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {

            if (canCastHeal && castHealAmount>0)
            {

                if(triggersSet.QuickHeal && player.HasBuff(BuffID.PotionSickness) && player.statMana-player.statManaMax>castHealCost)
                {

                    player.statMana = 0;
                    player.statLife = castHealAmount / 100 * player.statLifeMax;
                    curInvulnerabilityFrames = (curInvulnerabilityFrames <castHealInvulnerabilityTime) ? castHealInvulnerabilityTime : curInvulnerabilityFrames;

                }

            }

            if (canGlide)
            {
                if (triggersSet.Jump && curGlideTime > 0 && player.velocity.Y > glideFallSpeed)
                {
                    curGlideTime--;
                    player.velocity.Y = (player.velocity.Y > glideFallSpeed) ? glideFallSpeed : player.velocity.Y;
                    player.slowFall = true;
                    player.noFallDmg = true;
                }
            }

            if (canDash && player.dash<=0 && curDashReload<=0)
            {
                if (triggersSet.Left || triggersSet.Right)
                {
                    int curPress= (triggersSet.Left) ? -1 : 1;
                    tapTime++;
                    if (tapped)
                    {
                        if (lastPress == curPress && (canDashMidAir ||Math.Abs(player.velocity.Y)<0.15f))
                        {
                            if (Math.Abs(player.velocity.X) < dashSpeed + player.maxRunSpeed)
                            {
                                player.velocity.X += (triggersSet.Left) ? -dashSpeed : dashSpeed;
                                curDashReload = dashReloadSpeed;
                            }
                        }
                        tapped = false;
                    }
                    if (tapTime < 15)
                    {
                        lastPress = curPress;
                    }
                    else
                    {
                        lastPress = 0;
                    }
                }
                else
                {
                    if (lastPress!=0)
                    {
                        tapTime = 0;
                        reTapTime++;
                        tapped = (reTapTime < 15);
                    }
                    else
                    {
                        lastPress = 0;
                        tapTime = 0;
                        reTapTime = 0;
                    }
                }
            }

            if(canDoubleJump && doubleJumpHeight > 0)
            {

                if (player.velocity.Y == 0)
                {
                    jumped = false;
                    jumpCount = 0;
                }
                else
                {

                    if (triggersSet.Jump && player.wingTime<=0)
                    {
                        int initJump = 0;
                        int extradoubleJumps = 0;
                        if (player.doubleJumpBlizzard)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }
                        if (player.doubleJumpCloud)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }
                        if (player.doubleJumpFart)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }
                        if (player.doubleJumpSail)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }
                        if (player.doubleJumpSandstorm)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }
                        if (player.doubleJumpUnicorn)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }

                        if (jumped && jumpCount<doubleJumpQuantity+extradoubleJumps)
                        {
                            player.velocity.Y = (jumpCount >= initJump ) ?-doubleJumpHeight:player.velocity.Y;
                            jumpCount++;
                        }

                        jumped = false;
                    }
                    else
                    {
                        jumped = true;
                    }
                }

            }

        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            items.RemoveAt(0);

            Item item = new Item();
            item.SetDefaults(mod.ItemType("Keyblade"));
            item.stack = 1;
            item.Prefix(-1);
            items.Insert(0,item);
        }

        public override void PreUpdate()
        {

            if (curInvulnerabilityFrames > 0)
            {
                player.immune = true;
                curInvulnerabilityFrames--;
            }

            curDashReload--;
            curDashReload = (curDashReload > dashReloadSpeed) ? dashReloadSpeed : curDashReload;
            if (player.velocity.Y == 0)
            {
                curGlideTime = glideTime;
            }
            player.noFallDmg = tpFallImmunity>0;
            tpFallImmunity-=(tpFallImmunity>0)?1:0;
        }

        public void ChangeGlideFallSpeed(float fallspeed)
        {
            glideFallSpeed = (glideFallSpeed > fallspeed) ? fallspeed : glideFallSpeed;
        }

        public void ChangeDashReload(int reload)
        {
            dashReloadSpeed = (dashReloadSpeed < reload) ? reload : dashReloadSpeed;
        }

        public override void UpdateVanityAccessories()
        {
            for (int n = 13; n < 18 + player.extraAccessorySlots; n++)
            {
                Item item = player.armor[n];
                if (item.type == mod.ItemType("orgCoat"))
                {
                    orgCoatHideVanity = false;
                    orgCoatForceBanity = true;
                }
            }
        }

        public override void FrameEffects()
        {
            if ((orgCoatForceBanity) && !orgCoatHideVanity)
            {
                player.legs = mod.GetEquipSlot("orgCoatLegs", EquipType.Legs);
                player.body = mod.GetEquipSlot("orgCoatBody", EquipType.Body);
                player.head = mod.GetEquipSlot("orgCoatHead", EquipType.Head);
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {

            if (SCcurReload == 0 && hasSecondChance && secondChanceReload!=1000 && player.statLife!=1)
            {

                if(AHPcurReload==0 && hasAutoHP && autoHPRecover!=0)
                {
                    player.statLife += autoHPRecover;
                    player.HealEffect(autoHPRecover);

                    AHPcurReload = player.statLifeMax / autoHPReload;
                }
                else
                {
                    player.statLife += 1;
                }

                SCcurReload = secondChanceReload;
                curInvulnerabilityFrames = secondChanceInvulnerability;

                return false;

            }

            return base.PreKill(damage, hitDirection, pvp,ref playSound,ref genGore,ref damageSource);

        }

    }
}
