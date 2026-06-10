using System;
using System.Drawing;
using System.Windows.Forms;
using TrikiControl.Services;
using TrikiControl.Settings;

namespace TrikiControl;

public sealed class TrayApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _notifyIcon;
    private readonly MainForm _mainForm;
    private readonly TrikiDeviceService _triki = new();

    private readonly ToolStripMenuItem _connectItem;
    private readonly ToolStripMenuItem _disconnectItem;
    private readonly ToolStripMenuItem _statusItem;

    private readonly CancellationTokenSource _autoConnectCts = new();
    private readonly SettingsService _settings = new();

    public TrayApplicationContext()
    {
        _mainForm = new MainForm(_triki, _settings);

        _statusItem = new ToolStripMenuItem("Disconnected")
        {
            Enabled = false
        };

        _connectItem = new ToolStripMenuItem("Connect");
        _connectItem.Click += async (_, _) =>
        {
            SetTrayState("Connecting...", connectEnabled: false, disconnectEnabled: false);

            await _triki.ConnectAsync();

            if (!_triki.IsConnected)
            {
                SetTrayState("Disconnected", connectEnabled: true, disconnectEnabled: false);
            }
        };

        _disconnectItem = new ToolStripMenuItem("Disconnect");
        _disconnectItem.Click += async (_, _) =>
        {
            SetTrayState("Disconnecting...", connectEnabled: false, disconnectEnabled: false);

            await _triki.DisconnectAsync();

            SetTrayState("Disconnected", connectEnabled: true, disconnectEnabled: false);
        };

        var menu = new ContextMenuStrip();
        menu.Items.Add(_statusItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("Open", null, (_, _) => ShowMainForm());
        menu.Items.Add(_connectItem);
        menu.Items.Add(_disconnectItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("Exit", null, (_, _) => Exit());

        _notifyIcon = new NotifyIcon
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath),
            Text = "Disconnected",
            Visible = true,
            ContextMenuStrip = menu
        };

        _notifyIcon.DoubleClick += (_, _) => ShowMainForm();

        _triki.DeviceInfoReceived += (_, info) =>
        {
            var battery = info.BatteryLevelPercent is null ? "--" : $"{info.BatteryLevelPercent}%";
            var name = info.DeviceName ?? "Triki";

            SetTrayState($"{name} ({battery})", connectEnabled: false, disconnectEnabled: true);
        };

        _triki.ConnectionStateChanged += (_, connected) =>
        {
            if (connected)
            {
                SetTrayState("Triki connected", connectEnabled: false, disconnectEnabled: true);
            }
            else
            {
                SetTrayState("Disconnected", connectEnabled: true, disconnectEnabled: false);
            }
        };

        SetTrayState("Disconnected", connectEnabled: true, disconnectEnabled: false);
        _ = AutoConnectLoopAsync(_autoConnectCts.Token);
    }

    private void SetTrayState(string text, bool connectEnabled, bool disconnectEnabled)
    {
        if (_notifyIcon.ContextMenuStrip?.InvokeRequired == true)
        {
            _notifyIcon.ContextMenuStrip.BeginInvoke(
                () => SetTrayState(text, connectEnabled, disconnectEnabled));
            return;
        }

        var safeText = text.Length > 63 ? text[..63] : text;

        _notifyIcon.Text = safeText;
        _statusItem.Text = text;
        _connectItem.Enabled = connectEnabled;
        _disconnectItem.Enabled = disconnectEnabled;
    }

    private void ShowMainForm()
    {
        if (_mainForm.IsDisposed)
            return;

        _mainForm.Show();
        _mainForm.WindowState = FormWindowState.Normal;
        _mainForm.Activate();
    }

    private async Task AutoConnectLoopAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(1500, cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            if (!_triki.IsConnected && !_triki.IsConnecting)
            {
                SetTrayState("Waiting for Triki...", connectEnabled: false, disconnectEnabled: false);

                await _triki.ConnectAsync();

                await Task.Delay(5000, cancellationToken);

                if (!_triki.IsConnected)
                    SetTrayState("Disconnected", connectEnabled: true, disconnectEnabled: false);
            }

            await Task.Delay(10000, cancellationToken);
        }
    }

    private void Exit()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
        _mainForm.Dispose();
        _autoConnectCts.Cancel();
        _autoConnectCts.Dispose();
        Application.Exit();
    }
}