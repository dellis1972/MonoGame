using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.GamerServices
{
	internal class MonoGameGamerServicesHelper
	{
		private static MonoLiveGuide guide = null;

		public static void ShowSigninSheet()
		{
			if (Guide.IsVisible)
				return;

			guide.Enabled = true;
			guide.Visible = true;
			Guide.IsVisible = true;
			if (SignedInGamer.SignedInGamers.Count == 0) {
				#if !OUYA
				GooglePlayHelper.Instance.SignIn ();
				#endif
			}
		}

		internal static void Initialise(Game game)
		{
			if (guide == null)
			{
				guide = new MonoLiveGuide(game);                
				game.Components.Add(guide);
			}
		}}

	internal class MonoLiveGuide : DrawableGameComponent
	{
		public MonoLiveGuide(Game game)
			: base(game)
		{
			this.Enabled = false;
			this.Visible = false;
			//Guide.IsVisible = false;
			this.DrawOrder = Int32.MaxValue;
			#if !OUYA
			GooglePlayHelper.Instance.OnSignInFailed += OnSignInFailed;
			GooglePlayHelper.Instance.OnSignedIn += OnSignInSucceeded;
			GooglePlayHelper.Instance.Initialize ();
			#endif
		}

		public override void Initialize()
		{
			base.Initialize();
			#if !OUYA
			GooglePlayHelper.Instance.Start ();
			#endif
		}
			

		protected override void LoadContent()
		{
			base.LoadContent();
		}

		protected override void UnloadContent()
		{
			base.UnloadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}

		TimeSpan gt = TimeSpan.Zero;
		TimeSpan last = TimeSpan.Zero;
		int delay = 2;

		public override void Update(GameTime gameTime)
		{
            OnSignInFailed(this, EventArgs.Empty);
			base.Update(gameTime);
		}

		#region IGameHelperListener implementation
		public void OnSignInFailed (object sender, EventArgs e)
		{
			string name = "androiduser";
			try
			{
				Android.Accounts.AccountManager mgr = (Android.Accounts.AccountManager)Android.App.Application.Context.GetSystemService(Android.App.Activity.AccountService);
				if (mgr != null)
				{
					var accounts = mgr.GetAccounts();
					if (accounts != null && accounts.Length > 0)
					{							
						name = accounts[0].Name;
						if (name.Contains("@"))
						{
							// its an email 
							name = name.Substring(0, name.IndexOf("@"));
						}
					}
				}
			}
			catch
			{
			}
			if (Gamer.SignedInGamers.FirstOrDefault (x => x.DisplayName == name) == null) {
				SignedInGamer sig = new SignedInGamer ();
				sig.DisplayName = name;
				sig.Gamertag = name;
				sig.IsSignedInToLive = false;

				Gamer.SignedInGamers.Add (sig);
                #if __ANDROID__ && !OUYA
				sig.SignIn ();
                #endif
			}

			this.Visible = false;
			this.Enabled = false;
			Guide.IsVisible = false;
		}
		public void OnSignInSucceeded (object sender, EventArgs e)
		{
			#if !OUYA
			var name = GooglePlayHelper.Instance.PlayerId;
			if (name.Contains ("@")) {
				// its an email 
				name = name.Substring (0, name.IndexOf ("@"));
			}
			var tag = name;
			if (string.IsNullOrWhiteSpace (GooglePlayHelper.Instance.PlayerId)) {
				tag = GooglePlayHelper.Instance.PlayerId;
			}

			if (Gamer.SignedInGamers.FirstOrDefault(x => x.DisplayName == name) == null) {
				SignedInGamer sig = new SignedInGamer ();
				sig.LeaderboardWriter = new LeaderboardWriter ();
				sig.DisplayName = name;
				sig.Gamertag = tag;
				sig.IsSignedInToLive = true;
				Gamer.SignedInGamers.Add(sig);
				sig.SignIn ();
			}

			this.Visible = false;
			this.Enabled = false;
			Guide.IsVisible = false;

			GooglePlayHelper.Instance.LoadAchievements();
			#endif

		}
		#endregion
	}
}


