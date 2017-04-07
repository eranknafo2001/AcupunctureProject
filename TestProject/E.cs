using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestProject
{
	public class E : INotifyPropertyChanged
	{
		private string _S;
		public string S
		{
			get => _S;
			set
			{
				if(_S!=value)
				{
					_S = value;
					OnPropertyChanged();
				}
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
