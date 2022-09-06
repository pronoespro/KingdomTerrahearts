using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Bases
{


    public abstract class RodBase : ModItem
    {

        public enum projShootType
        {
            normal,
            circular,
            square,
            enix,
            rand
        }

        //Use related
        public int[] damages = new int[] { 1, 1 };
        public int[] projectiles = new int[] { ProjectileID.EnchantedBoomerang, ProjectileID.IceBoomerang };
        public int[] projectilCombos = new int[] { 2, 1 };

        //Extra projectile related
        public int[] projectileAmmount = new int[] { 1, 5 };
        public projShootType[] shootType = new projShootType[] { projShootType.normal, projShootType.rand };
        public float[] projectileDistanceWhenShot = new float[] { 0, 10 };
        public float[] shootSpeeds = new float[] { 15,25};

        //Combo related
        public int curCombo;
        public int curProjectile;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.holdStyle = -1;
            Item.useTime = Item.useAnimation = 15;
            Item.width = Item.height = 26;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.DamageType = DamageClass.Magic;
        }

        public override bool CanUseItem(Player player)
        {

            curCombo++;
            if (curCombo >= projectilCombos[curProjectile])
            {
                curCombo = 0;
                curProjectile++;
                if (curProjectile >= projectiles.Length)
                {
                    curProjectile = 0;
                }
            }

            Item.shoot = projectiles[curProjectile];
            Item.damage = damages[curProjectile];
            Item.shootSpeed = shootSpeeds[curProjectile];

            return base.CanUseItem(player);

        }

        public override Nullable<bool> UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (shootType[curProjectile] != projShootType.normal)
            {

                Vector2 offset;
                switch (shootType[curProjectile])
                {
                    case projShootType.circular:

                        for (int i = 0; i < projectileAmmount[curProjectile]; i++)
                        {
                            float curTangent = (float)Math.PI * 2 * (projectileAmmount[curProjectile] / 1f / i);
                            offset = new Vector2((float)Math.Sin(curTangent), (float)Math.Cos(curTangent)) * projectileDistanceWhenShot[curProjectile];

                            Projectile.NewProjectile(source, position + offset, velocity, type, damage, knockback, player.whoAmI);
                        }

                        break;
                    case projShootType.enix:
                        for (int i = 0; i < projectileAmmount[curProjectile]; i++)
                        {
                            float distance;

                            if (i % 2 == 0)
                            {
                                distance = (i / projectileAmmount[curProjectile]) * projectileDistanceWhenShot[curProjectile] - projectileDistanceWhenShot[curProjectile] / 2f;


                                offset = new Vector2(distance, distance);
                            }
                            else
                            {
                                distance = (i / projectileAmmount[curProjectile]) * projectileDistanceWhenShot[curProjectile] - projectileDistanceWhenShot[curProjectile] / 2f;


                                offset = new Vector2(-distance, distance);
                            }

                            Projectile.NewProjectile(source, position + offset, velocity, type, damage, knockback, player.whoAmI);
                        }
                        break;
                    case projShootType.rand:

                        for (int i = 0; i < projectileAmmount[curProjectile]; i++)
                        {
                            offset = new Vector2(
                                Main.rand.NextFloat(-projectileDistanceWhenShot[curProjectile], projectileDistanceWhenShot[curProjectile]),
                                Main.rand.NextFloat(-projectileDistanceWhenShot[curProjectile], projectileDistanceWhenShot[curProjectile]));

                            Projectile.NewProjectile(source, position + offset, velocity, type, damage, knockback, player.whoAmI);
                        }
                        break;
                    case projShootType.square:
                        break;
                }

                return false;
            }
            else
            {
                for (int i = 0; i < projectileAmmount[curProjectile] - 1; i++)
                {
                    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                }
                return true;
            }

        }

        public override void HoldItemFrame(Player player)
        {
            player.bodyFrame.Y = player.bodyFrame.Height*3;
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation.X = player.Center.X + player.width * (player.direction<=0?1:0);
            player.itemLocation.Y = player.Center.Y;
            if (player.direction < 0)
            {
                player.itemRotation=(float)Math.PI / 4 * -2;
            }
            else
            {
                player.itemRotation = (float)Math.PI / 4 * 2;
            }
        }
    }
}
