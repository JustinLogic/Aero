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
    class LevelOne : Level
    {
        public LevelOne()
            : base()
        {
            boss = new ArmyBoss();
            done = false;
            levelTimeout = maxTimeout;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            levelTimeout -= (float)elapsedTime.TotalSeconds;
            //Decide when to spawn first boss.
            if (levelTimeout < 0 && !boss.Active && bossSpawned == false)
            {
                spawnBoss();
                spawnPowerUp(3);
                objectsSpawned += 1;
            }
            //Decide when to spawn cruisers
            if (!boss.Exploding)
            {
                spawnCruiserCooldown -= (float)elapsedTime.TotalSeconds;
                if (spawnCruiserCooldown < 0)
                {
                    if (boss.Active)
                        spawnCruiserCooldown = 1.0f;
                    else
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
