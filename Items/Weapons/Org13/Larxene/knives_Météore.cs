using KingdomTerrahearts.Items.Weapons.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Org13.Larxene
{
    public class knives_Météore : ThrowingKnivesBase
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Météore");
            Tooltip.SetDefault("Fall on your enemies like a meteor");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.autoReuse = true;
            Item.damage = 35;
            Item.useTime = Item.useAnimation = 25;
            Item.knockBack = 0.1f;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Yellow;
            Item.scale = 0.5f;
            Item.shootSpeed = 25;
            Item.UseSound = SoundID.Item19;
            Item.value = 2000;

            knivesToThrow = 10;
            thrownManaUsage = 40;
            meleeDamage = 41;
            thrownDamage = 41;
            meleeSpeed = 17;
            thrownSpeed = 15;
            spread = 1.5f;
            maxCombo = 3;

            gravityScale = 3f;
            pierceAmmount = -1;
            collisionProjectile = ProjectileID.Meteor1;
            projectileTimeLeft = 60;
            projectileVelMult = 2f;
            manaSteal = 5;

            tpProjectile = ProjectileID.Meteor2;
            tpProjVelMult = 1f;
            tpProjIgnoresGround = true;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Knives_Trancheuse>())
                .AddIngredient(ModContent.ItemType<Materials.lightningShard>(), 10)
                .AddIngredient(ItemID.GoldBar, 2)
                .AddIngredient(ItemID.MeteoriteBar,5)
                .AddTile(TileID.Anvils).Register();
        }

    }
}
