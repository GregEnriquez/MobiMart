using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobiMart.Model;

public class CalendarDay : INotifyPropertyChanged
{
    public int DayIdx { get; set; }
    public int Value { get; set; }
    private bool _isSelected = false;
    public bool IsSelected { get => _isSelected; set { _isSelected = value; OnPropertyChanged(); } }
    private bool _hasReminders = false;
    public bool HasReminders { get => _hasReminders; set { _hasReminders = value; OnPropertyChanged(); } }
    private bool _isVisible = true;
    public bool IsVisible { get => _isVisible; set { _isVisible = value; OnPropertyChanged(); } }

    public CalendarDay(int dayIdx)
    {
        DayIdx = dayIdx;
        Value = dayIdx + 1;
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
