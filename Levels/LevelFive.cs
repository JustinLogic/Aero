using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aero
{
    class LevelFive : Level
    {
        public LevelFive()
            : base()
        {
            boss = new ShadowBoss();
            done = false;
            levelTimeout = maxTimeout;
            spawnKamicazeCooldown = 2.0f;
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
            //Spawn Kamicazie
            spawnKamicazeCooldown -= (float)elapsedTime.TotalSeconds;
            if (spawnKamicazeCooldown < 0 && !kamacazie.Active)
            {
                spawnEnemy(kamacazie);
                spawnKamicazeCooldown = 2.0f;
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
