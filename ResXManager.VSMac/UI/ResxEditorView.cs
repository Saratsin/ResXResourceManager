﻿using Gtk;
using System;

namespace ResXManager.VSMac.UI
{
	public class ResxEditorView : VBox
	{
		public ResxEditorView()
		{
			ResourceList = new ResourceList();
			ResourceControlBar = new ResourceControlBar();

			ResourceControlBar.OnAddResource += (sender, e) => OnAddResource(this, e);
			ResourceControlBar.OnRemoveResource += (sender, e) => OnRemoveResource(this, e);

			var listContainer = new ScrolledWindow();
			listContainer.Add(ResourceList);

			PackStart(ResourceControlBar, false, true, 5);
			PackEnd(listContainer);
		}

		public event EventHandler OnAddResource;

		public event EventHandler OnRemoveResource;

		public ResourceList ResourceList { get; }

		public ResourceControlBar ResourceControlBar { get; }
	}

}
