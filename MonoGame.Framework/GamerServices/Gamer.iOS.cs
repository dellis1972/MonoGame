using System;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework.GamerServices
{
	public abstract partial class Gamer
	{
		GamerProfile profile = null;

		public GamerProfile GetProfile()
		{
			if (profile == null)
				profile = new GamerProfile ();
			return profile;
		}

		public Task<GamerProfile> GetProfileAsync ()
		{
			var tcs = new TaskCompletionSource<GamerProfile> ();
			Task.Run (() => {
				tcs.SetResult(GetProfile());
			});
			return tcs.Task;
		}
	}
}

