using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace KingdomTerrahearts.NPCs
{
    public class PotSpider : BasicGroundEnemy
    {

        public float attackRange = 150;

        public bool activated = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pot Spider");
            Main.npcFrameCount[NPC.type] = 4;
            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 0f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = -1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("Don't be fooled by this pot looking creature or it will come at you and take your money away" +
                "\nKeep hurting it to take it's money away instead.")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 100;
            NPC.damage = 20;
            NPC.defense = 2;
            NPC.knockBackResist = 1;
            NPC.width = 30;
            NPC.height = 30;
            NPC.value = 0;
            NPC.npcSlots = 0.1f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;

            canTeleport = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Underground.Chance*(NPC.downedBoss1||NPC.downedBoss2||NPC.downedBoss3?1:0);
        }

        public override void AI()
        {

            NPC.TargetClosest(false);
            Player p = Main.player[NPC.target];


            if (activated)
            {
                base.AI();
            }
            else
            {
                activated = Vector2.Distance(NPC.Center, p.Center) < attackRange;
            }

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

            NPC.frame.Y = frameHeight * curFrame;
        }


        public override void HitEffect(int hitDirection, double damage)
        {
            activated = true;
            Item.NewItem(NPC.getRect(), ItemID.CopperCoin,(Main.rand.Next(1,30)/10+1));
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

            //Bestiary
            int[] possibleItems = new int[] { ModContent.ItemType<Items.Materials.blazingShard>(), ModContent.ItemType<Items.Materials.betwixtShard>() , ModContent.ItemType<Items.Materials.frostShard>() , ModContent.ItemType<Items.Materials.frostShard>() , ModContent.ItemType<Items.Materials.lucidShard>() , ModContent.ItemType<Items.Materials.mythrilShard>() , ModContent.ItemType<Items.Materials.pulsingShard>() , ModContent.ItemType<Items.Materials.lightningShard>() , ModContent.ItemType<Items.Materials.twilightShard>() };

            npcLoot.Add(ItemDropRule.OneFromOptions(1,possibleItems));

            possibleItems = new int[] { ItemID.CopperOre,ItemID.SilverOre,ItemID.GoldOre,ItemID.LeadOre,ItemID.IronOre,ItemID.TinOre};

            npcLoot.Add(ItemDropRule.OneFromOptions(1, possibleItems));

        }

        public override bool PreKill()
        {
            Rectangle coinSpawnRect = NPC.getRect();
            coinSpawnRect.Width *= 4;
            coinSpawnRect.Height *= 4;
            Item.NewItem(coinSpawnRect, ItemID.GoldCoin, (Main.rand.Next(1, 4)  + 1));
            return base.PreKill();
        }

    }
}
