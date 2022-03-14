using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomTerrahearts.NPCs.Invasions;
using KingdomTerrahearts.CustomTownNPCAI;
using System.Collections.Generic;
using KingdomTerrahearts.Extra;
using System;
using KingdomTerrahearts.Interface;
using KingdomTerrahearts.DropRules;
using Terraria.GameContent.ItemDropRules;
using KingdomTerrahearts.Logic;

namespace KingdomTerrahearts
{
    class NPCOverride : GlobalNPC
    {

        public override bool InstancePerEntity => true;

        //partymember stuff
        public int partyOwner=-1;
        public Vector2 prevPos=new Vector2();

        //0: initDamage, 1: Cooldown, 2: RandomCooldown,3: target, 4: attackAnimationTime,5: attackCombo,6: npcType, 7: attackFrame, 8:walkFrameCounter
        int[] initTownNPCStats = new int[]{-1,-1,0,0,-1,-1,0,0,0};

        //boss stuff
        bool heartlessVerActive=false;
        bool spawnConversationDone = false;
        int proj;

        //Cutscene stuff
        public bool isCutsceneActor;
        public bool isMidCutscene;
        public int lastRemainingTime;
        public Vector2 lastPos;
        public Vector2 lastVel;
        public float[] lastAI;

        //Status effects stuff
        public bool timeFrozen;
        public int initDamage;
        public Color initClr;

        EntitySource_Parent s;

        public override void ResetEffects(NPC npc)
        {
            if (timeFrozen)
            {
                timeFrozen = false;
                npc.damage = initDamage;
                npc.color = initClr;
            }
        }

        public void Stop(NPC npc)
        {
            if (!timeFrozen)
            {
                timeFrozen = true;
                initDamage = npc.damage;
                initClr = npc.color;
                npc.damage = 0;
            }
        }

        //Get npc stats
        public override void SetDefaults(NPC npc)
        {
            base.SetDefaults(npc);

            initTownNPCStats[0] = npc.damage;
            initTownNPCStats[6] = npc.type;

            switch (npc.type)
            {
                case NPCID.Dryad:
                    initTownNPCStats[7] = 19;
                    break;
                default:
                case NPCID.Angler:
                case NPCID.Merchant:
                case NPCID.Nurse:
                case NPCID.Mechanic:
                case NPCID.Steampunker:
                case NPCID.Stylist:
                    initTownNPCStats[7] = 20;
                    break;
                case NPCID.Clothier:
                case NPCID.Wizard:
                    initTownNPCStats[7] = 21;
                    break;
                case NPCID.ArmsDealer:
                case NPCID.Demolitionist:
                case NPCID.DyeTrader:
                case NPCID.Painter:
                case NPCID.SantaClaus:
                    initTownNPCStats[7] = 22;
                    break;
                case NPCID.Guide:
                case NPCID.Cyborg:
                case NPCID.Pirate:
                case NPCID.TaxCollector:
                    initTownNPCStats[7] = 23;
                    break;
            }

            s = new EntitySource_Parent(npc);
        }

        //Change the spawn pool
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            //If the custom invasion is up and the invasion has reached the spawn pos
            if (KingdomWorld.customInvasionUp && (Main.invasionX == (double)Main.spawnTileX))
            {
                //Clear pool so that only the stuff you want spawns
                pool.Clear();

                //key = NPC ID | value = spawn weight
                //pool.add(key, value)

                //For every ID inside the invader array in our CustomInvasion file
                foreach (int i in ThousandHeartlessInvasion.heartless)
                {
                    pool.Add(i, 15f); //Add it to the pool with the same weight of 1
                }
                if (!FindBoss(ThousandHeartlessInvasion.heartlessBosses))
                {
                    foreach (int i in ThousandHeartlessInvasion.heartlessBosses)
                    {
                        pool.Add(i, 0.7f); //Add it to the pool with the same weight of 1
                    }
                }
            }
        }

        public bool FindBoss(int[] possibleBosses)
        {

            for (int i = 0; i < possibleBosses.Length; i++)
            {
                if (FindNPCType(possibleBosses[i]) != null)
                {
                    return true;
                }
            }

            return false;
        }

        //Changing the spawn rate
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            //Change spawn stuff if invasion up and invasion at spawn
            if (KingdomWorld.customInvasionUp && (Main.invasionX == (double)Main.spawnTileX))
            {
                spawnRate = 1; //The higher the number, the less chance it will spawn (thanks jopojelly for how spawnRate works)
                maxSpawns = 10000; //Max spawns of NPCs depending on NPC value
            }
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            if (sp.invincible)
            {
                spawnRate = 0;
                maxSpawns = 1000000000;
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case NPCID.Dryad:
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Keyblade_destiny>());
                    nextSlot++;
                    break;
                case NPCID.WitchDoctor:

                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Keyblade_witchDoctor>());
                    nextSlot++;
                    break;
                case NPCID.Merchant:
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.KupoCoin>());
                    nextSlot++;
                    break;
                case NPCID.Clothier:
                    shop.item[nextSlot].SetDefaults(ItemID.ClothierVoodooDoll);
                    nextSlot++;
                    break;
            }
            base.SetupShop(type, shop, ref nextSlot);
        }

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            if (npc.type == NPCID.Dryad && !firstButton && (WorldGen.tEvil==0 && WorldGen.tGood == 0))
            {
                Conversation[] conv = new Conversation[] { new Conversation("That was kindda pointless, but here you go, a Terra Blade for your effords", Color.DarkGreen, 75, "Creator") };
                DialogSystem.AddConversation(conv);
                EntitySource_Parent s = new EntitySource_Parent(npc);
                Item.NewItem(s,npc.getRect(), ItemID.TerraBlade);
            }
        }

        //Adding to the AI of an NPC
        public override void PostAI(NPC npc)
        {
            //Changes NPCs so they do not despawn when invasion up and invasion at spawn
            if (KingdomWorld.customInvasionUp && (Main.invasionX == (double)Main.spawnTileX))
            {
                npc.timeLeft = 1000;
            }

            Player p = Main.LocalPlayer;
            SoraPlayer sp = p.GetModPlayer<SoraPlayer>();
            if (sp.noControlTime >= 0 && !isCutsceneActor)
            {
                if (!isMidCutscene)
                {
                    isMidCutscene = true;
                    lastPos = npc.Center;
                    lastVel = npc.velocity;
                    lastRemainingTime = npc.timeLeft;
                    lastAI = npc.ai;
                }

                npc.Center = lastPos;
                npc.velocity = lastVel;
                npc.timeLeft = lastRemainingTime;
                npc.ai = lastAI;

                if (Vector2.Distance(npc.Center, p.Center) < 50 && !npc.friendly && !npc.townNPC && !npc.CountsAsACritter && npc.damage>0)
                {
                    npc.life = 0;
                    npc.checkDead();
                }
            }
        }

        public void SetCutsceneActor(bool isActor)
        {
            isCutsceneActor = isActor;
        }

        public override bool PreAI(NPC npc)
        {
            Player p = Main.LocalPlayer;
            if (isCutsceneActor || p.GetModPlayer<SoraPlayer>().midCutscene || timeFrozen)
            {
                return false;
            } else if (partyOwner>=0 && npc.townNPC) {

                CustomTownNPCAI(npc, npc.type);
                AI(npc);

                return false;
            } else {
                return true;
            }
        }

        public void CustomTownNPCAI(NPC npc, int type)
        {
            switch (type)
            {
                case NPCID.Guide:
                    PartyMemberAI.GuidePartyMemberAI(npc,prevPos,(Main.hardMode)?ProjectileID.JestersArrow : ProjectileID.FireArrow, ref initTownNPCStats);
                    break;
                case NPCID.Merchant:
                    PartyMemberAI.GuidePartyMemberAI(npc, prevPos, ProjectileID.GoldCoin, ref initTownNPCStats);
                    break;
                case NPCID.Nurse:
                    PartyMemberAI.HurtfullNursePartyMemberAI(npc, prevPos, ProjectileID.NurseSyringeHurt, ref initTownNPCStats);
                    break;
                case NPCID.Demolitionist:
                    PartyMemberAI.GuidePartyMemberAI(npc, prevPos, ProjectileID.Grenade, ref initTownNPCStats);
                    break;
                case NPCID.DyeTrader:
                    PartyMemberAI.GuidePartyMemberAI(npc, prevPos, ProjectileID.CrystalBullet, ref initTownNPCStats);
                    break;
                case NPCID.Angler:
                    PartyMemberAI.GuidePartyMemberAI(npc, prevPos, ProjectileID.FrostDaggerfish, ref initTownNPCStats);
                    break;
                case NPCID.Dryad:
                    PartyMemberAI.DryadPartyMemberAI(npc, prevPos, ProjectileID.Leaf, ref initTownNPCStats);
                    break;
                case NPCID.Painter:
                    PartyMemberAI.DryadPartyMemberAI(npc, prevPos, ProjectileID.PainterPaintball, ref initTownNPCStats);
                    break;
                case NPCID.ArmsDealer:
                    PartyMemberAI.DryadPartyMemberAI(npc, prevPos, ProjectileID.ChlorophyteBullet, ref initTownNPCStats);
                    break;
                case NPCID.DD2Bartender:
                    PartyMemberAI.DryadPartyMemberAI(npc, prevPos, ProjectileID.MolotovCocktail, ref initTownNPCStats);
                    break;
                case NPCID.Stylist:
                    PartyMemberAI.DryadPartyMemberAI(npc, prevPos, ProjectileID.BoulderStaffOfEarth, ref initTownNPCStats);
                    break;
                case NPCID.GoblinTinkerer:
                    PartyMemberAI.DryadPartyMemberAI(npc, prevPos, ProjectileID.SpikyBall, ref initTownNPCStats);
                    break;
                case NPCID.WitchDoctor:
                    PartyMemberAI.DryadPartyMemberAI(npc, prevPos, ProjectileID.CrystalDart, ref initTownNPCStats);
                    break;
                case NPCID.Clothier:
                    PartyMemberAI.DryadPartyMemberAI(npc, prevPos, ProjectileID.Skull, ref initTownNPCStats);
                    break;
                case NPCID.Mechanic:
                    PartyMemberAI.DryadPartyMemberAI(npc, prevPos, ProjectileID.MechanicWrench, ref initTownNPCStats);
                    break;
                case NPCID.PartyGirl:
                    PartyMemberAI.DryadPartyMemberAI(npc, prevPos, ProjectileID.ConfettiGun, ref initTownNPCStats);
                    break;

                default:
                    PartyMemberAI.GuidePartyMemberAI(npc, prevPos, ProjectileID.UnholyTridentFriendly, ref initTownNPCStats);
                    break;

            }
            if (npc.type == ModContent.NPCType<NPCs.TownNPCs.Neku>())
            {
                PartyMemberAI.GuidePartyMemberAI(npc, prevPos,ProjectileID.FallingStar, ref initTownNPCStats);
            }
            initTownNPCStats[8] = (initTownNPCStats[8]+1) % 40;

        }

        public override void AI(NPC npc)
        {
            if ((npc.aiStyle >= 107 && npc.aiStyle <= 111) || !npc.active)
            {
                return;
            }

            if (npc.townNPC)
            {
                partyOwner = PartyMemberLogic.IsPartyMember(npc.type);
            }


            if (npc.townNPC && partyOwner>=0)
            {
                npc.frameCounter = (npc.frameCounter + 1) % 40;
                if (npc.target <= 0)
                    npc.TargetClosest(false);
                npc.immortal = true;
                if (Vector2.Distance(npc.Center, Main.player[Main.myPlayer].Center)>750)
                {
                    npc.Teleport(Main.player[partyOwner].Center);
                }
                else
                {
                    if (Vector2.Distance(npc.Center, Main.player[partyOwner].Center) > 400) {
                        Vector2 vel = (Main.player[partyOwner].Center - npc.Center);
                        npc.direction = (int)(vel.X / MathHelp.Magnitude(vel));
                        npc.velocity =MathHelp.Normalize(vel)*Math.Max(5,MathHelp.Magnitude(Main.player[partyOwner].velocity));
                        npc.noTileCollide = true;
                    }
                    else
                    {
                        npc.noTileCollide = false;
                    }
                }
            }
            else
            {

                SoraPlayer sp = Main.player[npc.target].GetModPlayer<SoraPlayer>();
                switch (npc.type)
                {
                    //boss related
                    case NPCID.KingSlime:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] { new Conversation("You dare challenge the strongest of SLIMES?!", Color.Blue, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        if (sp.fightingInBattlegrounds)
                        {
                            if (!heartlessVerActive)
                            {
                                npc.damage = (int)(npc.damage * 1.3f);
                                npc.scale = npc.scale *= 0.5f;
                                heartlessVerActive = true;
                                npc.lifeMax *= 3;
                                npc.life = npc.lifeMax;
                                npc.lifeRegen = 1;
                            }

                            npc.reflectsProjectiles = npc.velocity.Y != 0;
                            npc.stairFall = false;

                            if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 550)
                            {
                                npc.Center = Main.player[npc.target].Center + new Vector2(0, -400);
                                npc.velocity.X = 0;
                            }

                            if (Main.expertMode)
                            {
                                npc.velocity.Y = (npc.velocity.Y > 0) ? npc.velocity.Y * 20 : npc.velocity.Y;
                            }

                            if (npc.life < npc.lifeMax / 4 * 3 && npc.target >= 0 && Main.player[npc.target].active && Main.time % 50 == 0)
                            {
                                int createdProj = Projectile.NewProjectile(s, Main.player[npc.target].Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.shadowCloneProjectile>(), 0, 10);

                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                            }
                        }
                        break;
                    case NPCID.SlimeSpiked:
                    case NPCID.BlueSlime:
                        if (NPC.AnyNPCs(NPCID.KingSlime))
                        {
                            if (Main.expertMode && sp.fightingInBattlegrounds && !heartlessVerActive)
                            {
                                npc.damage = (int)(npc.damage * 1.5f);
                                npc.scale *= 2;
                                heartlessVerActive = true;
                            }
                        }
                        else
                        {
                            if (heartlessVerActive)
                            {
                                npc.damage = (int)(npc.damage / 1.5f);
                                npc.scale /= 2;
                                heartlessVerActive = false;
                            }
                        }
                        break;

                    case NPCID.EyeofCthulhu:

                        if (sp.fightingInBattlegrounds)
                        {

                            if (npc.ai.Length < 6)
                            {
                                npc.ai = new float[6];
                            }
                            npc.ai[4]++;

                            if (npc.ai[4] % 15 == 0 && npc.life < npc.lifeMax / 2)
                            {
                                int createdProj = Projectile.NewProjectile(s,npc.Center - npc.velocity / npc.velocity.Length() * npc.width, new Vector2(), ProjectileID.DemonScythe, 15, 2);
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].timeLeft = 50;
                            }

                        }
                        break;

                    case NPCID.ServantofCthulhu:
                        if (sp.fightingInBattlegrounds)
                        {

                            if (!heartlessVerActive)
                            {
                                npc.alpha = 100;
                                if (Main.expertMode && sp.fightingInBattlegrounds)
                                {
                                    npc.damage = (int)(npc.damage * 1.5f);
                                    npc.scale = (int)(npc.scale * 1.5f);
                                }
                                else
                                {
                                    npc.scale = (int)(npc.scale * 1.2f);
                                }

                                heartlessVerActive = true;
                            }
                            if (Main.expertMode && sp.fightingInBattlegrounds)
                            {
                                npc.ai[0]++;
                                if (npc.ai[0] % 15 == 0)
                                {
                                    int createdProj = Projectile.NewProjectile(s,npc.Center - npc.velocity / npc.velocity.Length() * npc.width, new Vector2(), ProjectileID.DemonScythe, 15, 2);
                                    Main.projectile[createdProj].friendly = false;
                                    Main.projectile[createdProj].hostile = true;
                                    Main.projectile[createdProj].timeLeft = 50;
                                    Main.projectile[createdProj].scale /= 4;

                                    npc.velocity *= 2;
                                }
                            }
                        }
                        break;

                    case NPCID.EaterofWorldsHead:
                        if (sp.fightingInBattlegrounds)
                        {
                            if (npc.ai.Length < 7)
                            {
                                float[] prevAi = npc.ai;
                                npc.ai = new float[7];
                                for (int i = 0; i < prevAi.Length; i++)
                                {
                                    npc.ai[i] = prevAi[i];
                                }
                            }
                            npc.ai[6]++;

                            int firingTime = (sp.fightingInBattlegrounds && Main.expertMode) ? 5 : 1;
                            if (npc.ai[6] > 20 * firingTime && npc.ai[6] % 5 / firingTime == 0)
                            {
                                int createdProj = Projectile.NewProjectile(s,npc.Center, npc.velocity * 2, ProjectileID.CursedFlameHostile, npc.damage, 10);
                                Main.projectile[createdProj].alpha = 100;
                                Main.projectile[createdProj].friendly = false;
                                npc.ai[6] = (npc.ai[6] >= 250) ? 0 : npc.ai[6];
                            }
                        }
                        break;
                    case NPCID.EaterofWorldsBody:
                        int segments = NPC.CountNPCS(npc.type) + NPC.CountNPCS(NPCID.EaterofWorldsHead) + NPC.CountNPCS(NPCID.EaterofWorldsTail);
                        if (sp.fightingInBattlegrounds && Main.expertMode)
                        {
                            if (segments < 30)
                            {
                                if (npc.ai[0] % 20 == 0)
                                {
                                    int createdProj = Projectile.NewProjectile(s,npc.Center, Vector2.Zero, ProjectileID.CursedFlameHostile, npc.damage, 10);
                                    Main.projectile[createdProj].friendly = false;
                                    Main.projectile[createdProj].timeLeft = 20;
                                }
                            }
                        }
                        break;

                    case NPCID.SkeletronHead:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] { new Conversation("The curse spreads from the old man to you", Color.Black, DialogSystem.BOSS_DIALOGTIME) };
                            DialogSystem.AddConversation(conv);
                        }
                        break;
                    case NPCID.SkeletronHand:
                        break;


                    case NPCID.TheDestroyer:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] { new Conversation("REMOTE ACTIVATION DETECTED", Color.Red, DialogSystem.BOSS_DIALOGTIME, npc.FullName), new Conversation("ATTEMPTING IMMEDIATE DESTRUCTION", Color.Red, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        if (sp.fightingInBattlegrounds)
                        {
                            npc.ai[2]++;
                            if (npc.ai[2] % 150 == 0)
                            {
                                int createdProj = Projectile.NewProjectile(s,npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 15, ProjectileID.Electrosphere, npc.damage, 10);
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].timeLeft = 150;
                                Main.projectile[createdProj].scale = 0.2f;

                                npc.ai[2] = 0;
                            }
                        }
                        break;
                    case NPCID.Probe:
                        if (sp.fightingInBattlegrounds && Main.expertMode)
                        {
                            if (npc.ai[0] % 20 == 0)
                            {
                                int createdProj = Projectile.NewProjectile(s,npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 3, ProjectileID.Electrosphere, npc.damage, 10);
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].timeLeft = 20;
                            }
                        }
                        break;

                    case NPCID.Retinazer:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] { new Conversation("REMOTE ACTIVATION DETECTED", Color.Red, 300, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        if (sp.fightingInBattlegrounds)
                        {
                            if (!NPC.AnyNPCs(NPCID.Spazmatism))
                            {
                                npc.ai[2]++;
                                if (npc.ai[2] % 150 == 0)
                                {
                                    NPC.NewNPC(s,(int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
                                }
                                else if (npc.ai[2] % 150 == 100)
                                {
                                    NPC.NewNPC(s,(int)npc.Center.X, (int)npc.Center.Y, NPCID.Probe);
                                }
                            }
                        }
                        break;
                    case NPCID.Spazmatism:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] {  new Conversation("ATTEMPTING IMMEDIATE DESTRUCTION", Color.Red, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        if (sp.fightingInBattlegrounds)
                        {
                            if (!NPC.AnyNPCs(NPCID.Retinazer))
                            {
                                npc.ai[2]++;
                                if (npc.ai[2] % 150 == 0)
                                {
                                    NPC.NewNPC(s,(int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
                                }
                                if (npc.ai[2] % 150 == 100)
                                {
                                    NPC.NewNPC(s,(int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
                                }
                            }
                        }
                        break;

                    case NPCID.SkeletronPrime:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] { new Conversation("REMOTE ACTIVATION DETECTED", Color.Red, DialogSystem.BOSS_DIALOGTIME, npc.FullName) , new Conversation("ATTEMPTING IMMEDIATE DESTRUCTION", Color.Red, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        if (sp.fightingInBattlegrounds)
                        {
                            if (!heartlessVerActive)
                            {
                                //npc.scale = (int)(npc.scale * 0.8f);
                                npc.damage = npc.damage * 4;
                                heartlessVerActive = true;
                            }
                            if (npc.ai[0] % 10 == 0)
                            {
                                int createdProj = Projectile.NewProjectile(s,npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 3, ProjectileID.Skull, npc.damage, 10);
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].timeLeft = 20;
                            }
                        }
                        break;
                    case NPCID.PrimeCannon:
                        if (sp.fightingInBattlegrounds)
                        {
                            if (!heartlessVerActive)
                            {
                                //npc.scale = (int)(npc.scale * 0.75f);
                                npc.damage = (int)(npc.damage * 1.25f);
                                heartlessVerActive = true;

                                proj = Projectile.NewProjectile(s,npc.Center, Vector2.Zero, ProjectileID.Bubble, 0, 0);
                                Main.projectile[proj].friendly = false;
                                Main.projectile[proj].hostile = true;
                            }
                            Main.projectile[proj].scale = 100;
                            Main.projectile[proj].rotation = npc.rotation;
                            Main.projectile[proj].Center = npc.Center;
                            Main.projectile[proj].timeLeft = 5;
                            Main.projectile[proj].tileCollide = false;
                            Main.projectile[proj].penetrate = -1;
                            npc.life = npc.lifeMax;
                        }
                        break;
                    case NPCID.PrimeLaser:
                        if (sp.fightingInBattlegrounds)
                        {
                            if (!heartlessVerActive)
                            {
                                //npc.scale = (int)(npc.scale * 0.75f);
                                npc.damage = (int)(npc.damage * 1.25f);
                                heartlessVerActive = true;

                                proj = Projectile.NewProjectile(s,npc.Center, Vector2.Zero, ProjectileID.Bubble, 0, 0);
                                Main.projectile[proj].friendly = false;
                                Main.projectile[proj].hostile = true;
                            }
                            Main.projectile[proj].scale = 100;
                            Main.projectile[proj].rotation = npc.rotation;
                            Main.projectile[proj].Center = npc.Center;
                            Main.projectile[proj].timeLeft = 5;
                            Main.projectile[proj].tileCollide = false;
                            Main.projectile[proj].penetrate = -1;
                            npc.life = npc.lifeMax;
                        }
                        break;
                    case NPCID.PrimeSaw:
                        if (sp.fightingInBattlegrounds)
                        {
                            if (!heartlessVerActive)
                            {
                                //npc.scale = (int)(npc.scale * 0.75f);
                                npc.damage = (int)(npc.damage * 1.75f);
                                heartlessVerActive = true;

                                proj = Projectile.NewProjectile(s,npc.Center, Vector2.Zero, ProjectileID.Bubble, 0, 0);
                                Main.projectile[proj].friendly = false;
                                Main.projectile[proj].hostile = true;
                            }
                            Main.projectile[proj].scale = 100;
                            Main.projectile[proj].rotation = npc.rotation;
                            Main.projectile[proj].Center = npc.Center;
                            Main.projectile[proj].timeLeft = 5;
                            Main.projectile[proj].tileCollide = false;
                            Main.projectile[proj].penetrate = -1;
                            npc.life = npc.lifeMax;
                        }
                        break;
                    case NPCID.PrimeVice:
                        if (sp.fightingInBattlegrounds)
                        {
                            if (!heartlessVerActive)
                            {
                                //npc.scale = (int)(npc.scale * 0.75f);
                                npc.damage = (int)(npc.damage * 1.25f);
                                heartlessVerActive = true;

                                proj = Projectile.NewProjectile(s,npc.Center, Vector2.Zero, ProjectileID.Bubble, 0, 0);
                                Main.projectile[proj].friendly = false;
                                Main.projectile[proj].hostile = true;
                            }
                            Main.projectile[proj].scale = 100;
                            Main.projectile[proj].rotation = npc.rotation;
                            Main.projectile[proj].Center = npc.Center;
                            Main.projectile[proj].timeLeft = 5;
                            Main.projectile[proj].tileCollide = false;
                            Main.projectile[proj].penetrate = -1;
                            npc.life = npc.lifeMax;
                        }
                        break;

                    case NPCID.Plantera:
                        if (sp.fightingInBattlegrounds)
                        {
                            if (!heartlessVerActive)
                            {
                                npc.damage *= 2;
                                heartlessVerActive = true;
                            }
                        }
                        break;
                    case NPCID.PlanterasTentacle:
                        if (sp.fightingInBattlegrounds)
                        {
                            if (!heartlessVerActive)
                            {
                                npc.damage *= 2;
                                heartlessVerActive = true;
                            }

                            if (npc.ai[0] % 50 == 0)
                            {
                                int createdProj = Projectile.NewProjectile(s,npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 3, ProjectileID.SporeCloud, npc.damage/2, 10);
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].timeLeft = 50;
                            }

                        }
                        break;

                    case NPCID.Golem:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] { new Conversation("Profanity inside the temple detected", Color.Brown, DialogSystem.BOSS_DIALOGTIME, npc.FullName), new Conversation("Destruction of Terrarian started...", Color.Brown, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }

                        if (sp.fightingInBattlegrounds)
                        {
                            if (!heartlessVerActive)
                            {
                                heartlessVerActive = true;
                                npc.scale = (int)(npc.scale *1.2f);
                                npc.damage = (int)(npc.damage * 2.5f);
                            }
                            if (npc.life < npc.lifeMax / 4 * 3 && npc.target >= 0 && Main.player[npc.target].active && Main.time % 20 == 0)
                            {
                                int createdProj = Projectile.NewProjectile(s, Main.player[npc.target].Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.shadowCloneProjectile>(), 0, 10);

                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                            }
                        }
                        break;

                    case NPCID.DukeFishron:
                        if (sp.fightingInBattlegrounds)
                        {
                            if (!heartlessVerActive)
                            {
                                heartlessVerActive = true;
                                //npc.scale = (int)(npc.scale *0.9f);
                                npc.damage = (int)(npc.damage * 1.5f);
                            }

                            if (npc.life < npc.lifeMax / 4 && npc.ai[1]%20==0)
                            {
                                int createdProj = Projectile.NewProjectile(s,npc.Center, Vector2.Zero, ProjectileID.Bubble, npc.damage , 10);
                                Main.projectile[createdProj].scale*=2;
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].timeLeft = 40;
                            }

                        }
                        break;

                    case NPCID.CultistBoss:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] { new Conversation("You killed my brothers", Color.LightBlue, DialogSystem.BOSS_DIALOGTIME, npc.FullName), new Conversation("The end is near anyways... so just die", Color.LightBlue, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        break;

                    case NPCID.LunarTowerNebula:
                        if (!heartlessVerActive)
                        {
                            prevPos = npc.Center;
                            heartlessVerActive = true;
                        }
                        if (Vector2.Distance(Main.player[npc.target].Center, prevPos) < (npc.height + npc.width) * 6)
                            npc.Center = Main.player[npc.target].Center - new Vector2(0, npc.height);
                        else
                            npc.Center = prevPos;
                        break;
                    case NPCID.LunarTowerSolar:
                        if (!heartlessVerActive)
                        {
                            prevPos = npc.Center;
                            heartlessVerActive = true;
                        }
                        if (Vector2.Distance(Main.player[npc.target].Center, prevPos) < (npc.height + npc.width) * 6)
                            npc.Center = Main.player[npc.target].Center - new Vector2(0, npc.height);
                        else
                            npc.Center = prevPos;
                        break;
                    case NPCID.LunarTowerStardust:
                        if (!heartlessVerActive)
                        {
                            prevPos = npc.Center;
                            heartlessVerActive = true;
                        }
                        if (Vector2.Distance(Main.player[npc.target].Center, prevPos) < (npc.height + npc.width) * 6)
                            npc.Center = Main.player[npc.target].Center - new Vector2(0, npc.height);
                        else
                            npc.Center = prevPos;
                        break;
                    case NPCID.LunarTowerVortex:
                        if (!heartlessVerActive)
                        {
                            prevPos = npc.Center;
                            heartlessVerActive = true;
                        }
                        if (Vector2.Distance(Main.player[npc.target].Center, prevPos) < (npc.height + npc.width)*6)
                            npc.Center = Main.player[npc.target].Center - new Vector2(0, npc.height);
                        else if(npc.Center!=prevPos)
                            npc.Teleport(prevPos);
                        break;

                    case NPCID.MoonLordCore:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] { new Conversation("TIME TO DIE!!!", Color.LightBlue, DialogSystem.BOSS_DIALOGTIME, "Moon Lord") };
                            DialogSystem.AddConversation(conv);
                        }
                        break;
                    case NPCID.MoonLordHand:
                        break;
                    case NPCID.MoonLordHead:
                        break;
                    case NPCID.MoonLordFreeEye:
                        break;

                    //Mob related
                    case NPCID.MeteorHead:
                        if (Main.expertMode)
                        {
                            npc.color = Color.Purple;
                            if (npc.ai.Length < 3)
                            {
                                npc.ai = new float[4];
                            }
                            npc.ai[3]++;
                            if (npc.ai[3] % 15 == 0)
                            {
                                int createdProj = Projectile.NewProjectile(s,npc.Center, new Vector2(), ProjectileID.DemonSickle, 5, 2);
                                Main.projectile[createdProj].timeLeft = 150;
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].scale = 0.5f;
                            }
                        }
                        break;
                }

                if (npc.type == ModContent.NPCType<NPCs.Bosses.heartlessXeanorth>())
                {
                    if (sp.fightingInBattlegrounds)
                    {
                        if (!heartlessVerActive)
                        {
                            heartlessVerActive = true;
                            npc.lifeMax =(int)(npc.lifeMax* 1.1f);
                            npc.life = npc.lifeMax;
                            npc.damage = (int)(npc.damage * 1.5f);
                        }

                        if (npc.life < npc.lifeMax / 4*3 && npc.target>=0 && Main.player[npc.target].active && Main.time%20==0)
                        {
                            int createdProj = Projectile.NewProjectile(s, Main.player[npc.target].Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.shadowCloneProjectile>(), 0, 10);

                            Main.projectile[createdProj].friendly = false;
                            Main.projectile[createdProj].hostile = true;
                        }

                    }
                }

                base.AI(npc);
            }
            if (npc.townNPC)
                prevPos = npc.Center;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if ((npc.aiStyle >= 107 && npc.aiStyle<=111)|| !npc.active)
            {
                return;
            }
            SoraPlayer sp = Main.player[npc.target].GetModPlayer<SoraPlayer>();
            if (npc.boss && sp.fightingInBattlegrounds)
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("KingdomTerrahearts/NPCs/Bosses/HeartlessSigil");

                Vector2 symbolPos = new Vector2(npc.Center.X - Main.screenPosition.X - npc.width * 0.25f + texture.Width * 0.5f, npc.Center.Y - Main.screenPosition.Y - npc.height * 0.25f + texture.Height * 0.5f);
                switch (npc.type)
                {
                    case NPCID.EyeofCthulhu:
                        symbolPos = new Vector2
                    (npc.Center.X - Main.screenPosition.X - npc.width * 0.25f + texture.Width * 0.5f + 2f, npc.Center.Y - Main.screenPosition.Y - npc.height * 0.25f + texture.Height * 0.5f + 2f);
                        break;
                    case NPCID.QueenBee:
                        symbolPos = new Vector2(npc.Center.X - Main.screenPosition.X - npc.width * 0.25f + texture.Width * 0.5f + 2f, npc.Center.Y - Main.screenPosition.Y - npc.height * 0.25f + texture.Height * 0.5f + 2f);
                        break;
                    default:
                        symbolPos = new Vector2(npc.Center.X - Main.screenPosition.X - npc.width * 0.25f + texture.Width * 0.5f + 2f, npc.Center.Y - Main.screenPosition.Y - npc.height * 0.25f + texture.Height * 0.5f + 2f);
                        break;
                }
                if (npc.type == ModContent.NPCType<NPCs.Bosses.Darkside>())
                    symbolPos = new Vector2(npc.Center.X - Main.screenPosition.X - npc.width * 0.175f + texture.Width * 0.5f + 0f, npc.Center.Y - Main.screenPosition.Y - npc.height * 0.5f + texture.Height * 0.5f - 15f);
                else if (npc.type == ModContent.NPCType<NPCs.Bosses.Org13.xion_finalPhase>())
                    symbolPos = new Vector2(npc.Center.X - Main.screenPosition.X - npc.width * 0.125f / 2f + texture.Width * 0.5f + 2f, npc.Center.Y - Main.screenPosition.Y - npc.height * 0.125f + texture.Height * 0.5f + 2f);

                spriteBatch.Draw
                (
                    texture,
                    symbolPos,
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White,
                    npc.rotation,
                    texture.Size() * 0.5f,
                    npc.scale / 2,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        public override bool CheckDead(NPC npc)
        {
            SoraPlayer sp= Main.player[Main.myPlayer].GetModPlayer<SoraPlayer>();
            DeathConversation(npc);

            if (sp.isBoss(npc.whoAmI)){
                CheckBattlegroundDrops(npc.whoAmI, sp);
            }
            if (npc.type == NPCID.EaterofWorldsHead)
            {
                if (NPC.CountNPCS(NPCID.EaterofWorldsHead) <= 1 && !NPC.AnyNPCs(NPCID.EaterofWorldsBody))
                {
                    Item.NewItem(s,npc.Center, npc.width, npc.height, ItemID.WormFood, Stack: 5, noGrabDelay: true);
                    Item.NewItem(s,npc.Center, npc.width, npc.height, ModContent.ItemType<Items.Materials.pulsingShard>(), Stack: Main.rand.Next(10, 15), noGrabDelay: true);
                }
            }

            if (partyOwner>=0)
            {
                npc.life = npc.lifeMax;
                npc.Teleport(new Vector2(npc.homeTileX, npc.homeTileY));
                partyOwner = -1;
                return false;
            }

            int proj;

            switch (npc.type)
            {
                case NPCID.PrimeCannon:
                    proj = Projectile.NewProjectile(s,npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 5, ProjectileID.Bone,npc.damage * 3, 1);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].timeLeft = 1000;
                    break;
                case NPCID.PrimeLaser:
                    proj = Projectile.NewProjectile(s,npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 5, ProjectileID.Bone, npc.damage * 3, 1);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].timeLeft = 1000;
                    break;
                case NPCID.PrimeSaw:
                    proj = Projectile.NewProjectile(s,npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 5, ProjectileID.Bone, npc.damage*3, 1);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].timeLeft = 1000;
                    break;
                case NPCID.PrimeVice:
                    proj = Projectile.NewProjectile(s,npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 5, ProjectileID.Bone, npc.damage * 3, 1);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].timeLeft = 1000;
                    break;
                case NPCID.MeteorHead:
                    Item.NewItem(s,npc.getRect(), ItemID.Meteorite, Main.rand.Next(1, 5));
                    Item.NewItem(s,(int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Materials.blazingShard>(), Stack: Main.rand.Next(14) + 1);
                    break;
                case NPCID.Spore:
                    if (Main.player[npc.target].GetModPlayer<SoraPlayer>().fightingInBattlegrounds)
                    {
                        proj = Projectile.NewProjectile(s,npc.Center, npc.velocity, ProjectileID.SporeCloud, npc.damage * 2, 1);
                        Main.projectile[proj].friendly = false;
                        Main.projectile[proj].hostile = true;
                        Main.projectile[proj].timeLeft = 50;
                    }
                    break;
            }
            if (npc.type == ModContent.NPCType<NPCs.RedNocturne>() && NPC.TowerActiveSolar)
            {
                Player p = Main.player[npc.target];
                NPC towerSolar = FindNPCType(NPCID.LunarTowerSolar);
                if(towerSolar!=null)
                    Projectile.NewProjectile(s,p.Center, towerSolar.Center, ProjectileID.TowerDamageBolt, 100, 0,npc.target,ai1: 100);
                NPC.ShieldStrengthTowerSolar -= (NPC.ShieldStrengthTowerSolar>10)?10:0;
            }

            //When an NPC (from the invasion list) dies, add progress by decreasing size
            if (KingdomWorld.customInvasionUp)
            {
                //Gets IDs of invaders from CustomInvasion file
                foreach (int heartless in ThousandHeartlessInvasion.heartless)
                {
                    //If npc type equal to invader's ID decrement size to progress invasion
                    if (npc.type == heartless)
                    {
                        Main.invasionSize -= 1;
                    }
                }
                //Gets IDs of invaders from CustomInvasion file
                foreach (int heartless in ThousandHeartlessInvasion.heartlessBosses)
                {
                    //If npc type equal to invader's ID decrement size to progress invasion
                    if (npc.type == heartless)
                    {
                        Main.invasionSize -= 15;
                    }
                }
            }

            return base.CheckDead(npc);
        }

        public void DeathConversation(NPC npc)
        {
            Conversation[] conv;
            switch (npc.type)
            {

                //Bosses

                case NPCID.KingSlime:

                    conv = new Conversation[] { new Conversation("The strongest slime has failed!", Color.Blue, DialogSystem.BOSS_DIALOGTIME, npc.FullName), new Conversation("How could... this... be...", Color.Blue, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.SkeletronHead:
                    conv = new Conversation[] { new Conversation("The curse disperses", Color.Black, DialogSystem.BOSS_DIALOGTIME) };
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                    conv = new Conversation[] { new Conversation("DESTRUCTI()N.... F41|3D", Color.Red, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                    DialogSystem.AddConversation(conv);
                    break;
                case NPCID.TheDestroyer:
                    conv = new Conversation[] { new Conversation("DES1RU(11ON /|TTEMP1 FA1....", Color.Red, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.SkeletronPrime:
                    conv = new Conversation[] { new Conversation("|)ESTRUCTI()N ATT#MPT FA....", Color.Red, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.Golem:
                    conv = new Conversation[] { new Conversation("Destruction of Terrarian failed...", Color.Brown, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.CultistBoss:
                    conv = new Conversation[] { new Conversation("May the Lord of the Moon Destroy you...!", Color.LightBlue, DialogSystem.BOSS_DIALOGTIME, npc.FullName) };
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.MoonLordCore:

                    conv = new Conversation[] { new Conversation("IMPOSIBLE!!!", Color.LightBlue, DialogSystem.BOSS_DIALOGTIME, "Moon Lord") };
                    DialogSystem.AddConversation(conv);
                    break;

            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {

            SoraPlayer sp = Main.player[npc.target].GetModPlayer<SoraPlayer>();

            switch (npc.type)
            {
                //Bosses
                case NPCID.KingSlime:

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Custom.Keyblade_Slime>(), 3));
                    npcLoot.Add(ItemDropRule.Common(ItemID.SlimeCrown, 1, 15, 15));
                    break;
                case NPCID.QueenBee:

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Keyblade_Honey>(), 5));
                    npcLoot.Add(ItemDropRule.Common(ItemID.Abeemination, 1, 15, 15));
                    npcLoot.Add(ItemDropRule.Common(ItemID.Stinger, 1, 15, 45));

                    break;
                case NPCID.EyeofCthulhu:

                    npcLoot.Add(ItemDropRule.Common(ItemID.SuspiciousLookingEye, 1, 15, 15));
                    break;

                case NPCID.Deerclops:
                    npcLoot.Add(ItemDropRule.Common(ItemID.DeerThing, 1, 15, 15));
                    break;

                case NPCID.EaterofWorldsHead:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingShard>(),1,2,5));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingStone>(),3,1,3));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingShard>(),7));
                    break;
                case NPCID.BrainofCthulhu:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingShard>(), 1, 2, 5));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingStone>(), 3, 1, 3));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingShard>(), 7));
                    break;
                case NPCID.SkeletronHead:

                    npcLoot.Add(ItemDropRule.Common(ItemID.ClothierVoodooDoll));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.twilightShard>(), 1, 10, 15));
                    npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 10, 15));
                    break;
                case NPCID.SkeletronHand:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 5, 10));
                    break;
                case NPCID.WallofFlesh:

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.mythrilStone>(), 1, 10, 15));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.mythrilGem>(), 5, 1, 3));

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.blazingGem>(), 5, 1, 3));

                    npcLoot.Add(ItemDropRule.Common(ItemID.GuideVoodooDoll));
                    break;
                case NPCID.Retinazer:
                case NPCID.Spazmatism:

                    npcLoot.Add(ItemDropRule.ByCondition(new Conditions.MissingTwin(), ModContent.ItemType<Items.Materials.betwixtShard>(), 1, 10, 20));
                    npcLoot.Add(ItemDropRule.ByCondition(new Conditions.MissingTwin(), ModContent.ItemType<Items.Materials.betwixtStone>(), 1, 1, 5));
                    npcLoot.Add(ItemDropRule.ByCondition(new Conditions.MissingTwin(), ModContent.ItemType<Items.Materials.betwixtGem>(), 5, 1, 3));

                    npcLoot.Add(ItemDropRule.ByCondition(new Conditions.MissingTwin(), ItemID.MechanicalEye, 1, 10, 15));
                    break;
                case NPCID.TheDestroyer:

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.mythrilShard>(), 1, 10, 15));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningShard>(), 1, 10, 15));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.betwixtCrystal>(), 3));
                    npcLoot.Add(ItemDropRule.Common(ItemID.MechanicalWorm, 1, 10, 15));
                    break;
                case NPCID.Probe:
                    npcLoot.Add(ItemDropRule.Common(ItemID.IronBar, 1, 1, 2));
                    break;
                case NPCID.SkeletronPrime:

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.mythrilShard>(), 1, 10, 15));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningShard>(), 1, 10, 15));
                    npcLoot.Add(ItemDropRule.Common(ItemID.MechanicalSkull, 1, 10, 15));
                    break;
                case NPCID.Plantera:

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingStone>(), 1, 5, 7));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingGem>(), 5, 1, 3));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.PlanteraFlower>(), 1, 10, 15));
                    break;
                case NPCID.PlanterasTentacle:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingShard>(), 1, 1, 2));
                    break;
                case NPCID.Golem:

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.mythrilShard>(), 1, 10, 15));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.mythrilStone>(), 1, 1, 5));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.mythrilGem>(), 2, 1, 3));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.mythrilCrystal>(), 5));
                    npcLoot.Add(ItemDropRule.Common(ItemID.LihzahrdPowerCell, 1, 10, 15));
                    break;
                case NPCID.CultistBoss:

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningShard>(), 1, 10, 15));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningStone>(), 1, 5, 7));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningGem>(), 1, 1, 3));

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingGem>(), 5, 1, 3));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingCrystal>(), 5, 1, 2));

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.frostGem>(), 5, 1, 3));

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.blazingGem>(), 5, 1, 3));
                    break;

                case NPCID.DukeFishron:

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Custom.Keyblade_DukeFish>(), 2));
                    npcLoot.Add(ItemDropRule.Common(ItemID.TruffleWorm, 1, 10, 15));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningGem>(), 1, 5, 10));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningCrystal>(), 2));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.writhingGem>(), 1, 5, 10));
                    break;

                case NPCID.QueenSlimeBoss:
                    npcLoot.Add(ItemDropRule.Common(ItemID.QueenSlimeCrystal, 1, 10, 15));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lucidStone>(), 1, 3, 5));
                    break;

                case NPCID.HallowBoss:
                    npcLoot.Add(ItemDropRule.Common(ItemID.EmpressButterfly, 1, 10, 15));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lucidGem>(), 1, 5, 10));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lucidCrystal>(), 1, 1, 3));
                    break;

                case NPCID.MoonLordCore:

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Custom.Keyblade_MoonLord>(), 2));
                    npcLoot.Add(ItemDropRule.Common(ItemID.CelestialSigil, 1, 10, 15));

                    npcLoot.Add(ItemDropRule.Common(ItemID.FragmentNebula, 1, 50, 50));
                    npcLoot.Add(ItemDropRule.Common(ItemID.FragmentSolar, 1, 50, 50));
                    npcLoot.Add(ItemDropRule.Common(ItemID.FragmentStardust, 1, 50, 50));
                    npcLoot.Add(ItemDropRule.Common(ItemID.FragmentVortex, 1, 50, 50));

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.OrichalchumPlus>()));

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningCrystal>(), 1, 2, 5));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingCrystal>(), 1, 2, 5));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.frostCrystal>(), 1, 2, 5));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.blazingCrystal>(), 1, 2, 5));

                    break;
                case NPCID.MoonLordHead:
                case NPCID.MoonLordHand:
                    npcLoot.Add(ItemDropRule.Common(ItemID.LunarOre));
                    break;

                //Dungeon Defenders
                case NPCID.DD2Betsy:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Betsys_HatchingEgg>(),1,5,10));
                    npcLoot.Add(ItemDropRule.Common(ItemID.DefenderMedal, 1, 50, 75));

                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.blazingGem>(),1,5,10));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.blazingCrystal>(),1,1,3));
                    break;

                //Pilars

                //Vortex
                case NPCID.LunarTowerVortex:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningShard>(), 1, 30, 50));
                    npcLoot.Add(ItemDropRule.Common(ItemID.SniperScope));
                    break;

                case NPCID.VortexHornet:
                case NPCID.VortexHornetQueen:
                case NPCID.VortexLarva:
                case NPCID.VortexRifleman:
                case NPCID.VortexSoldier:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningShard>(), 3, 1, 3));
                    npcLoot.Add(ItemDropRule.Common(ItemID.FragmentVortex, 1, 1, 2));
                    break;

                //Solar
                case NPCID.LunarTowerSolar:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.blazingShard>(), 1, 30, 50));
                    npcLoot.Add(ItemDropRule.Common(ItemID.MoneyTrough));
                    break;

                case NPCID.SolarCorite:
                case NPCID.SolarCrawltipedeHead:
                case NPCID.SolarDrakomire:
                case NPCID.SolarDrakomireRider:
                case NPCID.SolarSpearman:
                case NPCID.SolarSolenian:
                case NPCID.SolarSroller:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.blazingShard>(), 3, 1, 3));
                    npcLoot.Add(ItemDropRule.Common(ItemID.FragmentSolar, 1, 1, 3));
                    break;

                //Nebula
                case NPCID.LunarTowerNebula:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lucidShard>(), 1, 30, 50));
                    npcLoot.Add(ItemDropRule.Common(ItemID.RodofDiscord));
                    break;

                case NPCID.NebulaBrain:
                case NPCID.NebulaBeast:
                case NPCID.NebulaHeadcrab:
                case NPCID.NebulaSoldier:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lucidShard>(), 3, 1, 3));
                    npcLoot.Add(ItemDropRule.Common(ItemID.FragmentNebula, 1, 1, 3));
                    break;

                //Stardust
                case NPCID.LunarTowerStardust:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.twilightShard>(), 1, 30, 50));
                    npcLoot.Add(ItemDropRule.Common(ItemID.CelestialShell));
                    break;

                case NPCID.StardustJellyfishBig:
                case NPCID.StardustSoldier:
                case NPCID.StardustCellBig:
                case NPCID.StardustSpiderBig:
                case NPCID.StardustWormHead:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.twilightShard>(), 3, 1, 3));
                    npcLoot.Add(ItemDropRule.Common(ItemID.FragmentStardust, 1, 1, 3));
                    break;

                //Meteorite

                //Shards

                //dense
                case NPCID.Skeleton:
                case NPCID.Paladin:
                case NPCID.Mimic:
                case NPCID.PossessedArmor:
                case NPCID.Wraith:
                case NPCID.WanderingEye:
                case NPCID.Reaper:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.betwixtShard>(), 2, 1, 3));
                    break;

                //frost
                case NPCID.IceSlime:
                case NPCID.IceBat:
                case NPCID.IceElemental:
                case NPCID.IceGolem:
                case NPCID.UndeadViking:
                case NPCID.ArmoredViking:
                case NPCID.IceTortoise:
                case NPCID.IcyMerman:
                case NPCID.Wolf:
                case NPCID.SnowFlinx:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.frostShard>(), 2, 1, 3));
                    break;

                //power
                case NPCID.JungleBat:
                case NPCID.JungleSlime:
                case NPCID.Hornet:
                case NPCID.MossHornet:
                case NPCID.Bee:
                case NPCID.BeeSmall:
                case NPCID.AngryTrapper:
                case NPCID.Derpling:
                case NPCID.GiantTortoise:
                case NPCID.GiantFlyingFox:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.mythrilShard>(), 2, 1, 3));
                    break;

                //pulsing
                case NPCID.EaterofSouls:
                case NPCID.CorruptSlime:
                case NPCID.Corruptor:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.pulsingShard>(), 2, 1, 3));
                    break;

                //thunder
                case NPCID.AngryNimbus:
                case NPCID.GreenJellyfish:
                case NPCID.GreekSkeleton:
                case NPCID.Medusa:
                case NPCID.Piranha:
                case NPCID.Nymph:
                case NPCID.UndeadMiner:
                case NPCID.Vulture:
                case NPCID.BlueJellyfish:
                case NPCID.MartianSaucer:
                case NPCID.MartianWalker:
                case NPCID.MartianDrone:
                case NPCID.MartianTurret:
                case NPCID.GigaZapper:
                case NPCID.Scutlix:
                case NPCID.RuneWizard:
                case NPCID.WyvernHead:
                case NPCID.Frankenstein:
                case NPCID.DeadlySphere:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningShard>(), 2, 1, 3));
                    break;

                //twilight
                case NPCID.IlluminantSlime:
                case NPCID.Pixie:
                case NPCID.Unicorn:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.twilightShard>(), 2, 1, 3));
                    break;

                //blazing

                //Crimson
                case NPCID.Crimera:
                case NPCID.FaceMonster:
                case NPCID.Crimslime:
                //Hell
                case NPCID.Demon:
                case NPCID.RedDevil:
                case NPCID.FireImp:
                case NPCID.LavaSlime:
                case NPCID.Lavabat:
                case NPCID.Hellbat:
                //Other
                case NPCID.MeteorHead:
                    break;
                case NPCID.HellArmoredBones:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.blazingShard>(), 2, 1, 3));
                    break;
            }
        }

        public void CheckBattlegroundDrops(int NPC, SoraPlayer sp)
        {
            NPC npc = Main.npc[NPC];
            EntitySource_Parent s = new EntitySource_Parent(npc);
            if (sp.fightingInBattlegrounds)
            {

                switch (npc.type)
                {
                    case NPCID.MoonLordCore:
                        Item.NewItem(s,npc.Center, npc.width, npc.height, ModContent.ItemType<Items.ContributorItems.ScepTendo_gun>(), noGrabDelay: true);
                        break;
                    case NPCID.DukeFishron:
                        Item.NewItem(s,npc.Center, npc.width, npc.height, ItemID.BottomlessBucket, noGrabDelay: true);
                        Item.NewItem(s,npc.Center, npc.width, npc.height, ItemID.SuperAbsorbantSponge, noGrabDelay: true);
                        Item.NewItem(s,npc.Center, npc.width, npc.height, ItemID.BottomlessLavaBucket, noGrabDelay: true);
                        Item.NewItem(s,npc.Center, npc.width, npc.height, ItemID.LavaAbsorbantSponge, noGrabDelay: true);
                        break;
                    case NPCID.CultistBoss:
                        Item.NewItem(s,npc.Center, npc.width, npc.height, ItemID.SolarTablet, Stack: 5, noGrabDelay: true);
                        break;
                    case NPCID.Golem:
                        Item.NewItem(s,npc.Center, npc.width, npc.height, ItemID.CellPhone, noGrabDelay: true);
                        break;
                    case NPCID.Plantera:
                        Item.NewItem(s,npc.Center, npc.width, npc.height, ModContent.ItemType<Items.KairiHeart>(), noGrabDelay: true);
                        break;
                    case NPCID.SkeletronPrime:
                        Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.RocketLauncher, noGrabDelay: true);
                        Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.RocketIII, Stack: Main.rand.Next(20, 50), noGrabDelay: true);
                        break;
                    case NPCID.TheDestroyer:
                        Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.ActuationRod, noGrabDelay: true);
                        Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.Actuator, Stack: Main.rand.Next(20, 100), noGrabDelay: true);
                        break;
                    case NPCID.Retinazer:
                        if (!Terraria.NPC.AnyNPCs(NPCID.Spazmatism))
                        {
                            Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.LaserMachinegun, noGrabDelay: true);
                        }
                        break;
                    case NPCID.Spazmatism:
                        if (!Terraria.NPC.AnyNPCs(NPCID.Retinazer))
                        {
                            Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.LaserMachinegun, noGrabDelay: true);
                        }
                        break;
                    case NPCID.WallofFlesh:
                        Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.TitaniumBar, Stack: Main.rand.Next(80, 120), noGrabDelay: true);
                        break;
                    case NPCID.SkeletronHead:
                        Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.RedHat, noGrabDelay: true);
                        break;
                    case NPCID.EaterofWorldsHead:
                        if (Terraria.NPC.CountNPCS(NPCID.EaterofWorldsHead) <= 1 && !Terraria.NPC.AnyNPCs(NPCID.EaterofWorldsBody))
                        {
                            Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.WormholePotion, Stack: 10, noGrabDelay: true);
                        }
                        break;
                    case NPCID.EyeofCthulhu:
                        Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.TheEyeOfCthulhu, noGrabDelay: true);
                        break;
                    case NPCID.QueenBee:
                        Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.HoneyBucket, noGrabDelay: true);
                        Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.HoneyRocket, Stack: 66, noGrabDelay: true);
                        break;
                    case NPCID.KingSlime:
                        Item.NewItem(s, npc.Center, npc.width, npc.height, ItemID.ShinyRedBalloon, noGrabDelay: true);
                        break;
                }

            }
        }

        public NPC FindNPCType(int npctype)
        {
            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == npctype)
                {
                    return Main.npc[i];
                }
            }
            return null;
        }

        public override void TownNPCAttackCooldown(NPC npc, ref int cooldown, ref int randExtraCooldown)
        {
            initTownNPCStats[1] = (initTownNPCStats[1] == -1) ? cooldown: initTownNPCStats[1];
            initTownNPCStats[2] = (initTownNPCStats[2] == -1) ? randExtraCooldown : initTownNPCStats[2];
            if (npc.townNPC)
            {
                if (partyOwner>=0)
                {
                    cooldown = 1;
                    randExtraCooldown = 0;
                }
                else
                {
                    cooldown = initTownNPCStats[1];
                    randExtraCooldown = initTownNPCStats[2];
                }
            }
        }

        public override void TownNPCAttackProj(NPC npc, ref int projType, ref int attackDelay)
        {
            if (npc.townNPC)
            {
                switch (npc.type)
                {
                    case NPCID.Guide:
                        projType = (Main.hardMode)?ProjectileID.HolyArrow:projType;
                        break;
                }
            }
        }

        public override void FindFrame(NPC npc, int frameHeight)
        {
            base.FindFrame(npc, frameHeight);

            if (npc.townNPC && partyOwner >= 0)
            {
                if (initTownNPCStats[4] > 0 && initTownNPCStats[5] >= 0)
                {
                    npc.frame.Y = frameHeight * initTownNPCStats[7];
                    initTownNPCStats[4]--;
                }else if (Main.npc[initTownNPCStats[3]].active && initTownNPCStats[3] != 0 && initTownNPCStats[5] >= 0)
                {
                    npc.direction = (int)MathHelp.Normalize(Main.npc[initTownNPCStats[3]].Center - npc.Center).X;
                    npc.frame.Y = frameHeight * initTownNPCStats[7];
                }
                else
                {
                    npc.direction = (npc.velocity.X == 0) ? npc.spriteDirection : (int)MathHelp.Sign(npc.velocity.X);
                    if (Math.Abs(npc.velocity.X) > 0f)
                    {
                        npc.frame.Y = frameHeight * (
                            (initTownNPCStats[8] < 10 || (initTownNPCStats[8] > 20 && initTownNPCStats[8] < 30)) ? 2
                            : (initTownNPCStats[8] > 25 ? 5 : 12)) ;
                    }
                    else
                    {
                        npc.frame.Y = 0;
                    }
                    if (Math.Abs(npc.velocity.Y) > 0.4f)
                    {
                        npc.frame.Y = frameHeight;
                    }
                }
                npc.frame.Y = Math.Clamp(npc.frame.Y, 0, npc.frame.Height *
                (Main.npcFrameCount[npc.type] - 2));
            }
        }

    }
}
