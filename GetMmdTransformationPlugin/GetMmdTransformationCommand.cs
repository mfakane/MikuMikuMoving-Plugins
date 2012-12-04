using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using DxMath;
using Linearstar.MikuMikuMoving.Framework;
using Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Properties;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin
{
	public class GetMmdTransformationCommand : CommandBase
	{
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern IntPtr GetModuleHandle(string lpModuleName);

		public override void Run(CommandArgs e)
		{
			if (this.Scene.Mode != EditMode.ModelMode ||
				this.Scene.ActiveModel == null)
			{
				MessageBox.Show(this.ApplicationForm, Util.Bilingual
				(
					this.Scene.Language,
					"適用するモデルがありません。\r\nモデルを選択している状態で実行してください。",
					"No model to apply.\r\nPlease select a model to proceed."
				), Util.Bilingual(this.Scene.Language, this.Text, this.EnglishText), MessageBoxButtons.OK, MessageBoxIcon.Information);
				e.Cancel = true;

				return;
			}

			var mmds = Process.GetProcessesByName("MikuMikuDance")
							  .Where(_ => _.Responding)
							  .Select(_ => new MmdImport(_))
							  .ToArray();

			try
			{
				if (!mmds.Any())
				{
					MessageBox.Show(this.ApplicationForm, Util.Bilingual
					(
						this.Scene.Language,
						"MikuMikuDance が起動されていません。\r\nMikuMikuDance が起動中であり、モデルが読み込まれている状態で実行してください。",
						"Cannot find MikuMikuDance.\r\nPlease start MikuMikuDance and load a model to proceed."
					), Util.Bilingual(this.Scene.Language, this.Text, this.EnglishText), MessageBoxButtons.OK, MessageBoxIcon.Information);
					e.Cancel = true;

					return;
				}

				mmds = mmds.Where(_ => _.Models.Any()).ToArray();

				if (!mmds.Any())
				{
					MessageBox.Show(this.ApplicationForm, Util.Bilingual
					(
						this.Scene.Language,
						"MikuMikuDance にモデルが読み込まれていません。\r\nモデルが読み込まれている状態で実行してください。",
						"No model found on MikuMikuDance.\r\nNeeds at least one model loaded on MikuMikuDance to proceed."
					), Util.Bilingual(this.Scene.Language, this.Text, this.EnglishText), MessageBoxButtons.OK, MessageBoxIcon.Information);
					e.Cancel = true;

					return;
				}

				Action reset = null;

				using (var f = new GetMmdTransformationForm(this.Scene.Language, mmds))
				{
					f.SelectedModelChanged += (sender, e2) => SynchronizeTransformation(f.SelectedModel, ref reset);

					if (f.ShowDialog(this.ApplicationForm) != DialogResult.OK)
					{
						e.Cancel = true;

						if (reset != null)
						{
							reset();
							reset = null;
						}

						return;
					}

					SynchronizeTransformation(f.SelectedModel, ref reset);
				}
			}
			finally
			{
				foreach (var i in mmds)
					i.Dispose();
			}
		}

		
		void SynchronizeTransformation(MmdModel mmdModel, ref Action reset)
		{
			if (mmdModel == null)
				return;

			var model = this.Scene.ActiveModel;
			var bones = model.Bones;
			var morphs = model.Morphs;
			var boneList = mmdModel.Bones.Select(_ => new BoneTuple
			{
				MmdBone = _,
				Transform = _.Transform,
				Bone = bones[_.Name],
			}).Where(_ => _.Bone != null).ToArray();

			if (reset != null)
			{
				reset();
				reset = null;
			}

			foreach (var i in boneList)
			{
				var layer = i.Bone.Layers.First();
				var local = layer.CurrentLocalMotion;
				var isSelected = layer.Selected;
				var transform = i.Transform;
				var initialPosition = i.Bone.InitialPosition;
				Vector3 translation;
				Quaternion rotation;
				Vector3 scale;

				ResolveParent(boneList, i, ref transform, ref initialPosition);
				ResolveAffect(boneList, i, ref transform, ref initialPosition);

				transform *= Matrix.Translation(-initialPosition);
				transform.Decompose(out scale, out rotation, out translation);

				reset += () =>
				{
					layer.CurrentLocalMotion = local;
					layer.Selected = isSelected;
				};

				layer.CurrentLocalMotion = new MotionData((i.Bone.BoneFlags & BoneType.XYZ) != 0 ? translation : Vector3.Zero, rotation);
				layer.Selected = true;
			}

			foreach (var i in mmdModel.Morphs)
			{
				var morph = morphs[i.Name];

				if (morph != null)
				{
					var local = morph.CurrentWeight;
					var newWeight = i.Weight;

					reset += () => morph.CurrentWeight = local;
					morph.CurrentWeight = newWeight;
					morph.Selected = true;
				}
			}
		}

		static void ResolveParent(BoneTuple[] boneList, BoneTuple i, ref Matrix transform, ref Vector3 initialPosition)
		{
			if (i.Bone.ParentBoneID != -1)
			{
				var parentId = i.Bone.ParentBoneID;
				var parentBone = boneList.FirstOrDefault(_ => _.Bone.BoneID == parentId);

				if (parentBone != null)
				{
					initialPosition -= parentBone.Bone.InitialPosition;
					transform *= Matrix.Invert(parentBone.MmdBone.Transform);
				}
			}
		}

		static void ResolveAffect(BoneTuple[] boneList, BoneTuple i, ref Matrix transform, ref Vector3 initialPosition)
		{
			if ((i.Bone.BoneFlags & BoneType.InhereRotate) != 0 ||
				(i.Bone.BoneFlags & BoneType.InhereXYZ) != 0)
			{
				var affectId = i.Bone.InhereBoneID;
				var affectBone = boneList.FirstOrDefault(_ => _.Bone.BoneID == affectId);

				if (affectBone != null)
				{
					var affectInitialPosition = affectBone.Bone.InitialPosition;
					var affectTransform = affectBone.Transform;

					if (affectBone.Bone.ParentBoneID != -1)
						ResolveParent(boneList, affectBone, ref affectTransform, ref affectInitialPosition);

					Vector3 affectTranslation;
					Quaternion affectRotation;
					Vector3 affectScale;

					affectTransform.Decompose(out affectScale, out affectRotation, out affectTranslation);

					if ((i.Bone.BoneFlags & BoneType.InhereRotate) != 0)
						transform *= Matrix.Invert(Matrix.RotationAxis(affectRotation.Axis, affectRotation.Angle * i.Bone.InheraRate));

					if ((i.Bone.BoneFlags & BoneType.InhereXYZ) != 0)
						initialPosition += (affectTranslation - affectInitialPosition) * i.Bone.InheraRate;
				}
			}
		}

		public override string EnglishText
		{
			get
			{
				return Util.EnvorinmentNewLine("Get MMD\r\nTransformation");
			}
		}

		public override string Text
		{
			get
			{
				return Util.EnvorinmentNewLine("MMD\r\nポーズ取得");
			}
		}

		public override string Description
		{
			get
			{
				return Util.Bilingual(this.Scene.Language, "MikuMikuDance のモデル変形状態を取得します。", "Receives model transformation status from MikuMikuDance.");
			}
		}

		public override Guid GUID
		{
			get
			{
				return new Guid("2c309851-7f3e-4b62-87f9-44e5c492a0f8");
			}
		}

		public override Image Image
		{
			get
			{
				return Resources.GetMmdTransformation32;
			}
		}

		public override Image SmallImage
		{
			get
			{
				return Resources.GetMmdTransformation20;
			}
		}

		class BoneTuple
		{
			public MmdBone MmdBone
			{
				get;
				set;
			}

			public Bone Bone
			{
				get;
				set;
			}

			public Matrix Transform
			{
				get;
				set;
			}
		}
	}
}
