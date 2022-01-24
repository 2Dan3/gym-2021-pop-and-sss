﻿using SR36_2020_POP2021.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SR36_2020_POP2021.UI
{
    /// <summary>
    /// Interaction logic for TraineesWindow.xaml
    /// </summary>
    public partial class TraineesWindow : Window
    {
        ICollectionView view;
        public TraineesWindow()
        {
            InitializeComponent();
            UpdateView();
            view.Filter=CustomFilter;
        }

        private bool CustomFilter(object obj)
        {
            RegisteredUser ru = obj as RegisteredUser;
            //*TODO* Dodati proveru da li je type.equals("trainee")
            if (ru.Deleted.Equals("N"))
            {
                if (txtSearchBar.Text != "")
                {
                    return ru.Name.Contains(txtSearchBar.Text) || ru.LastName.Contains(txtSearchBar.Text);
                }
                else
                    return true;
            }
            return false;
        }

        private void UpdateView()
        {
            DGTrainees.ItemsSource = null;
            view = CollectionViewSource.GetDefaultView(FitnessCenter.Instance.RegisteredUsers);
            DGTrainees.ItemsSource = view;
            DGTrainees.IsSynchronizedWithCurrentItem = true;

            DGTrainees.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

            DGTrainees.SelectedItems.Clear();
        }

        private void DGTrainees_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.Equals("Deleted"))
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void TxtSearchBar_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            view.Refresh();
        }

        private void AddTrainee_Click(object sender, RoutedEventArgs e)
        {
            //Trainee tr = new Trainee();
            AddEditTrainee addEditTrainee = new AddEditTrainee(null); //tr
            this.Hide();
            if (!(bool)addEditTrainee.ShowDialog())
            {

            }
            this.Show();
        }

        private void ModifyTrainee_Click(object sender, RoutedEventArgs e)
        {
            RegisteredUser selectedTrainee = view.CurrentItem as RegisteredUser;

            RegisteredUser oldTr = selectedTrainee.Clone();

            AddEditTrainee addEditTrainee = new AddEditTrainee(selectedTrainee);
            this.Hide();
            if (!(bool)addEditTrainee.ShowDialog())
            {
                int index = FitnessCenter.Instance.RegisteredUsers.ToList().FindIndex(u => u.Jmbg.Equals(oldTr.Jmbg));
                FitnessCenter.Instance.RegisteredUsers[index] = oldTr;
            }
            this.Show();

            view.Refresh();
            DGTrainees.SelectedItems.Clear();

        }
        /* TODO Otkomentarisi i preradi metodu
        private void DeleteTrainee_Click(object sender, RoutedEventArgs e)
        {
            RegisteredUser traineeToBeDeleted = view.CurrentItem as RegisteredUser;
            FitnessCenter.Instance.DeleteUser(traineeToBeDeleted.Jmbg);

            int index = FitnessCenter.Instance.RegisteredUsers.ToList().FindIndex(user => user.Jmbg.Equals(traineeToBeDeleted.Jmbg));
            FitnessCenter.Instance.RegisteredUsers[index].Deleted = "D";


            UpdateView();
            view.Refresh();
        }*/
    }
}
