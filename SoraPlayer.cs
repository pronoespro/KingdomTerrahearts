using KingdomTerrahearts.Items.Weapons;
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

    public class SoraPlayer :ModPlayer
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
        public int reviveTime = 0;

        public bool skipToDay = false;
        public bool skipTime = false;

        public bool enlightened = false;
        public bool fightingInBattleground = false;
        public Vector2 initPosTrap, endPosTrap;

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

        public int[] partyUpgrades;

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

            enlightened = false;

        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            
            if (KingdomTerrahearts.PartySelectHotkey.JustPressed)
            {
                KingdomTerrahearts.instance.TogglePartyUI();
            }

            if (player.mount.Type == -1)
            {
                if (canCastHeal && castHealAmount > 0 && player.statLife < (player.statLifeMax / 4 * 3))
                {

                    if (triggersSet.QuickHeal && player.HasBuff(BuffID.PotionSickness) && ((player.statManaMax > 0) ? player.statMana / player.statManaMax * 100 : 1) > castHealCost)
                    {

                        player.statMana = 0;
                        player.HealEffect((int)((float)castHealAmount / 100f * (float)player.statLifeMax));
                        player.statLife += (int)((float)castHealAmount / 100f * (float)player.statLifeMax);
                        curInvulnerabilityFrames = (curInvulnerabilityFrames < castHealInvulnerabilityTime) ? castHealInvulnerabilityTime : curInvulnerabilityFrames;

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

                if (canDash && player.dash <= 0 && curDashReload <= 0)
                {
                    if (triggersSet.Left || triggersSet.Right)
                    {
                        int curPress = (triggersSet.Left) ? -1 : 1;
                        tapTime++;
                        if (tapped)
                        {
                            if (lastPress == curPress && (canDashMidAir || Math.Abs(player.velocity.Y) < 0.15f))
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
                        if (lastPress != 0)
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

                if (canDoubleJump && doubleJumpHeight > 0)
                {

                    if (player.velocity.Y == 0)
                    {
                        jumped = false;
                        jumpCount = 0;
                    }
                    else
                    {

                        if (triggersSet.Jump && player.wingTime <= 0)
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

                            if (jumped && jumpCount < doubleJumpQuantity + extradoubleJumps)
                            {
                                player.velocity.Y = (jumpCount >= initJump) ? -doubleJumpHeight : player.velocity.Y;
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

        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {

            //Second Chance && Auto-HP
            if (SCcurReload == 0 && hasSecondChance && secondChanceReload != 1000 && player.statLife != 1)
            {

                if (AHPcurReload == 0 && hasAutoHP && autoHPRecover != 0)
                {
                    player.statLife += autoHPRecover;
                    player.HealEffect(autoHPRecover);

                    AHPcurReload = player.statLifeMax / ((autoHPReload!=0)? autoHPReload : 1 );
                }
                else
                {
                    player.statLife += 1;
                }

                SCcurReload = secondChanceReload;
                curInvulnerabilityFrames = secondChanceInvulnerability;

                return false;

            }

            //Kupo Coin
            if (player.HasItem(mod.ItemType("KupoCoin")) && reviveTime <= 0)
            {
                player.ConsumeItem(mod.ItemType("KupoCoin"));
                player.statLife = player.statLifeMax2;
                player.HealEffect(player.statLifeMax2);
                player.immune = true;
                player.immuneTime = player.longInvince ? 180 : 120;
                for (int k = 0; k < player.hurtCooldowns.Length; k++)
                {
                    player.hurtCooldowns[k] = player.longInvince ? 180 : 120;
                }
                Main.PlaySound(SoundID.Item29, player.position);
                reviveTime = 60 * 60 * 3;
                return false;
            }



            reviveTime = 0;
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            items.RemoveAt(0);

            Item item = new Item();
            item.SetDefaults(mod.ItemType("Keyblade_wood"));
            item.stack = 1;
            items.Insert(0,item);
        }

        public override void PreUpdate()
        {

            if (skipTime && Main.dayTime!=skipToDay)
            {
                Main.time += 100;
            }
            else if(Main.dayTime == skipToDay)
            {
                skipTime = false;
            }

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

            if (--secondChanceReload <= 0) secondChanceReload = 0;
            if (--autoHPReload <= 0) autoHPReload = 0;
            
            player.noFallDmg = tpFallImmunity>0;
            tpFallImmunity-=(tpFallImmunity>0)?1:0;

            base.PreUpdate();
        }

        public override void PostUpdate()
        {

            base.PostUpdate();

            if (fightingInBattleground)
            {
                //check if really trapped
                bool newTrapped = false;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    newTrapped = (Main.npc[i].boss && Main.npc[i].life>0 && Main.npc[i].active) ? true : newTrapped;
                }
                fightingInBattleground = newTrapped;
                //effects
                player.position.X =  Math.Max(initPosTrap.X, player.position.X);
                player.position.X = Math.Min(endPosTrap.X, player.position.X);
                player.position.Y = Math.Max(initPosTrap.Y, player.position.Y);
                player.position.Y = Math.Min(endPosTrap.Y, player.position.Y);
            }

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
            base.UpdateVanityAccessories();
        }

        public override void FrameEffects()
        {
            if ((orgCoatForceBanity) && !orgCoatHideVanity)
            {
                player.legs = mod.GetEquipSlot("orgCoatLegs", EquipType.Legs);
                player.body = mod.GetEquipSlot("orgCoatBody", EquipType.Body);
                player.head = mod.GetEquipSlot("orgCoatHead", EquipType.Head);
            }
            base.FrameEffects();
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"partyUpgrades", partyUpgrades}
            };
        }

        public override void Load(TagCompound tag)
        {
            partyUpgrades = tag.GetIntArray("partyUpgrades");
        }

        public void GetPartyMember(int npcNum)
        {
            if (npcNum<Main.maxNPCs && Main.npc[npcNum].active && Main.npc[npcNum].townNPC )
            {
                NPC npc = Main.npc[npcNum];
                if (Main.npc[npcNum].HasBuff(mod.BuffType("PMemberBuff")))
                {
                    npc.DelBuff(npc.FindBuffIndex(mod.BuffType("PMemberBuff")));
                    npc.color = Color.White;
                    npc.Teleport(new Vector2(npc.homeTileX * 16, npc.homeTileY * 16 - npc.height)-npc.velocity);
                    npc.velocity = new Vector2();
                }
                else
                {
                    npc.AddBuff(mod.BuffType("PMemberBuff"), 1000000000);
                }
            }
        }

        public bool NPCisPartyMember(int npcType)
        {
            for (int i = 0; i < Main.maxNPCTypes; i++)
            {
                if (i < Main.maxNPCs && Main.npc[i].active && Main.npc[i].townNPC && Main.npc[i].type == npcType)
                {
                    return Main.npc[i].HasBuff(mod.BuffType("PMemberBuff"));
                }
            }
            return false;
        }

        public NPC GetModdedNPC(string name)
        {
            for(int i = 0; i < Main.maxNPCTypes; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == mod.NPCType(name)) { 
                    return Main.npc[i]; 
                }
            }
            return null;
        }

    }
}
