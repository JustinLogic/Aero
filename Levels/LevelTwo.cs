using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Aero
{
    class LevelTwo : Level
    {
        public LevelTwo()
            : base()
        {
            boss = new ArtilleryBoss();
            done = false;
            levelTimeout = maxTimeout;
            spawnFighterCooldown = 2.0f;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            levelTimeout -= (float)elapsedTime.TotalSeconds;
            //Decide when to spawn first boss.
            if (levelTimeout < 0 && !boss.Alive && bossSpawned == false)
            {
                spawnBoss();
                spawnPowerUp(3);
                objectsSpawned += 1;
            }
            //Spawn Fighters
            spawnFighterCooldown -= (float)elapsedTime.TotalSeconds;
            if (spawnFighterCooldown < 0 && !fighter.Active)
            {
                spawnEnemy(fighter);
                spawnFighterCooldown = 2.0f;
            }
            //Spawn Enemies
            if (!boss.Active)
            {
                //Spawnn Cruisers
                spawnCruiserCooldown -= (float)elapsedTime.TotalSeconds;
                if (spawnCruiserCooldown < 0)
                {
                    spawnCruiserCooldown = 0.5f;
                    foreach (Cruiser e in cruisers)
                        if (!e.Active)
                        {
                            spawnEnemy(e);
                            return;
                        }
                }
            }
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
        }
    }
}
