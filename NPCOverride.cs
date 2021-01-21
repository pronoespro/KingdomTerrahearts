using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomTerrahearts.NPCs.Invasions;
using KingdomTerrahearts.CustomTownNPCAI;
using System.Collections.Generic;
using KingdomTerrahearts.Extra;
using System;

namespace KingdomTerrahearts
{
    class NPCOverride : GlobalNPC
    {

        public override bool InstancePerEntity => true;

        //partymember stuff
        public bool isPartyMember;
        public Vector2 prevPos=new Vector2();

        int initDamage;
        int[] initTownNPCStats = new int[]{-1,-1,0,0,-1,-1};

        //Get npc stats
        public override void SetDefaults(NPC npc)
        {
            base.SetDefaults(npc);
            if (npc.townNPC)
                initDamage = npc.damage;
            
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
                    pool.Add(i, 1f); //Add it to the pool with the same weight of 1
                }
                foreach (int i in ThousandHeartlessInvasion.heartlessBosses)
                {
                    pool.Add(i, 0.05f); //Add it to the pool with the same weight of 1
                }
            }
        }

        //Changing the spawn rate
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            //Change spawn stuff if invasion up and invasion at spawn
            if (KingdomWorld.customInvasionUp && (Main.invasionX == (double)Main.spawnTileX))
            {
                spawnRate = 35; //The higher the number, the less chance it will spawn (thanks jopojelly for how spawnRate works)
                maxSpawns = 10000; //Max spawns of NPCs depending on NPC value
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case NPCID.Dryad:
                    shop.item[nextSlot].SetDefaults(mod.ItemType("Keyblade_destiny"));
                    nextSlot++;
                    break;
                case NPCID.WitchDoctor:

                    shop.item[nextSlot].SetDefaults(mod.ItemType("Keyblade_witchDoctor"));
                    nextSlot++;
                    break;
                case NPCID.Merchant:
                    shop.item[nextSlot].SetDefaults(mod.ItemType("KupoCoin"));
                    nextSlot++;
                    break;
            }
            base.SetupShop(type, shop, ref nextSlot);
        }

        //Adding to the AI of an NPC
        public override void PostAI(NPC npc)
        {
            //Changes NPCs so they do not despawn when invasion up and invasion at spawn
            if (KingdomWorld.customInvasionUp && (Main.invasionX == (double)Main.spawnTileX))
            {
                npc.timeLeft = 1000;
            }
        }

        public override void NPCLoot(NPC npc)
        {
            SoraPlayer sp = Main.player[npc.target].GetModPlayer<SoraPlayer>();
            switch (npc.type)
            {
                case NPCID.QueenBee:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.HoneyBalloon);
                    break;
                case NPCID.EyeofCthulhu:
                    if(sp.fightingInBattleground)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ThePersistencyofEyes);
                    break;
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
        }

        public override bool PreAI(NPC npc)
        {

            if (isPartyMember && npc.townNPC)
            {

                CustomTownNPCAI(npc, npc.type);
                AI(npc);

                return false;
            }
            else
            {
                return true;
            }
        }

        public void CustomTownNPCAI(NPC npc, int type)
        {
            switch (type)
            {
                case NPCID.Guide:
                    PartyMemberAI.GuidePartyMemberAI(npc,prevPos,ProjectileID.FireArrow,ref initTownNPCStats);
                    break;
                case NPCID.Merchant:
                    PartyMemberAI.GuidePartyMemberAI(npc, prevPos, ProjectileID.GoldCoin, ref initTownNPCStats);
                    break;
                case NPCID.Nurse:
                    PartyMemberAI.GuidePartyMemberAI(npc, prevPos, ProjectileID.NurseSyringeHurt, ref initTownNPCStats);
                    break;
                case NPCID.Demolitionist:
                    PartyMemberAI.GuidePartyMemberAI(npc, prevPos, ProjectileID.Bomb, ref initTownNPCStats);
                    break;
                case NPCID.DyeTrader:
                    PartyMemberAI.GuidePartyMemberAI(npc, prevPos, ProjectileID.CrystalBullet, ref initTownNPCStats);
                    break;
                case NPCID.Angler:
                    PartyMemberAI.GuidePartyMemberAI(npc, prevPos, ProjectileID.ZephyrFish, ref initTownNPCStats);
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
                    PartyMemberAI.DryadPartyMemberAI(npc, prevPos, ProjectileID.Stinger, ref initTownNPCStats);
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
            if (npc.type == mod.NPCType("Neku"))
            {
                PartyMemberAI.GuidePartyMemberAI(npc, prevPos,ProjectileID.FallingStar, ref initTownNPCStats);
            }
        }

        public override void AI(NPC npc)
        {

            if (npc.townNPC) isPartyMember = npc.HasBuff(mod.BuffType("PMemberBuff"));

            if(npc.target<=0)
                npc.TargetClosest(false);

            SoraPlayer sp = Main.player[npc.target].GetModPlayer<SoraPlayer>();


            if (npc.townNPC && isPartyMember)
            {
                npc.immortal = true;
                if (Vector2.Distance(npc.Center, Main.player[Main.myPlayer].Center)>750)
                {
                    npc.Teleport(Main.player[Main.myPlayer].Center);
                }
                else
                {
                    if (Vector2.Distance(npc.Center, Main.player[Main.myPlayer].Center) > 300) {
                        Vector2 vel = (Main.player[Main.myPlayer].Center - npc.Center);
                        npc.direction = (int)(vel.X / MathHelp.Magnitude(vel));
                        npc.velocity =MathHelp.Normalize(vel)*Math.Max(5,MathHelp.Magnitude(Main.player[Main.myPlayer].velocity));
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
                switch (npc.type)
                {
                    case NPCID.EyeofCthulhu:

                        if (Main.expertMode || sp.fightingInBattleground)
                        {

                            if (npc.ai.Length < 6)
                            {
                                npc.ai = new float[6];
                            }
                            npc.ai[4]++;

                            if (npc.ai[4] % 15 == 0 && npc.life < npc.lifeMax / 2)
                            {
                                int createdProj = Projectile.NewProjectile(npc.Center - npc.velocity / npc.velocity.Length() * npc.width, new Vector2(), ProjectileID.DemonScythe, 15, 2);
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].timeLeft = 50;
                            }

                        }

                        break;

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
                                int createdProj = Projectile.NewProjectile(npc.Center, new Vector2(), ProjectileID.DemonSickle, 5, 2);
                                Main.projectile[createdProj].timeLeft = 150;
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].scale = 0.5f;
                            }
                        }
                        break;
                    case NPCID.EaterofWorldsHead:
                        if (sp.fightingInBattleground)
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

                            int firingTime = (sp.fightingInBattleground && Main.expertMode) ? 5 : 1;
                            if (npc.ai[6] > 100 * firingTime && npc.ai[6] % 5 / firingTime == 0)
                            {
                                int createdProj = Projectile.NewProjectile(npc.Center, npc.velocity * 2, ProjectileID.CursedFlameHostile, npc.damage, 10);
                                Main.projectile[createdProj].alpha = 100;
                                Main.projectile[createdProj].friendly = false;
                                npc.ai[6] = (npc.ai[6] >= 250) ? 0 : npc.ai[6];
                            }
                        }

                        break;
                }
                base.AI(npc);
                prevPos = npc.Center;
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            SoraPlayer sp = Main.player[npc.target].GetModPlayer<SoraPlayer>();
            if (npc.boss && sp.fightingInBattleground)
            {
                Texture2D texture = mod.GetTexture("NPCs/Bosses/HeartlessSigil");

                Vector2 symbolPos= new Vector2(npc.Center.X - Main.screenPosition.X - npc.width * 0.25f + texture.Width * 0.5f, npc.Center.Y - Main.screenPosition.Y - npc.height * 0.25f + texture.Height * 0.5f);
                switch (npc.type)
                {
                    case NPCID.EyeofCthulhu:
                        symbolPos = new Vector2
                    (npc.Center.X - Main.screenPosition.X - npc.width * 0.25f + texture.Width * 0.5f + 2f,npc.Center.Y - Main.screenPosition.Y - npc.height * 0.25f + texture.Height * 0.5f + 2f);
                        break;
                    case NPCID.QueenBee:
                        symbolPos = new Vector2(npc.Center.X - Main.screenPosition.X - npc.width * 0.25f + texture.Width * 0.5f + 2f, npc.Center.Y - Main.screenPosition.Y - npc.height * 0.25f + texture.Height * 0.5f + 2f);
                        break;
                    default:
                        symbolPos = new Vector2(npc.Center.X - Main.screenPosition.X - npc.width * 0.25f + texture.Width * 0.5f + 2f, npc.Center.Y - Main.screenPosition.Y - npc.height * 0.25f + texture.Height * 0.5f + 2f);
                        break;
                }
                if (npc.type == mod.NPCType("Darkside"))
                    symbolPos = new Vector2(npc.Center.X - Main.screenPosition.X - npc.width * 0.175f + texture.Width * 0.5f + 0f, npc.Center.Y - Main.screenPosition.Y - npc.height * 0.5f + texture.Height * 0.5f - 15f);

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
            if (isPartyMember)
            {
                npc.life = npc.lifeMax;
                npc.Teleport(new Vector2(npc.homeTileX, npc.homeTileY));
                isPartyMember = false;
                return false;
            }
            return base.CheckDead(npc);
        }

        public override void TownNPCAttackCooldown(NPC npc, ref int cooldown, ref int randExtraCooldown)
        {
            initTownNPCStats[1] = (initTownNPCStats[1] == -1) ? cooldown: initTownNPCStats[1];
            initTownNPCStats[2] = (initTownNPCStats[2] == -1) ? randExtraCooldown : initTownNPCStats[2];
            if (npc.townNPC)
            {
                if (isPartyMember)
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
            if (npc.townNPC && isPartyMember)
            {
                switch (npc.type)
                {
                    case NPCID.Guide:
                        projType = ProjectileID.HolyArrow;
                        break;
                }
            }
        }

        public override void DrawTownAttackGun(NPC npc, ref float scale, ref int item, ref int closeness)
        {

        }

        public override void TownNPCAttackProjSpeed(NPC npc, ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {

        }

        public override void FindFrame(NPC npc, int frameHeight)
        {
            if(initTownNPCStats[4]>0 && npc.townNPC && isPartyMember)
            {
                npc.frame.Y = frameHeight * 23;
                initTownNPCStats[4]--;
            }
            if (Main.npc[initTownNPCStats[3]].active && initTownNPCStats[3] != 0)
            {
                npc.spriteDirection = (int)MathHelp.Normalize(Main.npc[initTownNPCStats[3]].Center - npc.Center).X;
            }
            else
            {
                npc.spriteDirection = (npc.velocity.X == 0) ? npc.spriteDirection : (int)MathHelp.Sign(npc.velocity.X);
            }
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            return ((projectile.type == mod.ProjectileType("PartyMemberSelectProj") || projectile.type == mod.ProjectileType("PartyMemberDeselectProj")) && npc.townNPC)?true:base.CanBeHitByProjectile(npc,projectile);
        }

    }
}
