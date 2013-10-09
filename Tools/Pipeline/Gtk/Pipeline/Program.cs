using System;
using Gtk;
using Pipeline.MacOS;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Pipeline
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			var win = new Gtk.Window (WindowType.Toplevel);
			win.Name = "MonoGame Content Pipeline";
			win.SetSizeRequest (800,600);

			var view = new MainView();
			if (args != null && args.Length > 0)
			{
				var projectFilePath = string.Join(" ", args);
				view.OpenProjectPath =  System.IO.Path.GetFullPath (projectFilePath);
			}

			MainView.CreateControllers (view);

			win.Add (view);
			win.DeleteEvent += (o, a) => {
				if (view.Exit())
					Application.Quit();
			};
			win.ShowAll ();
			Application.Run ();
		}
	}
}
