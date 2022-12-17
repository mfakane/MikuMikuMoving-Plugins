using System.Drawing;
using System.Reflection;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework;

public abstract class CommandBase : PluginBase, ICommandPlugin
{
	protected CommandBase()
	{
		var assembly = Assembly.GetExecutingAssembly();
		if (assembly.GetManifestResourceStream("Icon20") is { } icon20)
			SmallImage = Image.FromStream(icon20);
		if (assembly.GetManifestResourceStream("Icon32") is { } icon32)
			Image = Image.FromStream(icon32);
	}

	public virtual void Run(CommandArgs e)
	{
	}

	public abstract string EnglishText { get; }

	public Image? Image { get; }

	public Image? SmallImage { get; }

	public abstract string Text { get; }
}