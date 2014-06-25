using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aero
{
    class ScoreSystem
    {
        int runningScore;
        public int RunningScore
        {
            get
            {
                return runningScore;
            }
        }
        int totalScore;
        public int TotalScore
        {
            get
            {
                return totalScore;
            }
        }
        string scoreString;

        public ScoreSystem() {
            runningScore = 0;
            totalScore = 0;
            scoreString = "Score: " + runningScore.ToString();
        }

        public void Add(AeroObject item)
        {
            //if (!Level.Boss.Alive)
            if(!GamePlayScreen.levels[GamePlayScreen.level - 1].Boss.Alive)
            {
                if (item is Cruiser)
                {
                    runningScore += 20;
                    totalScore += 20;
                }
                else if (item is Fighter)
                {
                    runningScore += 40;
                    totalScore += 40;
                }
                else if (item is Kamacazie)
                {
                    runningScore += 60;
                    totalScore += 60;
                }
                else if (item is ArmyBoss)
                {
                    runningScore += 500;
                    totalScore += 500;
                }
                else if (item is KamacazieBoss)
                {
                    runningScore += 500;
                    totalScore += 500;
                }
                else if (item is TwinBoss)
                {
                    runningScore += 700;
                    totalScore += 700;
                }
                else if (item is ArtilleryBoss)
                {
                    runningScore += 700;
                    totalScore += 700;
                }
                else if (item is ShadowBoss)
                {
                    runningScore += 1000;
                    totalScore += 1000;
                }
                scoreString = "Score: " + runningScore.ToString(); 
            }
        }

        public void Subtract(int i)
        {
            runningScore -= i;
            scoreString = "Score: " + runningScore.ToString();
        }

        public string ScoreString
        {
            get
            {
                return scoreString;
            }
        }

    }
}
