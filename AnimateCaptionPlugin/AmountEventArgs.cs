using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin
{
	public class AmountEventArgs : EventArgs
	{
		public float Amount
		{
			get;
			set;
		}

		public int Index
		{
			get;
			set;
		}
	}
}
