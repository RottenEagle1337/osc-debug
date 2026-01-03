using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Rug.Osc;

namespace osc_debug
{
    public partial class Form1 : Form
    {
        private OscReceiver? oscReceiver;
        private CancellationTokenSource receiveCts = new();
        private Task? receiveTask;

        private readonly List<LogEntry> receiveLogEntries = new();
        private const int MaxReceiveLogEntries = 500;


        private OscSender? oscSender;
        private readonly object senderLock = new();

        private readonly List<LogEntry> sendLogEntries = new();
        private const int MaxSendLogEntries = 100;

        private class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Address { get; set; }
            public string[] Arguments { get; set; }
            public bool IsBundle { get; set; }
            public bool IsSystemMessage { get; set; }

            public string ToDisplayString()
            {
                var sb = new StringBuilder();
                sb.AppendLine($"[{Timestamp:HH:mm:ss.fff}] {(IsBundle ? "[BUNDLE]" : Address)}");

                if (IsSystemMessage && Arguments.Length > 0)
                {
                    sb.AppendLine(Arguments[0]);
                }
                else if (Arguments.Length > 0)
                {
                    sb.AppendLine($"Arguments: {string.Join(", ", Arguments)}");
                }

                return sb.ToString();
            }
        }

        public Form1()
        {
            InitializeComponent();
            ConfigureForm();

            InitializeEvents();
        }

        private void ConfigureForm()
        {
            Address.Width = 300;
            Arguments.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //AddTestData("/test/1", "1; 2.5; hello");
            //AddTestData("/test/2", "100; 200");

            ConfigureLogTextBox(txtLog);
            ConfigureLogTextBox(txtSendLog);

            btnReceiveDisconnect.Enabled = false;
            btnSendDisconnect.Enabled = false;
        }

        private void ConfigureLogTextBox(TextBox textBox)
        {
            textBox.Font = new Font("Consolas", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void InitializeEvents()
        {
            btnReceiveConnect.Click -= BtnReceiveConnect_Click;
            btnReceiveConnect.Click += BtnReceiveConnect_Click;

            btnReceiveDisconnect.Click -= BtnReceiveDisconnect_Click;
            btnReceiveDisconnect.Click += BtnReceiveDisconnect_Click;

            btnSendConnect.Click -= BtnSendConnect_Click;
            btnSendConnect.Click += BtnSendConnect_Click;

            btnSendDisconnect.Click -= BtnSendDisconnect_Click;
            btnSendDisconnect.Click += BtnSendDisconnect_Click;

            btnSend.Click -= BtnSend_Click;
            btnSend.Click += BtnSend_Click;

            btnSave.Click -= BtnSave_Click;
            btnSave.Click += BtnSave_Click;

            btnLoad.Click -= BtnLoad_Click;
            btnLoad.Click += BtnLoad_Click;

            btnClearLog.Click -= (s, e) => ClearLog(receiveLogEntries, txtLog);
            btnClearLog.Click += (s, e) => ClearLog(receiveLogEntries, txtLog);

            btnClearSendLog.Click -= (s, e) => ClearLog(sendLogEntries, txtSendLog);
            btnClearSendLog.Click += (s, e) => ClearLog(sendLogEntries, txtSendLog);
        }

        private void AddTestData(params object[] values)
        {
            dgvSendCommands.Rows.Add(values);
        }


        #region receive
        private void BtnReceiveConnect_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtReceivePort.Text, out int port) || port <= 0)
            {
                AddSystemLog("Invalid port number", receiveLogEntries, txtLog);
                return;
            }

            try
            {
                oscReceiver = new OscReceiver(port);
                oscReceiver.Connect();

                btnReceiveConnect.Enabled = false;
                btnReceiveDisconnect.Enabled = true;
                AddSystemLog($"Listening on port {port}", receiveLogEntries, txtLog);

                receiveCts.Cancel();
                receiveCts.Dispose();
                receiveCts = new CancellationTokenSource();

                receiveTask = Task.Run(() => ReceiveMessagesAsync(receiveCts.Token), receiveCts.Token);
            }
            catch (Exception ex)
            {
                AddSystemLog($"Receive initialization failed: {ex.Message}", receiveLogEntries, txtLog);
                CleanupReceiver();
            }
        }

        private async Task ReceiveMessagesAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested && oscReceiver != null && oscReceiver.State == OscSocketState.Connected)
            {
                try
                {
                    ct.ThrowIfCancellationRequested();

                    var packet = oscReceiver.Receive();
                    if (packet != null && !ct.IsCancellationRequested)
                    {
                        ProcessReceivedPacket(packet);
                    }
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (!ct.IsCancellationRequested)
                    {
                        AddSystemLog($"Receiving error: {ex.Message}", receiveLogEntries, txtLog);
                    }
                    break;
                }
            }
        }

        private void ProcessReceivedPacket(OscPacket? packet)
        {
            if (packet == null || IsDisposed) return;

            LogEntry entry = new LogEntry { Timestamp = DateTime.Now };

            if (packet is OscMessage message)
            {
                entry.Address = message.Address;
                entry.Arguments = message.ToArray().Select(a => a.ToString() ?? "null").ToArray();
                entry.IsBundle = false;
                entry.IsSystemMessage = false;
            }
            else if (packet is OscBundle bundle)
            {
                entry.Address = "OSC Bundle";
                entry.Arguments = bundle.Select(m =>
                    m is OscMessage msg ? $"{msg.Address}: {string.Join(", ", msg.ToArray().Select(a => a.ToString() ?? "null"))}" : "Unknown").ToArray();
                entry.IsBundle = true;
                entry.IsSystemMessage = false;
            }
            else
            {
                return;
            }

            AddLogEntry(entry, receiveLogEntries, txtLog);
        }

        private async void BtnReceiveDisconnect_Click(object? sender, EventArgs e)
        {
            btnReceiveDisconnect.Enabled = false;
            await StopReceivingAsync();

            btnReceiveConnect.Enabled = true;
            if (!IsDisposed)
            {
                AddSystemLog("Disconnected", receiveLogEntries, txtLog);
            }
        }

        private async Task StopReceivingAsync()
        {
            receiveCts.Cancel();

            try
            {
                oscReceiver?.Close();
            }
            catch {}

            try
            {
                if (receiveTask != null)
                {
                    await Task.WhenAny(receiveTask, Task.Delay(500));

                    if (!receiveTask.IsCompleted)
                    {
                        await receiveTask;
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                AddSystemLog($"Error stopping receiver: {ex.Message}", receiveLogEntries, txtLog);
            }
            finally
            {
                CleanupReceiver();
            }
        }

        private void CleanupReceiver()
        {
            try
            {
                oscReceiver?.Close();
                oscReceiver?.Dispose();
                oscReceiver = null;
                btnReceiveConnect.Enabled = true;
                btnReceiveDisconnect.Enabled = false;
            }
            catch (Exception ex)
            {
                AddSystemLog($"Receiver cleanup failed: {ex.Message}", receiveLogEntries, txtLog);
            }
        }
        #endregion

        #region sender
        private void BtnSendConnect_Click(object? sender, EventArgs e)
        {
            if (!IPAddress.TryParse(txtSendIP.Text, out IPAddress? remoteIP))
            {
                AddSystemLog("Invalid IP address", sendLogEntries, txtSendLog);
                return;
            }

            if (!int.TryParse(txtSendPort.Text, out int port) || port <= 0)
            {
                AddSystemLog("Invalid port number", sendLogEntries, txtSendLog);
                return;
            }

            try
            {
                oscSender = new OscSender(remoteIP, port);
                oscSender.Connect();

                AddSystemLog($"Connected to {remoteIP}:{port}", sendLogEntries, txtSendLog);

                btnSendConnect.Enabled = false;
                btnSendDisconnect.Enabled = true;
            }
            catch (Exception ex)
            {
                AddSystemLog($"Sender connection failed: {ex.Message}", sendLogEntries, txtSendLog);
                CleanupSender();
            }
        }

        private void BtnSendDisconnect_Click(object? sender, EventArgs e)
        {
            CleanupSender();
            AddSystemLog("Disconnected", sendLogEntries, txtSendLog);
        }

        private void CleanupSender()
        {
            try
            {
                oscSender?.Close();
                oscSender?.Dispose();
                oscSender = null;
                btnSendConnect.Enabled = true;
                btnSendDisconnect.Enabled = false;
            }
            catch (Exception ex)
            {
                AddSystemLog($"Sender cleanup failed: {ex.Message}", sendLogEntries, txtSendLog);
            }
        }

        private async void BtnSend_Click(object? sender, EventArgs e)
        {
            if (oscSender == null || oscSender.State != OscSocketState.Connected)
            {
                AddSystemLog("Sender is not connected", sendLogEntries, txtSendLog);
                return;
            }

            var selectedRows = dgvSendCommands.SelectedRows
                .Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow)
                .ToList();

            if (!selectedRows.Any())
            {
                AddSystemLog("Please select rows to send", sendLogEntries, txtSendLog);
                return;
            }

            btnSend.Enabled = false;
            try
            {
                int sentCount = 0;
                foreach (DataGridViewRow row in selectedRows)
                {
                    if (row.Cells["Address"].Value is not string address || string.IsNullOrWhiteSpace(address))
                        continue;

                    string argsString = row.Cells["Arguments"].Value?.ToString() ?? "";
                    object[] args = ParseArguments(argsString);

                    await SendOscMessageAsync(address, args);
                    AddSystemLog($"Sent: {address} ({string.Join(", ", args)})", sendLogEntries, txtSendLog);
                    sentCount++;
                    await Task.Delay(10);
                }

                //AddSystemLog($"Successfully sent {sentCount} messages", sendLogEntries, txtSendLog);
            }
            catch (Exception ex)
            {
                AddSystemLog($"Sending failed: {ex.Message}", sendLogEntries, txtSendLog);
            }
            finally
            {
                btnSend.Enabled = true;
            }
        }

        private async Task SendOscMessageAsync(string address, object[] args)
        {
            if (oscSender == null || oscSender.State != OscSocketState.Connected)
                throw new InvalidOperationException("Sender is not connected");

            await Task.Run(() =>
            {
                lock (senderLock)
                {
                    var message = new OscMessage(address, args);
                    oscSender.Send(message);
                }
            });
        }

        private object[] ParseArguments(string argsString)
        {
            if (string.IsNullOrWhiteSpace(argsString))
                return Array.Empty<object>();

            return argsString.Split(';')
                .Select(part => part.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .Select(ParseArgument)
                .ToArray();
        }

        private static object ParseArgument(string input)
        {
            string normalized = input.Replace(',', '.');

            if (int.TryParse(normalized, out int intValue)) return intValue;
            if (float.TryParse(normalized, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float floatValue))
                return floatValue;
            if (bool.TryParse(normalized, out bool boolValue)) return boolValue;

            return input;
        }
        #endregion

        #region save and load
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            using var saveDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if (saveDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                using var writer = new StreamWriter(saveDialog.FileName);
                foreach (DataGridViewRow row in dgvSendCommands.Rows)
                {
                    if (row.IsNewRow) continue;

                    string address = row.Cells["Address"].Value?.ToString() ?? "";
                    string arguments = row.Cells["Arguments"].Value?.ToString() ?? "";
                    writer.WriteLine($"{address};{arguments}");
                }
                AddSystemLog($"Commands saved to {saveDialog.FileName}", sendLogEntries, txtSendLog);
            }
            catch (Exception ex)
            {
                AddSystemLog($"Save failed: {ex.Message}", sendLogEntries, txtSendLog);
            }
        }

        private void BtnLoad_Click(object? sender, EventArgs e)
        {
            using var openDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if (openDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                dgvSendCommands.Rows.Clear();
                foreach (string line in File.ReadAllLines(openDialog.FileName))
                {
                    string[] parts = line.Split(new[] { ';' }, 2);
                    dgvSendCommands.Rows.Add(parts[0], parts.Length > 1 ? parts[1] : "");
                }
                AddSystemLog($"Commands loaded from {openDialog.FileName}", sendLogEntries, txtSendLog);
            }
            catch (Exception ex)
            {
                AddSystemLog($"Load failed: {ex.Message}", sendLogEntries, txtSendLog);
            }
        }
        #endregion

        #region logging
        private void AddLogEntry(LogEntry entry, List<LogEntry> logEntries, TextBox logTextBox)
        {
            if (IsDisposed) return;

            lock (logEntries)
            {
                logEntries.Add(entry);
                if (logEntries.Count > (logTextBox == txtLog ? MaxReceiveLogEntries : MaxSendLogEntries))
                {
                    logEntries.RemoveAt(0);
                }
            }

            UpdateLogDisplay(logEntries, logTextBox);
        }

        private void AddSystemLog(string message, List<LogEntry> logEntries, TextBox logTextBox)
        {
            if (IsDisposed) return;

            var entry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Address = "[SYSTEM]",
                Arguments = new[] { message },
                IsBundle = false,
                IsSystemMessage = true
            };

            AddLogEntry(entry, logEntries, logTextBox);
        }

        private void UpdateLogDisplay(List<LogEntry> logEntries, TextBox logTextBox)
        {
            if (IsDisposed || logTextBox.IsDisposed) return;

            try
            {
                if (logTextBox.InvokeRequired)
                {
                    logTextBox.Invoke((Action)(() => UpdateLogDisplay(logEntries, logTextBox)));
                    return;
                }

                var sb = new StringBuilder();
                lock (logEntries)
                {
                    foreach (var entry in logEntries)
                    {
                        sb.Append(entry.ToDisplayString());
                        sb.AppendLine(new string('-', 50));
                    }
                }

                logTextBox.Text = sb.ToString();
                logTextBox.SelectionStart = logTextBox.Text.Length;
                logTextBox.ScrollToCaret();
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        private void ClearLog(List<LogEntry> logEntries, TextBox logTextBox)
        {
            lock (logEntries)
            {
                logEntries.Clear();
            }
            UpdateLogDisplay(logEntries, logTextBox);
        }
        #endregion

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            receiveCts.Cancel();
            await StopReceivingAsync();
            CleanupSender();
            base.OnFormClosing(e);
        }

        private void HandleError(string context, Exception ex)
        {
            if (IsDisposed) return;

            Invoke(() =>
            {
                MessageBox.Show($"{context}: {ex.Message}\n{ex.StackTrace}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            });
        }
    }
}