/// <summary>
/// The names used to identify important entities in the game
/// Names refer to individual GameObjects
/// </summary>
public static class Names
{
    public const string PowerUp = "Power Up";
    public const string HealthKit = "Health Kit";

    public const string BlueFlag = "Blue Flag";
    public const string RedFlag = "Red Flag";

    public const string RedBase = "Red Base";
    public const string BlueBase = "Blue Base";

    public const string BlueTeamMember1 = "Blue Team Member 1";
    public const string BlueTeamMember2 = "Blue Team Member 2";
    public const string BlueTeamMember3 = "Blue Team Member 3";

    public const string RedTeamMember1 = "Red Team Member 1";
    public const string RedTeamMember2 = "Red Team Member 2";
    public const string RedTeamMember3 = "Red Team Member 3";
}

/// <summary>
/// The tags used to identify important entity groups in the game
/// Tags refer to groups of related objects
/// </summary>
public static class Tags
{
    public const string BlueTeam = "Blue Team";
    public const string RedTeam = "Red Team";
    public const string Collectable = "Collectable";
    public const string Flag = "Flag";
}

/// <summary>The delays for various states.</summary>
public static class AISystem
{
    /// <summary>The delay to use for processing global state logic.</summary>
    public const float GlobalDelay = 0.5f;
    /// <summary>The delay to use for processing current state logic.</summary>
    public const float CurrentDelay = 0.5f;
    /// <summary>The bias value to use when weighting normal bots.</summary>
    public const float NormalCharacterBias = .5f;
    /// <summary>The bias to use when weighting cowardly bots.</summary>
    public const float CowardCharacterBias = 0.25f;
    /// <summary>The bias to use when weighting aggressive bots.</summary>
    public const float AggressorCharacterBias = 1f;
}