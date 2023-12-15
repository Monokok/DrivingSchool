﻿using Autoschool.Utils;
using Autoschool.ViewModel;
using Ninject;
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
using System.Windows.Shapes;

namespace Autoschool.View
{
    /// <summary>
    /// Логика взаимодействия для StudentMainWindow.xaml
    /// </summary>
    public partial class StudentMainWindow : Window
    {
        public StudentMainWindow()
        {
            InitializeComponent();
            DataContext = Injector.Kernel.Get<AppMainViewModel>();
        }

       
    }
}
