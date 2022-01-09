#region 'Using' information
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class PlayerData
{
    public string name = "";

    /// <summary>
    /// These ints are used in LevelLoader.cs to show stars (unlocked via Timer.cs) on the level select screen.
    /// </summary>

    public int level1Stars = 0;
    public int level2Stars = 0;
    public int level3Stars = 0;
    public int level4Stars = 0;
    public int level5Stars = 0;

    /// <summary>
    /// These bool-y bad boys are used in LevelLoader.cs to show the next level on the level select screen when the previous level has been completed.
    /// </summary>

    public bool level1Complete;
    public bool level2Complete;
    public bool level3Complete;
    public bool level4Complete;
    public bool level5Complete;
}
