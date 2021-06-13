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

namespace KingdomTerrahearts.Items.ContributorItems
{
    public class ScepTendo_gun:ModItem
    {

        Color[] itemNameCycleColors = new Color[]{
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
            item.ranged = true;
            item.width = 60;
            item.height = 34;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = 100;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.useAnimation = item.useTime = 10;
            item.mana = 20;
            item.noMelee = true;

        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            maxShell = (player.HasItem(mod.ItemType("lostDog") ) || sp.hasZafi) ? 6 : 5;
            if (player.altFunctionUse == 2)
            {
                if (lastUsedTime > 5)
                {
                    Main.PlaySound(SoundID.Grab, player.Center);
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
                        item.shoot = ProjectileID.Leaf;
                        item.damage = 255;
                        item.shootSpeed = 15;
                        item.mana = 1;
                        item.useAnimation = item.useTime = 3;
                        item.autoReuse = true;
                        break;
                    case 1:
                        item.shoot = mod.ProjectileType("Persona_projectile");
                        item.shootSpeed = 7;
                        item.damage = 1000;
                        item.autoReuse = false;
                        item.mana = 15;
                        item.useAnimation = item.useTime = 7;
                        item.autoReuse = true;
                        break;
                    case 2:
                        item.shoot = mod.ProjectileType("Vergil_projectile");
                        item.shootSpeed = 10;
                        item.damage = 266;
                        item.autoReuse = false;
                        item.mana = 10;
                        item.useAnimation = item.useTime =25;
                        item.autoReuse = true;
                        break;
                    case 3:
                        item.shoot = mod.ProjectileType("Wunderwaffe_projectile");
                        item.shootSpeed = 10;
                        item.damage = 400;
                        item.autoReuse = false;
                        item.mana = 30;
                        item.useAnimation = item.useTime = 75;
                        item.autoReuse = false;
                        if (isProjectile(item.shoot))
                            return false;
                        break;
                    case 4:
                        item.shoot = mod.ProjectileType("Escuregot_projectile");
                        item.damage = 0;
                        item.shootSpeed = 10;
                        item.mana = 25;
                        item.useAnimation = item.useTime = 15;
                        item.autoReuse = false;
                        if (isProjectile(item.shoot))
                            return false;
                        break;
                    case 5:
                        item.shoot = mod.ProjectileType("halo_projectile");
                        item.damage = 500;
                        item.shootSpeed = 10;
                        item.mana = 25;
                        item.useAnimation = item.useTime = 15;
                        item.autoReuse = false;
                        if (isProjectile(item.shoot))
                            return false;
                        break;
                    case 6:
                        item.shoot = mod.ProjectileType("zafiProtector");
                        item.damage = 500;
                        item.shootSpeed = 10;
                        item.mana = 0;
                        item.useAnimation = item.useTime = 15;
                        item.autoReuse = false;
                        if (isProjectile(item.shoot))
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

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int proj=0;
            Vector2 dir = new Vector2(speedX, speedY);
            switch (shellType)
            {
                case 0:
                    speedY += Main.rand.Next(-3, 3);
                    speedX += Main.rand.Next(-3, 3);
                    position += player.velocity;
                    break;

                case 1:
                    proj = Projectile.NewProjectile(position, dir + new Vector2(2, 0), mod.ProjectileType("Persona_projectile"), damage, knockBack);
                    proj = Projectile.NewProjectile(position, dir + new Vector2(-2, 0), mod.ProjectileType("Persona_projectile"), damage, knockBack);
                    Main.projectile[proj].owner = item.owner;
                    break;

                case 3:
                    Main.PlaySound(SoundID.Item12, position);
                    break;

                case 4:
                    Vector2 offset;
                    for (int i = 0; i < 5; i++)
                    {
                        offset = new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1));
                        Projectile.NewProjectile(position, dir+offset, mod.ProjectileType("Escuregot_explosion"), 0, 0);
                        Main.PlaySound(SoundID.NPCHit1,position);
                    }
                    break;

                case 5:
                    proj=Projectile.NewProjectile(position, dir + new Vector2(0, -5), mod.ProjectileType("halo_projectile"), damage, knockBack);
                    Main.projectile[proj].owner = item.owner;

                    proj = Projectile.NewProjectile(position, dir + new Vector2(-5,0), mod.ProjectileType("halo_projectile"), damage, knockBack);
                    Main.projectile[proj].owner = item.owner;

                    proj = Projectile.NewProjectile(position, dir + new Vector2(5, 0), mod.ProjectileType("halo_projectile"), damage, knockBack);
                    Main.projectile[proj].owner = item.owner;

                    Projectile.NewProjectile(position, dir, mod.ProjectileType("halo_dust"), 0, 0);
                    break;

            }
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool UseItem(Player player)
        {

            return base.UseItem(player);
        }

        public override void UpdateInventory(Player player)
        {
            lastUsedTime++;
            switch (shellType)
            {
                case 0:
                    item.color = Color.Green;
                    shellDesc = "Based on Pokemon\nShoots razor leaf projectiles";
                    break;
                case 1:
                    item.color = Color.Red;
                    shellDesc = "Based on Persona\nSlices enemies uppon contact";
                    break;
                case 2:
                    item.color = Color.Aquamarine;
                    shellDesc = "Based on Devil May Cry\nExplodes into several slashes in a wide area";
                    break;
                case 3:
                    item.color = Color.LightBlue;
                    shellDesc = "Based on Call of duty zombies\nShoots a lightning bolt that instantly kills lesser enemies";
                    break;
                case 4:
                    item.color = Color.Brown;
                    shellDesc = "Based on Monster Hunter Rise\nShoots a Escurego friend to heal you and nearby friends";
                    break;
                case 5:
                    item.color = Color.Blue;
                    shellDesc = "Based on Halo\nShoots homing projectiles that deal massive damage";
                    break;
                case 6:
                    item.color = Color.Black;
                    shellDesc = "Shoots a guardian to protect you and kill everything he touches\nHe is a good boy";
                    break;

            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            if(line==null)
                line = new TooltipLine(mod, "ShellType", "No shell type loaded");

            if (!tooltips.Contains(line))
                tooltips.Add(line);

            foreach(TooltipLine line2 in tooltips)
            {
                if (line2.mod == mod.Name && line2.Name== "ShellType")
                {
                    line2.text = shellDesc;
                }
            }

            TooltipLine tooltip = new TooltipLine(mod, "SupportItem", "Supporter Item");
            tooltip.overrideColor = Color.LightBlue;
            if (!tooltips.Contains(tooltip))
            {
                tooltips.Insert(1, tooltip);
            }
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == mod.Name && line2.Name == "SupportItem")
                {
                    float fade = Main.GameUpdateCount % 60 / 60f;
                    int index = (int)(Main.GameUpdateCount / 60 % 4);
                    line2.overrideColor = Color.Lerp(itemNameCycleColors[index], itemNameCycleColors[(index + 1) % 4], fade);
                }
            }
        }

    }
}
