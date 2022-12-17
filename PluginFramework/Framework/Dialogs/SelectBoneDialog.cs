using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework
{
	public partial class SelectBoneDialog : Form
	{
		public Bone SelectedBone
		{
			get;
			set;
		}

		public MotionLayer SelectedLayer
		{
			get;
			set;
		}

		public Model Model
		{
			get;
			set;
		}

		public SelectBoneDialog()
		{
			InitializeComponent();
		}

		void CreateFromDisplayFrame(IEnumerable<DisplayFrame> frames)
		{
			boneTreeView.Nodes.Clear();
			boneTreeView.Nodes.AddRange(frames.Select(d =>
				new TreeNode(d.DisplayName, d.Bones.Select(b =>
					new TreeNode(b.DisplayName, 1, 1, b.Layers.Skip(1).Select(l =>
						new TreeNode(l.Name, 2, 2)
						{
							Tag = new NodeTag
							{
								Bone = b,
								Layer = l,
							},
						}).ToArray())
					{
						Tag = new NodeTag
						{
							Bone = b,
							Layer = b.Layers.First(),
						},
					}).ToArray())).ToArray());
			boneTreeView.ExpandAll();
		}

		void CreateFromTree(IEnumerable<Bone> bones, Bone root)
		{
			boneTreeView.Nodes.Clear();
			boneTreeView.Nodes.Add(CreateNode(bones, root));
			boneTreeView.ExpandAll();
		}

		TreeNode CreateNode(IEnumerable<Bone> bones, Bone item)
		{
			return new TreeNode(item.Name, 1, 1, item.Layers.Skip(1).Select(l =>
				new TreeNode(l.Name, 2, 2)
				{
					Tag = new NodeTag
					{
						Bone = item,
						Layer = l,
					},
				}).Concat(bones.Where(_ => _.ParentBoneID == item.BoneID).Select(_ => CreateNode(bones, _)).ToArray()).ToArray())
			{
				Tag = new NodeTag
				{
					Bone = item,
					Layer = item.Layers.First(),
				},
			};
		}

		IEnumerable<TreeNode> SelectNodes(TreeNode node)
		{
			return node.Nodes.Cast<TreeNode>().SelectMany(SelectNodes);
		}

		void SelectBoneDialog_Load(object sender, EventArgs e)
		{
			CreateFromDisplayFrame(this.Model.DisplayFrame);

			if (this.SelectedBone != null &&
				this.SelectedLayer != null)
				boneTreeView.SelectedNode = boneTreeView.Nodes.Cast<TreeNode>().SelectMany(SelectNodes).Where(_ => _.Tag is NodeTag).Select(_ => new
				{
					Node = _,
					Tag = (NodeTag)_.Tag,
				}).Where(_ => _.Tag.Bone == this.SelectedBone && _.Tag.Layer == this.SelectedLayer).Select(_ => _.Node).FirstOrDefault();
		}

		void displayFrameRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (displayFrameRadioButton.Checked)
				CreateFromDisplayFrame(this.Model.DisplayFrame);
			else
				CreateFromTree(this.Model.Bones, this.Model.Bones.RootBone);
		}

		void boneTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (okButton.Enabled = e.Node != null &&
				e.Node.Tag is NodeTag)
			{
				var tag = (NodeTag)e.Node.Tag;

				this.SelectedBone = tag.Bone;
				this.SelectedLayer = tag.Layer;
			}
			else
			{
				this.SelectedBone = null;
				this.SelectedLayer = null;
			}
		}

		void boneTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (okButton.Enabled)
				okButton.PerformClick();
		}

		class NodeTag
		{
			public Bone Bone
			{
				get;
				set;
			}

			public MotionLayer Layer
			{
				get;
				set;
			}
		}
	}
}
