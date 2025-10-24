namespace IPWhiteListManager.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvIPAddresses = new System.Windows.Forms.DataGridView();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.cmbSystemFilter = new System.Windows.Forms.ComboBox();
            this.cmbEnvironmentFilter = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnAddIP = new System.Windows.Forms.Button();
            this.btnAddSystem = new System.Windows.Forms.Button();
            this.btnRegisterNamen = new System.Windows.Forms.Button();
            this.txtTechInfo = new System.Windows.Forms.TextBox();
            this.lblSelectedItem = new System.Windows.Forms.Label();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.lblEnvironment = new System.Windows.Forms.Label();
            this.lblSystem = new System.Windows.Forms.Label();
            this.lblFilter = new System.Windows.Forms.Label();
            this.panelTechInfo = new System.Windows.Forms.Panel();
            this.btnDeleteSystem = new System.Windows.Forms.Button();
            this.btnEditSystem = new System.Windows.Forms.Button();
            this.btnDeleteIP = new System.Windows.Forms.Button();
            this.btnEditIP = new System.Windows.Forms.Button();
            this.tblContacts = new System.Windows.Forms.TableLayoutPanel();
            this.lblOwnerEmailValue = new System.Windows.Forms.Label();
            this.lblOwnerEmailTitle = new System.Windows.Forms.Label();
            this.lblOwnerNameValue = new System.Windows.Forms.Label();
            this.lblOwnerTitle = new System.Windows.Forms.Label();
            this.lblTechEmailValue = new System.Windows.Forms.Label();
            this.lblTechEmailTitle = new System.Windows.Forms.Label();
            this.lblTechNameValue = new System.Windows.Forms.Label();
            this.lblTechTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIPAddresses)).BeginInit();
            this.panelFilters.SuspendLayout();
            this.panelTechInfo.SuspendLayout();
            this.tblContacts.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvIPAddresses
            // 
            this.dgvIPAddresses.AllowUserToAddRows = false;
            this.dgvIPAddresses.AllowUserToDeleteRows = false;
            this.dgvIPAddresses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIPAddresses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIPAddresses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIPAddresses.Location = new System.Drawing.Point(0, 60);
            this.dgvIPAddresses.Name = "dgvIPAddresses";
            this.dgvIPAddresses.ReadOnly = true;
            this.dgvIPAddresses.RowHeadersVisible = false;
            this.dgvIPAddresses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIPAddresses.Size = new System.Drawing.Size(1000, 410);
            this.dgvIPAddresses.TabIndex = 0;
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(33, 12);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(160, 20);
            this.txtFilter.TabIndex = 1;
            // 
            // cmbSystemFilter
            // 
            this.cmbSystemFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSystemFilter.FormattingEnabled = true;
            this.cmbSystemFilter.Location = new System.Drawing.Point(225, 12);
            this.cmbSystemFilter.Name = "cmbSystemFilter";
            this.cmbSystemFilter.Size = new System.Drawing.Size(210, 21);
            this.cmbSystemFilter.TabIndex = 2;
            // 
            // cmbEnvironmentFilter
            // 
            this.cmbEnvironmentFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEnvironmentFilter.FormattingEnabled = true;
            this.cmbEnvironmentFilter.Location = new System.Drawing.Point(467, 12);
            this.cmbEnvironmentFilter.Name = "cmbEnvironmentFilter";
            this.cmbEnvironmentFilter.Size = new System.Drawing.Size(150, 21);
            this.cmbEnvironmentFilter.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(727, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 25);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Поиск";
            this.btnSearch.UseVisualStyleBackColor = false;
            //
            // btnAddIP
            //
            this.btnAddIP.Location = new System.Drawing.Point(917, 9);
            this.btnAddIP.Name = "btnAddIP";
            this.btnAddIP.Size = new System.Drawing.Size(80, 25);
            this.btnAddIP.TabIndex = 5;
            this.btnAddIP.Text = "Добавить IP";
            this.btnAddIP.UseVisualStyleBackColor = true;
            //
            // btnAddSystem
            //
            this.btnAddSystem.Location = new System.Drawing.Point(817, 9);
            this.btnAddSystem.Name = "btnAddSystem";
            this.btnAddSystem.Size = new System.Drawing.Size(100, 25);
            this.btnAddSystem.TabIndex = 6;
            this.btnAddSystem.Text = "Добавить ИС";
            this.btnAddSystem.UseVisualStyleBackColor = true;
            // 
            // btnRegisterNamen
            // 
            this.btnRegisterNamen.BackColor = System.Drawing.Color.LightGreen;
            this.btnRegisterNamen.Enabled = false;
            this.btnRegisterNamen.Location = new System.Drawing.Point(13, 145);
            this.btnRegisterNamen.Name = "btnRegisterNamen";
            this.btnRegisterNamen.Size = new System.Drawing.Size(180, 25);
            this.btnRegisterNamen.TabIndex = 6;
            this.btnRegisterNamen.Text = "Зарегистрировать в naumen";
            this.btnRegisterNamen.UseVisualStyleBackColor = false;
            // 
            // txtTechInfo
            // 
            this.txtTechInfo.BackColor = System.Drawing.Color.AliceBlue;
            this.txtTechInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTechInfo.Location = new System.Drawing.Point(390, 35);
            this.txtTechInfo.Multiline = true;
            this.txtTechInfo.Name = "txtTechInfo";
            this.txtTechInfo.ReadOnly = true;
            this.txtTechInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTechInfo.Size = new System.Drawing.Size(590, 100);
            this.txtTechInfo.TabIndex = 7;
            // 
            // lblSelectedItem
            // 
            this.lblSelectedItem.AutoSize = true;
            this.lblSelectedItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSelectedItem.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblSelectedItem.Location = new System.Drawing.Point(10, 10);
            this.lblSelectedItem.Name = "lblSelectedItem";
            this.lblSelectedItem.Size = new System.Drawing.Size(361, 15);
            this.lblSelectedItem.TabIndex = 8;
            this.lblSelectedItem.Text = "Выберите запись в таблице для просмотра деталей";
            // 
            // panelFilters
            // 
            this.panelFilters.BackColor = System.Drawing.Color.Lavender;
            this.panelFilters.Controls.Add(this.btnExportCsv);
            this.panelFilters.Controls.Add(this.lblEnvironment);
            this.panelFilters.Controls.Add(this.lblSystem);
            this.panelFilters.Controls.Add(this.lblFilter);
            this.panelFilters.Controls.Add(this.txtFilter);
            this.panelFilters.Controls.Add(this.cmbSystemFilter);
            this.panelFilters.Controls.Add(this.cmbEnvironmentFilter);
            this.panelFilters.Controls.Add(this.btnSearch);
            this.panelFilters.Controls.Add(this.btnAddSystem);
            this.panelFilters.Controls.Add(this.btnAddIP);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 0);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(10);
            this.panelFilters.Size = new System.Drawing.Size(1000, 60);
            this.panelFilters.TabIndex = 9;
            //
            // btnExportCsv
            //
            this.btnExportCsv.Location = new System.Drawing.Point(627, 9);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(90, 25);
            this.btnExportCsv.TabIndex = 9;
            this.btnExportCsv.Text = "Экспорт CSV";
            this.btnExportCsv.UseVisualStyleBackColor = true;
            //
            // lblEnvironment
            //
            this.lblEnvironment.AutoSize = true;
            this.lblEnvironment.Location = new System.Drawing.Point(445, 15);
            this.lblEnvironment.Name = "lblEnvironment";
            this.lblEnvironment.Size = new System.Drawing.Size(42, 13);
            this.lblEnvironment.TabIndex = 8;
            this.lblEnvironment.Text = "Контур";
            // 
            // lblSystem
            // 
            this.lblSystem.AutoSize = true;
            this.lblSystem.Location = new System.Drawing.Point(220, 15);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(22, 13);
            this.lblSystem.TabIndex = 7;
            this.lblSystem.Text = "ИС";
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(10, 15);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(17, 13);
            this.lblFilter.TabIndex = 6;
            this.lblFilter.Text = "IP";
            // 
            // panelTechInfo
            // 
            this.panelTechInfo.BackColor = System.Drawing.Color.AliceBlue;
            this.panelTechInfo.Controls.Add(this.btnDeleteSystem);
            this.panelTechInfo.Controls.Add(this.btnEditSystem);
            this.panelTechInfo.Controls.Add(this.btnDeleteIP);
            this.panelTechInfo.Controls.Add(this.btnEditIP);
            this.panelTechInfo.Controls.Add(this.tblContacts);
            this.panelTechInfo.Controls.Add(this.lblSelectedItem);
            this.panelTechInfo.Controls.Add(this.txtTechInfo);
            this.panelTechInfo.Controls.Add(this.btnRegisterNamen);
            this.panelTechInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTechInfo.Location = new System.Drawing.Point(0, 470);
            this.panelTechInfo.Name = "panelTechInfo";
            this.panelTechInfo.Padding = new System.Windows.Forms.Padding(10);
            this.panelTechInfo.Size = new System.Drawing.Size(1000, 180);
            this.panelTechInfo.TabIndex = 10;
            // 
            // btnDeleteSystem
            //
            this.btnDeleteSystem.Location = new System.Drawing.Point(740, 145);
            this.btnDeleteSystem.Name = "btnDeleteSystem";
            this.btnDeleteSystem.Size = new System.Drawing.Size(170, 25);
            this.btnDeleteSystem.TabIndex = 12;
            this.btnDeleteSystem.Text = "Удалить ИС";
            this.btnDeleteSystem.UseVisualStyleBackColor = true;
            //
            // btnEditSystem
            //
            this.btnEditSystem.Location = new System.Drawing.Point(560, 145);
            this.btnEditSystem.Name = "btnEditSystem";
            this.btnEditSystem.Size = new System.Drawing.Size(170, 25);
            this.btnEditSystem.TabIndex = 10;
            this.btnEditSystem.Text = "Редактировать ИС";
            this.btnEditSystem.UseVisualStyleBackColor = true;
            //
            // btnDeleteIP
            //
            this.btnDeleteIP.Location = new System.Drawing.Point(380, 145);
            this.btnDeleteIP.Name = "btnDeleteIP";
            this.btnDeleteIP.Size = new System.Drawing.Size(170, 25);
            this.btnDeleteIP.TabIndex = 11;
            this.btnDeleteIP.Text = "Удалить IP";
            this.btnDeleteIP.UseVisualStyleBackColor = true;
            //
            // btnEditIP
            //
            this.btnEditIP.Location = new System.Drawing.Point(200, 145);
            this.btnEditIP.Name = "btnEditIP";
            this.btnEditIP.Size = new System.Drawing.Size(170, 25);
            this.btnEditIP.TabIndex = 9;
            this.btnEditIP.Text = "Редактировать IP";
            this.btnEditIP.UseVisualStyleBackColor = true;
            // 
            // tblContacts
            // 
            this.tblContacts.ColumnCount = 2;
            this.tblContacts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tblContacts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblContacts.Controls.Add(this.lblOwnerEmailValue, 1, 1);
            this.tblContacts.Controls.Add(this.lblOwnerEmailTitle, 0, 1);
            this.tblContacts.Controls.Add(this.lblOwnerNameValue, 1, 0);
            this.tblContacts.Controls.Add(this.lblOwnerTitle, 0, 0);
            this.tblContacts.Controls.Add(this.lblTechEmailValue, 1, 3);
            this.tblContacts.Controls.Add(this.lblTechEmailTitle, 0, 3);
            this.tblContacts.Controls.Add(this.lblTechNameValue, 1, 2);
            this.tblContacts.Controls.Add(this.lblTechTitle, 0, 2);
            this.tblContacts.Location = new System.Drawing.Point(13, 35);
            this.tblContacts.Name = "tblContacts";
            this.tblContacts.RowCount = 4;
            this.tblContacts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tblContacts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tblContacts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tblContacts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tblContacts.Size = new System.Drawing.Size(360, 100);
            this.tblContacts.TabIndex = 9;
            // 
            // lblOwnerEmailValue
            // 
            this.lblOwnerEmailValue.AutoSize = true;
            this.lblOwnerEmailValue.Location = new System.Drawing.Point(163, 25);
            this.lblOwnerEmailValue.Name = "lblOwnerEmailValue";
            this.lblOwnerEmailValue.Size = new System.Drawing.Size(10, 13);
            this.lblOwnerEmailValue.TabIndex = 7;
            this.lblOwnerEmailValue.Text = "-";
            // 
            // lblOwnerEmailTitle
            // 
            this.lblOwnerEmailTitle.AutoSize = true;
            this.lblOwnerEmailTitle.Location = new System.Drawing.Point(3, 25);
            this.lblOwnerEmailTitle.Name = "lblOwnerEmailTitle";
            this.lblOwnerEmailTitle.Size = new System.Drawing.Size(38, 13);
            this.lblOwnerEmailTitle.TabIndex = 6;
            this.lblOwnerEmailTitle.Text = "Почта";
            // 
            // lblOwnerNameValue
            // 
            this.lblOwnerNameValue.AutoSize = true;
            this.lblOwnerNameValue.Location = new System.Drawing.Point(163, 0);
            this.lblOwnerNameValue.Name = "lblOwnerNameValue";
            this.lblOwnerNameValue.Size = new System.Drawing.Size(10, 13);
            this.lblOwnerNameValue.TabIndex = 5;
            this.lblOwnerNameValue.Text = "-";
            // 
            // lblOwnerTitle
            // 
            this.lblOwnerTitle.AutoSize = true;
            this.lblOwnerTitle.Location = new System.Drawing.Point(3, 0);
            this.lblOwnerTitle.Name = "lblOwnerTitle";
            this.lblOwnerTitle.Size = new System.Drawing.Size(96, 13);
            this.lblOwnerTitle.TabIndex = 4;
            this.lblOwnerTitle.Text = "Владелец системы";
            // 
            // lblTechEmailValue
            // 
            this.lblTechEmailValue.AutoSize = true;
            this.lblTechEmailValue.Location = new System.Drawing.Point(163, 75);
            this.lblTechEmailValue.Name = "lblTechEmailValue";
            this.lblTechEmailValue.Size = new System.Drawing.Size(10, 13);
            this.lblTechEmailValue.TabIndex = 3;
            this.lblTechEmailValue.Text = "-";
            // 
            // lblTechEmailTitle
            // 
            this.lblTechEmailTitle.AutoSize = true;
            this.lblTechEmailTitle.Location = new System.Drawing.Point(3, 75);
            this.lblTechEmailTitle.Name = "lblTechEmailTitle";
            this.lblTechEmailTitle.Size = new System.Drawing.Size(38, 13);
            this.lblTechEmailTitle.TabIndex = 2;
            this.lblTechEmailTitle.Text = "Почта";
            // 
            // lblTechNameValue
            // 
            this.lblTechNameValue.AutoSize = true;
            this.lblTechNameValue.Location = new System.Drawing.Point(163, 50);
            this.lblTechNameValue.Name = "lblTechNameValue";
            this.lblTechNameValue.Size = new System.Drawing.Size(10, 13);
            this.lblTechNameValue.TabIndex = 1;
            this.lblTechNameValue.Text = "-";
            // 
            // lblTechTitle
            // 
            this.lblTechTitle.AutoSize = true;
            this.lblTechTitle.Location = new System.Drawing.Point(3, 50);
            this.lblTechTitle.Name = "lblTechTitle";
            this.lblTechTitle.Size = new System.Drawing.Size(133, 13);
            this.lblTechTitle.TabIndex = 0;
            this.lblTechTitle.Text = "Технический специалист";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Controls.Add(this.dgvIPAddresses);
            this.Controls.Add(this.panelTechInfo);
            this.Controls.Add(this.panelFilters);
            this.Name = "MainForm";
            this.Text = "IP White List Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIPAddresses)).EndInit();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.panelTechInfo.ResumeLayout(false);
            this.panelTechInfo.PerformLayout();
            this.tblContacts.ResumeLayout(false);
            this.tblContacts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvIPAddresses;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.ComboBox cmbSystemFilter;
        private System.Windows.Forms.ComboBox cmbEnvironmentFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAddIP;
        private System.Windows.Forms.Button btnAddSystem;
        private System.Windows.Forms.Button btnRegisterNamen;
        private System.Windows.Forms.TextBox txtTechInfo;
        private System.Windows.Forms.Label lblSelectedItem;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Label lblEnvironment;
        private System.Windows.Forms.Label lblSystem;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Panel panelTechInfo;
        private System.Windows.Forms.TableLayoutPanel tblContacts;
        private System.Windows.Forms.Label lblOwnerEmailValue;
        private System.Windows.Forms.Label lblOwnerEmailTitle;
        private System.Windows.Forms.Label lblOwnerNameValue;
        private System.Windows.Forms.Label lblOwnerTitle;
        private System.Windows.Forms.Label lblTechEmailValue;
        private System.Windows.Forms.Label lblTechEmailTitle;
        private System.Windows.Forms.Label lblTechNameValue;
        private System.Windows.Forms.Label lblTechTitle;
        private System.Windows.Forms.Button btnEditSystem;
        private System.Windows.Forms.Button btnEditIP;
        private System.Windows.Forms.Button btnDeleteSystem;
        private System.Windows.Forms.Button btnDeleteIP;
        private System.Windows.Forms.Button btnExportCsv;
    }
}
