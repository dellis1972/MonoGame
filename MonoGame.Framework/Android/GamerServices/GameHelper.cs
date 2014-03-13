using System;
using Android.Gms.Common;
using Android.Gms.Games;
using Android.Gms.Games.Achievement;
using Android.Gms.Games.MultiPlayer;
using Android.App;
using Android.Views;
using Android.Content;
using Android.Runtime;
using Android.OS;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.GamerServices
{
	public class GameHelper : Java.Lang.Object
	, IGooglePlayServicesClientConnectionCallbacks
	, IGooglePlayServicesClientOnConnectionFailedListener
	, IOnAchievementsLoadedListener
	{

		private static GameHelper instance = null;

		internal static GameHelper Instance {
			get {
				if (instance == null) {
					instance = new GameHelper (Game.Activity);
				}
				return instance;
			}
		}


		public interface IGameHelperListener {
			/*			*
         * Called when sign-in fails. As a result, a "Sign-In" button can be
         * shown to the user; when that button is clicked, call
         * @link{GamesHelper#beginUserInitiatedSignIn}. Note that not all calls to this
         * method mean an error; it may be a result of the fact that automatic
         * sign-in could not proceed because user interaction was required
         * (consent dialogs). So implementations of this method should NOT
         * display an error message unless a call to @link{GamesHelper#hasSignInError}
         * indicates that an error indeed occurred.
         */
			void OnSignInFailed();

			/*			* Called when sign-in succeeds. */
			void OnSignInSucceeded();
		}

		[Flags]
		public enum ClientsEnum {
			None = 0x00,
			Games = 0x01,
			Plus = 0x02,
			AppState = 0x04
		}

		static ClientsEnum AllClients = ClientsEnum.Games | ClientsEnum.Plus | ClientsEnum.AppState;

		Activity activity;
		GamesClient client;
		IGameHelperListener listener;

		public GamesClient GameClient {
			get { return client; }
		}

		public GameHelper (Activity activity)
		{
			this.activity = activity;
			this.Achievements = new List<Achievement> ();
		}

		public void Initialize(IGameHelperListener listener, ClientsEnum clients = ClientsEnum.Games) {

			this.listener = listener;
				client = new GamesClient.Builder (activity, this, this)
				.SetGravityForPopups ((int)(GravityFlags.Top | GravityFlags.Center))
				.SetScopes (new string[] { Scopes.Games })
				.Create ();
		}

		public void Start() {

			if (client != null && !client.IsConnected) {
				client.Connect ();
			}
		}

		public void Stop() {

			if (client != null && client.IsConnected) {
				client.Disconnect ();
			}
		}

		public void Reconnect() {
			if (client != null)
				client.Reconnect ();
		}

		public void SignIn() {

			if (client == null)
				return;

			if (client.IsConnected)
				return;

			if (client.IsConnecting)
				return;


			var result = GooglePlayServicesUtil.IsGooglePlayServicesAvailable (activity);
			if (result != ConnectionResult.Success) {
				return;
			}

			Start ();

		}
		/*
		public void ShowAlert(String title, String message) {
			new AlertDialog.Builder (activity).SetTitle (title).SetMessage (message)
				.SetNeutralButton ("OK", null).Create ().Show ();
		}

		public void ShowAlert(String message) {
			new AlertDialog.Builder (activity).SetMessage (message)
				.SetNeutralButton ("OK", null).Create ().Show ();
		}
*/

		#region IGooglePlayServicesClientConnectionCallbacks implementation

		string invitationId;

		public void OnConnected (Bundle connectionHint)
		{
			// yay we connected :D
			if (client != null) {
				/*IInvitation inv = (IInvitation)connectionHint.GetParcelable(GamesClient.ExtraInvitation);
				if (inv != null && !string.IsNullOrEmpty(inv.InvitationId))
					invitationId = inv.InvitationId;*/
			}

			listener.OnSignInSucceeded ();
		}

		public void LoadAchievements() {
			if (client != null && client.IsConnected) {
				client.LoadAchievements (this, false);
			}
		}

		public void OnDisconnected ()
		{
			System.Diagnostics.Debug.WriteLine ("Disconnect");
			client.Disconnect ();
			listener.OnSignInFailed ();
		}

		#endregion

		bool resolving = false;
		const int RC_RESOLVE = 9001;
		const int RC_ACHIEVEMENTS = 9002;
		const int RC_LEADERBOARDS = 9003;

		public void OnActivityResult (int requestCode, Result resultCode, Intent data) {

			System.Diagnostics.Debug.WriteLine (string.Format ("OnActivityResult {0} :{1}", requestCode, resultCode));
			if (requestCode == RC_RESOLVE) {
				if (resultCode == Result.Ok) {
					Start ();
				} else {
					listener.OnSignInFailed ();
				}
			}

			if (requestCode == RC_ACHIEVEMENTS) {
				showingAchievements = false;
			}

			if (requestCode == RC_LEADERBOARDS) {
				showingLeaderboard = false;
			}
		}

		public void OnConnectionFailed (ConnectionResult result)
		{
			if (resolving)
				return;

			if (result.HasResolution) {
				resolving = true;
				result.StartResolutionForResult (activity, RC_RESOLVE);
				return;
			}
			System.Diagnostics.Debug.WriteLine ("Failed " + result.ToString());
			listener.OnSignInFailed ();
		}

		class AchievementMe : Java.Lang.Object {

			Java.Lang.Object parent;

			public AchievementMe (Java.Lang.Object parent)
			{
				this.parent = parent;
			}

			public string getId() {
				var m= JNIEnv.GetMethodID (parent.Class.Handle, "getAchievementId", "()Ljava/lang/String;");
				var result = JNIEnv.CallObjectMethod (parent.Handle, m);
				return Java.Lang.Object.GetObject<Java.Lang.String> (result, JniHandleOwnership.TransferLocalRef).ToString();
			}

			public int getState() {
				var m= JNIEnv.GetMethodID (parent.Class.Handle, "getState", "()I");
				return JNIEnv.CallIntMethod (parent.Handle, m);
			}

			public int getType() {
				var m= JNIEnv.GetMethodID (parent.Class.Handle, "getType", "()I");
				return JNIEnv.CallIntMethod (parent.Handle, m);
			}

			public string getName() {
				var m= JNIEnv.GetMethodID (parent.Class.Handle, "getName", "()Ljava/lang/String;");
				var result = JNIEnv.CallObjectMethod (parent.Handle, m);
				return Java.Lang.Object.GetObject<Java.Lang.String> (result, JniHandleOwnership.TransferLocalRef).ToString();
			}

			public int getTotalSteps() {
				var m= JNIEnv.GetMethodID (parent.Class.Handle, "getTotalSteps", "()I");
				return JNIEnv.CallIntMethod (parent.Handle, m);
			}

			public int getCurrentSteps() {
				var m= JNIEnv.GetMethodID (parent.Class.Handle, "getCurrentSteps", "()I");
				return JNIEnv.CallIntMethod (parent.Handle, m);
			}

			public DateTime getLastUpdated() {
				var m = JNIEnv.GetMethodID (parent.Class.Handle, "getLastUpdatedTimestamp", "()J");
				var t = JNIEnv.CallLongMethod (parent.Handle, m);
				return t != -1 ? new DateTime (1970, 1, 1).AddMilliseconds (t) : DateTime.MinValue;
		  	}
		}

		public void OnAchievementsLoaded (int p0, AchievementBuffer p1)
		{
			if (p0 == GamesClient.StatusOk) {
				for (int i = 0; i < p1.Count; i++) {
					var a = p1.Get (i);
					if (a != null) {
						var m = new AchievementMe (a);
						try {
							var isIncremental = m.getType () == Android.Gms.Games.Achievement.Achievement.TypeIncremental;
							Achievements.Add (new Achievement () { 
								Name = m.getName (),
								IsEarned = m.getState () == Android.Gms.Games.Achievement.Achievement.StateUnlocked,
								DisplayBeforeEarned = m.getState () != Android.Gms.Games.Achievement.Achievement.StateHidden,
								Key = m.getId (),
								AchievementType = !isIncremental ? Achievement.AchievementTypeEnum.Standard : Achievement.AchievementTypeEnum.Incremental,
								TotalSteps = isIncremental ? m.getTotalSteps () : 0,
								CurrentSteps = isIncremental ? m.getCurrentSteps () : 0,
								EarnedDateTime = m.getLastUpdated (),
								EarnedOnline = true,
								GamerScore = 0
							});
						} finally {
							m.Dispose ();
						}
					}
				}
			}
		}

		public List<Microsoft.Xna.Framework.GamerServices.Achievement> Achievements { get; set;}

		public void AwardAchievement (string achievementId, double percentageComplete)
		{
			foreach (var a in Achievements) {
				if (a.Key == achievementId) {
					switch (a.AchievementType) {
					case Achievement.AchievementTypeEnum.Standard:
						GameClient.UnlockAchievement (a.Key);
						break;
					case Achievement.AchievementTypeEnum.Incremental:
						var progress = (int)(a.TotalSteps * percentageComplete);
						GameClient.IncrementAchievement (a.Key, progress - a.CurrentSteps);
						a.CurrentSteps = progress;
						break;
					}
				}
			}
		}

		bool showingLeaderboard = false;


		public void ShowLeaderboard ()
		{
			if (showingLeaderboard)
				return;

			var intent = GameClient.AllLeaderboardsIntent;
			showingLeaderboard = true;
			this.activity.StartActivityForResult (intent, RC_LEADERBOARDS);
		}

		bool showingAchievements = false;

		public void ShowAchievements ()
		{
			if (showingAchievements)
				return;

			var intent = GameClient.AchievementsIntent;
			showingAchievements = true;
			this.activity.StartActivityForResult (intent, RC_ACHIEVEMENTS);
		}
	}
}

