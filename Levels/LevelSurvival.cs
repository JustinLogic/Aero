using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aero
{
    class LevelSurvival : Level
    {
        float time;
        float maxTime;
        float spawnFatBossCooldown;

        public LevelSurvival()
            : base()
        {
            boss = new ShadowBoss();
            done = false;
            time = maxTime = 120.0f;
            spawnKamicazeCooldown = 2.0f;
            spawnFighterCooldown = 2.0f;
            spawnFatBossCooldown = 5.0f;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            if (time > 0)
                time -= (float)elapsedTime.TotalSeconds;
            float freq = time / maxTime;
            //Decide when to spawn first boss.
            if (!fatBoss.Active)
                spawnFatBossCooldown -= (float)elapsedTime.TotalSeconds;
            if (spawnFatBossCooldown < 0)
            {
                if (time <= 0)
                    fatBoss.Difficulty(2);
                else if (time < maxTime / 2)
                    fatBoss.Difficulty(1);
                else
                    fatBoss.Difficulty(0);
                spawnEnemy(fatBoss);
                spawnPowerUp(5);
                objectsSpawned += 1;
                spawnFatBossCooldown = 5.0f;
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
