﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AcupunctureProject.Database;
using DPoint = AcupunctureProject.Database.Point;

namespace AcupunctureProject.GUI
{
	/// <summary>
	/// Interaction logic for PointInfo.xaml
	/// </summary>
	public partial class PointInfo : Window, INotifyPropertyChanged
	{
		private DPoint point;

		private List<Symptom> SymptomToRemove { get; set; }
		private List<SymptomPoint> SymptomToAdd { get; set; }

		private List<DPoint> _AllPoints;
		public List<DPoint> AllPoints
		{
			get => _AllPoints;
			set
			{
				if (_AllPoints != value)
				{
					_AllPoints = value;
					OnPropertyChanged();
				}
			}
		}

		List<Symptom> _SymptomSearchListItems;
		public List<Symptom> SymptomSearchListItems
		{
			get => _SymptomSearchListItems;
			set
			{
				if (value != _SymptomSearchListItems)
				{
					_SymptomSearchListItems = value;
					OnPropertyChanged();
				}
			}
		}

		public PointInfo(DPoint point)
		{
			DataContext = this;
			InitializeComponent();
			Title += point.Name;
			Main.UpdatePoints += new Main.EventHandler(Update);
			this.point = point;
			SymptomToAdd = new List<SymptomPoint>();
			SymptomToRemove = new List<Symptom>();
			ReloadSymptomSearchList();
			Update();
			syptomTreeView.Items.Clear();
			DatabaseConnection.GetChildren(point);
			List<Symptom> symptomList = DatabaseConnection.GetChildren(point.Symptoms);
			foreach (var sym in symptomList)
				AddItemToSymptomTree(sym);
			var folder = System.Reflection.Assembly.GetEntryAssembly().Location;
			folder = folder.Remove(folder.LastIndexOf('\\') + 1);
			if (System.IO.File.Exists(point.Image))
				pointImage.Source = new BitmapImage(new Uri(folder + point.Image));
			name.Content = point.Name;
			minDepth.Text = point.MinNeedleDepth.ToString();
			maxDepth.Text = point.MaxNeedleDepth.ToString();
			place.Text = point.Position;
			note.Text = point.Note;
			comment1.Text = point.Comment1;
			comment2.Text = point.Comment2;
		}

		~PointInfo() => Main.UpdatePoints -= new Main.EventHandler(Update);

		private void Points_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender is ListView s)
			{
				var item = s.SelectedItem as DPoint;
				if (item == null)
					return;
				SetAll(item);
			}
		}

		private void SetAll(DPoint point) =>
			new PointInfo(point).Show();

		private void SyptomTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var item = (TreeViewItem)syptomTreeView.SelectedItem;
			if (item == null)
				return;
			if (item.DataContext.GetType() == typeof(DPoint))
				SetAll((DPoint)item.DataContext);
		}

		private void SaveData()
		{
			point.Comment1 = comment1.Text;
			point.Comment2 = comment2.Text;
			point.MaxNeedleDepth = int.Parse(maxDepth.Text);
			point.MinNeedleDepth = int.Parse(minDepth.Text);
			point.Note = note.Text;
			point.Position = place.Text;
			for (int i = 0; i < SymptomToAdd.Count; i++)
			{
				int j = 0;
				while (j < SymptomToRemove.Count)
				{
					if (SymptomToAdd[i].Symptom.Equals(SymptomToRemove[j]))
					{
						SymptomToRemove.RemoveAt(j);
						SymptomToAdd.RemoveAt(i);
						continue;
					}
					j++;
				}
			}
			point.SymptomConnections.AddRange(SymptomToAdd);
			foreach (var sym in SymptomToRemove)
				point.SymptomConnections.RemoveAll(s => s.SymptomId == sym.Id);
			DatabaseConnection.Update(point);
		}

		private void Censel_Click(object sender, RoutedEventArgs e) =>
			Close();

		private void SaveAndExit_Click(object sender, RoutedEventArgs e)
		{
			SaveData();
			Close();
		}

		private void Save_Click(object sender, RoutedEventArgs e) =>
			SaveData();

		private void PointImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
			new FullWindowPic(pointImage.Source, (int)(pointImage.ActualWidth * 2),
												 (int)(pointImage.ActualHeight * 2)).Show();

		private void PointsSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			var start = DateTime.Now;
			Update();
			Console.WriteLine($"update time: {(DateTime.Now - start).TotalMilliseconds}");
		}

		private void SetSymptomSearchListVisability(bool val)
		{
			if (val)
			{
				symptomSearchList.Margin = new Thickness(symptomSearch.Margin.Left, symptomSearch.Margin.Top + symptomSearch.ExtentHeight, 0, 0);
				symptomSearchList.Visibility = Visibility.Visible;
				symptomSearchList.MaxHeight = 100;
			}
			else
			{
				symptomSearchList.Visibility = Visibility.Hidden;
				symptomSearchList.MaxHeight = 0;
			}
		}

		private void ReloadSymptomSearchList() =>
			SymptomSearchListItems = DatabaseConnection.FindSymptom(symptomSearch.Text);

		private void SelectSymptom()
		{
			var item = (Symptom)symptomSearchList.SelectedItem;
			if (item == null)
				return;
			for (int i = 0; i < syptomTreeView.Items.Count; i++)
				if (item.Equals(((TreeViewItem)syptomTreeView.Items[i]).DataContext))
					return;

			SymptomToAdd.Add(new SymptomPoint() { Symptom = item, Importance = symptomSearchImportece.SelectedIndex, Comment = "" });
			AddItemToSymptomTree(item);
			SetSymptomSearchListVisability(false);
		}

		private void AddItemToSymptomTree(Symptom sym)
		{
			TreeViewItem symptom = new TreeViewItem() { Header = sym.ToString(), DataContext = sym };
			foreach (var con in sym.PointsConnections)
			{
				DatabaseConnection.GetChildren(con);
				symptom.Items.Add(new TreeViewItem()
				{
					Header = con.Point.ToString(),
					DataContext = con.Point,
					Foreground = new SolidColorBrush()
					{
						Color = DatabaseConnection.GetLevel(con.Importance)
					}
				});
			}
			foreach (var cancon in sym.ChannelConnections)
			{
				DatabaseConnection.GetChildren(cancon);
				symptom.Items.Add(new TreeViewItem()
				{
					Header = cancon.Channel.ToString(),
					DataContext = cancon.Channel,
					Foreground = new SolidColorBrush()
					{
						Color = DatabaseConnection.GetLevel(cancon.Importance)
					}
				});
			}
			syptomTreeView.Items.Add(symptom);
		}

		private void SymptomSearch_TextChanged(object sender, TextChangedEventArgs e) => ReloadSymptomSearchList();

		private void SymptomSearchList_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				SelectSymptom();
		}

		private void SymptomSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e) => 
			SelectSymptom();

		private void DeleteSymptom_Click(object sender, RoutedEventArgs e)
		{
			TreeViewItem item = (TreeViewItem)syptomTreeView.SelectedItem;
			if (item == null)
				return;
			while (item.Parent.GetType() == typeof(TreeViewItem))
				item = (TreeViewItem)item.Parent;
			SymptomToRemove.Add((Symptom)item.DataContext);
			syptomTreeView.Items.Remove(item);
		}

		private void SymptomSearch_GotFocus(object sender, RoutedEventArgs e) => 
			SetSymptomSearchListVisability(true);

		private void SymptomSearch_LostFocus(object sender, RoutedEventArgs e) =>
			SetSymptomSearchListVisability(false);

		private void SymptomSearchList_GotFocus(object sender, RoutedEventArgs e) =>
			SetSymptomSearchListVisability(true);

		private void SymptomSearchList_LostFocus(object sender, RoutedEventArgs e) => 
			SetSymptomSearchListVisability(false);

		private void Update()
		{
			if (pointsSearch.Text == "")
				AllPoints = Main.AllPoints;
			else
				AllPoints = Main.AllPoints.FindAll(s => s.Name.ToLower().Contains(pointsSearch.Text.ToLower()));

		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
