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

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Mmd;

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

	public string ExpGetPmdFilename(int arg0)
	{
		return InvokeRemote<string>("ExpGetPmdFilename", new int[] { arg0 });
	}

	public int ExpGetPmdOrder(int arg0)
	{
		return InvokeRemote<int>("ExpGetPmdOrder", new int[] { arg0 });
	}

	public int ExpGetPmdMatNum(int arg0)
	{
		return InvokeRemote<int>("ExpGetPmdMatNum", new int[] { arg0 });
	}

	public int ExpGetPmdBoneNum(int arg0)
	{
		return InvokeRemote<int>("ExpGetPmdBoneNum", new int[] { arg0 });
	}

	public string ExpGetPmdBoneName(int arg0, int arg1)
	{
		return InvokeRemote<string>("ExpGetPmdBoneName", new int[] { arg0, arg1 });
	}

	public Matrix ExpGetPmdBoneWorldMat(int arg0, int arg1)
	{
		return InvokeRemote<Matrix>("ExpGetPmdBoneWorldMat", new int[] { arg0, arg1 });
	}

	public int ExpGetPmdMorphNum(int arg0)
	{
		return InvokeRemote<int>("ExpGetPmdMorphNum", new int[] { arg0 });
	}

	public string ExpGetPmdMorphName(int arg0, int arg1)
	{
		return InvokeRemote<string>("ExpGetPmdMorphName", new int[] { arg0, arg1 });
	}

	public float ExpGetPmdMorphValue(int arg0, int arg1)
	{
		return InvokeRemote<float>("ExpGetPmdMorphValue", new int[] { arg0, arg1 });
	}

	public bool ExpGetPmdDisp(int arg0)
	{
		return InvokeRemote<bool>("ExpGetPmdDisp", new int[] { arg0 });
	}

	public int ExpGetPmdID(int arg0)
	{
		return InvokeRemote<int>("ExpGetPmdID", new int[] { arg0 });
	}

	public int ExpGetAcsNum()
	{
		return InvokeRemote<int>("ExpGetAcsNum", new int[] {  });
	}

	public int ExpGetPreAcsNum()
	{
		return InvokeRemote<int>("ExpGetPreAcsNum", new int[] {  });
	}

	public string ExpGetAcsFilename(int arg0)
	{
		return InvokeRemote<string>("ExpGetAcsFilename", new int[] { arg0 });
	}

	public int ExpGetAcsOrder(int arg0)
	{
		return InvokeRemote<int>("ExpGetAcsOrder", new int[] { arg0 });
	}

	public Matrix ExpGetAcsWorldMat(int arg0)
	{
		return InvokeRemote<Matrix>("ExpGetAcsWorldMat", new int[] { arg0 });
	}

	public float ExpGetAcsX(int arg0)
	{
		return InvokeRemote<float>("ExpGetAcsX", new int[] { arg0 });
	}

	public float ExpGetAcsY(int arg0)
	{
		return InvokeRemote<float>("ExpGetAcsY", new int[] { arg0 });
	}

	public float ExpGetAcsZ(int arg0)
	{
		return InvokeRemote<float>("ExpGetAcsZ", new int[] { arg0 });
	}

	public float ExpGetAcsRx(int arg0)
	{
		return InvokeRemote<float>("ExpGetAcsRx", new int[] { arg0 });
	}

	public float ExpGetAcsRy(int arg0)
	{
		return InvokeRemote<float>("ExpGetAcsRy", new int[] { arg0 });
	}

	public float ExpGetAcsRz(int arg0)
	{
		return InvokeRemote<float>("ExpGetAcsRz", new int[] { arg0 });
	}

	public float ExpGetAcsSi(int arg0)
	{
		return InvokeRemote<float>("ExpGetAcsSi", new int[] { arg0 });
	}

	public float ExpGetAcsTr(int arg0)
	{
		return InvokeRemote<float>("ExpGetAcsTr", new int[] { arg0 });
	}

	public bool ExpGetAcsDisp(int arg0)
	{
		return InvokeRemote<bool>("ExpGetAcsDisp", new int[] { arg0 });
	}

	public int ExpGetAcsID(int arg0)
	{
		return InvokeRemote<int>("ExpGetAcsID", new int[] { arg0 });
	}

	public int ExpGetAcsMatNum(int arg0)
	{
		return InvokeRemote<int>("ExpGetAcsMatNum", new int[] { arg0 });
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
