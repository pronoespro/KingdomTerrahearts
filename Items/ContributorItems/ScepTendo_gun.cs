using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using KingdomTerrahearts.Extra;
using Terraria.ModLoader.IO;
using System.Collections.ObjectModel;
using Terraria.DataStructures;
using Terraria.Audio;

namespace KingdomTerrahearts.Items.ContributorItems
{
    public class ScepTendo_gun:ModItem
    {

        Color[] ItemNameCycleColors = new Color[]{
            new Color(254, 105, 47),
            new Color(190, 30, 209),
            new Color(34, 221, 151),
            new Color(0, 106, 185)
        };

        int shellType=0;
        static string shellDesc= "Based on Pokemon\nShoots razor leaf projectiles";
        int maxShell=5;
        int lastUsedTime;
        TooltipLine line;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Gamer's Ordnance");
            Tooltip.SetDefault("Shoots different types of projectiles" +
                "\nRight click to change switch Shells");
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.width = 60;
            Item.height = 34;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = 100;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.useAnimation = Item.useTime = 10;
            Item.mana = 20;
            Item.noMelee = true;

        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            maxShell = (player.HasItem(ModContent.ItemType<Items.lostDog>() ) || sp.hasZafi) ? 6 : 5;
            if (player.altFunctionUse == 2)
            {
                if (lastUsedTime > 5)
                {
                    SoundEngine.PlaySound(SoundID.Grab, player.Center);
                    shellType = (shellType >=maxShell) ? 0 : shellType + 1;
                }
                lastUsedTime = 0;
                return false;
            }
            else
            {
                switch (shellType)
                {
                    case 0:
                        Item.shoot = ProjectileID.Leaf;
                        Item.damage = 255;
                        Item.shootSpeed = 15;
                        Item.mana = 1;
                        Item.useAnimation = Item.useTime = 3;
                        Item.autoReuse = true;
                        break;
                    case 1:
                        Item.shoot = ModContent.ProjectileType<Projectiles.ScepTend.Persona_projectile>();
                        Item.shootSpeed = 7;
                        Item.damage = 1000;
                        Item.autoReuse = false;
                        Item.mana = 15;
                        Item.useAnimation = Item.useTime = 7;
                        Item.autoReuse = true;
                        break;
                    case 2:
                        Item.shoot = ModContent.ProjectileType<Projectiles.ScepTend.Vergil_projectile>();
                        Item.shootSpeed = 10;
                        Item.damage = 266;
                        Item.autoReuse = false;
                        Item.mana = 10;
                        Item.useAnimation = Item.useTime =25;
                        Item.autoReuse = true;
                        break;
                    case 3:
                        Item.shoot = ModContent.ProjectileType<Projectiles.ScepTend.Wunderwaffe_projectile>();
                        Item.shootSpeed = 10;
                        Item.damage = 400;
                        Item.autoReuse = false;
                        Item.mana = 30;
                        Item.useAnimation = Item.useTime = 75;
                        Item.autoReuse = false;
                        if (isProjectile(Item.shoot))
                            return false;
                        break;
                    case 4:
                        Item.shoot = ModContent.ProjectileType<Projectiles.ScepTend.Escuregot_projectile>();
                        Item.damage = 0;
                        Item.shootSpeed = 10;
                        Item.mana = 25;
                        Item.useAnimation = Item.useTime = 15;
                        Item.autoReuse = false;
                        if (isProjectile(Item.shoot))
                            return false;
                        break;
                    case 5:
                        Item.shoot = ModContent.ProjectileType<Projectiles.ScepTend.halo_projectile>();
                        Item.damage = 500;
                        Item.shootSpeed = 10;
                        Item.mana = 25;
                        Item.useAnimation = Item.useTime = 15;
                        Item.autoReuse = false;
                        if (isProjectile(Item.shoot))
                            return false;
                        break;
                    case 6:
                        Item.shoot = ModContent.ProjectileType<Projectiles.ScepTend.zafiProtector>();
                        Item.damage = 500;
                        Item.shootSpeed = 10;
                        Item.mana = 0;
                        Item.useAnimation = Item.useTime = 15;
                        Item.autoReuse = false;
                        if (isProjectile(Item.shoot))
                            return false;
                        break;
                }
            }
            lastUsedTime = 0;
            return true;
        }

        bool isProjectile(int projType)
        {
            for(int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == projType) return true;
            }
            return false;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = 0;
            Vector2 dir = velocity;

            ProjectileSource_Item s = new ProjectileSource_Item(player, Item);

            switch (shellType)
            {
                case 0:
                    velocity.X += Main.rand.Next(-3, 3);
                    velocity.Y += Main.rand.Next(-3, 3);
                    position += player.velocity;
                    break;

                case 1:
                    proj = Projectile.NewProjectile(s,position, dir + new Vector2(2, 0), ModContent.ProjectileType<Projectiles.ScepTend.Persona_projectile>(), damage, knockback);
                    proj = Projectile.NewProjectile(s,position, dir + new Vector2(-2, 0), ModContent.ProjectileType<Projectiles.ScepTend.Persona_projectile>(), damage, knockback);
                    Main.projectile[proj].owner = Item.playerIndexTheItemIsReservedFor;
                    break;

                case 3:
                    SoundEngine.PlaySound(SoundID.Item12, position);
                    break;

                case 4:
                    Vector2 offset;
                    for (int i = 0; i < 5; i++)
                    {
                        offset = new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1));
                        Projectile.NewProjectile(s,position, dir + offset, ModContent.ProjectileType<Projectiles.ScepTend.Escuregot_explosion>(), 0, 0);
                        SoundEngine.PlaySound(SoundID.NPCHit1, position);
                    }
                    break;

                case 5:
                    proj = Projectile.NewProjectile(s,position, dir + new Vector2(0, -5), ModContent.ProjectileType<Projectiles.ScepTend.halo_projectile>(), damage, knockback);
                    Main.projectile[proj].owner = Item.playerIndexTheItemIsReservedFor;

                    proj = Projectile.NewProjectile(s,position, dir + new Vector2(-5, 0), ModContent.ProjectileType<Projectiles.ScepTend.halo_projectile>(), damage, knockback);
                    Main.projectile[proj].owner = Item.playerIndexTheItemIsReservedFor;

                    proj = Projectile.NewProjectile(s,position, dir + new Vector2(5, 0), ModContent.ProjectileType<Projectiles.ScepTend.halo_projectile>(), damage, knockback);
                    Main.projectile[proj].owner = Item.playerIndexTheItemIsReservedFor;

                    Projectile.NewProjectile(s,position, dir, ModContent.ProjectileType<Projectiles.ScepTend.halo_dust>(), 0, 0);
                    break;

            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void UpdateInventory(Player player)
        {
            lastUsedTime++;
            switch (shellType)
            {
                case 0:
                    Item.color = Color.Green;
                    shellDesc = "Based on Pokemon\nShoots razor leaf projectiles";
                    break;
                case 1:
                    Item.color = Color.Red;
                    shellDesc = "Based on Persona\nSlices enemies uppon contact";
                    break;
                case 2:
                    Item.color = Color.Aquamarine;
                    shellDesc = "Based on Devil May Cry\nExplodes into several slashes in a wide area";
                    break;
                case 3:
                    Item.color = Color.LightBlue;
                    shellDesc = "Based on Call of duty zombies\nShoots a lightning bolt that instantly kills lesser enemies";
                    break;
                case 4:
                    Item.color = Color.Brown;
                    shellDesc = "Based on Monster Hunter Rise\nShoots a Escurego friend to heal you and nearby friends";
                    break;
                case 5:
                    Item.color = Color.Blue;
                    shellDesc = "Based on Halo\nShoots homing projectiles that deal massive damage";
                    break;
                case 6:
                    Item.color = Color.Black;
                    shellDesc = "Shoots a guardian to protect you and kill everything he touches\nHe is a good boy";
                    break;

            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            if(line==null)
                line = new TooltipLine(Mod, "ShellType", "No shell type loaded");

            if (!tooltips.Contains(line))
                tooltips.Add(line);

            foreach(TooltipLine line2 in tooltips)
            {
                if (line2.mod == Mod.Name && line2.Name== "ShellType")
                {
                    line2.text = shellDesc;
                }
            }

            TooltipLine tooltip = new TooltipLine(Mod, "SupportItem", "Supporter Item");
            tooltip.overrideColor = Color.LightBlue;
            if (!tooltips.Contains(tooltip))
            {
                tooltips.Insert(1, tooltip);
            }
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == Mod.Name && line2.Name == "SupportItem")
                {
                    float fade = Main.GameUpdateCount % 60 / 60f;
                    int index = (int)(Main.GameUpdateCount / 60 % 4);
                    line2.overrideColor = Color.Lerp(ItemNameCycleColors[index], ItemNameCycleColors[(index + 1) % 4], fade);
                }
            }
        }

    }
}
