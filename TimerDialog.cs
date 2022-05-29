namespace Mousetrap;

public partial class TimerDialog : Form
{
    public TimerDialog() : base()
    {
        InitializeComponent();
        _ok.Click += Ok;
    }

    private void Ok(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    public TimeSpan Value => GetValue();

    private TimeSpan GetValue()
    {
        int hours = Convert.ToInt32(_hours.Value);
        int minutes = Convert.ToInt32(_minutes.Value);
        int seconds = Convert.ToInt32(_seconds.Value);
        TimeSpan value = new TimeSpan(hours: hours, minutes: minutes, seconds: seconds);
        return value;
    }
}