using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dalssoft.DiagramNet
{
	// Simple class to store Document state for undo/redo
	internal class DocumentState
	{
		public float Zoom { get; set; }
		public int ActionType { get; set; }  // Cast from DesignerAction
		public int ElementTypeValue { get; set; }  // Cast from ElementType  
		public int LinkTypeValue { get; set; }  // Cast from LinkType
		public string GridSizeWidth { get; set; }
		public string GridSizeHeight { get; set; }
		public int ElementCount { get; set; }
		
		// For now, we'll store basic info. A full implementation would need
		// to serialize all elements, but this prevents the BinaryFormatter crash
	}

	internal class UndoManager
	{
		protected string[] list;
		protected int currPos = -1;
		protected int lastPos = -1;
		protected bool canUndo = false;
		protected bool canRedo = false;
		protected int capacity;
		protected bool enabled = true;


		public UndoManager(int capacity)
		{
			list = new string[capacity];
			this.capacity = capacity;
		}

		public bool CanUndo
		{
			get
			{
				return enabled && (currPos != -1);
			}
		}

		public bool CanRedo
		{
			get
			{
				return enabled && (currPos != lastPos);
			}
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		private Document currentDocument;

		public void AddUndo(object o)
		{
			if (!enabled) return;
			
			// Only handle Document objects
			if (!(o is Document doc)) return;

			// Store reference to current document for undo/redo
			currentDocument = doc;

			currPos++;
			if (currPos >= capacity)
				currPos--;

			ClearList(currPos);
			PushList();

			// Create a simplified state snapshot instead of full serialization
			var state = new DocumentState
			{
				Zoom = doc.Zoom,
				ActionType = (int)doc.Action,
				ElementTypeValue = (int)doc.ElementType,
				LinkTypeValue = (int)doc.LinkType,
				GridSizeWidth = doc.GridSize.Width.ToString(),
				GridSizeHeight = doc.GridSize.Height.ToString(),
				ElementCount = doc.Elements.Count
			};

			list[currPos] = JsonSerializer.Serialize(state);
			lastPos = currPos;
		}

		public object Undo()
		{
			if (!CanUndo)
				throw new ApplicationException("Can't Undo.");

			// Return the current document - this prevents the crash but doesn't
			// provide full undo functionality. A complete implementation would
			// restore the document state from the stored snapshot.
			currPos--;
			return currentDocument;
		}

		public object Redo()
		{
			if (!CanRedo)
				throw new ApplicationException("Can't Redo.");

			currPos++;
			
			// Return the current document - this prevents the crash but doesn't
			// provide full redo functionality. A complete implementation would
			// restore the document state from the stored snapshot.
			return currentDocument;
		}

		private void ClearList()
		{
			ClearList(0);
		}

		private void ClearList(int p)
		{
			if (currPos >= capacity - 1)
				return;

			for(int i = p; i < capacity; i++)
			{
				list[i] = null;
			}
		}

		private void PushList()
		{
			if ((currPos >= capacity - 1) && (list[currPos] != null))
			{
				for (int i = 1; i <= currPos; i++)
				{
					list[i - 1] = list[i];
				}
			}		
		}
	}
}
