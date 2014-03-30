using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class LeaderboardWriter : IDisposable
    {
	private SignedInGamer gamer;

	internal static Queue<LeaderboardEntry> pendingUpdates = new Queue<LeaderboardEntry> ();

	internal LeaderboardWriter (SignedInGamer gamer)
	{
		this.gamer = gamer;
	}

        public LeaderboardWriter ()
        {
        }

	public LeaderboardEntry GetLeaderboard (LeaderboardIdentity leaderboardId)
        {
		var entry =  new LeaderboardEntry () { Gamer = gamer, Key =leaderboardId.Key };
		pendingUpdates.Enqueue (entry);
		return entry;
        }

        #region IDisposable implementation

        void IDisposable.Dispose ()
        {
           
        }

        #endregion
    }
}

