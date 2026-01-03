using System.Drawing;
using System.Windows.Forms;

namespace osc_debug
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox grpReceive;
        private System.Windows.Forms.TextBox txtReceiveIP;
        private System.Windows.Forms.TextBox txtReceivePort;
        private System.Windows.Forms.Button btnReceiveConnect;
        private System.Windows.Forms.Button btnReceiveDisconnect;
        private System.Windows.Forms.GroupBox grpSend;
        private System.Windows.Forms.TextBox txtSendIP;
        private System.Windows.Forms.TextBox txtSendPort;
        private System.Windows.Forms.Button btnSendConnect;
        private System.Windows.Forms.Button btnSendDisconnect;
        private System.Windows.Forms.DataGridView dgvSendCommands;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Arguments;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.TextBox txtSendLog;
        private System.Windows.Forms.Button btnClearSendLog;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.grpReceive = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnReceiveDisconnect = new System.Windows.Forms.Button();
            this.btnReceiveConnect = new System.Windows.Forms.Button();
            this.txtReceivePort = new System.Windows.Forms.TextBox();
            this.txtReceiveIP = new System.Windows.Forms.TextBox();
            this.grpSend = new System.Windows.Forms.GroupBox();
            this.txtSendLog = new System.Windows.Forms.TextBox();
            this.btnClearSendLog = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSendIP = new System.Windows.Forms.TextBox();
            this.dgvSendCommands = new System.Windows.Forms.DataGridView();
            this.btnSendDisconnect = new System.Windows.Forms.Button();
            this.btnSendConnect = new System.Windows.Forms.Button();
            this.txtSendPort = new System.Windows.Forms.TextBox();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Arguments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.grpReceive.SuspendLayout();
            this.grpSend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendCommands)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.grpReceive);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.grpSend);
            this.splitContainer.Size = new System.Drawing.Size(1008, 601);
            this.splitContainer.SplitterDistance = 504;
            this.splitContainer.TabIndex = 0;
            // 
            // grpReceive
            // 
            this.grpReceive.Controls.Add(this.txtLog);
            this.grpReceive.Controls.Add(this.btnClearLog);
            this.grpReceive.Controls.Add(this.btnReceiveDisconnect);
            this.grpReceive.Controls.Add(this.btnReceiveConnect);
            this.grpReceive.Controls.Add(this.txtReceivePort);
            this.grpReceive.Controls.Add(this.txtReceiveIP);
            this.grpReceive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpReceive.Location = new System.Drawing.Point(0, 0);
            this.grpReceive.Name = "grpReceive";
            this.grpReceive.Size = new System.Drawing.Size(504, 601);
            this.grpReceive.TabIndex = 0;
            this.grpReceive.TabStop = false;
            this.grpReceive.Text = "Receiver";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(10, 50);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(488, 545);
            this.txtLog.TabIndex = 5;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(319, 20);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 4;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            // 
            // btnReceiveDisconnect
            // 
            this.btnReceiveDisconnect.Enabled = false;
            this.btnReceiveDisconnect.Location = new System.Drawing.Point(238, 20);
            this.btnReceiveDisconnect.Name = "btnReceiveDisconnect";
            this.btnReceiveDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnReceiveDisconnect.TabIndex = 3;
            this.btnReceiveDisconnect.Text = "Disconnect";
            this.btnReceiveDisconnect.UseVisualStyleBackColor = true;
            // 
            // btnReceiveConnect
            // 
            this.btnReceiveConnect.Location = new System.Drawing.Point(157, 20);
            this.btnReceiveConnect.Name = "btnReceiveConnect";
            this.btnReceiveConnect.Size = new System.Drawing.Size(75, 23);
            this.btnReceiveConnect.TabIndex = 2;
            this.btnReceiveConnect.Text = "Connect";
            this.btnReceiveConnect.UseVisualStyleBackColor = true;
            // 
            // txtReceivePort
            // 
            this.txtReceivePort.Location = new System.Drawing.Point(106, 22);
            this.txtReceivePort.Name = "txtReceivePort";
            this.txtReceivePort.Size = new System.Drawing.Size(45, 20);
            this.txtReceivePort.TabIndex = 1;
            this.txtReceivePort.Text = "7000";
            // 
            // txtReceiveIP
            // 
            this.txtReceiveIP.Location = new System.Drawing.Point(10, 22);
            this.txtReceiveIP.Name = "txtReceiveIP";
            this.txtReceiveIP.Size = new System.Drawing.Size(90, 20);
            this.txtReceiveIP.TabIndex = 0;
            this.txtReceiveIP.Text = "0.0.0.0";
            // 
            // grpSend
            // 
            this.grpSend.Controls.Add(this.txtSendLog);
            this.grpSend.Controls.Add(this.btnClearSendLog);
            this.grpSend.Controls.Add(this.btnLoad);
            this.grpSend.Controls.Add(this.btnSave);
            this.grpSend.Controls.Add(this.btnSend);
            this.grpSend.Controls.Add(this.txtSendIP);
            this.grpSend.Controls.Add(this.dgvSendCommands);
            this.grpSend.Controls.Add(this.btnSendDisconnect);
            this.grpSend.Controls.Add(this.btnSendConnect);
            this.grpSend.Controls.Add(this.txtSendPort);
            this.grpSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSend.Location = new System.Drawing.Point(0, 0);
            this.grpSend.Name = "grpSend";
            this.grpSend.Size = new System.Drawing.Size(500, 601);
            this.grpSend.TabIndex = 0;
            this.grpSend.TabStop = false;
            this.grpSend.Text = "Sender";
            // 
            // txtSendLog
            // 
            this.txtSendLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSendLog.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtSendLog.Location = new System.Drawing.Point(6, 510);
            this.txtSendLog.Multiline = true;
            this.txtSendLog.Name = "txtSendLog";
            this.txtSendLog.ReadOnly = true;
            this.txtSendLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSendLog.Size = new System.Drawing.Size(488, 85);
            this.txtSendLog.TabIndex = 12;
            // 
            // btnClearSendLog
            // 
            this.btnClearSendLog.Location = new System.Drawing.Point(318, 19);
            this.btnClearSendLog.Name = "btnClearSendLog";
            this.btnClearSendLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearSendLog.TabIndex = 11;
            this.btnClearSendLog.Text = "Clear Log";
            this.btnClearSendLog.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Location = new System.Drawing.Point(262, 476);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 10;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(181, 476);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSend.Location = new System.Drawing.Point(6, 476);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(90, 23);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "Send Selected";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // txtSendIP
            // 
            this.txtSendIP.Location = new System.Drawing.Point(6, 22);
            this.txtSendIP.Name = "txtSendIP";
            this.txtSendIP.Size = new System.Drawing.Size(90, 20);
            this.txtSendIP.TabIndex = 0;
            this.txtSendIP.Text = "127.0.0.1";
            // 
            // dgvSendCommands
            // 
            this.dgvSendCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSendCommands.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSendCommands.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Address,
            this.Arguments});
            this.dgvSendCommands.Location = new System.Drawing.Point(6, 48);
            this.dgvSendCommands.Name = "dgvSendCommands";
            this.dgvSendCommands.Size = new System.Drawing.Size(488, 422);
            this.dgvSendCommands.TabIndex = 4;
            // 
            // btnSendDisconnect
            // 
            this.btnSendDisconnect.Enabled = false;
            this.btnSendDisconnect.Location = new System.Drawing.Point(238, 19);
            this.btnSendDisconnect.Name = "btnSendDisconnect";
            this.btnSendDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnSendDisconnect.TabIndex = 3;
            this.btnSendDisconnect.Text = "Disconnect";
            this.btnSendDisconnect.UseVisualStyleBackColor = true;
            // 
            // btnSendConnect
            // 
            this.btnSendConnect.Location = new System.Drawing.Point(157, 19);
            this.btnSendConnect.Name = "btnSendConnect";
            this.btnSendConnect.Size = new System.Drawing.Size(75, 23);
            this.btnSendConnect.TabIndex = 2;
            this.btnSendConnect.Text = "Connect";
            this.btnSendConnect.UseVisualStyleBackColor = true;
            // 
            // txtSendPort
            // 
            this.txtSendPort.Location = new System.Drawing.Point(102, 22);
            this.txtSendPort.Name = "txtSendPort";
            this.txtSendPort.Size = new System.Drawing.Size(45, 20);
            this.txtSendPort.TabIndex = 1;
            this.txtSendPort.Text = "7001";
            // 
            // Address
            // 
            this.Address.HeaderText = "Address";
            this.Address.Name = "Address";
            this.Address.Width = 300;
            // 
            // Arguments
            // 
            this.Arguments.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Arguments.HeaderText = "Arguments (разделитель ;)";
            this.Arguments.Name = "Arguments";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.splitContainer);
            this.Name = "Form1";
            this.Text = "osc-debug";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.grpReceive.ResumeLayout(false);
            this.grpReceive.PerformLayout();
            this.grpSend.ResumeLayout(false);
            this.grpSend.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendCommands)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}