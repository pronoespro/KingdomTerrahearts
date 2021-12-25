using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace KingdomTerrahearts.NPCs
{
    class SurveillanceRobotHeartless:ModNPC
    {

        float maxDistance=250;
        float speed = 5;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Watcher Heartless");
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("These small flying drone Heartless fire lasers that can be returned with a block, but they will respond with an even stronger power laser")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 30;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.knockBackResist = 1;
            NPC.width = 31;
            NPC.height = 42;
            NPC.scale = 1.25f;
            NPC.value = 10;
            NPC.npcSlots = 0.1f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }
        public override bool PreAI()
        {
            return true;
        }
        public override void AI()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.TargetClosest(true);

            Player target = Main.player[NPC.target];

            Vector2 dir = target.position - NPC.position;
            if (Math.Abs(dir.X)>maxDistance|| Math.Abs(dir.Y) > maxDistance)
            {
                NPC.velocity = dir / magnitude(dir)*speed;
                NPC.ai[0] =(NPC.ai[0]<120)? 120:NPC.ai[0];
            }
            else
            {
                if(NPC.direction!= (dir / magnitude(dir)).X)
                    NPC.velocity = dir / magnitude(dir) * speed;
                float yvel = target.position.Y - NPC.position.Y;
                if (Math.Abs(yvel) > speed/2)
                {
                    NPC.velocity.Y = yvel / Math.Abs(yvel) * speed/2;
                }
                else
                {
                    NPC.velocity = new Vector2();
                }
                NPC.ai[0]++;
                if (NPC.ai[0] > 180)
                {
                    Shoot();
                }
            }
            NPC.direction = (NPC.velocity.X==0)?NPC.direction: (NPC.velocity.X > 0) ? 1 : -1;

        }

        public void Shoot()
        {

            ProjectileSource_NPC s = new ProjectileSource_NPC(NPC);

            Vector2 projVel =new Vector2(NPC.direction,0);
            Projectile newProj=Projectile.NewProjectileDirect(s,NPC.Center, projVel*2, ProjectileID.DeathLaser, 3, 0.5f);
            newProj.friendly = false;
            newProj.timeLeft = 175;
            newProj.scale = 0.85f;

            NPC.ai[0]=0;
        }

        public float magnitude(Vector2 vector)
        {
            return Math.Abs(vector.X) + Math.Abs(vector.Y);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
            NPC.frame.Y = frameHeight * 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lightningGem>(), 10));
        }

    }
}
