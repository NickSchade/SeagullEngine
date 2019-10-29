using UnityEngine;

public static class FBuildQueue
{
    public static IBuildQueue Make(eBuildQueue type, Player player)
    {
        if (type == eBuildQueue.Brute)
        {
            return new BuildQueueBrute(player);
        }
        else if (type == eBuildQueue.Elegant)
        {
            return new BuildQueueElegant(player);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
