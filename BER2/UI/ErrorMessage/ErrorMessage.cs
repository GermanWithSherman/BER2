using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

public class ErrorMessage
{

    private static ErrorMessage _instance;
    private static ErrorMessage Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ErrorMessage();
            return _instance;
        }
    }


    private void ShowError(string msg)
    {
        MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public static void Show(Exception e)
    {
        Show("Error:"+e.Message);
    }

    public static void Show(string msg)
    {
        Instance.ShowError(msg);
    }

}
