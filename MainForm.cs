using TrikiControl.Actions;
using TrikiControl.Gestures;
using TrikiControl.Services;
using TrikiControl.Settings;

namespace TrikiControl
{
    public partial class MainForm : Form
    {
        private readonly TrikiDeviceService _triki;
        private readonly SettingsService _settings;
        private readonly GestureDetector _gestureDetector = new();

        private readonly ActionExecutor _actionExecutor = new();

        public MainForm(TrikiDeviceService triki, SettingsService settings)
        {
            InitializeComponent();

            _triki = triki;
            _settings = settings;

            btnConnect.Click += btnConnect_Click;
            btnDisconnect.Click += btnDisconnect_Click;
            btnSaveSettings.Click += btnSaveSettings_Click;

            cmbRotateClockwise.DataSource = Enum.GetValues<ActionType>();
            cmbRotateCounterClockwise.DataSource = Enum.GetValues<ActionType>();
            cmbShake.DataSource = Enum.GetValues<ActionType>();
            cmbFaceDown.DataSource = Enum.GetValues<ActionType>();

            _triki.LogMessage += (_, msg) =>
            {
                RunOnUiThread(() => Log(msg));
            };

            _triki.DeviceInfoReceived += (_, info) =>
            {
                RunOnUiThread(() =>
                {
                    lblStatus.Text = $"Status: Connected to {info.DeviceName ?? "Triki"}";
                    lblBattery.Text = $"Battery: {(info.BatteryLevelPercent?.ToString() ?? "--")}%";
                });
            };

            _triki.SampleReceived += (_, sample) =>
            {
                var gesture = _gestureDetector.Process(sample);

                if(gesture is not null)
                {
                    var mapping = GetMapping(gesture.Value.Type);
                    if (mapping is not null)
                    {
                        _actionExecutor.Execute(mapping, gesture.Value.Value);
                    }
                }

                RunOnUiThread(() =>
                {
                    lblStatus.Text = $"Status: Connected - sample {sample.FrameIndex}";

                    if (gesture is not null)
                    {
                        Log($"Gesture: {gesture.Value.Type}, value={gesture.Value.Value:F1}");
                    }
                });
            };

            LoadSettingsToUi();
        }

        private void RunOnUiThread(Action action)
        {
            if (IsDisposed)
                return;

            if (!IsHandleCreated)
            {
                action();
                return;
            }

            if (InvokeRequired)
                BeginInvoke(action);
            else
                action();
        }

        private void Log(string message)
        {
            txtLog.AppendText(message + Environment.NewLine);
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private async void btnConnect_Click(object? sender, EventArgs e)
        {
            RunOnUiThread(() => Log("Connect clicked"));
            await _triki.ConnectAsync();
        }

        private async void btnDisconnect_Click(object? sender, EventArgs e)
        {
            RunOnUiThread(() => Log("Disconnect clicked"));
            await _triki.DisconnectAsync();
        }

        private ActionMapping? GetMapping(GestureType gesture)
        {
            if (!_settings.Current.Mappings.TryGetValue(gesture, out var action))
                return null;

            return new ActionMapping(gesture, action);
        }

        private void LoadSettingsToUi()
        {
            chkAutoConnect.Checked = _settings.Current.AutoConnect;

            SetupActionCombo(cmbRotateClockwise, GestureType.RotateClockwise);
            SetupActionCombo(cmbRotateCounterClockwise, GestureType.RotateCounterClockwise);
            SetupActionCombo(cmbShake, GestureType.Shake);
            SetupActionCombo(cmbFaceDown, GestureType.FaceDown);
        }

        private void SetupActionCombo(ComboBox combo, GestureType gesture)
        {
            combo.DataSource = Enum.GetValues<ActionType>();

            if (_settings.Current.Mappings.TryGetValue(gesture, out var action))
                combo.SelectedItem = action;
        }

        private void btnSaveSettings_Click(object? sender, EventArgs e)
        {
            _settings.Current.AutoConnect = chkAutoConnect.Checked;

            _settings.Current.Mappings[GestureType.RotateClockwise] =
                (ActionType)cmbRotateClockwise.SelectedItem!;

            _settings.Current.Mappings[GestureType.RotateCounterClockwise] =
                (ActionType)cmbRotateCounterClockwise.SelectedItem!;

            _settings.Current.Mappings[GestureType.Shake] =
                (ActionType)cmbShake.SelectedItem!;

            _settings.Current.Mappings[GestureType.FaceDown] =
                (ActionType)cmbFaceDown.SelectedItem!;

            _settings.Save();

            Log("Settings saved.");
        }
    }
}
