using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace DevelopmentWithADot.AspNetTree
{
	public sealed class PopulateTreeNodeEventArgs : EventArgs
	{
		public PopulateTreeNodeEventArgs(TreeNode node)
		{
			this.Node = node;
			this.CanHaveChildren = true;
			this.CanSelect = true;
		}

		public TreeNode Node { get; private set; }

		public Boolean CanSelect { get; set; }

		public Boolean CanHaveChildren { get; set; }
	}
}
