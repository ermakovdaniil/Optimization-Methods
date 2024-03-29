﻿using System.Windows.Controls;

using View.Utils;


namespace View.UserInterface.Admin.User;

public partial class UserExplorerControl : UserControl
{
    private readonly UserExplorerControlVM _viewModel;

    public UserExplorerControl()
    {
        InitializeComponent();
        _viewModel = (UserExplorerControlVM?)VmLocator.Resolve<UserExplorerControl>();
        DataContext = _viewModel;
    }
}

