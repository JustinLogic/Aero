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
    class LevelThree : Level
    {
        public LevelThree()
            : base()
        {
            boss = new TwinBoss();
            done = false;
            levelTimeout = maxTimeout;
            spawnKamicazeCooldown = 5.0f;
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
            //Spawn Enemies
            if (!boss.Active)
            {
                //Spawn Fighters
                spawnKamicazeCooldown -= (float)elapsedTime.TotalSeconds;
                if (spawnKamicazeCooldown < 0 && !kamacazie.Active)
                {
                    spawnEnemy(kamacazie);
                    spawnKamicazeCooldown = 5.0f;
                }
                //Spawn Fighters
                spawnFighterCooldown -= (float)elapsedTime.TotalSeconds;
                if (spawnFighterCooldown < 0 && !fighter.Active)
                {
                    spawnEnemy(fighter);
                    spawnFighterCooldown = 2.0f;
                }
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
