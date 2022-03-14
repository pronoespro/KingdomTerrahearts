using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;

namespace KingdomTerrahearts.Projectiles
{
    public class guardProjectile : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guard");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 156 / 3;
            Projectile.scale = 2;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 1.2f;
            Projectile.timeLeft = 5;
            Projectile.ignoreWater = true;
            Projectile.rotation = 0;
            Projectile.alpha = 250;
        }

        public override void AI()
        {
            SoraPlayer sp = Main.player[Projectile.owner].GetModPlayer<SoraPlayer>();
            sp.guardTime = Projectile.timeLeft;
            sp.guardProj = Projectile.whoAmI;
            sp.guardType = blockingType.reflect;

            Projectile.position = Main.player[Projectile.owner].position - new Vector2(Projectile.width / 4.6f, 0);

            Projectile.ai[0]++;
            Projectile.ai[0] = (Projectile.ai[0] >= 30) ? 0 : Projectile.ai[0];
            Projectile.frame = (int)(Projectile.ai[0] / 10);

            Projectile.alpha = (sp.guardTime > 15) ? 0 : 150 - sp.guardTime;
            Projectile.scale = 1.5f;
        }

    }

    public class reflectProjectile : ModProjectile
    {

        public override string Texture => "KingdomTerrahearts/Projectiles/guardProjectile";

        int lastTimeLeft;
        int projectileDamage = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reflect");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 156 / 3;
            Projectile.scale = 1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 1.2f;
            Projectile.timeLeft= lastTimeLeft = 50;
            Projectile.ignoreWater = true;
            Projectile.rotation = 0;
            Projectile.alpha = 250;

        }

        public override void AI()
        {

            SoraPlayer sp = Main.player[Projectile.owner].GetModPlayer<SoraPlayer>();

            if (Projectile.timeLeft == 50)
            {
                sp.PlayGuardSound();
                sp.guardTime = 50;
                sp.guardProj = Projectile.whoAmI;
                sp.guardType = blockingType.special;
            }

            if (Projectile.timeLeft > lastTimeLeft)
            {
                projectileDamage += sp.lastHeldItem.damage;
            }

            if (Projectile.timeLeft > 1)
            {

            }
            else
            {

                Main.NewText(projectileDamage.ToString());

            }

            Projectile.position = Main.player[Projectile.owner].position - new Vector2(Projectile.width / 4.6f, 0);

            Projectile.ai[0]++;
            Projectile.ai[0] = (Projectile.ai[0] >= 30) ? 0 : Projectile.ai[0];
            Projectile.frame = (int)(Projectile.ai[0] / 10);

            Projectile.alpha = (sp.guardTime > 45) ? 0 : 150 - sp.guardTime;
            Projectile.scale = 3;
            Projectile.damage = 0;

            lastTimeLeft = Projectile.timeLeft;

        }

    }

}
