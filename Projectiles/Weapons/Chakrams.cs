using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using KingdomTerrahearts.Extra;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using System;
using Microsoft.Xna.Framework;

namespace KingdomTerrahearts.Projectiles.Weapons
{
    public class Chakram_EternalFlames:ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_EternalFlames";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eternal Flames");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height=30;
            Projectile.timeLeft = 70;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.light = 0.5f;

            fireTime = 240;
            trailSpeedMult = 3f;
            damageMult = 5f;
            minProjectiles = 20;
            maxProjectiles = 50;
            manaUsage = 10;
        }

    }

    public class Chacrams_DelayedAction : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_DelayedAction";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Delayed Action");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 70;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.DemonScythe;
            trailTimer = 20;
            trailSpeedMult = 0.3f;
            projectilesOnCollision = -1;
            fireTime = 3;
        }

    }

    public class Chacrams_Prometheus : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Prometheus";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prometheus");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.scale = 2;
            Projectile.timeLeft = 70;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.light = 0.5f;

            projectilesOnCollision = -1;
            projectileTrail = ProjectileID.MolotovFire;
            trailTimer = 3;
            trailSpeedMult = 0.1f;
            fireTime = 3;
            manaUsage = 2;
        }

    }

    public class Chacrams_DiveBombers : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_DiveBombers";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dive Bombers");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.scale =1.1f;
            Projectile.timeLeft = 75;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 6;
            Projectile.light = 0.5f;

            damageMult = 1;
            projectileTrail = ProjectileID.DemonScythe;
            trailTimer = 10;
            trailSpeedMult = 0.75f;
            projectilesOnCollision = ProjectileID.MolotovFire;
            minProjectiles = 3;
            maxProjectiles = 10;
            fireTime = 5;
            manaUsage = 4;
        }

    }

    public class Chacrams_Ashes : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Ashes";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ashes");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 65;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.light = 0.5f;

            projectileTrail = -1;
            projectilesOnCollision = -1;
        }

    }

    public class Chacrams_Doldrums : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Doldrums";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doldrums");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 68;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.light = 0.5f;

            projectilesOnCollision = ProjectileID.RubyBolt;
            damageMult = 0.5f;
            trailSpeedMult = 5;
            minProjectiles = 3;
            maxProjectiles = 6;
            fireTime = 4;
            manaUsage = 4;
        }

    }

    public class Chacrams_Combustion : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Combustion";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Combustion");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 72;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.MolotovFire;
            trailTimer = 5;
            projectilesOnCollision = ProjectileID.FlamesTrap;
            damageMult = 0.5f;
            trailSpeedMult = 0;
            minProjectiles = 3;
            maxProjectiles = 6;
            fireTime = 4;
            manaUsage = 10;
        }

    }

    public class Chacrams_MoulinRouge : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_MoulinRouge";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moulin Rouge");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 70;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.BallofFire;
            trailTimer = 7;
            projectilesOnCollision = -1;
            damageMult = 0.4f;
            trailSpeedMult = -0.4f;
            minProjectiles = 1;
            maxProjectiles = 2;
            fireTime = 7;
            manaUsage =5;
        }

    }

    public class Chacrams_BlazeOfGlory : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_BlazeOfGlory";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blaze of Glory");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 72;
            Projectile.scale = 1.1f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.FlamesTrap;
            trailTimer = 4;
            projectilesOnCollision = -1;
            damageMult = 0.25f;
            trailSpeedMult = -0.2f;
            minProjectiles = 1;
            maxProjectiles = 1;
            fireTime = 3;
            manaUsage = 8;
        }

    }

    public class Chacrams_Ifrit : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Ifrit";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ifrit");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 72;
            Projectile.scale = 1.1f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.FlamesTrap;
            trailTimer =5;
            projectilesOnCollision = -1;
            damageMult = 0.25f;
            trailSpeedMult = 0.75f;
            minProjectiles = 1;
            maxProjectiles = 2;
            fireTime = 3;
            manaUsage = 5;
        }

    }

    public class Chacrams_MagmaOcean : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_MagmaOcean";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Ocean");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 72;
            Projectile.scale = 1.25f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.Shuriken;
            trailTimer =3;
            projectilesOnCollision = -1;
            damageMult = 2f;
            trailSpeedMult = 1f;
            minProjectiles = 5;
            maxProjectiles = 7;
            fireTime = 5;
            manaUsage = 4;
        }

    }

    public class Chacrams_Volcanics : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Volcanics";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Volcanics");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 72;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 6;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.Shuriken;
            trailTimer = 1;
            projectilesOnCollision = ProjectileID.Shuriken;
            damageMult = 2.5f;
            trailSpeedMult = 1f;
            minProjectiles = 5;
            maxProjectiles = 7;
            fireTime = 7;
            manaUsage = 2;
        }

    }

    public class Chacrams_Inferno : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Inferno";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Inferno");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 72;
            Projectile.scale = 1.5f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 50;
            Projectile.light = 0.5f;

            projectileTrail = -1;
            trailTimer = 1;
            projectilesOnCollision = ProjectileID.Meteor1;
            damageMult = 3f;
            trailSpeedMult = 0.5f;
            minProjectiles = 10;
            maxProjectiles = 15;
            fireTime = 5;
            manaUsage = 2;
        }

    }

    public class Chacrams_SizzlingEdge : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_SizzlingEdge";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sizzling Edge");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 72;
            Projectile.scale = 1.75f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 55;
            Projectile.light = 0.5f;

            projectileTrail = -1;
            trailTimer = 1;
            projectilesOnCollision = ProjectileID.MeteorShot;
            damageMult = 5f;
            trailSpeedMult = 0.25f;
            minProjectiles = 13;
            maxProjectiles = 17;
            fireTime = 7;
            manaUsage = 3;
        }

    }

    public class Chacrams_Corona : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Corona";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corona");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 75;
            Projectile.scale = 2f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 150;
            Projectile.light = 0.5f;

            projectileTrail = -1;
            trailTimer = 1;
            projectilesOnCollision = ProjectileID.MeteorShot;
            damageMult = 6f;
            trailSpeedMult = 0.35f;
            minProjectiles = 20;
            maxProjectiles = 23;
            fireTime = 10;
            manaUsage = 10;
        }

    }

    public class Chacrams_FerrisWheels : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_FerrisWheels";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ferris Wheels");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 75;
            Projectile.scale = 0.75f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 150;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.MagicMissile;
            trailTimer = 10;
            projectilesOnCollision = ProjectileID.MagicMissile;
            damageMult = 0.25f;
            trailSpeedMult = 0.5f;
            minProjectiles = 1;
            maxProjectiles = 2;
            fireTime = 10;
            manaUsage = 10;
        }

    }
    
    public class Chacrams_Burnout : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Burnout";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Burnout");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 70;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 150;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.MagicMissile;
            trailTimer = 5;
            projectilesOnCollision = ProjectileID.MagicMissile;
            damageMult = 0.5f;
            trailSpeedMult = 0.75f;
            minProjectiles = 2;
            maxProjectiles = 4;
            fireTime = 5;
            manaUsage = 5;
        }

    }
    
    public class Chacrams_OmegaTrinity : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_OmegaTrinity";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Omega Trinity");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 85;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 150;
            Projectile.light = 0.5f;

            projectileTrail = -1;
            trailTimer = 5;
            projectilesOnCollision = ProjectileID.Fireball;
            damageMult = 0.2f;
            trailSpeedMult = 0.5f;
            minProjectiles = 2;
            maxProjectiles = 4;
            fireTime = 5;
            manaUsage = 5;
        }

    }
    
    public class Chacrams_Outbreak : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Outbreak";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Outbreak");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 90;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 150;
            Projectile.light = 0.5f;

            projectileTrail = -1;
            trailTimer = 5;
            projectilesOnCollision = ProjectileID.Fireball;
            damageMult = 0.25f;
            trailSpeedMult = 0.5f;
            minProjectiles = 2;
            maxProjectiles = 4;
            fireTime = 5;
            manaUsage = 5;
        }

    }
    
    public class Chacrams_DoubleEdge : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_DoubleEdge";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Double Edge");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 75;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 150;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.MagicMissile;
            trailTimer = 10;
            projectilesOnCollision = -1;
            damageMult = 0.4f;
            trailSpeedMult = 0.5f;
            minProjectiles = 2;
            maxProjectiles = 4;
            fireTime = 5;
            manaUsage = 7;
        }

    }
    
    public class Chacrams_Wildfire : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Wildfire";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wildfire");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 75;
            Projectile.scale = 1.2f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 150;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.MagicMissile;
            trailTimer = 7;
            projectilesOnCollision = -1;
            damageMult = 0.4f;
            trailSpeedMult = 0.6f;
            minProjectiles = 3;
            maxProjectiles = 5;
            fireTime = 6;
            manaUsage = 8;
        }

    }
    
    public class Chacrams_Prominence : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Prominence";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prominence");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 85;
            Projectile.scale = 1.3f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 150;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.GoldenShowerFriendly;
            trailTimer = 7;
            projectilesOnCollision = -1;
            damageMult = 0.5f;
            trailSpeedMult = -0.4f;
            minProjectiles = 1;
            maxProjectiles = 2;
            fireTime = 5;
            manaUsage = 20;
        }

    }
    
    public class Chacrams_Conformers : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_Conformers";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Conformers");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 85;
            Projectile.scale = 1.75f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.light = 0.5f;

            projectileTrail = ProjectileID.DemonScythe;
            trailTimer = 5;
            projectilesOnCollision = ProjectileID.GoldenShowerFriendly;
            damageMult = 1f;
            trailSpeedMult = -0.4f;
            minProjectiles = 3;
            maxProjectiles = 10;
            fireTime = 10;
            manaUsage = 10;
        }

    }

    public class Chacrams_Papou : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/papouFruit";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Papou's love");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 150;
            Projectile.scale = 5f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 999999;
            Projectile.light = 0.5f;

            projectileTrail = -1;
            projectilesOnCollision = ProjectileID.FruitcakeChakram;
            damageMult = 1f;
            trailSpeedMult = 15f;
            minProjectiles = 7;
            maxProjectiles = 13;
            fireTime = 15;
            manaUsage = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Lovestruck, 1000);

            EntitySource_Parent s = new EntitySource_Parent(Projectile);
            for (int i = 0; i < 8; i++)
            {
                Projectile.NewProjectile(s, Projectile.Center, new Vector2((float)Math.Cos(i / 8f), (float)Math.Sin(i / 8f)), ModContent.ProjectileType<teleportThrownKey>(), 1, 0, Projectile.owner);
            }
        }

    }

    public class Chacrams_PizzaCut : ChakramProjBase
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Axel/Chacrams_PizzaCut";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pizza Cut");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 72;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 6;
            Projectile.light = 0.5f;

            projectileTrail = -1;
            projectilesOnCollision = ModContent.ProjectileType<PizzaProjectile>();
            damageMult = 1f/8f;
            trailSpeedMult = 0.75f;
            minProjectiles = 7;
            maxProjectiles = 8;
            fireTime = 10;
            manaUsage = 10;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Slow,60);

            EntitySource_Parent s = new EntitySource_Parent(Projectile);
            for (int i = 0; i < 8; i++) {
                Projectile.NewProjectile(s, Projectile.Center,new Vector2((float)Math.Cos(i/8f),(float)Math.Sin(i/8f)), ModContent.ProjectileType<PizzaProjectile>(), (int)(Projectile.damage * damageMult), 0, Projectile.owner);
            }
        }

    }

    public class PizzaProjectile : ModProjectile
    {

        public override string Texture => "Terraria/Images/Item_4029";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pizza slice");
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.timeLeft = 100;
            Projectile.scale = 1.5f;
            Projectile.aiStyle =ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            Vector2 rot = MathHelp.Normalize(Projectile.velocity);
            Projectile.rotation = (float)Math.Atan2(rot.Y, rot.X);
            Projectile.scale = Projectile.timeLeft / 100f*2f;
            Projectile.frame = (int)(Projectile.timeLeft / 100f * 3);
        }

    }

}

/*
Chacrams_Corona
*/