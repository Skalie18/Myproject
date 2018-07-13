using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sars.Systems.Controls;


public delegate void NonResidancySelectedHandler(NonResidancyEventArgs e);

public class NonResidancyEventArgs : EventArgs
{
    public NonResidancyEventArgs(string selectedValue)
    {
        
    }
    public NonResidancyEventArgs()
    {

    }
    public string SelectedValue { get; set; }
    public string SelectedText { get; set; }
    public RadioButtonListField ListField { get; set; }
}