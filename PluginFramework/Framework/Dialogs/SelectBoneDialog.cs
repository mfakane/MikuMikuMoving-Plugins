using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework;

public partial class SelectBoneDialog : Form
{
	public Bone? SelectedBone { get; set; }

	public MotionLayer? SelectedLayer { get; set; }

	public Model Model { get; } = null!;

	SelectBoneDialog()
	{
		InitializeComponent();
	}

	public SelectBoneDialog(Model model)
		: this()
	{
		Model = model;
	}

	void CreateFromDisplayFrame(IReadOnlyCollection<DisplayFrame> frames)
	{
		boneTreeView.Nodes.Clear();
		boneTreeView.Nodes.AddRange(frames
			.Select(d => new TreeNode(d.DisplayName, d.Bones
				.Select(b => new TreeNode(b.DisplayName, 1, 1, b.Layers
					.Skip(1)
					.Select(l => new TreeNode(l.Name, 2, 2)
					{
						Tag = new NodeTag(b, l),
					})
					.ToArray())
				{
					Tag = new NodeTag(b, b.Layers.First()),
				})
				.ToArray()))
			.ToArray());
		boneTreeView.ExpandAll();
	}

	void CreateFromTree(IReadOnlyCollection<Bone> bones, Bone root)
	{
		boneTreeView.Nodes.Clear();
		boneTreeView.Nodes.Add(CreateNode(bones, root));
		boneTreeView.ExpandAll();
	}

	TreeNode CreateNode(IReadOnlyCollection<Bone> bones, Bone item) =>
		new(item.Name, 1, 1, item.Layers
			.Skip(1)
			.Select(layer => new TreeNode(layer.Name, 2, 2)
			{
				Tag = new NodeTag(item, layer),
			})
			.Concat(bones
				.Where(bone => bone.ParentBoneID == item.BoneID)
				.Select(bone => CreateNode(bones, bone))
				.ToArray())
			.ToArray())
		{
			Tag = new NodeTag(item, item.Layers.First()),
		};

	IEnumerable<TreeNode> SelectNodes(TreeNode node) =>
		node.Nodes.Cast<TreeNode>().SelectMany(SelectNodes);

	void SelectBoneDialog_Load(object sender, EventArgs e)
	{
		CreateFromDisplayFrame(Model.DisplayFrame.ToArray());

		if (SelectedBone != null &&
		    SelectedLayer != null)
			boneTreeView.SelectedNode = boneTreeView.Nodes.Cast<TreeNode>()
				.SelectMany(SelectNodes)
				.Where(node => node.Tag is NodeTag)
				.Select(node => new
				{
					Node = node,
					Tag = (NodeTag)node.Tag,
				})
				.Where(node => node.Tag.Bone == SelectedBone && node.Tag.Layer == SelectedLayer)
				.Select(node => node.Node)
				.FirstOrDefault();
	}

	void displayFrameRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		if (displayFrameRadioButton.Checked)
			CreateFromDisplayFrame(Model.DisplayFrame.ToArray());
		else
			CreateFromTree(Model.Bones.ToArray(), Model.Bones.RootBone);
	}

	void boneTreeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		if (e.Node.Tag is NodeTag tag)
		{
			SelectedBone = tag.Bone;
			SelectedLayer = tag.Layer;
			okButton.Enabled = true;
		}
		else
		{
			SelectedBone = null;
			SelectedLayer = null;
			okButton.Enabled = false;
		}
	}

	void boneTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		if (okButton.Enabled)
			okButton.PerformClick();
	}

	class NodeTag
	{
		public Bone Bone { get; }
		public MotionLayer Layer { get; }
		
		public NodeTag(Bone bone, MotionLayer layer)
		{
			Bone = bone;
			Layer = layer;
		}
	}
}