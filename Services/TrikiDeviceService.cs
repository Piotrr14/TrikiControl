using TrikiControl.Ble;
using TrikiControl.Models;

namespace TrikiControl.Services;

public sealed class TrikiDeviceService
{
    public event EventHandler<string>? LogMessage;
    public event EventHandler<ImuSample>? SampleReceived;
    public event EventHandler<TrikiDeviceInfo>? DeviceInfoReceived;
    public event EventHandler<bool>? ConnectionStateChanged;

    public bool IsConnected { get; private set; }

    private CancellationTokenSource? _connectionCts;
    private Task? _connectionTask;

    public bool IsConnecting { get; private set; }

    public Task ConnectAsync()
    {
        if (IsConnecting)
        {
            Log("Already connecting.");
            return Task.CompletedTask;
        }

        if (_connectionTask is not null && !_connectionTask.IsCompleted)
        {
            Log("Already connecting/connected.");
            return Task.CompletedTask;
        }

        IsConnecting = true;
        SetConnected(false);

        _connectionCts?.Dispose();
        _connectionCts = new CancellationTokenSource();

        var reader = new TrikiBleReader(AppOptions.Default);

        reader.LogMessage += (_, msg) => Log(msg);

        reader.SampleReceived += (_, sample) =>
        {
            SampleReceived?.Invoke(this, sample);
        };

        reader.DeviceInfoReceived += (_, info) =>
        {
            IsConnecting = false;
            SetConnected(true);
            DeviceInfoReceived?.Invoke(this, info);
        };

        reader.ConnectionLost += (_, _) =>
        {
            IsConnecting = false;
            SetConnected(false);
            Log("Disconnected.");
        };

        Log("Starting BLE connection...");
        _connectionTask = reader.RunAsync(_connectionCts.Token);

        _connectionTask.ContinueWith(task =>
        {
            IsConnecting = false;

            if (task.IsFaulted)
                Log("Connection task failed: " + task.Exception?.GetBaseException().Message);

            SetConnected(false);
        }, TaskScheduler.Default);

        return Task.CompletedTask;
    }

    public async Task DisconnectAsync()
    {
        if (_connectionCts is null)
        {
            SetConnected(false);
            return;
        }

        Log("Disconnect requested.");

        try
        {
            await _connectionCts.CancelAsync();

            if (_connectionTask is not null)
            {
                await _connectionTask;
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            _connectionCts.Dispose();
            _connectionCts = null;
            _connectionTask = null;
            SetConnected(false);
        }
    }

    private void SetConnected(bool connected)
    {
        if (IsConnected == connected)
            return;

        IsConnected = connected;
        ConnectionStateChanged?.Invoke(this, connected);
    }

    private void Log(string message)
    {
        LogMessage?.Invoke(this, message);
    }
}