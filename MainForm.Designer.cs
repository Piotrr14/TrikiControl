using TrikiControl.Services;

namespace TrikiControl
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnConnect = new Button();
            btnDisconnect = new Button();
            lblStatus = new Label();
            lblBattery = new Label();
            txtLog = new RichTextBox();
            chkAutoConnect = new CheckBox();
            cmbRotateClockwise = new ComboBox();
            btnSaveSettings = new Button();
            cmbRotateCounterClockwise = new ComboBox();
            cmbShake = new ComboBox();
            cmbFaceDown = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(12, 283);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(94, 29);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(112, 283);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(94, 29);
            btnDisconnect.TabIndex = 1;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(212, 287);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(146, 20);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Status: Disconnected";
            // 
            // lblBattery
            // 
            lblBattery.AutoSize = true;
            lblBattery.Location = new Point(665, 287);
            lblBattery.Name = "lblBattery";
            lblBattery.Size = new Size(75, 20);
            lblBattery.TabIndex = 3;
            lblBattery.Text = "Battery: --";
            // 
            // txtLog
            // 
            txtLog.Location = new Point(12, 318);
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(776, 120);
            txtLog.TabIndex = 5;
            txtLog.Text = "";
            // 
            // chkAutoConnect
            // 
            chkAutoConnect.AutoSize = true;
            chkAutoConnect.Location = new Point(5, 12);
            chkAutoConnect.Name = "chkAutoConnect";
            chkAutoConnect.Size = new Size(119, 24);
            chkAutoConnect.TabIndex = 6;
            chkAutoConnect.Text = "Auto connect";
            chkAutoConnect.UseVisualStyleBackColor = true;
            // 
            // cmbRotateClockwise
            // 
            cmbRotateClockwise.FormattingEnabled = true;
            cmbRotateClockwise.Location = new Point(5, 42);
            cmbRotateClockwise.Name = "cmbRotateClockwise";
            cmbRotateClockwise.Size = new Size(151, 28);
            cmbRotateClockwise.TabIndex = 7;
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.Location = new Point(5, 189);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(94, 29);
            btnSaveSettings.TabIndex = 8;
            btnSaveSettings.Text = "Save";
            btnSaveSettings.UseVisualStyleBackColor = true;
            // 
            // cmbRotateCounterClockwise
            // 
            cmbRotateCounterClockwise.FormattingEnabled = true;
            cmbRotateCounterClockwise.Location = new Point(5, 76);
            cmbRotateCounterClockwise.Name = "cmbRotateCounterClockwise";
            cmbRotateCounterClockwise.Size = new Size(151, 28);
            cmbRotateCounterClockwise.TabIndex = 9;
            // 
            // cmbShake
            // 
            cmbShake.FormattingEnabled = true;
            cmbShake.Location = new Point(5, 144);
            cmbShake.Name = "cmbShake";
            cmbShake.Size = new Size(151, 28);
            cmbShake.TabIndex = 10;
            // 
            // cmbFaceDown
            // 
            cmbFaceDown.FormattingEnabled = true;
            cmbFaceDown.Location = new Point(5, 110);
            cmbFaceDown.Name = "cmbFaceDown";
            cmbFaceDown.Size = new Size(151, 28);
            cmbFaceDown.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(162, 50);
            label1.Name = "label1";
            label1.Size = new Size(122, 20);
            label1.TabIndex = 12;
            label1.Text = "Rotate Clockwise";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(162, 84);
            label2.Name = "label2";
            label2.Size = new Size(178, 20);
            label2.TabIndex = 13;
            label2.Text = "Rotate Counter Clockwise";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(162, 118);
            label3.Name = "label3";
            label3.Size = new Size(79, 20);
            label3.TabIndex = 14;
            label3.Text = "Face down";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(162, 152);
            label4.Name = "label4";
            label4.Size = new Size(48, 20);
            label4.TabIndex = 15;
            label4.Text = "Shake";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGreen;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cmbFaceDown);
            Controls.Add(cmbShake);
            Controls.Add(cmbRotateCounterClockwise);
            Controls.Add(btnSaveSettings);
            Controls.Add(cmbRotateClockwise);
            Controls.Add(chkAutoConnect);
            Controls.Add(txtLog);
            Controls.Add(lblBattery);
            Controls.Add(lblStatus);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            MaximizeBox = false;
            Name = "MainForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "TrikiControl";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                return;
            }

            base.OnFormClosing(e);
        }

        private Button btnConnect;
        private Button btnDisconnect;
        private Label lblStatus;
        private Label lblBattery;
        private RichTextBox txtLog;
        private CheckBox chkAutoConnect;
        private ComboBox cmbRotateClockwise;
        private Button btnSaveSettings;
        private ComboBox cmbRotateCounterClockwise;
        private ComboBox cmbShake;
        private ComboBox cmbFaceDown;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
