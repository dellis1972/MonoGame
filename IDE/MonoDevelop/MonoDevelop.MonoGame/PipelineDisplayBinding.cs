using System;
using MonoDevelop.Ide.Gui;
using System.IO;
using System.Diagnostics;
using MonoDevelop.Ide.Gui.Content;
using Gtk;

namespace MonoDevelop.MonoGame
{
	public class PipelineDisplayBinding: IViewDisplayBinding
	{
		#region IViewDisplayBinding implementation

		public IViewContent CreateContent (MonoDevelop.Core.FilePath fileName, string mimeType, MonoDevelop.Projects.Project ownerProject)
		{
			return new MonoGameContentEditorViewContent (fileName, ownerProject);
		}

		public string Name {
			get {
				return "MonoGame Content Builder";
			}
		}

		#endregion

		#region IDisplayBinding implementation

		public bool CanHandle (MonoDevelop.Core.FilePath fileName, string mimeType, MonoDevelop.Projects.Project ownerProject)
		{
			return mimeType == "text/x-mgcb";
		}

		public bool CanUseAsDefault {
			get {
				return true;
			}
		}

		#endregion
	}
}

