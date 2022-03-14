using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.IO;
using KingdomTerrahearts.Extra;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Microsoft.Xna.Framework;

namespace KingdomTerrahearts.structures
{
    public class StructureGenerator: ModSystem
    {

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int structureIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Disney Worlds"));
			if (structureIndex!=-1)
			{
				tasks.RemoveAt(structureIndex);
            }
			structureIndex = tasks.FindIndex(genpass => genpass.Name.ToLower().Contains("micro biomes"));
			tasks.Insert(structureIndex + 1, new PassLegacy("Disney Worlds", GenerateDisneyWorlds));
		}

        public void GenerateDisneyWorlds(GenerationProgress p,GameConfiguration conf)
        {
			StructureComplete cloud = StructureData.GetStructure("OlimpusCloud1");
			Vector2 structPos = new Vector2(Main.spawnTileX / 2, (int)(cloud.blocks.element.GetLength(0) / 4f * 3f) + 20);
			MakeCloud(cloud, structPos);

			cloud = StructureData.GetStructure("OlimpusCloud2");
			structPos.X += cloud.blocks.element.GetLength(1);
			MakeCloud(cloud, structPos);

			cloud = StructureData.GetStructure("OlimpusLake");
			structPos.X += cloud.blocks.element.GetLength(1);
			MakeCloud(cloud, structPos);

		}

        #region Make structures

		public void MakeCloud(StructureComplete cloud, Vector2 structPos)
		{
			if (cloud != null && cloud.blocks != null)
			{
				MakeStructureOnSky(cloud, (int)structPos.X, (int)structPos.Y, true);
			}
		}

		private void MakeStructureOnSky(StructureComplete structure,int posX=-1,int posY=-1, bool forceMake=false)
		{
            if (structure == null)
            {
				return;
			}

			if (forceMake)
			{
				PlaceStructure(posX,posY, structure);

				return;
			}
			else
			{

				float widthScale = Main.maxTilesX * 1f / structure.blocks.element.GetLength(0);
				int numberToGenerate = WorldGen.genRand.Next(1, (int)(2f * widthScale));
				for (int k = 0; k < numberToGenerate; k++)
				{
					bool success = false;
					int attempts = 0;
					while (!success)
					{
						attempts++;
						if (attempts > 1000 && !forceMake)
						{
							success = true;
							continue;
						}

						int i = WorldGen.genRand.Next(300, Main.maxTilesX / 2 - 300);
						if (i <= Main.maxTilesX / 2 - 50 || i >= Main.maxTilesX / 2 + 50)
						{
							int j = (int)Math.Clamp((Main.worldSurface / 2 - WorldGen.genRand.Next(structure.blocks.element.GetLength(1))), 0, Main.worldSurface);

							if (j > 0)
							{
								bool placementOK = true;
								for (int l = i - structure.blocks.element.GetLength(0) / 2; l < i + structure.blocks.element.GetLength(0) / 2; l++)
								{
									for (int m = j - structure.blocks.element.GetLength(1) / 2; m < j + structure.blocks.element.GetLength(1) / 2; m++)
									{
										if (Main.tile[l, m].TileType!=null)
										{
											int type = (int)Main.tile[l, m].TileType;
											if (type == TileID.BlueDungeonBrick || type == TileID.GreenDungeonBrick || type == TileID.PinkDungeonBrick || type == TileID.Cloud || type == TileID.RainCloud)
											{
												placementOK = forceMake;
											}
										}
									}
								}
								if (placementOK || forceMake)
								{
									success = PlaceStructure(i, j, structure);
								}
							}
						}
					}
				}
			}
		}

		public bool PlaceStructure(int i, int j,StructureComplete structure)
		{
			int tileType;
			for (int structY = 0; structY < structure.ReturnLength(1); structY++)
			{
				for (int structX = 0; structX < structure.ReturnLength(0); structX++)
				{
					int k = i - (structure.ReturnLength(0)/2) + structX;
					int l = j - (structure.ReturnLength(1)/2) + structY;

					int flippedY = (structure.ReturnLength(1)) - structY-1;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);

						if (structure.blocks.types[structure.blocks.element[flippedY, structX]] >= 0 && structure.hasSlopes)
						{
							//the type of block this is
							tile.TileType = (ushort)structure.blocks.types[structure.blocks.element[flippedY, structX]];
							tile.ClearTile();

							//checking if the block is a platform and checking the type it is
							tileType = structure.blocks.types[structure.blocks.element[flippedY, structX]];
							//placing the block
							WorldGen.PlaceTile(k, l, tileType, false,false,-1,style:(tileType==TileID.Platforms)?structure.platformsType:0);

							switch (structure.blockSlopes[flippedY, structX])
							{
								default:
								case 0:
									break;
								case 1:
									tile.IsHalfBlock = true;
									break;
								case 2:
									tile.Slope = SlopeType.SlopeDownRight;
									break;
								case 3:
									tile.Slope = SlopeType.SlopeDownLeft;
									break;
								case 4:
									tile.Slope = SlopeType.SlopeUpRight;
									break;
								case 5:
									tile.Slope = SlopeType.SlopeUpLeft;
									break;
							}

							if (structure.hasBlockPaint && flippedY < structure.blockColors.element.GetLength(0) && structX<structure.blockColors.element.GetLength(1))
							{
								WorldGen.paintTile(k, l, (byte)structure.blockColors.types[structure.blockColors.element[flippedY, structX]]);
							}

						} else
						{
							tile.ClearTile();
						}

						//Place furniture
                        if (structure.containsFurniture && 
							flippedY< structure.furnitureTiles.element.GetLength(0) && structX<structure.furnitureTiles.element.GetLength(1))
                        {

                            if (structure.furnitureTiles.types[structure.furnitureTiles.element[flippedY, structX]]>=0)
                            {
								WorldGen.PlaceTile(k, l, structure.furnitureTiles.types[structure.furnitureTiles.element[flippedY, structX]], mute: true, forced:true,-1,structure.furnitureStyles.types[structure.furnitureStyles.element[flippedY,structX]]);
                            }

                        }

						//Place chests
						if(structure.containsChests && flippedY<structure.chests.element.GetLength(0) && structX < structure.chests.element.GetLength(1))
                        {
                            if (structure.chests.types[structure.chests.element[flippedY, structX]] >= 0)
                            {
								int chestIndex=WorldGen.PlaceChest(k, l, style: structure.structureChestStyle);

                                if (chestIndex >= 0)
                                {
									for(int item = 0; item < structure.chestPosibleContent.GetLength(structure.chests.types[structure.chests.element[flippedY,structX]]);item++)
                                    {
										Main.chest[chestIndex].item[item].SetDefaults(
											structure.chestPosibleContent[structure.chests.types[structure.chests.element[flippedY, structX]],item]);
                                    }
                                }
                            }
                        }

						//Actuate blocks
						if (structure.hasActuatedBlocks &&
							structure.actuatedBlocks.GetLength(0)>=flippedY && structure.actuatedBlocks.GetLength(1)>=structX)
						{
							tile.IsActuated = structure.actuatedBlocks[flippedY, structX];
						}

						if (structure.walls.types[structure.walls.element[flippedY, structX]] != WallID.None)
						{
							ushort wall = (ushort)structure.walls.types[structure.walls.element[flippedY, structX]];
							tile.WallType =(ushort)(wall==0?WallID.None:wall);

							if (structure.hasWallPaint && structure.wallColors.element.GetLength(0)>flippedY && structure.wallColors.element.GetLength(1)>structX)
							{
								tile.WallColor = (byte)structure.wallColors.types[structure.wallColors.element[flippedY, structX]];
							}
                        }
                        else
						{
							WorldUtils.ClearWall(k, l);
						}

						if (structure.containsLiquids && structure.liquids.types[structure.liquids.element[flippedY, structX]] > 0)
						{
							tile.LiquidAmount = (byte)structure.liquids.types[structure.liquids.element[flippedY, structX]];
						}
					}
				}
			}
			return true;
		}

        #endregion
    }

}
