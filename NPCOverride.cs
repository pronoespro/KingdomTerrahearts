using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomTerrahearts.NPCs.Invasions;
using System.Collections.Generic;

namespace KingdomTerrahearts
{
    class NPCOverride:GlobalNPC
    {
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
            }
        }

        public override void AI(NPC npc)
        {
            SoraPlayer sp = Main.player[npc.target].GetModPlayer<SoraPlayer>();

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

                        if (npc.ai[4] % 15 == 0 && npc.life<npc.lifeMax/2)
                        {
                            int createdProj = Projectile.NewProjectile(npc.Center - npc.velocity / npc.velocity.Length() * npc.width, new Vector2(), ProjectileID.DemonScythe, 15, 2);
                            Main.projectile[createdProj].friendly = false;
                            Main.projectile[createdProj].hostile = true;
                            Main.projectile[createdProj].timeLeft = 50;
                        }


                    }

                    break;

                case NPCID.MeteorHead:
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
                    break;
                case NPCID.EaterofWorldsHead:
                    if ( sp.fightingInBattleground)
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
                        if (npc.ai[6] > 100*firingTime && npc.ai[6] % 5/firingTime == 0)
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
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            SoraPlayer sp = Main.player[npc.target].GetModPlayer<SoraPlayer>();
            if (npc.boss && sp.fightingInBattleground)
            {
                Texture2D texture = mod.GetTexture("NPCs/Bosses/HeartlessSigil");

                spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        npc.Center.X - Main.screenPosition.X - npc.width * 0.25f + texture.Width * 0.5f + 2f,
                        npc.Center.Y - Main.screenPosition.Y - npc.height * 0.25f + texture.Height * 0.5f + 2f
                    ),
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

    }
}
