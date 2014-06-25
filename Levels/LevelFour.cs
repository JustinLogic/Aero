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
    class LevelFour : Level
    {
        private Kamacazie[] kamacazieWave;
        float waveTimer = 0;
        float waveTimerMax = 10.0f;

        public LevelFour()
            : base()
        {
            boss = new KamacazieBoss();
            done = false;
            levelTimeout= maxTimeout;
            spawnKamicazeCooldown = 2.0f;
            spawnFighterCooldown = 2.0f;
            kamacazieWave = new Kamacazie[3];
            for (int i = 0; i < 3; i++)
            {
                kamacazieWave[i] = new Kamacazie();
            }
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
            //Spawn Enemies
            if (!boss.Active)
            {
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
                //Spawn Cruisers
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
            else
            {
                waveTimer += (float)elapsedTime.TotalSeconds;
                if (waveTimer >= waveTimerMax)
                {
                    SpawnWave();
                    waveTimer = 0;
                }
            }
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
        }

        private void SpawnWave()
        {
            for (int i = 0; i < 3; i++)
            {
                kamacazieWave[i].spawn(new Vector2(rand.Next(viewportRect.Width - kamacazieWave[i].Texture.Width), 0), Player.Position.X);
                activeObjects.Add(kamacazieWave[i]);
            }
        }
    }
}
