using System;
using System.Runtime.Serialization;

namespace Microsoft.Xna.Framework.GamerServices
{
    [DataContract]
    public sealed class LeaderboardEntry
    {
	    private long rating = 0;

	    [DataMember]
	    public long Rating
	    {
		    get { return rating; }
		    set {
			    if (rating != value) {
				    rating = value;
			    }
		    }
	    }

        [DataMember]
        public PropertyDictionary Columns { get; internal set; }

        [DataMember]
        public Gamer Gamer { get; internal set; }

	internal LeaderboardKey Key { get; set; }

	public LeaderboardEntry ()
        {
        }
    }
}

