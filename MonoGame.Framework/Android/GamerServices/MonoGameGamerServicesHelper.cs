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
				GameHelper.Instance.SignIn ();
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
	#if !OUYA
	, GameHelper.IGameHelperListener
	#endif
    {
        SpriteBatch spriteBatch;
        Texture2D signInProgress;
        Color alphaColor = new Color(128, 128, 128, 0);
        byte startalpha = 0;

        public MonoLiveGuide(Game game)
            : base(game)
        {
            this.Enabled = false;
            this.Visible = false;
            //Guide.IsVisible = false;
            this.DrawOrder = Int32.MaxValue;
			#if !OUYA
			GameHelper.Instance.Initialize (this);
			#endif
        }

        public override void Initialize()
        {
            base.Initialize();
			#if !OUYA
			GameHelper.Instance.Start ();
			#endif
        }

        Texture2D Circle(GraphicsDevice graphics, int radius)
        {
            int aDiameter = radius * 2;
            Vector2 aCenter = new Vector2(radius, radius);

            Texture2D aCircle = new Texture2D(graphics, aDiameter, aDiameter, false, SurfaceFormat.Color);
            Color[] aColors = new Color[aDiameter * aDiameter];

            for (int i = 0; i < aColors.Length; i++)
            {
                int x = (i + 1) % aDiameter;
                int y = (i + 1) / aDiameter;

                Vector2 aDistance = new Vector2(Math.Abs(aCenter.X - x), Math.Abs(aCenter.Y - y));


                if (Math.Sqrt((aDistance.X * aDistance.X) + (aDistance.Y * aDistance.Y)) > radius)
                {
                    aColors[i] = Color.Transparent;
                }
                else
                {
                    aColors[i] = Color.White;
                }
            }

            aCircle.SetData<Color>(aColors);

            return aCircle;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            signInProgress = Circle(this.Game.GraphicsDevice, 10);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
			/* spriteBatch.Begin();//SpriteSortMode.Immediate, BlendState.AlphaBlend);

            Vector2 center = new Vector2(this.Game.GraphicsDevice.Viewport.Width / 2, this.Game.GraphicsDevice.Viewport.Height - 100);
            Vector2 loc = Vector2.Zero;
            alphaColor.A = startalpha;
            for (int i = 0; i < 12; i++)
            {
                float angle = (float)(i / 12.0 * Math.PI * 2);
                loc = new Vector2(center.X + (float)Math.Cos(angle) * 50, center.Y + (float)Math.Sin(angle) * 50);
                spriteBatch.Draw(signInProgress, loc, alphaColor);
                alphaColor.A += 255 / 12;
                if (alphaColor.A > 255) alphaColor.A = 0;
            }
            spriteBatch.End();*/
            base.Draw(gameTime);
        }

        TimeSpan gt = TimeSpan.Zero;
        TimeSpan last = TimeSpan.Zero;
		int delay = 2;

        public override void Update(GameTime gameTime)
        {
            if (gt == TimeSpan.Zero) gt = last = gameTime.TotalGameTime;

            if ((gameTime.TotalGameTime - last).Milliseconds > 100)
            {
                last = gameTime.TotalGameTime;
                startalpha += 255 / 12;
            }
			#if OUYA
			OnSignInFailed();
			#endif
            base.Update(gameTime);
        }

		#region IGameHelperListener implementation
		public void OnSignInFailed ()
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
			}

			this.Visible = false;
			this.Enabled = false;
			Guide.IsVisible = false;
		}
		public void OnSignInSucceeded ()
		{
			#if !OUYA



			var name = GameHelper.Instance.GameClient.CurrentAccountName;

			if (Gamer.SignedInGamers.FirstOrDefault(x => x.DisplayName == name) == null) {
				SignedInGamer sig = new SignedInGamer();
				sig.DisplayName = name;
				sig.Gamertag = name;
				sig.IsSignedInToLive = true;

				Gamer.SignedInGamers.Add(sig);
			}

			this.Visible = false;
			this.Enabled = false;
			Guide.IsVisible = false;

			GameHelper.Instance.LoadAchievements();
			#endif

		}
		#endregion
    }
}


