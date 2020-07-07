using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerSessionDto
{
    public int TutorialStep;
}

public static class PlayerSession
{
    static string Filepath => Path.Combine( Application.persistentDataPath, "player-session.json" );

    static PlayerSessionDto cachedSession;
    static PlayerSessionDto Session
    {
        get
        {
            if ( cachedSession == null )
            {
                cachedSession = new PlayerSessionDto();
                if ( File.Exists( Filepath ) )
                {
                    cachedSession = JsonConvert.DeserializeObject<PlayerSessionDto>( File.ReadAllText( Filepath ) );
                }
            }
            return cachedSession;
        }
        set
        {
            cachedSession = value;
            Debug.Log( Filepath );
            File.WriteAllText( Filepath, JsonConvert.SerializeObject( value, Formatting.Indented ) );
        }
    }

    public static void Reset()
    {
        Session = new PlayerSessionDto();
    }

    public static void IncrementTutorialStep()
    {
        Session = new PlayerSessionDto
        {
            TutorialStep = Session.TutorialStep + 1
        };
    }

    public static int GetTutorialStep()
    {
        return Session.TutorialStep;
    }
}