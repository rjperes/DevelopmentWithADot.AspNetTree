using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DevelopmentWithADot.AspNetTree
{
	public class Tree : WebControl, INamingContainer
	{
		private readonly TreeView tree = new TreeView();

		public Tree()
		{
			//these are here so that they can be overwriten on markup
			this.RootValue = 0.ToString();
		}

		public event EventHandler<PopulateTreeNodeEventArgs> PopulateTreeNode;

		[DefaultValue("")]
		public String OnNodeClientClick { get; set; }

		[DefaultValue("")]
		public String RootText { get; set; }

		[DefaultValue("")]
		[UrlProperty("*.png;*.gif;*.jpg")]
		public String RootImageUrl { get; set; }

		[DefaultValue("0")]
		protected String RootValue { get; set; }

		[DefaultValue(TreeNodeTypes.None)]
		public TreeNodeTypes ShowCheckBoxes
		{
			get
			{
				return (this.tree.ShowCheckBoxes);
			}
			set
			{
				this.tree.ShowCheckBoxes = value;
			}
		}

		[DefaultValue(true)]
		public Boolean ShowExpandCollapse
		{
			get
			{
				return (this.tree.ShowExpandCollapse);
			}
			set
			{
				this.tree.ShowExpandCollapse = value;
			}
		}

		[DefaultValue(false)]
		public Boolean ShowLines
		{
			get
			{
				return (this.tree.ShowLines);
			}
			set
			{
				this.tree.ShowLines = value;
			}
		}

		[DefaultValue('/')]
		public Char PathSeparator
		{
			get
			{
				return (this.tree.PathSeparator);
			}
			set
			{
				this.tree.PathSeparator = value;
			}
		}

		public CssStyleCollection TreeStyle
		{
			get
			{
				return (this.tree.Style);
			}
		}

		[CssClassProperty]
		[DefaultValue("")]
		public String TreeCssClass
		{
			get
			{
				return (this.tree.CssClass);
			}
			set
			{
				this.tree.CssClass = value;
			}
		}

		public TreeNodeStyle NodeStyle
		{
			get
			{
				return (this.tree.NodeStyle);
			}
		}

		public TreeNodeStyle RootNodeStyle
		{
			get
			{
				return (this.tree.RootNodeStyle);
			}
		}

		public TreeNodeStyle LeafNodeStyle
		{
			get
			{
				return (this.tree.LeafNodeStyle);
			}
		}

		public Style HoverNodeStyle
		{
			get
			{
				return (this.tree.HoverNodeStyle);
			}
		}

		public TreeNodeStyle SelectedNodeStyle
		{
			get
			{
				return (this.tree.SelectedNodeStyle);
			}
		}

		[DefaultValue("")]
		[UrlProperty("*.png;*.gif;*.jpg")]
		public String ExpandImageUrl
		{
			get
			{
				return (this.tree.ExpandImageUrl);
			}
			set
			{
				this.tree.ExpandImageUrl = value;
			}
		}

		[DefaultValue("")]
		[UrlProperty("*.png;*.gif;*.jpg")]
		public String NoExpandImageUrl
		{
			get
			{
				return (this.tree.NoExpandImageUrl);
			}
			set
			{
				this.tree.NoExpandImageUrl = value;
			}
		}

		[DefaultValue("")]
		[UrlProperty("*.png;*.gif;*.jpg")]
		public String CollapseImageUrl
		{
			get
			{
				return (this.tree.CollapseImageUrl);
			}
			set
			{
				this.tree.CollapseImageUrl = value;
			}
		}

		public String SelectedValue
		{
			get
			{
				var selectedNode = this.tree.SelectedNode;

				return ((selectedNode != null) ? selectedNode.Value : null);
			}
		}

		public TreeNodeCollection CheckedNodes
		{
			get
			{
				return (this.tree.CheckedNodes);
			}
		}

		public String SelectedText
		{
			get
			{
				var selectedNode = this.tree.SelectedNode;

				return ((selectedNode != null) ? selectedNode.Text : null);
			}
		}

		public String SelectedValuePath
		{
			get
			{
				var selectedNode = this.tree.SelectedNode;

				return ((selectedNode != null) ? selectedNode.ValuePath : null);
			}
		}

		protected TreeNode FindNode(String valuePath, TreeNode parent)
		{
			if (parent.ValuePath == valuePath)
			{
				return (parent);
			}

			if ((parent.Expanded != true) && (parent.SelectAction != TreeNodeSelectAction.None) && (parent.SelectAction != TreeNodeSelectAction.Select))
			{
				this.OnPopulateTreeNode(this, new TreeNodeEventArgs(parent));
			}

			TreeNode node = null;

			foreach (var child in parent.ChildNodes.OfType<TreeNode>())
			{
				node = this.FindNode(valuePath, child);

				if (node != null)
				{
					break;
				}
			}

			return (node);
		}

		public void SelectNode(String valuePath)
		{
			var node = this.FindNode(valuePath, this.tree.Nodes[0]);

			if (node != null)
			{
				for (var parent = node; parent != null; parent = parent.Parent)
				{
					parent.Expanded = true;
					parent.PopulateOnDemand = false;
				}

				node.Select();
			}
		}

		protected override void CreateChildControls()
		{
			//selection tree			
			this.tree.ID = String.Concat(this.ID, "_TreeView");
			this.tree.TreeNodePopulate += this.OnPopulateTreeNode;
			this.tree.Nodes.Add(this.CreateRootNode());
			this.tree.SkipLinkText = String.Empty;
			this.Controls.Add(this.tree);

			this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID + "getSelectedValuePath", String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {{ document.getElementById('{0}').getSelectedValuePath = function() {{ var h = document.getElementById(document.getElementById('{1}_SelectedNode').value); if (!h) {{ return null }} else {{ return h.href.split('\\'')[3].substr(1).replace(/\\\\/g, '/') }} }} }});\n", this.ClientID, this.tree.ClientID), true);
			this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID + "getSelectedValue", String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {{ document.getElementById('{0}').getSelectedValue = function() {{ var h = document.getElementById(document.getElementById('{1}_SelectedNode').value); if (!h) {{ return null }} else {{ var i = h.href.lastIndexOf('\\\\'); return h.href.substr(i + 1, h.href.length - i - 3) }} }} }});\n", this.ClientID, this.tree.ClientID), true);
			this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID + "getSelectedText", String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {{ document.getElementById('{0}').getSelectedText = function() {{ var h = document.getElementById(document.getElementById('{1}_SelectedNode').value); if (!h) {{ return null }} else {{ return h.innerHTML }}  }} }});\n", this.ClientID, this.tree.ClientID), true);
			this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID + "getCheckedNodes", String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {{ document.getElementById('{0}').getCheckedNodes = function() {{ return document.querySelectorAll('input[type=checkbox][id^={1}]:checked') }} }});\n", this.ClientID, this.tree.ClientID), true);
			this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID + "selectNode", String.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function() {{ document.getElementById('{0}').selectNode = function(valuePath) {{ var arg = 's' + valuePath.replace(/\\//g, '\\\\\'); __doPostBack('{1}', arg) }} }});\n", this.ClientID, this.tree.UniqueID), true);
		
			base.CreateChildControls();
		}

		protected virtual TreeNode CreateRootNode()
		{
			var node = new TreeNode(this.RootText, this.RootValue);
			node.PopulateOnDemand = true;
			node.Expanded = false;
			node.SelectAction = TreeNodeSelectAction.Expand;
			node.ImageToolTip = node.Text;
			node.ImageUrl = this.RootImageUrl;

			return (node);
		}

		protected void OnPopulateTreeNode(object sender, TreeNodeEventArgs e)
		{
			var handler = this.PopulateTreeNode;
			var node = e.Node;

			if (handler != null)
			{
				var args = new PopulateTreeNodeEventArgs(node);

				handler(this, args);

				var count = node.ChildNodes.Count;

				for (var i = 0;i < count; ++i)
				{
					var child = node.ChildNodes[i];
					child.Expanded = false;
					child.PopulateOnDemand = (args.CanHaveChildren == true) && (child.ChildNodes.Count == 0);

					if (args.CanHaveChildren == false)
					{
						if (args.CanSelect == true)
						{
							child.SelectAction = TreeNodeSelectAction.Select;
						}
						else
						{
							child.SelectAction = TreeNodeSelectAction.None;
						}
					}
					else
					{
						if (args.CanSelect == true)
						{
							child.SelectAction = TreeNodeSelectAction.SelectExpand;
						}
						else
						{
							child.SelectAction = TreeNodeSelectAction.Expand;
						}
					}
				}
			}
		}
	}
}