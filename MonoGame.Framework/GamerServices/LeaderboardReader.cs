using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.GamerServices
{
    public sealed class LeaderboardReader : IDisposable
    {
        public LeaderboardReader ()
        {
        }

        /*
        public IAsyncResult BeginPageDown(AsyncCallback aAsyncCallback, object aAsyncState)
        {
            throw new NotImplementedException ();
        }

        public IAsyncResult BeginPageUp(AsyncCallback aAsyncCallback, object aAsyncState)
        {
            throw new NotImplementedException ();
        }

        public LeaderboardReader EndPageDown(IAsyncResult result)
        {
            throw new NotImplementedException ();
        }

        public LeaderboardReader EndPageUp(IAsyncResult result)
        {
            throw new NotImplementedException ();
        }*/

	public static IAsyncResult BeginRead (LeaderboardIdentity id, SignedInGamer gamer, int leaderboardPageSize, AsyncCallback leaderboardReadCallback, SignedInGamer gamer2)
        {
		
		var ar = new LeaderboardAsyncResult (null);
		DoLoadLeaderboard (id, gamer, leaderboardPageSize, leaderboardReadCallback, ar);
		return ar;
        }

	public static IAsyncResult BeginRead (LeaderboardIdentity id, IEnumerable<Gamer> gamers, Gamer pivotGamer, int pageSize, AsyncCallback callback, Object asyncState)
	{
		var ar = new LeaderboardAsyncResult (asyncState);
		DoLoadLeaderboard (id, pivotGamer, pageSize, callback, ar);
		return ar;
	}

        public static LeaderboardReader EndRead(IAsyncResult result)
        {
		var aad = (LeaderboardAsyncResult)result;
		return new LeaderboardReader () { Entries =aad.Entries, 
			CanPageDown = false, 
			CanPageUp = false 
		};
        }

	static void DoLoadLeaderboard (LeaderboardIdentity id, Gamer gamer, int leaderboardPageSize, AsyncCallback callback, LeaderboardAsyncResult ar)
	{
#if ANDROID
#if !OUYA
		if (GameHelper.Instance != null) {
			GameHelper.Instance.LoadLeaderboard (Game.Activity.GetLeaderBoardId (id.Key), () => {
				ar.Entries = GameHelper.Instance.Entries;
				callback (ar);
			});
		}
#endif
#endif
	}
	
	/*
        public void PageDown()
        {
            throw new NotImplementedException ();
        }

        public void PageUp()
        {
            throw new NotImplementedException ();
        }
        */

        public bool CanPageDown {
            get;
            set;
        }

        public bool CanPageUp {
            get;
            set;
        }

        public IEnumerable<LeaderboardEntry> Entries {
            get;
            set;
        }

        #region IDisposable implementation

        public void Dispose ()
        {
            
        }

        #endregion

	public class LeaderboardAsyncResult : IAsyncResult
	{
		public object AsyncState
		{
			get;
			set;
		}

		public System.Threading.WaitHandle AsyncWaitHandle
		{
			get { return null; }
		}

		public bool CompletedSynchronously
		{
			get { return false; }
		}

		public bool IsCompleted
		{
			get;
			set;
		}

		public LeaderboardAsyncResult (object asyncState)
		{
			// TODO: Complete member initialization
			this.AsyncState = asyncState;
		}

		public List<Microsoft.Xna.Framework.GamerServices.LeaderboardEntry> Entries = new List<LeaderboardEntry> ();

	}
    }
}

