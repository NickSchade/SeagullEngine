using UnityEngine;

public static class StructureFactory
{
    public static HomelandsStructure Make(HomelandsGame game, dStructurePlacement data)
    {
        eGame gameType = game._settings._gameType;
        Debug.Log($@"Created Structure({gameType})");

        if (gameType == eGame.Exodus)
        {
            return new ExodusStructure(game, data);
        }
        else if (gameType == eGame.Sandbox)
        {
            return new SandboxStructure(game, data);
        }
        else
        {
            return new SandboxStructure(game, data);
            //throw new System.NotImplementedException();
        }
    }
}
