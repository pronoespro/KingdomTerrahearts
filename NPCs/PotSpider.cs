using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.NPCs
{
    public class PotSpider : BasicGroundEnemy
    {

        public float attackRange = 150;

        public bool activated = false;

        public override void AI()
        {

            npc.TargetClosest(false);
            Player p = Main.player[npc.target];


            if (activated)
                base.AI();
            else
                activated = Vector2.Distance(npc.Center, p.Center) < attackRange;

        }

        public override void SpecialAction()
        {
        }

        public override void SpecialAttack()
        {
        }

        public override void FindFrame(int frameHeight)
        {
            if (activated)
            {
                walkcycle--;
                walkcycle = (walkcycle < -30) ? 30 : walkcycle;
                int walk = 3;
                if (walkcycle > -15) walk = 2;
                if (walkcycle > 0) walk = 1;
                if (walkcycle > 15) walk = 0;

                switch (walk)
                {
                    case 0:
                    case 2:
                        curFrame = 1;
                        break;
                    case 1:
                        curFrame = 2;
                        break;
                    case 3:
                    default:
                        curFrame = 3;
                        break;
                }
            }

            npc.frame.Y = frameHeight * curFrame;
        }


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pot Spider");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 100;
            npc.damage = 20;
            npc.defense = 2;
            npc.knockBackResist = 1;
            npc.width = 30;
            npc.height = 30;
            npc.value = 0;
            npc.npcSlots = 0.1f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath6;

            canTeleport = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Underground.Chance*30;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            activated = true;
            Item.NewItem(npc.getRect(), ItemID.GoldCoin,(Main.rand.Next(1,30)/10+1));
        }

        public override void NPCLoot()
        {
            Random r = new Random();
            switch (Main.rand.Next(5))
            {
                case 0:
                default:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("blazingShard"), Main.rand.Next(5) + 1);
                    break;
                case 1:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("denseShard"), Main.rand.Next(5) + 1);
                    break;
                case 2:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("frostShard"), Main.rand.Next(5) + 1);
                    break;
                case 3:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("lucidShard"), Main.rand.Next(5) + 1);
                    break;
                case 4:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("powerShard"), Main.rand.Next(5) + 1);
                    break;
                case 5:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("pulsingShard"), Main.rand.Next(5) + 1);
                    break;
                case 6:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("thunderShard"), Main.rand.Next(5) + 1);
                    break;
                case 7:
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("twilightShard"), Main.rand.Next(5) + 1);
                    break;
            }
            Rectangle coinSpawnRect = npc.getRect();
            coinSpawnRect.Width *= 4;
            coinSpawnRect.Height *= 4;
            Item.NewItem(coinSpawnRect, ItemID.GoldCoin, (Main.rand.Next(1, 30) / 10 + 1));
        }

    }
}
