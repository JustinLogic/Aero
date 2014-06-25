using System;
using System.Collections.Generic;
using System.Text;

namespace Aero
{
    public enum GameState
    {
        Exit,
        None,
        TitleScreen,
        GameScreen
    }

    public enum PowerUpType
    {
        None,
        RapidFire,
        TriCannon,
        QuintuCannon,
        Invincibility,
        Shop
    }

    public enum RoundCurve
    {
        None,
        Degree45,
        DegreeNegative45,
        QuintuOutside,
        QuintuInside,
        QuintuNegativeOutside,
        QuintuNegativeInside,
    }
}
