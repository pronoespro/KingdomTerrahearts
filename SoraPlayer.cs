using KingdomTerrahearts.Interface;
using KingdomTerrahearts.Items;
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

        //glide related
        public bool canGlide = false;
        public float glideTime = 0;
        public float glideFallSpeed=10;
        //dash related
        public bool canDash = false;
        public float dashSpeed=0;
        public int dashReloadSpeed = 100;
        public bool canDashMidAir = false;
        public int dashInvulnerability = 0;
        //double jump related
        public bool canDoubleJump = false;
        public float doubleJumpHeight = 0;
        public int doubleJumpQuantity = 0;
        //second chance related
        public bool hasSecondChance = false;
        public int secondChanceInvulnerability = 0;
        public int secondChanceReload = 1000;
        public bool hasAutoHP = false;
        public int autoHPReload = 1000;
        public int autoHPRecover = 0;
        //tp related?
        public int tpFallImmunity = 0;
        //heal related
        public bool canCastHeal = false;
        public int castHealInvulnerabilityTime = 0;
        public int castHealAmount = 0;
        public int castHealCost = 1000;
        public int reviveTime = 0;
        //vanity related
        public bool orgCoatAccesory=false;
        public bool orgCoatHideVanity = false;
        public bool orgCoatForceBanity = false;
        //time skip related
        public bool skipToDay = false;
        public bool skipTime = false;
        //Buff/Debuff related
        public bool enlightened = false;
        public bool fightingInBattleground = false;
        public Vector2 initPosTrap, endPosTrap;

        //current abilities related
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

        //keyblade related
        public int guardTime = -45;
        int guardProj;
        public blockingType guardType=blockingType.none;
        public bool blockedAttack=false;
        public int levelUpShowingTime = 0;
        public static Projectile[] summonProjectiles = new Projectile[0];

        //form related
        public Color formColor;
        public bool usingForm;

        public Item lastHeldItem;

        public int lastHeldKeyblade = 0;

        //Team related
        public int[] partyUpgrades;

        //Biome related
        public bool inTwilightTown = false;

        //Personal and not KH related
        public bool invincible;
        public bool hasZafi;

        public bool playerCreated;

        public void AddInvulnerability(int time)
        {
            curInvulnerabilityFrames += (curInvulnerabilityFrames > time) ? 0 : time;
        }

        public bool IsInvulnerable()
        {
            return curInvulnerabilityFrames > 0;
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
            dashInvulnerability = 0;

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

            player.noFallDmg = false;

            invincible = false;
            hasZafi = false;

    }

        public override void PlayerDisconnect(Player player)
        {
            inTwilightTown = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {

            if (KingdomTerrahearts.PartySelectHotkey.JustPressed)
            {
                KingdomTerrahearts.instance.TogglePartyUI();
            }

            if (player.mount.Type == -1)
            {
                if(KingdomTerrahearts.GuardHotKey.JustPressed && guardTime < -30 && lastHeldKeyblade>0 && !invincible && curInvulnerabilityFrames<=0)
                {
                    PlayGuardSound();
                    guardTime = 30;
                }
                if (canCastHeal && castHealAmount > 0 && player.statLife < (player.statLifeMax / 4 * 3))
                {

                    if (triggersSet.QuickHeal && player.HasBuff(BuffID.PotionSickness) && (((player.statManaMax > 0) ? player.statMana / player.statManaMax * 100 : 1) > castHealCost/player.statManaMax || (player.statMana>=player.statManaMax/2 && castHealCost>player.statManaMax/2)))
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

                curDashReload--;
                curDashReload = (curDashReload > dashReloadSpeed) ? dashReloadSpeed : curDashReload;

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
                                    AddInvulnerability(dashInvulnerability);
                                }
                            }
                            tapped = false;
                        }
                        if (tapTime < 30)
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

        public void PlayGuardSound()
        {
            if (guardType == blockingType.reflect)
                Main.PlaySound(SoundLoader.customSoundType, player.Center, mod.GetSoundSlot(SoundType.Item, "Sounds/keybladeBlocking"));
            else if (guardType == blockingType.normal || guardType == blockingType.reversal)
                Main.PlaySound(SoundID.Item1.SoundId, x: (int)player.Center.X, y: (int)player.Center.Y, volumeScale: 3);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {

            if (invincible)
                return false;

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
            if (levelUpShowingTime > 0)
                KingdomTerrahearts.instance.ShowLevelUpUI();
            else
                KingdomTerrahearts.instance.HideLevelUpUI();

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

            if (player.velocity.Y == 0)
            {
                curGlideTime = glideTime;
            }

            if (--secondChanceReload <= 0) secondChanceReload = 0;
            if (--autoHPReload <= 0) autoHPReload = 0;

            player.noFallDmg = (tpFallImmunity > 0) ? true : player.noFallDmg;

            if (invincible)
            {
                player.ghost =false;
                player.statMana = player.statLifeMax;
                player.statLife = player.statLifeMax;
                player.maxMinions = 1000;
                player.slotsMinions = 1000;
                player.merman = true;
                player.nightVision = true;
                player.noKnockback = true;
                player.blockRange = 1000;
                player.wallSpeed = 1000;
                player.tileSpeed = 1000;
                player.cLight = 1;
                player.blockRange = 1000;
                player.autoJump = true;
                player.extraAccessorySlots = 4;
                player.wingTimeMax = 1000000;
                player.wingTime = 1000000;
                player.autoJump = true;

                /*For Rolling
                
                Vector2 rotOrigin = player.Center - player.position;
                rotOrigin.Y *= 1.25f;
                player.fullRotationOrigin = rotOrigin;
                player.fullRotation = (float)Math.PI;

                */
            }

            if (fightingInBattleground)
            {
                //check if really trapped
                bool newTrapped = false;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    newTrapped = (isBoss(i) && Main.npc[i].life > 0 && Main.npc[i].active) ? true : newTrapped;
                }
                fightingInBattleground = newTrapped;

                //effects
                Vector2 nextPos = player.Center + player.velocity;
                if (nextPos.X> endPosTrap.X || nextPos.X < initPosTrap.X)
                    player.velocity.X = 0;
                if (nextPos.Y > endPosTrap.Y || nextPos.Y < initPosTrap.Y)
                    player.velocity.Y = 0;

                Vector2 clampedPos = player.Center;
                clampedPos.X = Math.Min(endPosTrap.X, Math.Max(initPosTrap.X, clampedPos.X));
                clampedPos.Y = Math.Min(endPosTrap.Y, Math.Max(initPosTrap.Y, clampedPos.Y));
                player.Center = clampedPos;
            }
            else
            {
                initPosTrap = Vector2.Zero;
                endPosTrap = Vector2.Zero;
            }

            tpFallImmunity -= (tpFallImmunity>0)?1:0;

            base.PreUpdate();


            guardTime = (guardTime > -100) ? guardTime - 1 : -100;

            if (guardTime <= 0)
            {
                guardProj = -1;
                guardType = blockingType.none;
                blockedAttack = false;
            }

            if (guardType != blockingType.none)
            {
                player.velocity = Vector2.Zero;
                int projType = mod.ProjectileType("Vergil_boubble");

                float projScale = 1;
                switch (guardType)
                {
                    case blockingType.normal:
                        projType = ProjectileID.Typhoon;
                        break;
                    case blockingType.reflect:
                        projType = mod.ProjectileType("guardProjectile");
                        break;
                    case blockingType.reversal:
                        projScale = 5;
                        break;
                }

                if (guardProj == -1 || !Main.projectile[guardProj].active)
                {
                    guardProj = Projectile.NewProjectile(player.Center, player.velocity, projType, 0, 0,Owner:player.whoAmI);
                }
                Main.projectile[guardProj].scale = projScale;
                Main.projectile[guardProj].timeLeft = guardTime;
            }

        }

        public void SetTrapLimits(Vector2 init,Vector2 end)
        {
            if(initPosTrap==Vector2.Zero && endPosTrap == Vector2.Zero)
            {
                initPosTrap = init;
                endPosTrap = end;
            }
            else
            {
                initPosTrap = new Vector2((init.X < initPosTrap.X) ? init.X : initPosTrap.X,
                    (init.Y < initPosTrap.Y) ? init.Y : initPosTrap.Y);
                endPosTrap = new Vector2((end.X>endPosTrap.X)?end.X:endPosTrap.X,
                    (end.Y>endPosTrap.Y)?end.Y:endPosTrap.Y
                    );
            }
        }

        public override void PostUpdate()
        {

            NPC.MoonLordCountdown = (NPC.MoonLordCountdown > 250) ? 250 : NPC.MoonLordCountdown;

            if (invincible)
                player.ghost = false;

            CommandLogic.instance.Update();
            if (Main.playerInventory)
            {
                KingdomTerrahearts.instance.HideCommandUI();
            }
            else
            {
                if (lastHeldKeyblade > 0)
                    KingdomTerrahearts.instance.ShowCommandUI();
                else
                    KingdomTerrahearts.instance.HideCommandUI();
            }
            if(lastHeldKeyblade<0)
                guardType = blockingType.none;
            if (guardTime == 30)
                PlayGuardSound();

            lastHeldKeyblade--;

            base.PostUpdate();


            lastHeldItem = player.HeldItem;
        }

        public void ChangeGlideFallSpeed(float fallspeed)
        {
            glideFallSpeed = (glideFallSpeed > fallspeed) ? fallspeed : glideFallSpeed;
        }

        public void ChangeDashReload(int reload)
        {
            dashReloadSpeed = (dashReloadSpeed > reload) ? reload : dashReloadSpeed;
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
            if ((orgCoatForceBanity || orgCoatAccesory) && !orgCoatHideVanity)
            {
                player.legs = mod.GetEquipSlot("orgCoatLegs", EquipType.Legs);
                player.body = mod.GetEquipSlot("orgCoatBody", EquipType.Body);
                player.head = mod.GetEquipSlot("orgCoatHead", EquipType.Head);
            }
            base.FrameEffects();
        }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (usingForm)
            {
                r = formColor.R;
                g = formColor.G;
                b = formColor.B;
                a = formColor.A;
            }
            else
            {
                r = 1;
                g = 1;
                b = 1;
                a = 1;
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"partyUpgrades", partyUpgrades},
                {"playerCreated" ,playerCreated}
            };
        }

        public override void Load(TagCompound tag)
        {
            partyUpgrades = tag.GetIntArray("partyUpgrades");
            playerCreated = tag.GetBool("playerCreated");
            KingdomTerrahearts.instance.ShowDialogUI();
        }

        public void GetPartyMember(int npcNum)
        {
            if (npcNum<Main.maxNPCs && Main.npc[npcNum].active && Main.npc[npcNum].townNPC )
            {
                NPC npc = Main.npc[npcNum];
                if (Main.npc[npcNum].HasBuff(mod.BuffType("PMemberBuff")))
                {
                    Conversation conv = new Conversation(GetMemberDialog(npc.type), Color.White, 20000, npc.GivenOrTypeName);
                    DialogSystem.AddConversation(conv);

                    npc.DelBuff(npc.FindBuffIndex(mod.BuffType("PMemberBuff")));
                    npc.color = Color.White;
                    npc.Teleport(new Vector2(npc.homeTileX * 16, npc.homeTileY * 16 - npc.height)-npc.velocity);
                    npc.velocity = new Vector2();
                }
                else
                {
                    Conversation conv = new Conversation(GetMemberDialog(npc.type,true), Color.White, 20000, npc.GivenOrTypeName);
                    DialogSystem.AddConversation(conv);
                    npc.AddBuff(mod.BuffType("PMemberBuff"), 1000000000);
                }
            }
        }

        public string GetMemberDialog(int type,bool joinedParty=false)
        {
            if (joinedParty)
            {
                switch (type)
                {
                    case NPCID.Guide:
                        return "Do you need my help? Alright";
                    case NPCID.Clothier:
                        return (Main.rand.Next(150)<3)? "Heeeello Terraria Enthusiasts":"I'll help you just as you helped me";
                    case NPCID.Dryad:
                        return "Hey, you are supposed to be the hero, remember?";
                    case NPCID.ArmsDealer:
                        return "Oh yeah, time to join the action";
                    case NPCID.Mechanic:
                        return "I am not comfortable with fighting, but if you need me...";
                    case NPCID.Nurse:
                        return "I'll help you out. Don't get too cooky though";
                    case NPCID.PartyGirl:
                        return "Let's party!";
                    default:
                        return "Hello, thanks for letting me join you";
                }
            }
            else
            {
                switch (type)
                {
                    case NPCID.Guide:
                        return "I'll be there to help you whenever you need me";
                    case NPCID.Clothier:
                        return "Come to my shop if you need a new set of clothes after all this fighting you do!";
                    case NPCID.Dryad:
                        return "I like helping, but try to do things yourself, jeez";
                    case NPCID.ArmsDealer:
                        return "I hope the ladies were looking";
                    case NPCID.Mechanic:
                        return (NPC.downedMechBossAny) ? "That wasn't as bad as I thought" :"I hope I can fix the mistakes I made some day";
                    case NPCID.PartyGirl:
                        return "Till next time!! Don't party without me, you hear me?";
                    case NPCID.Nurse:
                        return "Don't get too hurt, I want to see you in one pice when you get back";
                    default:
                        return "Godbye!";
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

        public override bool CanSellItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (item.type == mod.ItemType("KuppoCoin"))
                return false;
            return base.CanSellItem(vendor, shopInventory, item);
        }

        public bool isBlocking()
        {
            return guardTime > 0;
        }

        public void StopBlocking()
        {
            guardTime = -15;
            blockedAttack = false;
            AddInvulnerability(15);
        }

        public override bool ConsumeAmmo(Item weapon, Item ammo)
        {
            return (invincible)?false:base.ConsumeAmmo(weapon, ammo);
        }

        public override void GetFishingLevel(Item fishingRod, Item bait, ref int fishingLevel)
        {
            fishingLevel += (player.HasBuff(mod.BuffType("EnlightenedBuff"))) ? 50 : 0;
            fishingLevel = (invincible) ? 1000 : fishingLevel;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (invincible)
                player.dead = false;
        }

        public override float MeleeSpeedMultiplier(Item item)
        {
            return (player.HasBuff(mod.BuffType("EnlightenedBuff"))) ?1.75f:base.MeleeSpeedMultiplier(item); 
        }

        public override void OnEnterWorld(Player player)
        {
            if (!playerCreated)
            {
                playerCreated = true;
                Conversation[] conv = new Conversation[] { new Conversation("", Color.Blue, 15000, ""), new Conversation("I need your help, we'll talk soon", Color.Blue, 75000, "Sora"), new Conversation("I've given you the power to use a Keyblade", Color.Blue, 75000, "Sora"), new Conversation("I hope to meet you soon", Color.Blue, 75000, "Sora") };
                DialogSystem.AddConversation(conv);
            }
            else
            {
                Conversation[] conv = new Conversation[] { new Conversation("I trully hope to meet you soon", Color.Blue, 75000, "Sora") };
                DialogSystem.AddConversation(conv);
            }
        }

        public override void OnRespawn(Player player)
        {
            Conversation[] conv = new Conversation[] {new Conversation("So you can come back from death? How can you do that?", Color.Blue, 75000, "Sora") };
            DialogSystem.AddConversation(conv);
        }

        public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (invincible)
            {
                Vector2 vel = new Vector2(speedX, speedY);
                Vector2 offset = new Vector2(10, 0);
                Projectile.NewProjectile(position + offset, vel, type, damage / 2, knockBack, player.whoAmI);
                Projectile.NewProjectile(position - offset, vel, type, damage / 2, knockBack, player.whoAmI);

                offset = new Vector2(0, 10);
                Projectile.NewProjectile(position + offset, vel, type, damage / 2, knockBack, player.whoAmI);
                Projectile.NewProjectile(position - offset, vel, type, damage / 2, knockBack, player.whoAmI);
            }

            return base.Shoot(item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override float UseTimeMultiplier(Item item)
        {
            return (invincible)?3f:base.UseTimeMultiplier(item);
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (curInvulnerabilityFrames > 0 || invincible)
            {
                return false;
            }
            if (guardTime > 0)
            {
                blockedAttack = true;
                if (guardTime <= 30)
                {
                    guardTime += 25; 
                    PlayGuardSound();
                }
                return false;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            if(lastHeldKeyblade>0)
                CommandLogic.instance.HitAttack();
        }

        public void RaiseMobilityLevel(MaxMobility item)
        {

            item.jumpHeight += 0.5f;
            if (item.level % 4==1)
            {
                item.jumpCount++;
            }

            item.glideTime += 60;
            if (item.level > 3)
            {
                item.noFallDamage = true;
            }

            item.dashSpeed += 1.5f;
            item.dashReaload -= 20;
            if (item.dashReaload <= 10)
            {
                item.dashReaload = 10;
            }
            if (item.level > 2)
            {
                item.canDashMidair = true;
            }
        }

        public void RaiseDoubleJumpLevel(doubleJump item)
        {
            item.jumpHeight += 0.5f;
            if (item.level % 4 == 1)
            {
                item.jumpCount++;
            }
        }

        public void RaiseQuickRunLevel(quickRun item)
        {

            item.dashSpeed += 1.5f;
            item.dashReaload -= 20;
            if (item.dashReaload <= 10)
            {
                item.dashReaload = 10;
            }
            if (item.level > 2)
            {
                item.canDashMidair = true;
            }
        }

        public void RaiseGlideLevel(glide item)
        {

            item.glideTime += 60;
            if (item.level > 3)
            {
                item.noFallDamage = true;
            }
        }

        public void RaiseHealLevel(Heal item)
        {
            item.healAmmount += 5;
            if (item.level > 3)
            {
                item.minManaCost -= 5;
                item.invulnerability += 2;
            }
        }

        public void RaiseSecondChanceLevel(secondChance item)
        {
            item.invulnerabilityFrames += 2;
            item.reloadTime -= 10;
            if (item.level > 5)
            {
                item.autoHP = true;
                item.autoHPReload -= 60 * 15;
                item.recoveredHp += 10;
            }
        }

        public bool isBoss(int npc)
        {
            switch (Main.npc[npc].type)
            {
                case NPCID.EaterofWorldsHead:
                    return true;
                case NPCID.TheDestroyer:
                    return true;
            }
            return Main.npc[npc].boss;
        }

        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            mult = (invincible) ? 0 : mult;
        }

        public override void UpdateBiomes()
        {
            inTwilightTown = KingdomWorld.twilightBiome > 75 && !Main.gameMenu;
        }

        public virtual int CheckPlayerLevel()
        {
            int level = 0;

            if (NPC.downedBoss1)
                level++;
            if (NPC.downedBoss2)
                level++;
            if (NPC.downedBoss3)
                level++;
            if (NPC.downedSlimeKing)
                level++;
            if (NPC.downedQueenBee)
                level++;
            if (Main.hardMode)
                level++;
            if (NPC.downedMechBossAny)
                level++;
            if (NPC.downedMechBoss1)
                level++;
            if (NPC.downedMechBoss2)
                level++;
            if (NPC.downedMechBoss3)
                level++;
            if (NPC.downedPlantBoss)
                level++;
            if (NPC.downedGolemBoss)
                level++;
            if (NPC.downedMoonlord)
                level++;

            return level;
        }

    }
}
