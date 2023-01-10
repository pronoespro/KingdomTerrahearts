using KingdomTerrahearts.Items.Weapons.Bases;
using Microsoft.Xna.Framework;
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
    public class Knives_Etoile : ThrowingKnivesBase
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Etoile");
            Tooltip.SetDefault("Slice the stars with your light");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.autoReuse = true;
            Item.damage = 45;
            Item.useTime = Item.useAnimation = 25;
            Item.knockBack = 0.1f;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Yellow;
            Item.scale = 0.5f;
            Item.shootSpeed = 25;
            Item.UseSound = SoundID.Item19;
            Item.value = 2000;

            knivesToThrow = 6;
            thrownManaUsage = 7;
            meleeDamage = 10;
            thrownDamage = 60;
            meleeSpeed = 15;
            thrownSpeed = 15;
            spread =0.3f;
            maxCombo = 3;

            gravityScale = 0f;
            pierceAmmount = 3;
            collisionProjectile = ProjectileID.StarCannonStar;
            projectileTimeLeft = 35;
            projectileVelMult = 1f;
            manaSteal = 35;

            dashDir = new Vector2(5, 60);
            dashTime = 20;

            tpProjectile = ProjectileID.StarCloakStar;
            tpProjVelMult = 0.5f;
            tpProjIgnoresGround = true;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Knives_Trancheuse>())
                .AddIngredient(ModContent.ItemType<Materials.lightningShard>(), 10)
                .AddIngredient(ItemID.FallenStar, 10)
                .AddIngredient(ItemID.StarCannon)
                .AddTile(TileID.Anvils).Register();
        }

    }
}
