using System;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Games.Achievement;
using Android.Gms.Games.LeaderBoard;
using Android.Gms.Games;
using Android.App;
using Android.Content;
using Android.Views;
using Java.Interop;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.GamerServices
{
	/// <summary>
	/// Basic wrapper for interfacing with the GooglePlayServices Game API's
	/// </summary>
    public class GooglePlayHelper: Java.Lang.Object, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
	{
		private static GooglePlayHelper instance = null;

		internal static GooglePlayHelper Instance {
			get {
				if (instance == null) {
					instance = new GooglePlayHelper ();
				}
				return instance;
			}
		}

		GoogleApiClient client;
		bool signedOut = true;
		bool signingin = false;
		bool resolving = false;
		List<Achievement> achievments = new List<Achievement>();
		Dictionary<string, List<LeaderboardEntry>> scores = new Dictionary<string, List<LeaderboardEntry>> ();
		AchievementsCallback achievmentsCallback;
		LeaderBoardsCallback leaderboardsCallback;

		const int REQUEST_LEADERBOARD = 9002;
		const int REQUEST_ALL_LEADERBOARDS = 9003;
		const int REQUEST_ACHIEVEMENTS = 9004;
		const int RC_RESOLVE = 9001;

		/// <summary>
		/// Gets or sets a value indicating whether the user is signed out or not.
		/// </summary>
		/// <value><c>true</c> if signed out; otherwise, <c>false</c>.</value>
		public bool SignedOut {
			get { return signedOut; }
			set {
				if (signedOut != value) {
					signedOut = value;
					// Store if we Signed Out so we don't bug the player next time.
					using (var settings = Activity.GetSharedPreferences ("googleplayservicessettings", FileCreationMode.Private)) {
						using (var e = settings.Edit ()) {
							e.PutBoolean ("SignedOut", signedOut);
							e.Commit ();
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the gravity for the GooglePlay Popups. 
		/// Defaults to Bottom|Center
		/// </summary>
		/// <value>The gravity for popups.</value>
		public GravityFlags GravityForPopups { get; set; }

		/// <summary>
		/// The View on which the Popups should show
		/// </summary>
		/// <value>The view for popups.</value>
		public View ViewForPopups {get;set;}

		/// <summary>
		/// This event is fired when a user successfully signs in
		/// </summary>
		public event EventHandler OnSignedIn;

		/// <summary>
		/// This event is fired when the Sign in fails for any reason
		/// </summary>
		public event EventHandler OnSignInFailed;

		/// <summary>
		/// This event is fired when the user Signs out
		/// </summary>
		public event EventHandler OnSignedOut;

		/// <summary>
		/// List of Achievements. Populated by LoadAchievements
		/// </summary>
		/// <value>The achievements.</value>
		public List<Achievement> Achievements { 
			get { return achievments; }
		}

		public List<LeaderboardEntry> GetScores(string index) {
			return scores[index];
		}

		public bool Available {
			get { 
				return GooglePlayServicesUtil.IsGooglePlayServicesAvailable (Activity) == 0;
			}
		}

		public string PlayerId {
			get { 
				using (var settings = Activity.GetSharedPreferences ("googleplayservicessettings", FileCreationMode.Private)) {
					return settings.GetString ("playerid", String.Empty);
				}
			}
		}

		[CLSCompliant(false)]
		public AndroidGameActivity Activity {
			get { return Game.Activity; }
		}

		public GooglePlayHelper ()
		{
			this.GravityForPopups = GravityFlags.Bottom | GravityFlags.Center;
			this.ViewForPopups = Game.Instance.Services.GetService<View>();
			achievmentsCallback = new AchievementsCallback (this);
			leaderboardsCallback = new LeaderBoardsCallback (this);

			Activity.OnActivityResultEvent += (requestCode, resultCode, data) => {
				OnActivityResult(requestCode, resultCode, data);
			};

			Activity.OnStartEvent += (sender, e) => {
				Start();
			};
			Activity.OnStopEvent += (sender, e) => {
				Stop();
			};
		}

		public void Initialize() {

			using (var settings = Activity.GetSharedPreferences ("googleplayservicessettings", FileCreationMode.Private)) {
				signedOut = settings.GetBoolean ("SignedOut", true);

				if (!signedOut)
					CreateClient ();
			}
		}

		private void CreateClient() {

            var settings = Activity.GetSharedPreferences ("googleplayservicessettings", FileCreationMode.Private);
            var id = settings.GetString ("playerid", String.Empty);

            var builder = new GoogleApiClient.Builder (Activity, this, this);
            builder.AddApi (Android.Gms.Games.GamesClass.API);
            builder.AddScope (Android.Gms.Games.GamesClass.ScopeGames);
            builder.SetGravityForPopups ((int)GravityForPopups);
            if (ViewForPopups != null)
                builder.SetViewForPopups (ViewForPopups);
            if (!string.IsNullOrEmpty (id)) {
                builder.SetAccountName (id);
            }
            client = builder.Build ();
		}

		/// <summary>
		/// Start the GooglePlayClient. This should be called from your Activity Start
		/// </summary>
		public void Start() {

			if(SignedOut && !signingin)
				return;

			if (client != null && !client.IsConnected) {
				client.Connect ();
			}
		}

		/// <summary>
		/// Disconnects from the GooglePlayClient. This should be called from your Activity Stop
		/// </summary>
		public void Stop() {

			if (client != null && client.IsConnected) {
				client.Disconnect ();
			}
		}

		/// <summary>
		/// Reconnect to google play.
		/// </summary>
		public void Reconnect() {
			if (client != null)
				client.Reconnect ();
		}

		/// <summary>
		/// Sign out of Google Play and make sure we don't try to auto sign in on the next startup
		/// </summary>
		public void SignOut() {

			SignedOut = true;
			if (client.IsConnected) {
				GamesClass.SignOut (client);
				Stop ();
				using (var settings = Activity.GetSharedPreferences ("googleplayservicessettings", FileCreationMode.Private)) {
					using (var e = settings.Edit ()) {
						e.PutString ("playerid",String.Empty);
						e.Commit ();
					}
				}
				client.Dispose ();
				client = null;
				if (OnSignedOut != null)
					OnSignedOut (this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Attempt to Sign in to Google Play
		/// </summary>
		public void SignIn() {

			signingin = true;
			if (client == null)
				CreateClient ();

			if (client.IsConnected)
				return;

			if (client.IsConnecting)
				return;

			var result = GooglePlayServicesUtil.IsGooglePlayServicesAvailable (Activity);
			if (result != ConnectionResult.Success) {
				return;
			}

			Start ();

		}

		/// <summary>
		/// Unlocks the achievement.
		/// </summary>
		/// <param name="achievementCode">Achievement code from you applications Google Play Game Services Achievements Page</param>
		public void UnlockAchievement(string achievementCode) {
			GamesClass.Achievements.Unlock (client, achievementCode);
		}

		public void IncrementAchievement(string achievementCode, int progress) {
			GamesClass.Achievements.Increment (client, achievementCode, progress);
		}

		public void AwardAchievement (string achievementId, double percentageComplete)
		{
			foreach (var a in Achievements) {
				if (a.Key == achievementId) {
					switch (a.AchievementType) {
					case Achievement.AchievementTypeEnum.Standard:
						UnlockAchievement (a.Key);
						break;
					case Achievement.AchievementTypeEnum.Incremental:
						var progress = (int)(a.TotalSteps * percentageComplete);
						IncrementAchievement (a.Key, progress - a.CurrentSteps);
						a.CurrentSteps = progress;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Show the built in google Achievements Activity. This will cause your application to go into a Paused State
		/// </summary>
		public void ShowAchievements() {
			var  intent = GamesClass.Achievements.GetAchievementsIntent (client);
			Activity.StartActivityForResult (intent, REQUEST_ACHIEVEMENTS);
		}

		/// <summary>
		/// Submit a score to google play. The score will only be updated if it is greater than the existing score. 
		/// This is not immediate but will occur at the next sync of the google play client.
		/// </summary>
		/// <param name="leaderboardCode">Leaderboard code from you applications Google Play Game Services Leaderboards Page</param>
		/// <param name="value">The value of the score</param>
		public void SubmitScore(string leaderboardCode, long value) {
			GamesClass.Leaderboards.SubmitScore (client, leaderboardCode, value);
		}

		/// <summary>
		/// Submit a score to google play. The score will only be updated if it is greater than the existing score. 
		/// This is not immediate but will occur at the next sync of the google play client.
		/// </summary>
		/// <param name="leaderboardCode">Leaderboard code from you applications Google Play Game Services Leaderboards Page</param>
		/// <param name="value">The value of the score</param>
		/// <param name="value">Additional MetaData to attach. Must be a URI safe string with a max length of 64 characters</param>
		public void SubmitScore(string leaderboardCode, long value, string metadata) {
			GamesClass.Leaderboards.SubmitScore (client, leaderboardCode, value, metadata);
		}

		internal void SubmitScore (LeaderboardKey key, long score)
		{
			var resolver = Game.Instance.Services.GetService<ILeaderboardResolver> ();
			if (resolver == null)
				return;
			var id = resolver.GetLeaderboardIdFromKey (key);
			if (!string.IsNullOrEmpty(id))
				SubmitScore (id, score);
		}

		/// <summary>
		/// Show the built in leaderboard activity for the leaderboard code.
		/// </summary>
		/// <param name="leaderboardCode">Leaderboard code from you applications Google Play Game Services Leaderboards Page</param>
		public void ShowLeaderBoardIntentForLeaderboard(string leaderboardCode) {
			var intent = GamesClass.Leaderboards.GetLeaderboardIntent (client, leaderboardCode);
			Activity.StartActivityForResult (intent, REQUEST_LEADERBOARD);
		}

		/// <summary>
		/// Show the built in leaderboard activity for all the leaderboards setup for your application
		/// </summary>
		public void ShowAllLeaderBoardsIntent() {
			var intent = GamesClass.Leaderboards.GetAllLeaderboardsIntent (client);
			Activity.StartActivityForResult (intent, REQUEST_ALL_LEADERBOARDS);
		}

		/// <summary>
		/// Load the Achievments. This populates the Achievements property
		/// </summary>
		public void LoadAchievements() {
			var pendingResult = GamesClass.Achievements.Load (client, false);
			pendingResult.SetResultCallback (achievmentsCallback);
		}

		Action leaderboardLoaded = null;
		bool loadingleaderboards = false;

		public void LoadTopScores(string leaderboardCode, Action loaded) {
			leaderboardLoaded = loaded;
			loadingleaderboards = true;
			var pendingResult = GamesClass.Leaderboards.LoadTopScores (client, leaderboardCode, 2, 0, 25);
			pendingResult.SetResultCallback (leaderboardsCallback);
		}

		#region IGoogleApiClientConnectionCallbacks implementation

		public void OnConnected (Android.OS.Bundle connectionHint)
		{
			resolving = false;
			SignedOut = false;
			signingin = false;

			using (var settings = Activity.GetSharedPreferences ("googleplayservicessettings", FileCreationMode.Private)) {
				using (var e = settings.Edit ()) {
					e.PutString ("playerid",GamesClass.GetCurrentAccountName(client));
					e.Commit ();
				}
			}

			var player = GamesClass.Players.GetCurrentPlayer (client);
			if (player.HasIconImage) {
				GetIcon (player.IconImageUrl);
			}

			if (OnSignedIn != null)
				OnSignedIn (this, EventArgs.Empty);
		}

		public byte[] Icon { get; private set; }

		private async void GetIcon(string url) {
			var request = System.Net.HttpWebRequest.CreateHttp (url);
			var response = await request.GetResponseAsync ();
			var data = new byte[response.ContentLength];
			var i = await response.GetResponseStream ().ReadAsync(data, 0, data.Length);
			if (i == response.ContentLength)
				Icon = data;
			else 
				Icon = new byte[0];
		}

		public void OnConnectionSuspended (int resultCode)
		{
			resolving = false;
			SignedOut = false;
			signingin = false;
			client.Disconnect ();
			if (OnSignInFailed != null)
				OnSignInFailed (this, EventArgs.Empty);
		}

		public void OnConnectionFailed (ConnectionResult result)
		{
			if (resolving)
				return;

			if (result.HasResolution) {
				resolving = true;
				result.StartResolutionForResult (Activity, RC_RESOLVE);
				return;
			}

			resolving = false;
			SignedOut = false;
			signingin = false;
			if (OnSignInFailed != null)
				OnSignInFailed (this, EventArgs.Empty);
		}
		#endregion

		/// <summary>
		/// Processes the Activity Results from the Signin process. MUST be called from your activity OnActivityResult override.
		/// </summary>
		/// <param name="requestCode">Request code.</param>
		/// <param name="resultCode">Result code.</param>
		/// <param name="data">Data.</param>
		public void OnActivityResult (int requestCode, Result resultCode, Intent data) {

			if (requestCode == RC_RESOLVE) {
				if (resultCode == Result.Ok) {
					Start ();
				} else {
					if (OnSignInFailed != null)
						OnSignInFailed (this, EventArgs.Empty);
				}
			}
		}


		internal class AchievementsCallback : Java.Lang.Object, IResultCallback {

			GooglePlayHelper helper;

			public AchievementsCallback (GooglePlayHelper helper): base()
			{
				this.helper = helper;
			}

			#region IResultCallback implementation
			public void OnResult (Java.Lang.Object result)
			{
				var ar = result.JavaCast<IAchievementsLoadAchievementsResult>();
				if (ar != null) {
					helper.achievments.Clear ();
					var count = ar.Achievements.Count;
					for (int i = 0; i < count; i++) {
						var item = ar.Achievements.Get (i);
						var a = item.JavaCast<IAchievement> ();
						var isIncremental = a.Type  == Android.Gms.Games.Achievement.Achievement.TypeIncremental;
						helper.achievments.Add (
							new Achievement () { 
								Name = a.Name,
								IsEarned = a.State == Android.Gms.Games.Achievement.Achievement.StateUnlocked,
								DisplayBeforeEarned = a.State != Android.Gms.Games.Achievement.Achievement.StateHidden,
								Key = a.AchievementId,
								Description = a.Description,
								HowToEarn = a.Description,
								EarnedDateTime = a.LastUpdatedTimestamp != -1 
								? new DateTime (1970, 1, 1).AddMilliseconds (a.LastUpdatedTimestamp) 
								: DateTime.MinValue,
								EarnedOnline = true,
								AchievementType = !isIncremental ? Achievement.AchievementTypeEnum.Standard : Achievement.AchievementTypeEnum.Incremental,
								TotalSteps = isIncremental ? a.TotalSteps : 0,
								CurrentSteps = isIncremental ? a.CurrentSteps : 0,
								GamerScore = 0
							});
					}
				}
			}
			#endregion
		}

		internal class LeaderBoardsCallback : Java.Lang.Object, IResultCallback {

			GooglePlayHelper helper;

			public LeaderBoardsCallback (GooglePlayHelper helper): base()
			{
				this.helper = helper;
			}

			#region IResultCallback implementation
			public void OnResult (Java.Lang.Object result)
			{
				var ar = result.JavaCast<ILeaderboardsLoadScoresResult>();
				if (ar != null) {
					var id = ar.Leaderboard.LeaderboardId;
					if (!helper.scores.ContainsKey (id)) {
						helper.scores.Add (id, new List<LeaderboardEntry> ());
					}
					helper.scores [id].Clear ();
					var count = ar.Scores.Count;
					for (int i = 0; i < count; i++) {
						var score = ar.Scores.Get(i).JavaCast<ILeaderboardScore> ();
						var name = score.ScoreHolderDisplayName;
						helper.scores [id].Add (new LeaderboardEntry() {
							Rating = score.RawScore,
							Gamer = new SignedInGamer () {
								DisplayName = name,
								IsSignedInToLive = false,
								Gamertag = name,
							}
						});
					}
				}

				if (helper.leaderboardLoaded != null) {
					helper.leaderboardLoaded ();
				}

				helper.loadingleaderboards = false;
			}
			#endregion
		}
	}

	public interface ILeaderboardResolver {
		string GetLeaderboardIdFromKey(LeaderboardKey key);
	}
}