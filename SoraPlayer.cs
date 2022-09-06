using KingdomTerrahearts.Extra;
using KingdomTerrahearts.Interface;
using KingdomTerrahearts.Items;
using KingdomTerrahearts.Items.Weapons;
using KingdomTerrahearts.Logic;
using KingdomTerrahearts.NPCs.Invasions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
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
        //time skip related
        public bool skipToDay = false;
        public bool skipTime = false;
        //Buff/Debuff related
        public bool enlightened = false;
        public bool fightingInBattlegrounds,fightingInArena;
        public Vector2 initPosTrap, endPosTrap;
        //Costume related
        public int curCostume = 0;

        //Teleport related
        public Vector2 originalSpawnPoint;
        public bool canTPToSavePoints = false;
        public DiscoveredSavePoint[] discoveredSavePoints=new DiscoveredSavePoint[0];

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
        public int guardProj;
        public keybladeBlockingType guardType = keybladeBlockingType.none;
        public bool blockedAttack=false;
        public int levelUpShowingTime = 0;
        public static Projectile[] summonProjectiles = new Projectile[0];
        public int noContactDamageTime = 0;

        //Weapon attacks related
        public Vector2 weaponAttackVel;
        public int attackVelTime;
        public bool rotateToAttackVel = false;
        public bool attackVelIgnoreGround;

        //form related
        public bool usingForm;

        public Item lastHeldItem;

        public int lastHeldKeyblade = 0;

        //Team related
        public int[] partyUpgrades;

        //Biome related
        public bool inTwilightTown = false;

        //Music related
        public bool up, down, left, right;
        public bool justPressUp, justPressDown, justPressLeft, justPressRight;
        public Vector2 collisionPoints;

        //Extra collision realated
        public bool collisionUp, collisionDown, collisionLeft, collisionRight;

        //Cutscene related
        public int noControlTime = 0;
        public Vector2 cameraOffset;
        public float cameraZoom;
        public float screenShakeStrength;
        public float screenShakeSpeed;
        public float customCameraPercent;
        public bool midCutscene;

        //Personal and not KH related
        public bool invincible;
        public bool hasZafi;

        public bool playerCreated;
        public bool playerDied;

        public void SetContactinvulnerability(int time)
        {
            noContactDamageTime = time;
        }

        public void AddInvulnerability(int time)
        {
            curInvulnerabilityFrames += (curInvulnerabilityFrames > time) ? 0 : time;
        }

        public bool IsInvulnerable()
        {
            return curInvulnerabilityFrames > 0 || noControlTime>0;
        }

        public void ResetTimers()
        {
            SCcurReload = 0;
            AHPcurReload = 0;
        }

        public override void ResetEffects()
        {
            ModifyCutsceneCamera(Vector2.Zero);

            curCostume = -1;

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

            enlightened = false;

            Player.noFallDmg = false;

            invincible = false;
            hasZafi = false;
            canTPToSavePoints = false;

            collisionRight = false;
            collisionLeft = false;
            collisionUp = false;
            collisionDown = false;
            
            midCutscene = false;

        }

        public override void PlayerDisconnect(Player player)
        {
            inTwilightTown = false;

            if (player == Main.LocalPlayer)
            {
                DialogSystem.RemoveConversations(10000000);
            }
        }

        public bool hasHealingPotion()
        {

            for(int i = 0; i < Player.inventory.Length; i++)
            {
                if(Player.inventory[i].consumable && Player.inventory[i].healLife > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (noControlTime <= 0)
            {
                /*
                Go To Space
                KingdomWorld kingdom = mod.GetModWorld("KingdomWorld") as KingdomWorld;
                Main.NewText(Main.worldName);
                kingdom.GoToSpace();
                Go To world
                KingdomWorld kingdom = mod.GetModWorld("KingdomWorld") as KingdomWorld;
                Main.NewText(Main.worldName);
                kingdom.GoToMainDimension();
                */

                #region music
                up = down = left = right = false;
                justPressDown = justPressLeft = justPressRight = justPressUp = false;

                if (KingdomTerrahearts.MusicUpKey.JustPressed)
                {
                    justPressUp = true;
                    /*
                    Main.invasionDelay = 0;
                    Main.invasionProgress = 0;
                    Main.invasionProgressMax = 0;
                    Main.invasionSize = 0;
                    Main.invasionType = -1;
                    ThousandHeartlessInvasion.StartInvasion();
                    */

                }
                else if (KingdomTerrahearts.MusicLeftKey.JustPressed)
                {
                    justPressLeft = true;
                }
                else if (KingdomTerrahearts.MusicDownKey.JustPressed)
                {
                    justPressDown = true;
                }
                else if (KingdomTerrahearts.MusicRightKey.JustPressed)
                {
                    justPressRight = true;
                }

                if (KingdomTerrahearts.MusicUpKey.Current)
                {
                    up = true;
                }
                else if (KingdomTerrahearts.MusicLeftKey.Current)
                {
                    left = true;
                }
                else if (KingdomTerrahearts.MusicDownKey.Current)
                {
                    down = true;
                }
                else if (KingdomTerrahearts.MusicRightKey.Current)
                {
                    right = true;
                }

                #endregion

                #region party
                if (KingdomTerrahearts.PartySelectHotkey.JustPressed)
                {
                    KingdomTerrahearts.instance.TogglePartyUI();
                }
                #endregion

                #region movement

                if (Player.mount.Type == -1)
                {
                    if (KingdomTerrahearts.GuardHotKey.JustPressed && guardTime < -30 && lastHeldKeyblade > 0 && !invincible && curInvulnerabilityFrames <= 0)
                    {
                        PlayGuardSound();
                        guardTime = 30;
                    }
                    if (canCastHeal && castHealAmount > 0 && Player.statLife < (Player.statLifeMax / 4 * 3) && !Player.HasBuff(BuffID.ManaSickness) )
                    {

                        if (triggersSet.QuickHeal && ((Player.HasBuff(BuffID.PotionSickness)  && hasHealingPotion())|| 
                            !hasHealingPotion()) && (((Player.statManaMax > 0) ? Player.statMana / Player.statManaMax * 100 : 1) > castHealCost / Player.statManaMax || (Player.statMana >= Player.statManaMax / 2 && castHealCost > Player.statManaMax / 2)))
                        {

                            Player.statMana = 0;
                            Player.HealEffect((int)((float)castHealAmount / 100f * (float)Player.statLifeMax));
                            Player.statLife += (int)((float)castHealAmount / 100f * (float)Player.statLifeMax);
                            curInvulnerabilityFrames = (curInvulnerabilityFrames < castHealInvulnerabilityTime) ? castHealInvulnerabilityTime : curInvulnerabilityFrames;
                            Player.AddBuff(BuffID.ManaSickness, 500);

                        }

                    }

                    if (canGlide)
                    {
                        if (triggersSet.Jump && curGlideTime > 0 && Player.velocity.Y > glideFallSpeed)
                        {
                            curGlideTime--;
                            Player.velocity.Y = (Player.velocity.Y > glideFallSpeed) ? glideFallSpeed : Player.velocity.Y;
                            Player.slowFall = true;
                            Player.noFallDmg = true;
                        }
                    }

                    curDashReload--;
                    curDashReload = (curDashReload > dashReloadSpeed) ? dashReloadSpeed : curDashReload;

                    if (canDash && Player.dash <= 0 && curDashReload <= 0)
                    {
                        if (triggersSet.Left || triggersSet.Right)
                        {
                            int curPress = (triggersSet.Left) ? -1 : 1;
                            tapTime++;
                            if (tapped)
                            {
                                if (lastPress == curPress && (canDashMidAir || Math.Abs(Player.velocity.Y) < 0.15f))
                                {
                                    if (Math.Abs(Player.velocity.X) < dashSpeed + Player.maxRunSpeed)
                                    {
                                        Player.velocity.X += (triggersSet.Left) ? -dashSpeed : dashSpeed;
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

                        if (Player.velocity.Y == 0)
                        {
                            jumped = false;
                            jumpCount = 0;
                        }
                        else
                        {

                            if (triggersSet.Jump && Player.wingTime <= 0)
                            {
                                int initJump = 0;
                                int extradoubleJumps = 0;
                                if (Player.canJumpAgain_Blizzard)
                                {
                                    extradoubleJumps++;
                                    initJump++;
                                }
                                if (Player.canJumpAgain_Cloud)
                                {
                                    extradoubleJumps++;
                                    initJump++;
                                }
                                if (Player.canJumpAgain_Fart)
                                {
                                    extradoubleJumps++;
                                    initJump++;
                                }
                                if (Player.canJumpAgain_Sail)
                                {
                                    extradoubleJumps++;
                                    initJump++;
                                }
                                if (Player.canJumpAgain_Sandstorm)
                                {
                                    extradoubleJumps++;
                                    initJump++;
                                }
                                if (Player.canJumpAgain_Unicorn)
                                {
                                    extradoubleJumps++;
                                    initJump++;
                                }
                                if (Player.canJumpAgain_WallOfFleshGoat)
                                {
                                    extradoubleJumps++;
                                    initJump++;
                                }
                                if (Player.canJumpAgain_Basilisk)
                                {
                                    extradoubleJumps++;
                                    initJump++;
                                }

                                if (jumped && jumpCount < doubleJumpQuantity + extradoubleJumps)
                                {
                                    Player.velocity.Y = (jumpCount >= initJump) ? -doubleJumpHeight : Player.velocity.Y;
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
                #endregion
            }
        }

        public void PlayGuardSound()
        {
            switch (guardType)
            {
                case keybladeBlockingType.reflect:
                SoundEngine.PlaySound(new SoundStyle("KingdomTerrahearts/Sounds/keybladeBlocking"));
                    break;
                case keybladeBlockingType.normal:
                case keybladeBlockingType.reversal:
                SoundEngine.PlaySound(SoundID.Item1,new Vector2(Player.Center.X, Player.Center.Y));
                    break;
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {

            if (invincible)
            {
                return false;
            }

            //Second Chance && Auto-HP
            if (SCcurReload == 0 && hasSecondChance && secondChanceReload != 1000 && Player.statLife != 1)
            {

                if (AHPcurReload == 0 && hasAutoHP && autoHPRecover != 0)
                {
                    Player.statLife += autoHPRecover;
                    Player.HealEffect(autoHPRecover);

                    AHPcurReload = Player.statLifeMax / ((autoHPReload!=0)? autoHPReload : 1 );
                }
                else
                {
                    Player.statLife += 1;
                }

                SCcurReload = secondChanceReload;
                curInvulnerabilityFrames = secondChanceInvulnerability;

                return false;

            }

            //Kupo Coin
            if (Player.HasItem(ModContent.ItemType<KupoCoin>()) && reviveTime <= 0)
            {
                Player.ConsumeItem(ModContent.ItemType<KupoCoin>());
                Player.statLife = Player.statLifeMax2;
                Player.HealEffect(Player.statLifeMax2);
                Player.immuneTime = Player.longInvince ? 180 : 120;
                for (int k = 0; k < Player.hurtCooldowns.Length; k++)
                {
                    Player.hurtCooldowns[k] = Player.longInvince ? 180 : 120;
                }
                SoundEngine.PlaySound(SoundID.Item29, Player.position);
                reviveTime = 60 * 60 * 3;
                return false;
            }



            reviveTime = 0;
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
        {
            for(int i = 0; i < itemsByMod["Terraria"].Count; i++)
            {
                if (itemsByMod["Terraria"][i].type == ItemID.CopperShortsword)
                {
                    itemsByMod["Terraria"].RemoveAt(i);
                    itemsByMod["Terraria"].Add(new Item(ModContent.ItemType<Items.Weapons.Joke.Keyblade_wood>()));
                }
            }

        }

        public override void PreUpdate()
        {

            if (noControlTime > 0)
            {
                return;
            }

            midCutscene = (midCutscene)?midCutscene : noControlTime > 0;

            inTwilightTown = KingdomWorld.twilightBiome > 75 && !Main.gameMenu;

            if (levelUpShowingTime > 0)
            {
                KingdomTerrahearts.instance.ShowLevelUpUI();
            }
            else
            {
                KingdomTerrahearts.instance.HideLevelUpUI();
            }

            if (skipTime && Main.dayTime!=skipToDay)
            {
                Main.time += 100;
            }
            else if(Main.dayTime == skipToDay)
            {
                skipTime = false;
            }

            if (curInvulnerabilityFrames > -1)
            {
                curInvulnerabilityFrames--;
            }

            if (Grounded())
            {
                curGlideTime = glideTime;
            }

            if (--secondChanceReload <= 0) secondChanceReload = 0;
            if (--autoHPReload <= 0) autoHPReload = 0;

            Player.noFallDmg = (tpFallImmunity > 0) ? true : Player.noFallDmg;

            if (invincible)
            {
                Player.ghost =false;
                Player.statMana = Player.statLifeMax;
                Player.statLife = Player.statLifeMax;
                Player.maxMinions = 1000;
                Player.slotsMinions = 1000;
                Player.merman = true;
                Player.nightVision = true;
                Player.noKnockback = true;
                Player.blockRange = 1000;
                Player.wallSpeed = 1000;
                Player.tileSpeed = 1000;
                Player.cLight = 1;
                Player.blockRange = 1000;
                Player.autoJump = true;
                Player.wingTimeMax = 1000000;
                Player.wingTime = 1000000;
                Player.autoJump = true;

                /*For Rolling
                
                Vector2 rotOrigin = player.Center - player.position;
                rotOrigin.Y *= 1.25f;
                player.fullRotationOrigin = rotOrigin;
                player.fullRotation = (float)Math.PI;

                */
            }

            if (fightingInBattlegrounds || fightingInArena)
            {
                //check if really trapped
                bool newTrapped = false;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    newTrapped = (isBoss(i) && Main.npc[i].life > 0 && Main.npc[i].active) ? true : newTrapped;
                }
                fightingInBattlegrounds =(fightingInBattlegrounds)?newTrapped:false;
                fightingInArena =(fightingInArena)? newTrapped:false;

                //effects
                Vector2 nextPos = Player.Center + Player.velocity;

                collisionPoints.X = Math.Clamp(nextPos.X,initPosTrap.X,endPosTrap.X);
                collisionPoints.Y = Math.Clamp(nextPos.Y, initPosTrap.Y, endPosTrap.Y);

                collisionRight = nextPos.X >= endPosTrap.X;
                collisionLeft = nextPos.X <= initPosTrap.X;
                collisionDown = nextPos.Y >= endPosTrap.Y;
                collisionUp = nextPos.Y <= initPosTrap.Y;

                Player.Update_NPCCollision();

                Vector2 clampedPos = Player.Center;
                clampedPos.X = Math.Min(endPosTrap.X+Player.width*2, Math.Max(initPosTrap.X-Player.width*2, clampedPos.X));
                clampedPos.Y = Math.Min(endPosTrap.Y+Player.height*2, Math.Max(initPosTrap.Y-Player.height*2, clampedPos.Y));

                while (!Collision.EmptyTile(clampedPos.ToTileCoordinates().Y,clampedPos.ToTileCoordinates().X) && Math.Abs(clampedPos.Y-(initPosTrap.Y+endPosTrap.Y)/2)>Player.height*6)
                {
                    clampedPos.Y += Math.Sign(clampedPos.Y- (initPosTrap.Y + endPosTrap.Y) / 2f) * 16f;
                }

                Player.Center = clampedPos;
            }
            else
            {
                initPosTrap = Vector2.Zero;
                endPosTrap = Vector2.Zero;

                collisionRight = collisionLeft = collisionDown = collisionUp = false;
            }

            tpFallImmunity -= (tpFallImmunity>0)?1:0;


            guardTime = (guardTime > -100) ? guardTime - 1 : -100;

            if (guardTime <= 0)
            {
                guardProj = -1;
                guardType = keybladeBlockingType.none;
                blockedAttack = false;
            }

            if (guardType != keybladeBlockingType.none)
            {
                Player.velocity = Vector2.Zero;
                int projType = ModContent.ProjectileType<Projectiles.ScepTend.Vergil_Bubble>();

                float projScale = 1;
                switch (guardType)
                {
                    case keybladeBlockingType.normal:
                        projType = ProjectileID.Typhoon;
                        break;
                    case keybladeBlockingType.reflect:
                        projType = ModContent.ProjectileType<Projectiles.guardProjectile>();
                        break;
                    case keybladeBlockingType.reversal:
                        projScale = 5;
                        break;
                }

                if (guardProj == -1 || !Main.projectile[guardProj].active)
                {
                    EntitySource_ItemUse source = new EntitySource_ItemUse(Player,Player.HeldItem);
                    guardProj = Projectile.NewProjectile(source, Player.Center, Player.velocity, projType, 0, 0,Owner: Player.whoAmI);
                }
                Main.projectile[guardProj].scale = projScale;
                Main.projectile[guardProj].timeLeft = guardTime;
            }

            base.PreUpdate();

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
            CommandLogic.instance.Update();
            DialogSystem.Update();

            if (noControlTime >= 0)
            {
                KingdomTerrahearts.instance.HideCommandUI();
                Main.playerInventory = false;
                Main.gamePaused = false;
                Main.hideUI = true;
                noControlTime--;
                return;
            }
            else
            {
                if (noControlTime == -1)
                {
                    Main.hideUI = false;
                }
                midCutscene = false;
            }
            
            noContactDamageTime--;


            NPC.MoonLordCountdown = (NPC.MoonLordCountdown > 250) ? 250 : NPC.MoonLordCountdown;

            if (invincible)
            {
                Player.ghost = false;
            }

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
            if (lastHeldKeyblade < 0)
            {
                guardType = keybladeBlockingType.none;
            }
            if (guardTime == 30)
            {
                PlayGuardSound();
            }

            lastHeldKeyblade--;

            base.PostUpdate();

            lastHeldItem = Player.HeldItem;

            if(attackVelTime>0 && (Collision.CanHitLine(Player.Center, Player.width, Player.height, Player.Center+weaponAttackVel, Player.width, Player.height)||attackVelIgnoreGround))
            {
                Player.Center += weaponAttackVel/2;
                Player.velocity = weaponAttackVel/2;

                if (rotateToAttackVel){
                    Player.fullRotation = MathHelp.Lerp(0, MathF.Atan2(weaponAttackVel.Y, weaponAttackVel.X),MathF.Min(1,attackVelTime/3f));
                }
            }
            attackVelTime--;

        }

        public void AttackMovement(Vector2 vel, int time,bool ignoreGround=false)
        {
            attackVelIgnoreGround = ignoreGround;
            attackVelTime = time;
            weaponAttackVel = vel;

            Player.velocity = weaponAttackVel;
        }

        public void ChangeGlideFallSpeed(float fallspeed)
        {
            glideFallSpeed = (glideFallSpeed > fallspeed) ? fallspeed : glideFallSpeed;
        }

        public void ChangeDashReload(int reload)
        {
            dashReloadSpeed = (dashReloadSpeed > reload) ? reload : dashReloadSpeed;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["partyUpgrades"] = partyUpgrades;
            if (playerCreated)
            {
                tag["playerCreated"] = playerCreated;
            }
            if (playerDied)
            {
                tag["playerDied"] = playerDied;
            }
            tag["playerSpawn"] = originalSpawnPoint;
        }

        public override void LoadData(TagCompound tag)
        {
            partyUpgrades = tag.GetIntArray("partyUpgrades");
            playerCreated = tag.GetBool("playerCreated");
            playerDied = tag.GetBool("playerDied");
            if (tag.ContainsKey("playerSpawn"))
            {
                originalSpawnPoint = tag.Get<Vector2>("playerSpawn");
            }
            KingdomTerrahearts.instance.ShowDialogUI();
        }

        public void GetPartyMember(int npcNum)
        {
            if (npcNum<Main.maxNPCs && Main.npc[npcNum].active && Main.npc[npcNum].townNPC )
            {
                NPC npc = Main.npc[npcNum];
                if (PartyMemberLogic.IsPartyMember(Main.npc[npcNum].type)>=0)
                {
                    Conversation conv = new Conversation(GetMemberDialog(npc.type), Color.White, DialogSystem.NPC_DIALOGTIME, npc.GivenOrTypeName);
                    DialogSystem.AddConversation(conv);

                    PartyMemberLogic.RemovePartyMember(npc.type);
                    npc.color = Color.White;
                    npc.Teleport(new Vector2(npc.homeTileX * 16, npc.homeTileY * 16 - npc.height)-npc.velocity);
                    npc.velocity = new Vector2();
                }
                else
                {
                    Conversation conv = new Conversation(GetMemberDialog(npc.type,true), Color.White, DialogSystem.NPC_DIALOGTIME, npc.GivenOrTypeName);
                    DialogSystem.AddConversation(conv);
                    PartyMemberLogic.AddPartyMember(npc.type, Player.name);
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
                    case NPCID.Painter:
                        return "Let's paint the world crymson red! Or maybe a slight green would be better?";
                    case NPCID.Angler:
                        return "You may be my errand monkey, but sometimes I need to spread my legs as well";
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
                        return "I like helping, but try to do things yourself, jeez!";
                    case NPCID.ArmsDealer:
                        return "I hope the ladies were looking";
                    case NPCID.Mechanic:
                        return (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) ? "That wasn't as bad as I thought" :"I hope I can fix the mistakes I made some day";
                    case NPCID.PartyGirl:
                        return "Till next time!! Don't party without me, you hear me?";
                    case NPCID.Nurse:
                        return "Don't get too hurt, I want to see you in one pice when you get back";
                    case NPCID.Painter:
                        return "Time to wait for the paint to dry";
                    case NPCID.Angler:
                        return "I'm going to fish some bigger fish somewhere else";
                    default:
                        return "Godbye!";
                }
            }
        }

        public NPC GetModdedNPC(string name)
        {
            for(int i = 0; i < Main.maxNPCTypes; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type.ToString() == name) {
                    return Main.npc[i];
                }
            }
            return null;
        }

        public override bool CanSellItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (item.type == ModContent.ItemType<KupoCoin>())
            {
                return false;
            }
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

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (invincible)
            {
                damage *= 15;
            }
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            return (invincible) ? false : base.CanConsumeAmmo(weapon, ammo);
        }

        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {
            fishingLevel += (Player.HasBuff(ModContent.BuffType<Buffs.EnlightenedBuff>())) ? 50 : fishingLevel;
            fishingLevel = (invincible) ? 1000 : fishingLevel;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (invincible)
            {
                Player.dead = false;
                Player.statLife = Player.statLifeMax;
            }
        }

        public override void OnEnterWorld(Player player)
        {
            CutsceneLogic.instance.CutsceneReset();
            PartyMemberLogic.Reset();

            if (!playerCreated)
            {
                playerCreated = true;
                Conversation[] conv = new Conversation[] { new Conversation("", Color.Blue, DialogSystem.NPC_DIALOGTIME, ""), new Conversation("I need your help, we'll talk soon", Color.Blue, DialogSystem.NPC_DIALOGTIME, "Sora"), new Conversation("I've given you the power to use a Keyblade", Color.Blue, DialogSystem.NPC_DIALOGTIME, "Sora"), new Conversation("I hope to meet you soon", Color.Blue, DialogSystem.NPC_DIALOGTIME, "Sora") };
                DialogSystem.AddConversation(conv);
            }
            else
            {
                Conversation[] conv = new Conversation[] { new Conversation("I trully hope to meet you soon", Color.Blue, DialogSystem.NPC_DIALOGTIME, "Sora") };
                DialogSystem.AddConversation(conv);
            }

            if (MathHelp.Magnitude(originalSpawnPoint)==0)
            {
                originalSpawnPoint = player.position;
            }

        }

        public override void OnRespawn(Player player)
        {
            if (!playerDied)
            {
                Conversation[] conv = new Conversation[] { new Conversation("So you can come back from death? How can you do that?", Color.Blue, DialogSystem.NPC_DIALOGTIME, "Sora") };
                DialogSystem.AddConversation(conv);
                playerDied = true;
            }
        }

        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (invincible)
            {
                Vector2 offset = new Vector2(10, 0);

                Projectile.NewProjectile(source, position + offset, velocity, type, damage, knockback, Player.whoAmI);
                Projectile.NewProjectile(source, position - offset, velocity, type, damage, knockback, Player.whoAmI);

                offset = new Vector2(0, 10);
                Projectile.NewProjectile(source, position + offset, velocity, type, damage, knockback, Player.whoAmI);
                Projectile.NewProjectile(source, position - offset, velocity, type, damage, knockback, Player.whoAmI);
            }

            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }

        public override float UseTimeMultiplier(Item item)
        {
            return (invincible)?0.1f:base.UseTimeMultiplier(item);
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (noControlTime > 0)
            {
                return false;
            }
            if (noContactDamageTime > 0 && damageSource.SourceNPCIndex >= 0 && damageSource.SourceProjectileIndex < 0)
            {
                return false;
            }
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
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            if (lastHeldKeyblade > 0)
            {
                CommandLogic.instance.HitAttack();
            }
        }

        public int GetClosestEnemy(int radius)
        {
            int closest=-1;
            for(int i = 0; i <Main.maxNPCs;i++)
            {
                if(Main.npc[i].active && ((!Main.npc[i].friendly && !Main.npc[i].townNPC && !Main.npc[i].CountsAsACritter) || Main.npc[i].boss) && Vector2.Distance(Main.npc[i].Center,Player.Center)<radius)
                {
                    if(closest==-1 || Vector2.Distance(Main.npc[i].Center,Player.Center)< Vector2.Distance(Main.npc[closest].Center, Player.Center))
                    {
                        closest = i;
                    }
                }
            }
            return closest;
        }
        public void RaiseSoulLevel(WielderSoul item)
        {
            item.healAmmount += 5;
            if (item.level > 3)
            {
                item.minManaCost -= 5;
                item.invulnerability += 2;
            }

            item.invulnerabilityFrames += 2;
            item.reloadTime -= 10;
            if (item.level > 5)
            {
                item.autoHP = true;
                item.autoHPReload -= 60 * 15;
                item.recoveredHp += 10;
            }

            item.jumpHeight += 0.5f;
            if (item.level % 4 == 1)
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
            if (NPC.downedQueenSlime)
                level++;
            if (NPC.downedMechBossAny)
                level++;
            if (NPC.downedMechBoss1)
                level++;
            if (NPC.downedMechBoss2)
                level++;
            if (NPC.downedMechBoss3)
                level++;
            if (NPC.downedEmpressOfLight)
                level++;
            if (NPC.downedPlantBoss)
                level++;
            if (NPC.downedGolemBoss)
                level++;
            if (NPC.downedMoonlord)
                level++;

            return level;
        }

        public override void FrameEffects()
        {
            switch (curCostume)
            {
                case 0:
                    Player.body = KingdomTerrahearts.orgCoatSlots[0];
                    Player.legs = KingdomTerrahearts.orgCoatSlots[1];
                    Player.head = KingdomTerrahearts.orgCoatSlots[2];
                    break;
                case 1:
                    Player.body = KingdomTerrahearts.orgCoatSlots[0];
                    Player.legs = KingdomTerrahearts.orgCoatSlots[1];
                    break;
                case 2:
                    Player.body = KingdomTerrahearts.orgCoatSlots[3];
                    Player.legs = KingdomTerrahearts.orgCoatSlots[4];
                    break;
            }
        }

        public bool Grounded()
        {

            if(Collision.SolidCollision(Player.position, Player.width, Player.height+5, true))
            {
                return true;
            }

            Vector2 feetpos = Player.position+new Vector2(0,Player.height+5);

            if (Collision.IsWorldPointSolid(feetpos))
            {
                return true;
            }

            feetpos += new Vector2(Player.width, 0);

            if (Collision.IsWorldPointSolid(feetpos))
            {
                return true;
            }

            return false;
        }

        public void ModifyCutsceneCamera(Vector2 offset, float zoom = -1, float shakeForce = 0, float shakeSpeed = 1,float camPercentChange=10)
        {
            if (offset == Vector2.Zero && zoom == -1 && shakeForce == 0 && shakeSpeed == 1)
            {
                customCameraPercent = Math.Clamp(customCameraPercent - camPercentChange, 0, 100);
                
                if (customCameraPercent == 0 && cameraZoom>-2) {
                    cameraOffset = Vector2.Zero;
                    cameraZoom = -1;
                    screenShakeStrength = 0;
                    screenShakeSpeed = 1;
                }
            }
            else
            {
                cameraOffset =(offset==Vector2.Zero)?cameraOffset:offset;
                cameraZoom = (cameraZoom==-1)?cameraZoom:zoom;
                screenShakeStrength = (shakeForce==0)?screenShakeStrength:shakeForce;
                screenShakeSpeed =(shakeSpeed==1)?screenShakeSpeed:shakeSpeed;

                customCameraPercent = Math.Clamp(customCameraPercent+camPercentChange,0,100);
            }
        }

        public override void ModifyScreenPosition()
        {
            float customCamPower = customCameraPercent / 100f;

            if (cameraZoom > -2)
            {
                Main.GameZoomTarget = (cameraZoom > -1) ? cameraZoom * customCamPower : Main.ForcedMinimumZoom;
                cameraZoom = (cameraZoom==-1)?-2:cameraZoom;
            }

            Main.screenPosition += cameraOffset*customCamPower;
            Main.screenPosition += new Vector2((float)Math.Sin(Main.time*screenShakeSpeed),0f)*screenShakeStrength*KingdomTerrahearts.screenShakeStrength*customCamPower;

        }

        public override bool CanUseItem(Item item)
        {
            if (noControlTime > 0)
            {
                return false;
            }
            return base.CanUseItem(item);
        }

        public override void SetControls()
        {
            if (noControlTime > 0)
            {
                Player.controlDown =
                    Player.controlInv =
                    Player.controlJump =
                    Player.controlLeft =
                    Player.controlRight =
                    Player.controlSmart =
                    Player.controlThrow =
                    Player.controlTorch =
                    Player.controlUp =
                    Player.controlUseItem =
                    Player.controlUseTile=
                    Player.controlHook=
                     false;
            }
        }

        public void ControlDuringCutscene()
        {
            noControlTime = 5;
        }


        public void GetCloserToEnemy(int targetDistance,float forwardMovement)
        {
            SoraPlayer sp = Player.GetModPlayer<SoraPlayer>();
            int closest = sp.GetClosestEnemy(targetDistance);
            if (closest != -1 && Vector2.Distance(Main.npc[closest].Center, Player.Center) > forwardMovement + (Main.npc[closest].width / 2 + Main.npc[closest].height / 2) / 2)
            {
                Player.velocity = MathHelp.Normalize(Main.npc[closest].Center - Player.Center) * forwardMovement;
                Player.direction = (int)MathHelp.Sign((Main.npc[closest].Center - Player.Center).X);
            }
        }

    }

}
