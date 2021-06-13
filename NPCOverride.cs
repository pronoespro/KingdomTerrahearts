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
using KingdomTerrahearts.Interface;

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

        //boss stuff
        bool heartlessVerActive=false;
        bool spawnConversationDone = false;
        int proj;

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
                Conversation[] conv = new Conversation[] { new Conversation("That was kindda pointless, but here you go, a Terra Blade for your effords", Color.DarkGreen, 75000, "Creator") };
                DialogSystem.AddConversation(conv);
                Item.NewItem(npc.getRect(), ItemID.TerraBlade);
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
        }

        public override void NPCLoot(NPC npc)
        {
            SoraPlayer sp = Main.player[npc.target].GetModPlayer<SoraPlayer>();
            
            Conversation[] conv = new Conversation[0];

            switch (npc.type)
            {

                //Bosses

                case NPCID.KingSlime:
                    if (Main.rand.Next(0, 3) == 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Keyblade_Slime"));

                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,ItemID.SlimeCrown,Stack:15);

                    conv = new Conversation[] { new Conversation("The strongest slime has failed!", Color.Blue, 40000, npc.FullName), new Conversation("How could... this... be...", Color.Blue, 40000, npc.FullName) };
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.QueenBee:
                    if (sp.fightingInBattleground)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.HoneyBalloon);

                    if (Main.rand.Next(5) < 3)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Keyblade_Honey"));
                    }

                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Abeemination, Stack: 15);
                    break;

                case NPCID.EyeofCthulhu:

                    if(sp.fightingInBattleground)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ThePersistencyofEyes);

                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SuspiciousLookingEye,Stack:10);
                    break;

                case NPCID.EaterofWorldsHead:
                    if (NPC.CountNPCS(NPCID.EaterofWorldsHead)<=1 && !NPC.AnyNPCs(NPCID.EaterofWorldsBody))
                    {
                        if (sp.fightingInBattleground)
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.WormholePotion, Stack: Main.rand.Next(9) + 1);
                        }
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.WormFood, Stack: Main.rand.Next(9) + 1);
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("pulsingShard"), Stack: Main.rand.Next(14) + 1);
                    }
                    if (Main.rand.Next(3) <= 1)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("pulsingShard"));
                    break;

                case NPCID.SkeletronHead:
                    conv = new Conversation[] { new Conversation("The curse disperses", Color.Black, 40000) };
                    DialogSystem.AddConversation(conv);
                    if (sp.fightingInBattleground)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.RedHat, Stack: Main.rand.Next(9) + 1);
                    }
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ClothierVoodooDoll);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("twilightShard"), Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Bone, Stack: Main.rand.Next(14) + 1);
                    break;
                case NPCID.SkeletronHand:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Bone, Stack: Main.rand.Next(5) + 1);
                    break;

                case NPCID.WallofFlesh:
                    if (sp.fightingInBattleground)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TitaniumBar,Stack:20);
                    }
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("powerShard"), Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GuideVoodooDoll);
                    break;

                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                    if (!NPC.AnyNPCs(NPCID.Retinazer) || !NPC.AnyNPCs(NPCID.Spazmatism))
                    {
                        if (sp.fightingInBattleground)
                        {
                            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LaserMachinegun);
                        }
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("powerShard"), Stack: Main.rand.Next(14) + 1);
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("thunderShard"), Stack: Main.rand.Next(14) + 1);
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MechanicalEye, Stack: Main.rand.Next(9) + 1);
                    }
                    conv = new Conversation[] { new Conversation("DESTRUCTI()N.... F41|3D", Color.Red, 40000, npc.FullName)};
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.TheDestroyer:
                    if (sp.fightingInBattleground)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MechanicsRod);
                    }
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("powerShard"), Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("thunderShard"), Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MechanicalWorm, Stack: Main.rand.Next(9) + 1);
                    conv = new Conversation[] { new Conversation("DES1RU(11ON /|TTEMP1 FA1....", Color.Red, 40000, npc.FullName)};
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.Probe:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.IronBar);
                    break;

                case NPCID.SkeletronPrime:
                    if (sp.fightingInBattleground)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.RocketLauncher);
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.RocketI, Stack: (Main.rand.Next(9) + 1)*10);
                    }
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("powerShard"), Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("thunderShard"), Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MechanicalSkull, Stack: Main.rand.Next(9) + 1);
                    conv = new Conversation[] { new Conversation("DESTRUCTI()N ATT#MPT FAi....", Color.Red, 40000, npc.FullName) };
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.Plantera:
                    if (sp.fightingInBattleground)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KairiHeart"));
                    }
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("pulsingShard"), Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PlanteraFlower"), Stack: Main.rand.Next(14) + 1);
                    break;
                case NPCID.PlanterasTentacle:
                    if(Main.rand.Next(3)==0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("pulsingShard"));
                    break;
                case NPCID.Spore:
                    if (Main.player[npc.target].GetModPlayer<SoraPlayer>().fightingInBattleground)
                    {
                        proj = Projectile.NewProjectile(npc.Center, npc.velocity, ProjectileID.SporeCloud, npc.damage / 2, 1);
                        Main.projectile[proj].friendly = false;
                        Main.projectile[proj].hostile = true;
                        Main.projectile[proj].timeLeft = 50;
                    }
                    break;

                case NPCID.Golem:
                    if (sp.fightingInBattleground)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CellPhone);
                    }
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("powerShard"), Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LihzahrdPowerCell, Stack: Main.rand.Next(14) + 1);
                     conv = new Conversation[] {  new Conversation("Destruction of Terrarian failed...", Color.Brown, 40000, npc.FullName) };
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.CultistBoss:
                    if (sp.fightingInBattleground)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SolarTablet,Stack:5);
                    }
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("thunderShard"), Stack: Main.rand.Next(14) + 1);
                     conv = new Conversation[] { new Conversation("May the Lord of the Moon Destroy you...!", Color.LightBlue, 40000, npc.FullName)};
                    DialogSystem.AddConversation(conv);
                    break;

                case NPCID.DukeFishron:
                    if (sp.fightingInBattleground)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BottomlessBucket);
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SuperAbsorbantSponge);
                    }

                    if (Main.rand.Next(0, 2) == 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Keyblade_DukeFish"));

                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TruffleWorm, Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("thunderShard"), Stack: Main.rand.Next(39) + 11);
                    break;

                case NPCID.MoonLordCore:
                    if (sp.fightingInBattleground)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.VortexMonolith);
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.StardustMonolith);
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SolarMonolith);
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.PartyMonolith);
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.NebulaMonolith);
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ScepTendo_gun"));
                    }
                    if (Main.rand.Next(0, 2) == 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Keyblade_MoonLord"));
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CelestialSigil, Stack: Main.rand.Next(14) + 1);

                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentNebula, Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentSolar, Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentStardust, Stack: Main.rand.Next(14) + 1);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentVortex, Stack: Main.rand.Next(14) + 1);

                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Orichalchum"));

                    conv = new Conversation[] { new Conversation("IMPOSIBLE!!!", Color.LightBlue, 50000, "Moon Lord") };
                    DialogSystem.AddConversation(conv);
                    break;
                case NPCID.MoonLordHead:
                    if (Main.rand.Next(0, 2) == 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LunarOre,Stack:Main.rand.Next(14)+6);
                    break;
                case NPCID.MoonLordHand:
                    if (Main.rand.Next(0, 2) == 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LunarOre, Stack: Main.rand.Next(14) + 6);
                    break;

                //Pilars

                    //Vortex
                case NPCID.LunarTowerVortex:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("thunderShard"), Stack: Main.rand.Next(34) + 11);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SniperScope);
                    break;

                case NPCID.VortexHornet:
                case NPCID.VortexHornetQueen:
                case NPCID.VortexLarva:
                case NPCID.VortexRifleman:
                case NPCID.VortexSoldier:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("thunderShard"), Stack: Main.rand.Next(2) + 1);
                    if(Main.rand.Next(3)==0)
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentVortex);
                    break;

                    //Solar
                case NPCID.LunarTowerSolar:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("blazingShard"), Stack: Main.rand.Next(34) + 11);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MoneyTrough);
                    break;

                case NPCID.SolarCorite:
                case NPCID.SolarCrawltipedeHead:
                case NPCID.SolarDrakomire:
                case NPCID.SolarDrakomireRider:
                case NPCID.SolarSpearman:
                case NPCID.SolarSolenian:
                case NPCID.SolarSroller:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("blazingShard"), Stack: Main.rand.Next(2) + 1);
                    if (Main.rand.Next(3) == 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentSolar);
                    break;

                    //Nebula
                case NPCID.LunarTowerNebula:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("lucidShard"), Stack: Main.rand.Next(34) + 11);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.RodofDiscord);
                    break;

                case NPCID.NebulaBrain:
                case NPCID.NebulaBeast:
                case NPCID.NebulaHeadcrab:
                case NPCID.NebulaSoldier:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("lucidShard"), Stack: Main.rand.Next(2) + 1);
                    if (Main.rand.Next(3) == 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentNebula);
                    break;

                    //Stardust
                case NPCID.LunarTowerStardust:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("twilightShard"), Stack: Main.rand.Next(34) + 11);
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CelestialShell);
                    break;
                case NPCID.StardustJellyfishBig:
                case NPCID.StardustSoldier:
                case NPCID.StardustCellBig:
                case NPCID.StardustSpiderBig:
                case NPCID.StardustWormHead:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("twilightShard"), Stack: Main.rand.Next(2) + 1);
                    if (Main.rand.Next(3) == 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.FragmentStardust);
                    break;

                //Meteorite

                //Shards

                case NPCID.Skeleton:
                case NPCID.Paladin:
                case NPCID.Mimic:
                case NPCID.PossessedArmor:
                case NPCID.Wraith:
                case NPCID.WanderingEye:
                case NPCID.Reaper:
                    if (Main.rand.Next(0, 4) <= 2)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("denseShard"), Main.rand.Next(15) + 1);
                    break;
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
                    if (Main.rand.Next(0, 4) <=2)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("frostShard"), Main.rand.Next(5) + 1);
                    break;
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
                    if (Main.rand.Next(0, 4) <= 2)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("powerShard"), Main.rand.Next(5) + 1);
                    break;
                case NPCID.EaterofSouls:
                case NPCID.CorruptSlime:
                case NPCID.Corruptor:
                    if (Main.rand.Next(0, 4) <= 2)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("pulsingShard"), Main.rand.Next(5) + 1);
                    break;
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
                    if (Main.rand.Next(0, 4) <= 2)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("thunderShard"), Main.rand.Next(5) + 1);
                    break;
                case NPCID.IlluminantSlime:
                case NPCID.Pixie:
                case NPCID.Unicorn:
                    if (Main.rand.Next(0, 4) <= 2)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("twilightShard"), Main.rand.Next(5) + 1);
                    break;
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
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("blazingShard"), Stack: Main.rand.Next(14) + 1);
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
            if (npc.type == mod.NPCType("Neku"))
            {
                PartyMemberAI.GuidePartyMemberAI(npc, prevPos,ProjectileID.FallingStar, ref initTownNPCStats);
            }
        }

        public override void AI(NPC npc)
        {

            if (npc.townNPC) isPartyMember = npc.HasBuff(mod.BuffType("PMemberBuff"));


            SoraPlayer sp = Main.player[npc.target].GetModPlayer<SoraPlayer>();


            if (npc.townNPC && isPartyMember)
            {
                if (npc.target <= 0)
                    npc.TargetClosest(false);
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
                    //boss related
                    case NPCID.KingSlime:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] { new Conversation("You dare challenge the strongest of SLIMES?!", Color.Blue, 30000, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        if (sp.fightingInBattleground)
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

                            npc.reflectingProjectiles = npc.velocity.Y != 0;
                            npc.stairFall = false;

                            if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 550)
                            {
                                npc.Center = Main.player[npc.target].Center + new Vector2(0, -400);
                                npc.velocity.X = 0;
                            }

                            if (Main.expertMode && sp.fightingInBattleground)
                            {
                                npc.velocity.Y = (npc.velocity.Y > 0) ? npc.velocity.Y * 20 : npc.velocity.Y;
                            }
                        }
                        break;
                    case NPCID.SlimeSpiked:
                    case NPCID.BlueSlime:
                        if (NPC.AnyNPCs(NPCID.KingSlime))
                        {
                            if (Main.expertMode && sp.fightingInBattleground && !heartlessVerActive)
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

                        if (sp.fightingInBattleground)
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

                    case NPCID.ServantofCthulhu:
                        if (sp.fightingInBattleground)
                        {

                            if (!heartlessVerActive)
                            {
                                npc.alpha = 100;
                                if (Main.expertMode && sp.fightingInBattleground)
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
                            if (Main.expertMode && sp.fightingInBattleground)
                            {
                                npc.ai[0]++;
                                if (npc.ai[0] % 15 == 0)
                                {
                                    int createdProj = Projectile.NewProjectile(npc.Center - npc.velocity / npc.velocity.Length() * npc.width, new Vector2(), ProjectileID.DemonScythe, 15, 2);
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
                            if (npc.ai[6] > 20 * firingTime && npc.ai[6] % 5 / firingTime == 0)
                            {
                                int createdProj = Projectile.NewProjectile(npc.Center, npc.velocity * 2, ProjectileID.CursedFlameHostile, npc.damage, 10);
                                Main.projectile[createdProj].alpha = 100;
                                Main.projectile[createdProj].friendly = false;
                                npc.ai[6] = (npc.ai[6] >= 250) ? 0 : npc.ai[6];
                            }
                        }
                        break;
                    case NPCID.EaterofWorldsBody:
                        int segments = NPC.CountNPCS(npc.type) + NPC.CountNPCS(NPCID.EaterofWorldsHead) + NPC.CountNPCS(NPCID.EaterofWorldsTail);
                        if (sp.fightingInBattleground && Main.expertMode)
                        {
                            if (segments < 30)
                            {
                                if (npc.ai[0] % 20 == 0)
                                {
                                    int createdProj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.CursedFlameHostile, npc.damage, 10);
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
                            Conversation[] conv = new Conversation[] { new Conversation("The curse spreads from the old man to you", Color.Black, 40000)};
                            DialogSystem.AddConversation(conv);
                        }
                        break;
                    case NPCID.SkeletronHand:
                        break;


                    case NPCID.TheDestroyer:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] { new Conversation("REMOTE ACTIVATION DETECTED", Color.Red, 30000, npc.FullName), new Conversation("ATTEMPTING IMMEDIATE DESTRUCTION", Color.Red, 30000, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        if (sp.fightingInBattleground)
                        {
                            npc.ai[2]++;
                            if (npc.ai[2] % 150 == 0)
                            {
                                int createdProj = Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 15, ProjectileID.Electrosphere, npc.damage, 10);
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].timeLeft = 150;
                                Main.projectile[createdProj].scale = 0.2f;

                                npc.ai[2] = 0;
                            }
                        }
                        break;
                    case NPCID.Probe:
                        if (sp.fightingInBattleground && Main.expertMode)
                        {
                            if (npc.ai[0] % 20 == 0)
                            {
                                int createdProj = Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 3, ProjectileID.Electrosphere, npc.damage, 10);
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
                            Conversation[] conv = new Conversation[] { new Conversation("REMOTE ACTIVATION DETECTED", Color.Red, 30000, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        if (sp.fightingInBattleground)
                        {
                            if (!NPC.AnyNPCs(NPCID.Spazmatism))
                            {
                                npc.ai[2]++;
                                if (npc.ai[2] % 150 == 0)
                                {
                                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
                                }
                                else if (npc.ai[2] % 150 == 100)
                                {
                                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.Probe);
                                }
                            }
                        }
                        break;
                    case NPCID.Spazmatism:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] {  new Conversation("ATTEMPTING IMMEDIATE DESTRUCTION", Color.Red, 30000, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        if (sp.fightingInBattleground)
                        {
                            if (!NPC.AnyNPCs(NPCID.Retinazer))
                            {
                                npc.ai[2]++;
                                if (npc.ai[2] % 150 == 0)
                                {
                                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
                                }
                                if (npc.ai[2] % 150 == 100)
                                {
                                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
                                }
                            }
                        }
                        break;

                    case NPCID.SkeletronPrime:
                        if (!spawnConversationDone)
                        {
                            spawnConversationDone = true;
                            Conversation[] conv = new Conversation[] { new Conversation("REMOTE ACTIVATION DETECTED", Color.Red, 30000, npc.FullName) , new Conversation("ATTEMPTING IMMEDIATE DESTRUCTION", Color.Red, 30000, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        if (sp.fightingInBattleground)
                        {
                            if (!heartlessVerActive)
                            {
                                //npc.scale = (int)(npc.scale * 0.8f);
                                npc.damage = npc.damage * 4;
                                heartlessVerActive = true;
                            }
                            if (npc.ai[0] % 10 == 0)
                            {
                                int createdProj = Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 3, ProjectileID.Skull, npc.damage, 10);
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].timeLeft = 20;
                            }
                        }
                        break;
                    case NPCID.PrimeCannon:
                        if (sp.fightingInBattleground)
                        {
                            if (!heartlessVerActive)
                            {
                                //npc.scale = (int)(npc.scale * 0.75f);
                                npc.damage = (int)(npc.damage * 1.25f);
                                heartlessVerActive = true;

                                proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.Bubble, 0, 0);
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
                        if (sp.fightingInBattleground)
                        {
                            if (!heartlessVerActive)
                            {
                                //npc.scale = (int)(npc.scale * 0.75f);
                                npc.damage = (int)(npc.damage * 1.25f);
                                heartlessVerActive = true;

                                proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.Bubble, 0, 0);
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
                        if (sp.fightingInBattleground)
                        {
                            if (!heartlessVerActive)
                            {
                                //npc.scale = (int)(npc.scale * 0.75f);
                                npc.damage = (int)(npc.damage * 1.75f);
                                heartlessVerActive = true;

                                proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.Bubble, 0, 0);
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
                        if (sp.fightingInBattleground)
                        {
                            if (!heartlessVerActive)
                            {
                                //npc.scale = (int)(npc.scale * 0.75f);
                                npc.damage = (int)(npc.damage * 1.25f);
                                heartlessVerActive = true;

                                proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.Bubble, 0, 0);
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
                        if (sp.fightingInBattleground)
                        {
                            if (!heartlessVerActive)
                            {
                                npc.damage *= 2;
                                heartlessVerActive = true;
                            }
                        }
                        break;
                    case NPCID.PlanterasTentacle:
                        if (sp.fightingInBattleground)
                        {
                            if (!heartlessVerActive)
                            {
                                npc.damage *= 2;
                                heartlessVerActive = true;
                            }

                            if (npc.ai[0] % 50 == 0)
                            {
                                int createdProj = Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 3, ProjectileID.SporeCloud, npc.damage/2, 10);
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
                            Conversation[] conv = new Conversation[] { new Conversation("Profanity inside the temple detected", Color.Brown, 40000,npc.FullName), new Conversation("Destruction of Terrarian started...", Color.Brown, 40000, npc.FullName) };
                            DialogSystem.AddConversation(conv);
                        }
                        break;

                    case NPCID.DukeFishron:
                        if (sp.fightingInBattleground)
                        {
                            if (!heartlessVerActive)
                            {
                                heartlessVerActive = true;
                                //npc.scale = (int)(npc.scale *0.9f);
                                npc.damage = (int)(npc.damage * 1.5f);
                            }

                            if (npc.life < npc.lifeMax / 4 && npc.ai[1]%20==0)
                            {
                                int createdProj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.Bubble, npc.damage , 10);
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
                            Conversation[] conv = new Conversation[] { new Conversation("You killed my brothers", Color.LightBlue, 30000, npc.FullName), new Conversation("The end is near anyways... so just die", Color.LightBlue, 50000, npc.FullName) };
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
                            Conversation[] conv = new Conversation[] { new Conversation("TIME TO DIE!!!", Color.LightBlue, 50000, "Moon Lord") };
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
                                int createdProj = Projectile.NewProjectile(npc.Center, new Vector2(), ProjectileID.DemonSickle, 5, 2);
                                Main.projectile[createdProj].timeLeft = 150;
                                Main.projectile[createdProj].friendly = false;
                                Main.projectile[createdProj].hostile = true;
                                Main.projectile[createdProj].scale = 0.5f;
                            }
                        }
                        break;
                }
                base.AI(npc);
                Mod fargoDLC = ModLoader.GetMod("FargowiltasSoulsDLC");
                if (fargoDLC !=null)
                {
                    Player p = Main.player[npc.target];
                    if (npc.type== fargoDLC.NPCType("Echdeath") && sp.invincible)
                    {
                        if (Vector2.Distance(npc.Center, p.Center) < npc.width + npc.height)
                        {
                            npc.life = 0;
                            npc.checkDead();
                            if (sp.fightingInBattleground)
                            {
                                int itemEchdeath= Item.NewItem(npc.getRect(),ItemID.RagePotion,Stack:20);
                            }
                        }

                    }
                }
            }
            if (npc.townNPC)
                prevPos = npc.Center;
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
                else if (npc.type == mod.NPCType("xion_finalPhase"))
                    symbolPos = new Vector2(npc.Center.X - Main.screenPosition.X - npc.width * 0.125f/2f + texture.Width * 0.5f + 2f, npc.Center.Y - Main.screenPosition.Y - npc.height * 0.125f + texture.Height * 0.5f + 2f);

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

            int proj;

            switch (npc.type)
            {
                case NPCID.PrimeCannon:
                    proj = Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 5, ProjectileID.Bone,npc.damage / 3, 1);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].timeLeft = 1000;
                    break;
                case NPCID.PrimeLaser:
                    proj = Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 5, ProjectileID.Bone, npc.damage / 3, 1);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].timeLeft = 1000;
                    break;
                case NPCID.PrimeSaw:
                    proj = Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 5, ProjectileID.Bone, npc.damage/3, 1);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].timeLeft = 1000;
                    break;
                case NPCID.PrimeVice:
                    proj = Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.player[npc.target].Center - npc.Center) * 5, ProjectileID.Bone, npc.damage / 3, 1);
                    Main.projectile[proj].friendly = false;
                    Main.projectile[proj].hostile = true;
                    Main.projectile[proj].timeLeft = 1000;
                    break;
                case NPCID.MeteorHead:
                    Item.NewItem(npc.getRect(), ItemID.Meteorite, Main.rand.Next(1, 5));
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("blazingShard"), Stack: Main.rand.Next(14) + 1);
                    break;
            }
            if (npc.type == mod.NPCType("RedNocturne") && NPC.TowerActiveSolar)
            {
                Player p = Main.player[npc.target];
                NPC towerSolar = FindNPCType(NPCID.LunarTowerSolar);
                if(towerSolar!=null)
                    Projectile.NewProjectile(p.Center, towerSolar.Center, ProjectileID.TowerDamageBolt, 100, 0,npc.target,ai1: 100);
            }

            return base.CheckDead(npc);
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
                        projType = (Main.hardMode)?ProjectileID.HolyArrow:projType;
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
            if (npc.townNPC)
            {
                if (initTownNPCStats[4] > 0 && isPartyMember)
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
        }

        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            return ((projectile.type == mod.ProjectileType("PartyMemberSelectProj") || projectile.type == mod.ProjectileType("PartyMemberDeselectProj")) && npc.townNPC)?true:base.CanBeHitByProjectile(npc,projectile);
        }

    }
}
