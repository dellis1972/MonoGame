using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Tools.Pipeline;
using Gtk;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Reflection;


#if MACOS
using IgeMacIntegration;
#endif

namespace Pipeline.MacOS
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class MainView : Gtk.Bin, IView
	{
		PropertyGrid grid;
		IController _controller;
		Gtk.TreeStore projectItems;
		ProjectTreeView projectTree;
		TextView buildOutput;
		Gtk.Window host;

		public string OpenProjectPath;

		public static void CreateControllers(MainView view) {
			var model = new PipelineProject();
			view._controller = new PipelineController(view, model);
		}

		public bool Exit() {
			return _controller.Exit ();
		}

		public MainView (Gtk.Window win)
		{
			host = win;
			this.Build ();
		}

		public void BuildUI () {
			grid = new PropertyGrid ();
			grid.controller = _controller;
			hpaned1.Add2 (grid);

			projectTree = new ProjectTreeView () { HeightRequest = 300 };

			Gtk.TreeViewColumn projectItemColumn = new Gtk.TreeViewColumn ();
			projectItemColumn.Title = "";
			Gtk.CellRendererText projectItemCell = new Gtk.CellRendererText ();
			projectItemColumn.PackStart (projectItemCell, true);
			projectItemColumn.SetCellDataFunc (projectItemCell, RenderProjectItem);

			projectTree.AppendColumn (projectItemColumn);

			projectItems = new Gtk.TreeStore (typeof(ProjectNode), typeof (BaseNode));

			projectTree.Model = projectItems;

			projectTree.OnLeftClick += (object sender, EventArgs e) => {
				_controller.Selection.Clear (this);
				if (projectTree.SelectedNode != null) {
					_controller.Selection.Add(projectTree.SelectedNode.ProjectItem, this);
					if (grid != null)
						grid.CurrentObject = projectTree.SelectedNode.ProjectItem;
				}
			};

			projectTree.OnRightClick += (object sender, EventArgs e) => {
				// right click 
				Menu m = new Menu();
				MenuItem addItem = new MenuItem("Add");
				MenuItem removeItem = new MenuItem("Remove");

				addItem.ButtonPressEvent += HandleAddItemMenuEvent;
				removeItem.ButtonPressEvent += HandleRemoveItemMenuEvent;
				m.Add(addItem);
				m.Add(removeItem);
				m.ShowAll();
				m.Popup();
			};

			projectTree.Show ();
			projectContiner.Add1 (projectTree);
			buildOutput = new TextView ();
			projectContiner.Add2 (buildOutput);

			if (Assembly.GetEntryAssembly ().FullName.Contains ("Pipeline"))
				BuildMenu ();
			else {
				menuBar.Hide ();
				vbox1.Remove (menuBar);
			}
		}

		void BuildMenu() {


			if (Environment.OSVersion.Platform == PlatformID.Unix) {
				IgeMacMenu.GlobalKeyHandlerEnabled = true;

				//Tell the IGE library to use your GTK menu as the Mac main menu
				IgeMacMenu.MenuBar = menuBar;

				//tell IGE which menu item should be used for the app menu's quit item
				//IgeMacMenu.QuitMenuItem = yourQuitMenuItem;

				//add a new group to the app menu, and add some items to it
				var appGroup = IgeMacMenu.AddAppMenuGroup ();
				appGroup.AddMenuItem (new MenuItem(), "About Pipeline...");

				//hide the menu bar so it no longer displays within the window
				menuBar.Hide ();
				vbox1.Remove (menuBar);
			}
		}

		void HandleAddItemMenuEvent (object o, ButtonPressEventArgs args)
		{
			var node = projectTree.SelectedNode ?? projectTree.Root;
			_controller.Include (node.ProjectItem.Location);
		}

		void HandleRemoveItemMenuEvent (object o, ButtonPressEventArgs args)
		{
			if (this.projectTree.SelectedNode == null)
				return;

			var contentItem = projectTree.SelectedNode.ProjectItem as MonoGame.Tools.Pipeline.ContentItem;
			if (contentItem == null)
				return;

			List<MonoGame.Tools.Pipeline.ContentItem> items = new List<MonoGame.Tools.Pipeline.ContentItem> ();
			items.Add (contentItem);

			_controller.Exclude (items);
		}
			
		private void RenderProjectItem (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			var node = (BaseNode) model.GetValue (iter, 0);
			(cell as Gtk.CellRendererText).Text = node.Display;
		}

		protected override void OnSizeAllocated (Gdk.Rectangle allocation)
		{
			if (this.Child != null)
			{
				this.Child.Allocation = allocation;
			}
		}

		protected override void OnSizeRequested (ref Requisition requisition)
		{
			if (this.Child != null)
			{
				requisition = this.Child.SizeRequest ();
			}
		}

		protected override void OnShown ()
		{
			base.OnShown ();
			if (_controller != null) {
				if (!string.IsNullOrEmpty(OpenProjectPath))
				{
					_controller.OpenProject(OpenProjectPath);
					OpenProjectPath = null;
				}
			}
		}

		private void OnSelectionModified(MonoGame.Tools.Pipeline.Selection selection, object sender)
		{
			if (sender == this)
				return;

			//_treeView.SelectedNodes = _controller.Selection.Select(FindNode);

		}

		private void UpdateMenus()
		{
		}

		private void UpdateUndoRedo(bool canUndo, bool canRedo)
		{
		}

		#region IView implementation

		public void Attach (IController controller)
		{
			_controller = controller;
			var updateMenus = new System.Action(UpdateMenus);
			var invokeUpdateMenus = new System.Action(() => updateMenus());

			_controller.OnBuildStarted += invokeUpdateMenus;
			_controller.OnBuildFinished += invokeUpdateMenus;
			_controller.OnProjectLoading += invokeUpdateMenus;
			_controller.OnProjectLoaded += invokeUpdateMenus;

			var updateUndoRedo = new CanUndoRedoChanged(UpdateUndoRedo);
			var invokeUpdateUndoRedo = new CanUndoRedoChanged((u, r) => updateUndoRedo(u, r));

			_controller.OnCanUndoRedoChanged += invokeUpdateUndoRedo;
			_controller.Selection.Modified += OnSelectionModified;
		}

		public AskResult AskSaveOrCancel ()
		{
			using (var dlg = new Gtk.MessageDialog (null, DialogFlags.Modal,
				                 MessageType.Question, ButtonsType.YesNo,
				                 "Do you want to save the project first?")) {
				var respose = (ResponseType)dlg.Run ();
				dlg.Hide ();
				if (respose == ResponseType.Yes)
					return AskResult.Yes;
				if (respose == ResponseType.No)
					return AskResult.No;
				return AskResult.Cancel;
			}

		}

		public bool AskSaveName (ref string filePath, string title)
		{
			using(var dialog = new FileChooserDialog(title, host, FileChooserAction.Save,
				"Cancel", ResponseType.Cancel, "Save", ResponseType.Accept)) {
				var response = (ResponseType)dialog.Run();
				dialog.Hide();
				filePath = dialog.Filename;
				return response == ResponseType.Accept;
			}

		}
		public bool AskOpenProject (out string projectFilePath)
		{
			using (var dialog = new FileChooserDialog ("", host, FileChooserAction.Open, "Cancel", ResponseType.Cancel,
				                    "Open", ResponseType.Accept)) {
				dialog.Filter = new FileFilter ();
				dialog.Filter.Name = "MonoGame Content Build Files (*.mgcb)";
				dialog.Filter.AddPattern ("*.mgcb");
				var respose = (ResponseType)dialog.Run ();
				projectFilePath = dialog.Filename;
				dialog.Hide ();
				return respose == ResponseType.Accept;
			}
		}

		public bool AskImportProject (out string projectFilePath)
		{
			projectFilePath = String.Empty;
			ShowMessage ("Not Implemented on Mac yet!");
			return false;
		}

		public void ShowError (string title, string message)
		{
			using (var dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Error,
				                ButtonsType.Ok, "{0}", message)) {
				dlg.Run ();
				dlg.Hide ();
			}
		}

		public void ShowMessage (string message)
		{
			using (var dlg = new MessageDialog (null, DialogFlags.Modal, MessageType.Info,
				ButtonsType.Ok, "{0}", message)) {
				dlg.Run ();
				dlg.Hide ();
			}
		}

		public void BeginTreeUpdate ()
		{

		}

		public void SetTreeRoot (IProjectItem item)
		{
			projectItems.Clear ();

			var project = item as PipelineProject;
			if (project == null)
				return;

			var root = projectItems.AppendValues ( new ProjectNode(item));
			if (grid != null)
				grid.CurrentObject = item;
		}

		public void AddTreeItem (IProjectItem item)
		{
			var path = item.Location;
			var folders = path.Replace("\\","/").Split(new[] { System.IO.Path.DirectorySeparatorChar, 
				System.IO.Path.AltDirectorySeparatorChar}, StringSplitOptions.RemoveEmptyEntries);

			TreeIter parent;
			if (!projectItems.GetIterFirst (out parent))
				return;
			foreach (var folder in folders)
			{
				if (projectItems.IterHasChild (parent)) {
					var count = projectItems.IterNChildren (parent);
					bool found = false;
					TreeIter child;
					for (int i = 0; i < count; i++) {
						if (projectItems.IterNthChild (out child, parent, i)) {
							var node = projectItems.GetValue (child, 0) as FolderNode;
							if (node != null && node.Display == folder) {
								parent = child;
								found = true;
								break;
							}
						}
					}
					if (!found)
						parent = projectItems.AppendValues (parent,new FolderNode (folder));
				} else {
					parent = projectItems.AppendValues (parent, new FolderNode (folder));
				}
			}

			var newNode = projectItems.AppendValues (parent, new ProjectNode(item));
			projectTree.ExpandToPath (projectItems.GetPath(newNode));
		}

		public void RemoveTreeItem (MonoGame.Tools.Pipeline.ContentItem contentItem)
		{
			var path = contentItem.Location;
			var folders = path.Replace("\\","/").Split(new[] { System.IO.Path.DirectorySeparatorChar, 
				System.IO.Path.AltDirectorySeparatorChar}, StringSplitOptions.RemoveEmptyEntries);

			TreeIter parent;
			int count;

			if (!projectItems.GetIterFirst (out parent))
				return;


			foreach (var folder in folders) {
				if (projectItems.IterHasChild (parent)) {
					count = projectItems.IterNChildren (parent);
					bool found = false;
					TreeIter child;
					for (int i = 0; i < count; i++) {
						if (projectItems.IterNthChild (out child, parent, i)) {
							var node = projectItems.GetValue (child, 0) as FolderNode;
							if (node != null && node.Display == folder) {
								parent = child;
								found = true;
								break;
							}
						}
					}
				}
			}
			count = projectItems.IterNChildren (parent);
			for (int i = 0; i < count; i++) {
				TreeIter child;
				if (projectItems.IterNthChild (out child, parent, i)) {
					var node = projectItems.GetValue(child, 0) as ProjectNode;
					if (node != null && node.Display == contentItem.Name) {
						projectItems.Remove (ref child);
						break;
					}
				}
			}
		}

		public void UpdateTreeItem (IProjectItem item)
		{

		}

		public void EndTreeUpdate ()
		{

		}

		public void UpdateProperties (IProjectItem item)
		{

		}

		public void OutputAppend (string text)
		{
			buildOutput.Invoke(() => {
				buildOutput.Buffer.Text += text + "\n";
			});
		}

		public void OutputClear ()
		{
			buildOutput.Invoke (() => {
				buildOutput.Buffer.Clear ();
			});
		}

		public bool ChooseContentFile (string initialDirectory, out System.Collections.Generic.List<string> files)
		{
			files = new List<string> ();
			using (var dialog = new FileChooserDialog ("", host, FileChooserAction.Open, "Cancel", ResponseType.Cancel,
				"Open", ResponseType.Accept)) {
				dialog.Filter = new FileFilter ();
				dialog.Filter.Name = "All Files (*.*)";
				dialog.Filter.AddPattern ("*.*");
				dialog.SetCurrentFolder (initialDirectory);
				var respose = (ResponseType)dialog.Run ();
				if (respose == ResponseType.Accept)
					files.AddRange (dialog.Filenames);
				dialog.Hide ();
				return respose == ResponseType.Accept;
			}
		}

		public void OnTemplateDefined(ContentItemTemplate item)
		{
		}

		public Process CreateProcess(string exe, string commands)
		{
			var _buildProcess = new Process();
			if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX)
			{
				_buildProcess.StartInfo.FileName = exe;
				_buildProcess.StartInfo.Arguments = commands;
			}
			else
			{
				_buildProcess.StartInfo.FileName = "mono";
				_buildProcess.StartInfo.Arguments = string.Format("\"{0}\" {1}", exe, commands);
			}
			return _buildProcess;
		}

		protected void executeActionClick (object sender, EventArgs e)
		{
			_controller.Build (true);
		}

		#endregion

		public void Load(string filename) {
			_controller.OpenProject (filename);
		}

		public void LoadNew(Stream content, string mimeType)
		{
		}

		public void Undo() {
		}

		public void Save()
		{
			_controller.SaveProject (false);
		}

		public void Save(string filename)
		{
			_controller.SaveProject (false);
		}

		public bool IsDirty {
			get { return _controller.ProjectDirty;}
			set { }
		}

		protected void Exit_Clicked (object sender, EventArgs e)
		{
			if (Exit())
				Application.Quit();
		}

		protected void Build_Click (object sender, EventArgs e)
		{
			_controller.Build (false);
		}

		protected void Rebuild_Click (object sender, EventArgs e)
		{
			_controller.Build (true);
		}

		protected void Clean_Click (object sender, EventArgs e)
		{
			_controller.Clean ();
		}

		protected void Open_Click (object sender, EventArgs e)
		{
			_controller.OpenProject ();
		}

		protected void Save_CLick (object sender, EventArgs e)
		{
			_controller.SaveProject (false);
		}

		protected void Import_Click (object sender, EventArgs e)
		{
			_controller.ImportProject ();
		}

		protected void AddExisting_Click (object sender, EventArgs e)
		{
			var node = projectTree.SelectedNode;
			if (node == null)
				return;
			var item = node.ProjectItem;
			if (item != null)
				_controller.Include(item.Location);
		}

		protected void CloseProject_Click (object sender, EventArgs e)
		{
			_controller.CloseProject ();
			grid.CurrentObject = null;
		}

		protected void AddNew_Click (object sender, EventArgs e)
		{
			// create a new spritefont or xml file...
		}

		internal class ProjectTreeView : Gtk.TreeView {

			protected override bool OnButtonPressEvent (Gdk.EventButton evnt)
			{
				SelectedNode = null;
				Gtk.TreePath path = new Gtk.TreePath();
				// Get TreePath from the xy-coordinates of the mouse from the event
				// GetPathAtPos is a function from the TreeView class
				GetPathAtPos (System.Convert.ToInt16 (evnt.X), System.Convert.ToInt16 (evnt.Y), out path);

				Gtk.TreeIter iter;
				// Get iter from the path
				if (Model.GetIter (out iter, path)) {
					var node = Model.GetValue (iter, 0) as ProjectNode;
					if (node != null) {
						SelectedNode = node;
					}
				}

				if (evnt.Button == 3) {
					//Right Click
					if (OnRightClick != null)
						OnRightClick (this, EventArgs.Empty);
				}
				if (evnt.Button == 1) {
					if (OnLeftClick != null)
						OnLeftClick (this, EventArgs.Empty);
				}
				return base.OnButtonPressEvent (evnt);
			}

			public ProjectNode SelectedNode {get; private set;}
			public ProjectNode Root {
				get { 
					Gtk.TreeIter iter;
					if (Model.GetIterFirst (out iter)) {
						var node = Model.GetValue (iter, 0) as ProjectNode;
						if (node != null)
							return node;
					}
					return null;
				}
			}

			public event EventHandler OnRightClick;
			public event EventHandler OnLeftClick;
		}

		internal class ProjectNode : BaseNode {

			public IProjectItem ProjectItem { get; private set; }

			public ProjectNode (IProjectItem projectItem): base(projectItem.Location)
			{
				this.ProjectItem = projectItem;
			}

			public override string Display { get { return ProjectItem.Name; }}
		}

		internal class FolderNode : BaseNode {
			public FolderNode (string folder): base(folder)
			{

			}
		}

		internal class BaseNode
		{
			public BaseNode (string folder)
			{
				this.Display = folder;
			}

			public virtual string Display { get; private set;}
		}
	}

	public static class WidgetExtensions
	{
		private static Thread _uiThread;

		/// <summary>
		/// Determines whether we need to invoke to get onto the UI thread
		/// </summary>
		public static bool InvokeRequired(this Gtk.Widget widget)
		{
			if(_uiThread == null)
			{				
				Application.Invoke((_,__) =>
					{
						_uiThread = Thread.CurrentThread;
					});
				return true;
			}
			if(Thread.CurrentThread == _uiThread)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Runs the action on the UI thread,
		/// Optimized to only invoke if we are not already on the UI thread.
		/// </summary>
		public static void Invoke(this Gtk.Widget widget, System.Action action)
		{
			if(widget.InvokeRequired())
			{
				Application.Invoke((_,__) =>
					{
						action();
					});
				return;
			}
			action();
		}
	}
}

