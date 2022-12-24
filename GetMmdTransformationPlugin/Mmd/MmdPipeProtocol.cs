using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using DxMath;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Mmd;

class MmdPipeProtocol : IDisposable
{
	readonly PipeStream stream;
	readonly BinaryReader br;
	readonly BinaryWriter bw;
	readonly Encoding encoding = Encoding.GetEncoding(932);

	enum Command : byte
	{
		Exit,
		Invoke,
	}

	public MmdPipeProtocol(PipeStream stream)
	{
		this.stream = stream;
		br = new(stream);
		bw = new(stream);
	}

	public IEnumerable<Message> GetMessages()
	{
		while ((Command)br.ReadByte() != Command.Exit)
			yield return new(br.ReadString(), Enumerable.Range(0, br.ReadByte()).Select(_ => br.ReadInt32()).ToArray());
	}

	public void SendReply(bool value)
	{
		bw.Write(value);
		bw.Flush();
	}

	public void SendReply(int value)
	{
		bw.Write(value);
		bw.Flush();
	}

	public void SendReply(float value)
	{
		bw.Write(value);
		bw.Flush();
	}

	public void SendReply(string value)
	{
		bw.Write((byte)encoding.GetByteCount(value));
		bw.Write(encoding.GetBytes(value));
		bw.Flush();
	}

	public void SendReply(MmdMatrix value)
	{
		foreach (var i in value.Value)
			bw.Write(i);

		bw.Flush();
	}

	public T InvokeRemote<T>(string entryPoint, int[] parameters)
	{
		object rt;

		bw.Write((byte)Command.Invoke);
		bw.Write((byte)encoding.GetByteCount(entryPoint));
		bw.Write(encoding.GetBytes(entryPoint));
		bw.Write((byte)parameters.Length);

		foreach (var i in parameters)
			bw.Write(i);

		bw.Flush();
		stream.WaitForPipeDrain();

		if (typeof(T) == typeof(bool))
			rt = br.ReadBoolean();
		else if (typeof(T) == typeof(int))
			rt = br.ReadInt32();
		else if (typeof(T) == typeof(float))
			rt = br.ReadSingle();
		else if (typeof(T) == typeof(string))
			rt = encoding.GetString(br.ReadBytes(br.ReadByte()));
		else if (typeof(T) == typeof(Matrix))
			rt = new Matrix
			{
				M11 = br.ReadSingle(),
				M12 = br.ReadSingle(),
				M13 = br.ReadSingle(),
				M14 = br.ReadSingle(),
				M21 = br.ReadSingle(),
				M22 = br.ReadSingle(),
				M23 = br.ReadSingle(),
				M24 = br.ReadSingle(),
				M31 = br.ReadSingle(),
				M32 = br.ReadSingle(),
				M33 = br.ReadSingle(),
				M34 = br.ReadSingle(),
				M41 = br.ReadSingle(),
				M42 = br.ReadSingle(),
				M43 = br.ReadSingle(),
				M44 = br.ReadSingle(),
			};
		else
			throw new ArgumentException();

		return (T)rt;
	}

	void Disconnect()
	{
		try
		{
			bw.Write((byte)Command.Exit);
			bw.Flush();
		}
		catch (ObjectDisposedException)
		{
		}
	}

	public void Dispose()
	{
		if (stream is NamedPipeServerStream)
			Disconnect();

		Dispose(false);
		GC.SuppressFinalize(this);
	}

	void Dispose(bool disposed)
	{
		br.Close();
		bw.Close();
		stream.Dispose();
	}

	~MmdPipeProtocol()
	{
		Dispose(true);
	}

	public class Message
	{
		public string EntryPoint { get; }
		public int[] Arguments { get; }
		
		public Message(string entryPoint, int[] arguments)
		{
			EntryPoint = entryPoint;
			Arguments = arguments;
		}
	}
}