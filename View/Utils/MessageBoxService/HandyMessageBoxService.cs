﻿using System.Windows;

using MessageBox = HandyControl.Controls.MessageBox;


namespace View.Utils.MessageBoxService;

public class HandyMessageBoxService : IMessageBoxService
{
    public MessageBoxResult ShowMessage(string messageBoxText,
                                        string caption = null,
                                        MessageBoxButton button = MessageBoxButton.OK,
                                        MessageBoxImage icon = MessageBoxImage.None,
                                        MessageBoxResult defaultResult = MessageBoxResult.None)
    {
        return MessageBox.Show(messageBoxText, caption, button, icon, defaultResult);
    }
}

