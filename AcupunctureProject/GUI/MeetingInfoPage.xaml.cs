using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AcupunctureProject.Database;
using DPoint = AcupunctureProject.Database.Point;
using DColor = AcupunctureProject.Database.Color;
using Color = System.Windows.Media.Color;
using DTreatment = AcupunctureProject.Database.Treatment;
using vTreatmentList = AcupunctureProject.GUI.TreatmentList;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AcupunctureProject.GUI
{
	/// <summary>
	/// Interaction logic for MeetingInfoPage.xaml
	/// </summary>
	public partial class MeetingInfoPage : Page, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private void PropertyChangedEvent([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		private bool _Enable = true;
		public bool Enable
		{
			get => _Enable;
			set
			{
				if (_Enable != value)
				{
					_Enable = value;
					PropertyChangedEvent();
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
					PropertyChangedEvent();
				}
			}
		}

		List<DPoint> _PointThatUsedSearchListItems;
		public List<DPoint> PointThatUsedSearchListItems
		{
			get => _PointThatUsedSearchListItems;
			set
			{
				if (value != _PointThatUsedSearchListItems)
				{
					_PointThatUsedSearchListItems = value;
					PropertyChangedEvent();
				}
			}
		}

		private Window Perent;
		private Patient SelectedPatient;
		private Meeting Meeting;
		private List<DPoint> PointsToAdd;
		private List<DPoint> PointsToRemove;
		private List<Symptom> SymptomsToAdd;
		private List<Symptom> SymptomsToRemove;
		private List<DTreatment> TreatmentsToAdd;
		private List<DTreatment> TreatmentsToRemove;

		private MeetingInfoPage()
		{
			DataContext = this;
			InitializeComponent();
		}

		public MeetingInfoPage(Window perent, Meeting meeting) : this()
		{
			Perent = perent;
			Meeting = meeting;
			PointsToAdd = new List<DPoint>();
			PointsToRemove = new List<DPoint>();
			SymptomsToAdd = new List<Symptom>();
			SymptomsToRemove = new List<Symptom>();
			SelectedPatient = meeting.Patient;
			patientSearchTextBox.Text = SelectedPatient.Name;
			date.SelectedDate = meeting.Date;
			resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.NOT_SET.MyToString(), DataContext = ResultValue.NOT_SET });
			resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.BETTER.MyToString(), DataContext = ResultValue.BETTER });
			resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.WORSE.MyToString(), DataContext = ResultValue.WORSE });
			resolt.Items.Add(new ComboBoxItem() { Content = ResultValue.NO_CHANGE.MyToString(), DataContext = ResultValue.NO_CHANGE });
			resolt.SelectedIndex = (int)meeting.Result;
			level0.Background = new SolidColorBrush() { Color = DatabaseConnection.GetLevel(0) };
			level1.Background = new SolidColorBrush() { Color = DatabaseConnection.GetLevel(1) };
			level2.Background = new SolidColorBrush() { Color = DatabaseConnection.GetLevel(2) };
			level3.Background = new SolidColorBrush() { Color = DatabaseConnection.GetLevel(3) };
			level4.Background = new SolidColorBrush() { Color = DatabaseConnection.GetLevel(4) };
			level5.Background = new SolidColorBrush() { Color = DatabaseConnection.GetLevel(5) };
			patientSearchTextBox.IsEnabled = false;
			Enable = true;
			List<Symptom> symptoms = meeting.Symptoms;
			for (int i = 0; i < symptoms.Count; i++)
				SddItemToSymptomTree(symptoms[i]);
			FindConnectedPoint();
			List<DPoint> pointsThatUsed = meeting.Points;
			for (int i = 0; i < pointsThatUsed.Count; i++)
				pointThatUsed.Items.Add(pointsThatUsed[i]);
			notes.Text = meeting.Description;
			resoltSummeryTextBox.Text = meeting.ResultDescription;
			resolt.SelectedItem = meeting.Result;
			summeryTextBox.Text = meeting.Summery;
			TreatmentsToAdd = new List<DTreatment>();
			TreatmentsToRemove = new List<DTreatment>();
		}

		private void SymptomSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			RelaodSymptomList();
		}

		private void Censel_Click(object sender, RoutedEventArgs e)
		{
			Perent.Close();
		}

		private void RelaodSymptomList() =>
			SymptomSearchListItems =
				DatabaseConnection.FindSymptom(symptomSearch.Text);

		private void SetPointThatUsedSearchListViability(bool val)
		{
			if (val)
			{
				pointThatUsedSearchList.Margin = new Thickness(pointThatUsedSearch.Margin.Left, pointThatUsedSearch.Margin.Top + pointThatUsedSearch.ExtentHeight, 0, 0);
				pointThatUsedSearchList.Visibility = Visibility.Visible;
				pointThatUsedSearchList.MaxHeight = 100;
			}
			else
			{
				pointThatUsedSearchList.Visibility = Visibility.Hidden;
				pointThatUsedSearchList.MaxHeight = 0;
			}
		}

		private void SetSymptomListVisibility(bool val)
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

		private void SelectSymptom()
		{
			var item = (Symptom)symptomSearchList.SelectedItem;
			if (item == null)
				return;
			for (int i = 0; i < symptomTreeView.Items.Count; i++)
			{
				TreeViewItem tempItem = (TreeViewItem)symptomTreeView.Items[i];
				Symptom temps = (Symptom)tempItem.DataContext;
				Symptom temps2 = item;
				if (temps.Id == temps2.Id)
					return;
			}
			SddItemToSymptomTree(item);
			SymptomsToAdd.Add(item);
			FindConnectedPoint();
			SetSymptomListVisibility(false);
			symptomSearch.Clear();
			RelaodSymptomList();
		}

		private void FindConnectedPoint()
		{
			Dictionary<Type, Dictionary<int, List<TreeViewItem>>> count = new Dictionary<Type, Dictionary<int, List<TreeViewItem>>>();
			for (int i = 0; i < symptomTreeView.Items.Count; i++)
			{
				TreeViewItem symItem = (TreeViewItem)symptomTreeView.Items[i];
				for (int j = 0; j < symItem.Items.Count; j++)
				{
					TreeViewItem chItem = (TreeViewItem)symItem.Items[j];
					if (chItem.DataContext.GetType() == typeof(DPoint))
					{
						var point = (DPoint)chItem.DataContext;
						if (!count.ContainsKey(chItem.DataContext.GetType()))
							count.Add(chItem.DataContext.GetType(), new Dictionary<int, List<TreeViewItem>>());
						if (!count[chItem.DataContext.GetType()].ContainsKey(point.Id))
							count[chItem.DataContext.GetType()].Add(point.Id, new List<TreeViewItem>());
						count[chItem.DataContext.GetType()][point.Id].Add(chItem);
					}
					else if (chItem.DataContext.GetType() == typeof(Channel))
					{
						var point = (Channel)chItem.DataContext;
						if (!count.ContainsKey(chItem.DataContext.GetType()))
							count.Add(chItem.DataContext.GetType(), new Dictionary<int, List<TreeViewItem>>());
						if (!count[chItem.DataContext.GetType()].ContainsKey(point.Id))
							count[chItem.DataContext.GetType()].Add(point.Id, new List<TreeViewItem>());
						count[chItem.DataContext.GetType()][point.Id].Add(chItem);
					}
				}

			}

			List<Type> typeKeys = count.Keys.ToList();
			Random rand = new Random();
			for (int i = 0; i < typeKeys.Count; i++)
			{
				Type typeKey = typeKeys[i];
				List<int> intKeys = count[typeKey].Keys.ToList();
				for (int j = 0; j < intKeys.Count; j++)
				{
					int intKey = intKeys[j];
					if (count[typeKey][intKey].Count > 1)
					{
						Color c = new Color() { A = 128, R = (byte)rand.Next(0, 255), G = (byte)rand.Next(0, 255), B = (byte)rand.Next(0, 255) };
						for (int z = 0; z < count[typeKey][intKey].Count; z++)
						{
							count[typeKey][intKey][z].Background = new SolidColorBrush(c);
						}
					}
				}
			}
		}

		private void SddItemToSymptomTree(Symptom symptom)
		{
			TreeViewItem sym = new TreeViewItem() { Header = symptom.ToString(), DataContext = symptom, IsExpanded = true };
			foreach (var symcon in symptom.PointsConnections)
			{
				sym.Items.Add(new TreeViewItem()
				{
					Header = symcon.Point.ToString(),
					DataContext = symcon.Point,
					Foreground = new SolidColorBrush()
					{
						Color = DatabaseConnection.GetLevel(symcon.Importance)
					}
				});
			}
			foreach (var symchan in symptom.ChannelConnections)
			{
				sym.Items.Add(new TreeViewItem()
				{
					Header = symchan.Channel.ToString(),
					DataContext = symchan.Channel,
					Foreground = new SolidColorBrush()
					{
						Color = DatabaseConnection.GetLevel(symchan.Importance)
					}
				});
			}
			symptomTreeView.Items.Add(sym);
		}

		private void SymptomSearchList_LostFocus(object sender, RoutedEventArgs e)
		{
			SetSymptomListVisibility(false);
		}

		private void SymptomSearchList_GotFocus(object sender, RoutedEventArgs e)
		{
			SetSymptomListVisibility(true);
		}

		private void SymptomSearch_LostFocus(object sender, RoutedEventArgs e)
		{
			SetSymptomListVisibility(false);
		}

		private void SymptomSearch_GotFocus(object sender, RoutedEventArgs e)
		{
			SetSymptomListVisibility(true);
			RelaodSymptomList();
		}

		private void SymptomSearch_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key.Equals(Key.Enter))
				SelectSymptom();
		}

		private void SymptomSearchList_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key.Equals(Key.Enter))
				SelectSymptom();
		}

		private void SymptomSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			SelectSymptom();
		}

		private void SymptomTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem item = (TreeViewItem)symptomTreeView.SelectedItem;
			if (item == null)
				return;
			if (item.DataContext.GetType() == typeof(DPoint))
			{
				var con = (DPoint)item.DataContext;
				new PointInfo(con).Show();
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			SaveData();
		}

		private void SaveData()
		{
			Meeting.Date = (DateTime)date.SelectedDate;
			Meeting.Description = notes.Text;
			Meeting.Summery = summeryTextBox.Text;
			Meeting.ResultDescription = resoltSummeryTextBox.Text;
			Meeting.Result = (ResultValue)resolt.SelectedIndex;


			for (int i = 0; i < SymptomsToAdd.Count; i++)
			{
				int j = 0;
				while (j < SymptomsToRemove.Count)
				{
					if (SymptomsToAdd[i].Id == SymptomsToRemove[j].Id)
					{
						SymptomsToAdd.RemoveAt(i);
						SymptomsToRemove.RemoveAt(j);
						continue;
					}
					j++;
				}
			}

			foreach (var sym in SymptomsToRemove)
				Meeting.Symptoms.Remove(sym);
			foreach (var sym in SymptomsToAdd)
				Meeting.Symptoms.Add(sym);
			SymptomsToAdd.Clear();
			SymptomsToRemove.Clear();

			for (int i = 0; i < PointsToAdd.Count; i++)
			{
				int j = 0;
				while (j < PointsToRemove.Count)
				{
					if (PointsToAdd[i].Name == PointsToRemove[j].Name)
					{
						PointsToRemove.RemoveAt(j);
						PointsToAdd.RemoveAt(i);
						continue;
					}
					j++;
				}
			}

			foreach (var point in PointsToRemove)
				Meeting.Points.Remove(point);
			foreach (var point in PointsToAdd)
				Meeting.Points.Add(point);
			PointsToAdd.Clear();
			PointsToRemove.Clear();

			for (int i = 0; i < TreatmentsToAdd.Count; i++)
			{
				int j = 0;
				while (j < TreatmentsToRemove.Count)
				{
					if (TreatmentsToAdd[i].Name == TreatmentsToRemove[j].Name)
					{
						TreatmentsToRemove.RemoveAt(j);
						TreatmentsToAdd.RemoveAt(i);
						continue;
					}
					j++;
				}
			}

			foreach (var treatment in TreatmentsToRemove)
				Meeting.Treatments.Remove(treatment);
			foreach (var treatment in TreatmentsToAdd)
				Meeting.Treatments.Add(treatment);
			TreatmentsToAdd.Clear();
			TreatmentsToRemove.Clear();

			DatabaseConnection.Update(Meeting);
		}

		private void SaveAndExit_Click(object sender, RoutedEventArgs e)
		{
			SaveData();
			Perent.Close();
		}

		private void OpenPatientButton_Click(object sender, RoutedEventArgs e)
		{
			PatientInfo p = new PatientInfo(SelectedPatient);
			p.Show();
		}

		private void ReloadPointThatUsedSearchList()
		{
			PointThatUsedSearchListItems =
				Main.AllPoints
					.FindAll(p => p.Name.ToLower().Contains(pointThatUsedSearch.Text.ToLower()));
			pointThatUsedSearchList.SelectedIndex = 0;
		}

		private void PointThatUsedSearch_GotFocus(object sender, RoutedEventArgs e)
		{
			SetPointThatUsedSearchListViability(true);
			ReloadPointThatUsedSearchList();
		}

		private void PointThatUsedSearch_LostFocus(object sender, RoutedEventArgs e) => 
			SetPointThatUsedSearchListViability(false);

		private void PointThatUsedSearchList_LostFocus(object sender, RoutedEventArgs e) => 
			SetPointThatUsedSearchListViability(false);

		private void PointThatUsedSearchList_GotFocus(object sender, RoutedEventArgs e) => 
			SetPointThatUsedSearchListViability(true);

		private void PointThatUsedSearch_TextChanged(object sender, TextChangedEventArgs e) => 
			ReloadPointThatUsedSearchList();

		private void PointThatUsedSearchList_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				SelectPointThatUsed();
		}

		private void SelectPointThatUsed()
		{
			var item = (DPoint)pointThatUsedSearchList.SelectedItem;
			if (item == null)
				return;
			AddItemToPointThatUsed(item);
			SetPointThatUsedSearchListViability(false);
		}

		private void PointThatUsed_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				SelectPointThatUsed();
		}

		private void PointThatUsedSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e) => 
			SelectPointThatUsed();

		private void PointThatUsedDelete_Click(object sender, RoutedEventArgs e)
		{
			var item = (DPoint)pointThatUsed.SelectedItem;
			if (item == null)
				return;
			pointThatUsed.Items.RemoveAt(pointThatUsed.SelectedIndex);
			PointsToRemove.Add(item);
		}

		private void SymptomTreeDelete_Click(object sender, RoutedEventArgs e)
		{
			TreeViewItem item = (TreeViewItem)symptomTreeView.SelectedItem;
			if (item == null)
				return;
			while (item.Parent.GetType() == typeof(TreeViewItem))
				item = (TreeViewItem)item.Parent;
			if (symptomTreeView.Items.Contains(item))
				symptomTreeView.Items.Remove(item);
			SymptomsToRemove.Add((Symptom)item.DataContext);
		}

		private void PointThatUsed_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (pointThatUsed.SelectedIndex == -1)
				return;
			var item = (DPoint)pointThatUsed.SelectedItem;
			new PointInfo(item).Show();
		}

		private void PointThatUsed_Drop(object sender, DragEventArgs e)
		{
			string[] formats = e.Data.GetFormats();
			for (int i = 0; i < formats.Length; i++)
			{
				object data = e.Data.GetData(formats[i]);
				if (data == null)
					return;
				if (data.GetType() == typeof(TreeViewItem))
				{
					TreeViewItem item = (TreeViewItem)data;
					if (item.DataContext.GetType() == typeof(DPoint))
						AddItemToPointThatUsed((DPoint)item.DataContext);
					//else if (item.DataContext.GetType() == typeof(ConnectionValue<DPoint>))) ;
				}
			}
		}

		private void SymptomTreeView_MouseMove(object sender, MouseEventArgs e)
		{
			if (symptomTreeView.SelectedItem != null && e.LeftButton.Equals(MouseButtonState.Pressed))
				DragDrop.DoDragDrop(symptomTreeView, (TreeViewItem)symptomTreeView.SelectedItem, DragDropEffects.Copy);
		}

		private void AddItemToPointThatUsed(DPoint point)
		{
			for (int i = 0; i < pointThatUsed.Items.Count; i++)
			{
				var tempItem = (DPoint)pointThatUsed.Items[i];
				if (tempItem.Equals(point.ToString()))
					return;
			}
			pointThatUsed.Items.Add(point);
		}

		private void TreatmentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender.GetType() != typeof(ListBox))
				return;
			var s = (ListBox)sender;
			if (s.SelectedIndex == -1)
				return;
			new Treatment((DTreatment)s.SelectedItem).Show();
		}

		private void TreatmentList_KeyDown(object sender, KeyEventArgs e)
		{
			if (sender.GetType() != typeof(ListBox))
				return;
			var s = (ListBox)sender;
			if (s.SelectedIndex == -1)
				return;
			if (e.Key.Equals(Key.Enter))
				new Treatment((DTreatment)s.SelectedItem).Show();
		}

		private void TreatmentSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e) => 
			SelectTreatment();

		private void TreatmentSearchList_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key.Equals(Key.Enter))
				SelectTreatment();
		}

		private void SelectTreatment()
		{
			if (TreatmentSearchList.SelectedIndex == -1)
				return;
			TreatmentList.Items.Add((DTreatment)TreatmentSearchList.SelectedItem);
			TreatmentsToAdd.Add((DTreatment)TreatmentSearchList.SelectedItem);
		}

		private void TreatmentSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (sender.GetType() != typeof(TextBox))
				return;
			TreatmentSearchList.ItemsSource =
				Main.AllTreatments
				.FindAll(t => t.Name.ToLower().Contains(((TextBox)sender).Text.ToLower()));
		}

		private void TreatmentSearchVisFalse(object sender, RoutedEventArgs e) => 
			SetTreatmentSearchListViability(false);

		private void TreatmentSearchVisTrue(object sender, RoutedEventArgs e) => 
			SetTreatmentSearchListViability(true);

		private void SetTreatmentSearchListViability(bool val) => 
			SetVis(TreatmentSearchList, TreatmentSearchTextBox, val);

		private void SetVis(ListView o, TextBox textBox, bool val)
		{
			if (val)
			{
				o.Margin = new Thickness(textBox.Margin.Left, textBox.Margin.Top + textBox.ExtentHeight, 0, 0);
				o.Visibility = Visibility.Visible;
				o.Height = 100;
			}
			else
			{
				o.Visibility = Visibility.Collapsed;
				o.Height = 0;
			}
		}

		private void TreatmentSearchTextBox_KeyDown(object sender, KeyEventArgs e) => 
			SelectTreatment();

		private void DeleteTreatment_Click(object sender, RoutedEventArgs e)
		{
			var selectedItem = TreatmentList.SelectedItem as DTreatment;
			if (selectedItem == null)
				return;
			TreatmentsToRemove.Add(selectedItem);
			TreatmentList.Items.RemoveAt(TreatmentList.SelectedIndex);
		}
	}
}
