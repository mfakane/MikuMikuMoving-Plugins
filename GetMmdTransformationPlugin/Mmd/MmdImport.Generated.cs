using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using DxMath;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin
{
	partial class MmdImport
	{
		public float ExpGetFrameTime()
		{
			return InvokeRemote<float>("ExpGetFrameTime", new int[] {  });
		}

		public int ExpGetPmdNum()
		{
			return InvokeRemote<int>("ExpGetPmdNum", new int[] {  });
		}

		public string ExpGetPmdFilename(int arg1)
		{
			return InvokeRemote<string>("ExpGetPmdFilename", new int[] { arg1 });
		}

		public int ExpGetPmdOrder(int arg1)
		{
			return InvokeRemote<int>("ExpGetPmdOrder", new int[] { arg1 });
		}

		public int ExpGetPmdMatNum(int arg1)
		{
			return InvokeRemote<int>("ExpGetPmdMatNum", new int[] { arg1 });
		}

		public int ExpGetPmdBoneNum(int arg1)
		{
			return InvokeRemote<int>("ExpGetPmdBoneNum", new int[] { arg1 });
		}

		public string ExpGetPmdBoneName(int arg1, int arg2)
		{
			return InvokeRemote<string>("ExpGetPmdBoneName", new int[] { arg1, arg2 });
		}

		public Matrix ExpGetPmdBoneWorldMat(int arg1, int arg2)
		{
			return InvokeRemote<Matrix>("ExpGetPmdBoneWorldMat", new int[] { arg1, arg2 });
		}

		public int ExpGetPmdMorphNum(int arg1)
		{
			return InvokeRemote<int>("ExpGetPmdMorphNum", new int[] { arg1 });
		}

		public string ExpGetPmdMorphName(int arg1, int arg2)
		{
			return InvokeRemote<string>("ExpGetPmdMorphName", new int[] { arg1, arg2 });
		}

		public float ExpGetPmdMorphValue(int arg1, int arg2)
		{
			return InvokeRemote<float>("ExpGetPmdMorphValue", new int[] { arg1, arg2 });
		}

		public bool ExpGetPmdDisp(int arg1)
		{
			return InvokeRemote<bool>("ExpGetPmdDisp", new int[] { arg1 });
		}

		public int ExpGetPmdID(int arg1)
		{
			return InvokeRemote<int>("ExpGetPmdID", new int[] { arg1 });
		}

		public int ExpGetAcsNum()
		{
			return InvokeRemote<int>("ExpGetAcsNum", new int[] {  });
		}

		public int ExpGetPreAcsNum()
		{
			return InvokeRemote<int>("ExpGetPreAcsNum", new int[] {  });
		}

		public string ExpGetAcsFilename(int arg1)
		{
			return InvokeRemote<string>("ExpGetAcsFilename", new int[] { arg1 });
		}

		public int ExpGetAcsOrder(int arg1)
		{
			return InvokeRemote<int>("ExpGetAcsOrder", new int[] { arg1 });
		}

		public Matrix ExpGetAcsWorldMat(int arg1)
		{
			return InvokeRemote<Matrix>("ExpGetAcsWorldMat", new int[] { arg1 });
		}

		public float ExpGetAcsX(int arg1)
		{
			return InvokeRemote<float>("ExpGetAcsX", new int[] { arg1 });
		}

		public float ExpGetAcsY(int arg1)
		{
			return InvokeRemote<float>("ExpGetAcsY", new int[] { arg1 });
		}

		public float ExpGetAcsZ(int arg1)
		{
			return InvokeRemote<float>("ExpGetAcsZ", new int[] { arg1 });
		}

		public float ExpGetAcsRx(int arg1)
		{
			return InvokeRemote<float>("ExpGetAcsRx", new int[] { arg1 });
		}

		public float ExpGetAcsRy(int arg1)
		{
			return InvokeRemote<float>("ExpGetAcsRy", new int[] { arg1 });
		}

		public float ExpGetAcsRz(int arg1)
		{
			return InvokeRemote<float>("ExpGetAcsRz", new int[] { arg1 });
		}

		public float ExpGetAcsSi(int arg1)
		{
			return InvokeRemote<float>("ExpGetAcsSi", new int[] { arg1 });
		}

		public float ExpGetAcsTr(int arg1)
		{
			return InvokeRemote<float>("ExpGetAcsTr", new int[] { arg1 });
		}

		public bool ExpGetAcsDisp(int arg1)
		{
			return InvokeRemote<bool>("ExpGetAcsDisp", new int[] { arg1 });
		}

		public int ExpGetAcsID(int arg1)
		{
			return InvokeRemote<int>("ExpGetAcsID", new int[] { arg1 });
		}

		public int ExpGetAcsMatNum(int arg1)
		{
			return InvokeRemote<int>("ExpGetAcsMatNum", new int[] { arg1 });
		}

		public int ExpGetCurrentObject()
		{
			return InvokeRemote<int>("ExpGetCurrentObject", new int[] {  });
		}

		public int ExpGetCurrentMaterial()
		{
			return InvokeRemote<int>("ExpGetCurrentMaterial", new int[] {  });
		}

		public int ExpGetCurrentTechnic()
		{
			return InvokeRemote<int>("ExpGetCurrentTechnic", new int[] {  });
		}

		public int ExpGetRenderRepeatCount()
		{
			return InvokeRemote<int>("ExpGetRenderRepeatCount", new int[] {  });
		}

	}
}